#include "NetworkManager.h"

NetworkManager::NetworkManager(SensorHAL* sensor, ServoManager* servos, DroneEngine* engine, Config* config, VoltageReader* voltageReader) {
	this->sensor = sensor;
	this->servos = servos;
	this->engine = engine;
	this->config = config;
	this->voltageReader = voltageReader;

	_dataFeedSubscribed = false;
	_lastDataSend = 0;
	_lastLogSend = 0;
	_lastDebugDataSend = 0;
	dataRevision = 1;

	lastMovementRevision = 0;
	lastOtaRevision = 0;

	saveConfig = false;
	lastConfigSave = millis();

	Log::info("Network", "Starting network manager...");
	Log::debug("Network", "[Ports] hello: %u, control: %u, data: %u", config->NetworkHelloPort, config->NetworkControlPort, config->NetworkDataPort);

	Log::info("Network", "Creating UDP sockets...");

	helloUDP = new WiFiUDP();
	controlUDP = new WiFiUDP();
	dataUDP = new WiFiUDP();
	
	if (!helloUDP->begin(config->NetworkHelloPort))
		 Log::error("Network", "Error while creating hello socket");
	if (!controlUDP->begin(config->NetworkControlPort))
		 Log::error("Network", "Error while creating control socket");
	if (!dataUDP->begin(config->NetworkDataPort))
		 Log::error("Network", "Error while creating data socket");

	Log::info("Network", "Creating buffers...");

	readBuffer = new PacketBuffer(config->NetworkPacketBufferSize);
	writeBuffer = new PacketBuffer(config->NetworkPacketBufferSize);
}

bool NetworkManager::checkRevision(int a, int b) {
	if (a >= 0 && b < 0)
		return true;
	return b > a;
}

void NetworkManager::beginSaveConfig() {
	saveConfig = true;
}

void NetworkManager::checkSaveConfig() {
	if (saveConfig) {
		if (engine->state() == StateFlying || engine->state() == StateArmed)
			return;
		if (millis() - lastConfigSave  < TIME_CONFIG_SAVE)
			return;

		ESP.wdtDisable();
		servos->waitForDetach();

		ConfigManager::saveConfig(*config);

		servos->attach();
		handlePackets(20); // Catch up mit Paketen


		ESP.wdtEnable(WDTO_0MS);

		saveConfig = false;
		lastConfigSave = millis();
	}
}

void NetworkManager::handlePackets() {
	checkSaveConfig();

	Profiler::begin("readPackets()");
	handlePackets(5);
	Profiler::end();
}

void NetworkManager::handlePackets(uint16_t num) {
	uint16_t helloPackets = 0;
	while (beginParse(helloUDP) && helloPackets++ < num)
		handleHello(helloUDP);

	uint16_t controlPackets = 0;
	while (beginParse(controlUDP) && controlPackets++ < num)
		handleControl(controlUDP);
}

void NetworkManager::handleData() {
	handleData(dataUDP);
}

bool NetworkManager::beginParse(WiFiUDP* udp) {
	Profiler::begin("parsePacket()");
	int size = udp->parsePacket();
	Profiler::end();

	if (size <= 0)
		return false;

	// setSize vor readBytes sorgt daf�r, dass wenn Paket l�nger als interner Buffer ist eine Exception geworfen wird
	readBuffer->getError(); // Fehler l�schen
	readBuffer->resetPosition();
	readBuffer->setSize(size);
	udp->readBytes(readBuffer->getBuffer(), size);

	yield();

	// Pakete ohne richtige Magic Value am Anfang werden ignoriert
	if (size < 3)
		return false;
	if (readBuffer->readUint8() != 'F' || readBuffer->readUint8() != 'L' || readBuffer->readUint8() != 'Y')
		return false;

	return true;
}

void NetworkManager::sendPacket(WiFiUDP* udp) {
	sendPacket(udp, udp->remoteIP(), udp->remotePort());
}

void NetworkManager::sendPacket(WiFiUDP* udp, IPAddress remote, uint16_t remotePort) {
	Profiler::begin("sendPacket()");
	if (!writeBuffer->getError()) {
		udp->beginPacket(remote, remotePort);
		udp->write(writeBuffer->getBuffer(), writeBuffer->getPosition());
		udp->endPacket();

		yield();
	}

	writeBuffer->resetPosition();
	Profiler::end();
}

void NetworkManager::writeHeader(WiFiUDP* udp, int32_t revision, ControlPacketType packetType) {
	writeBuffer->write('F');
	writeBuffer->write('L');
	writeBuffer->write('Y');
	writeBuffer->write(revision);
	writeBuffer->write(uint8_t(0)); // kein Ack anfordern
	writeBuffer->write(uint8_t(packetType));
}

void NetworkManager::writeDataHeader(WiFiUDP* udp, int32_t revision, DataPacketType packetType) {
	writeBuffer->write('F');
	writeBuffer->write('L');
	writeBuffer->write('Y');
	writeBuffer->write(revision);
	writeBuffer->write(uint8_t(packetType));
}


void NetworkManager::sendAck(WiFiUDP* udp, int32_t revision) {
	writeHeader(udp, revision, AckPacket);
	sendPacket(udp);
}

void NetworkManager::sendData(WiFiUDP* udp) {
	sendPacket(udp, _dataFeedSubscriptor, config->NetworkDataPort);
}

void NetworkManager::echoPacket(WiFiUDP* udp) {
	Profiler::begin("echoPacket()");
	if (!readBuffer->getError()) {
		udp->beginPacket(udp->remoteIP(), udp->remotePort());
		udp->write(readBuffer->getBuffer(), readBuffer->getSize());
		udp->endPacket();
		
		yield();
	}
	Profiler::end();
}

void NetworkManager::handleHello(WiFiUDP* udp) {
	if (readBuffer->getSize() < 4 || readBuffer->readUint8() != HelloQuestion)
		return;

	Log::debug("Network", "Received hello question");

	writeBuffer->write('F');
	writeBuffer->write('L');
	writeBuffer->write('Y');
	writeBuffer->write((uint8_t)HelloAnswer);

	writeBuffer->writeString(config->DroneName);
	writeBuffer->writeString(MODEL_NAME);

	char serialCode[32];
	getBuildSerialCode(serialCode, sizeof(serialCode));
	writeBuffer->writeString(serialCode);

	writeBuffer->write(uint8_t(BUILD_VERSION));

	sendPacket(udp);
}

void NetworkManager::handleControl(WiFiUDP* udp) {
	if (readBuffer->getSize() < 9)
		return;

	int32_t revision = readBuffer->readInt32();
	bool ackRequested = readBuffer->readBoolean();

	ControlPacketType type = static_cast<ControlPacketType>(readBuffer->readUint8());

#if VERBOSE_PACKET_LOG
	Log::debug("Network", "[Packet] %s, size %u, rev %d", getControlPacketName(type), readBuffer->getSize(), revision);
#endif
	
	if (ackRequested && type != DataOTA) // DataOTA sendet selber Ack
		sendAck(udp, revision);


	switch (type) {
	case MovementPacket: {
		if (!checkRevision(lastMovementRevision, revision))
			return;

		lastMovementRevision = revision;

		int16_t roll = readBuffer->readInt16();
		int16_t pitch = readBuffer->readInt16();
		int16_t yaw = readBuffer->readInt16();
		int16_t thrust = readBuffer->readInt16();

		if (readBuffer->getError())
			return;

		engine->setTargetMovement(roll, pitch, yaw, thrust);
		break;
	}
						 
	case RawSetPacket: {
		uint16_t fl = readBuffer->readUint16();
		uint16_t fr = readBuffer->readUint16();
		uint16_t bl = readBuffer->readUint16();
		uint16_t br = readBuffer->readUint16();

		if (readBuffer->getError())
			return;

		if (fl > config->ServoMax) {
			Log::error("Network", "[RawSetPacket] Invalid value for fl");
			return;
		}

		if (fr > config->ServoMax) {
			Log::error("Network", "[RawSetPacket] Invalid value for fr");
			return;
		}

		if (bl > config->ServoMax) {
			Log::error("Network", "[RawSetPacket] Invalid value for bl");
			return;
		}

		if (br > config->ServoMax) {
			Log::error("Network", "[RawSetPacket] Invalid value for br");
			return;
		}

		engine->setRawServoValues(fl, fr, bl, br);
		break;
	}
	case StopPacket:
		engine->stop(User);
		break;
	case ArmPacket:
		if (readBuffer->readUint8() == 'A' && readBuffer->readUint8() == 'R' && readBuffer->readUint8() == 'M') {
			bool arm = readBuffer->readBoolean();
			if (readBuffer->getError())
				return;

			if (arm)
				engine->arm();
			else
				engine->disarm();
		}
		break;
	case PingPacket:
		echoPacket(udp);

		engine->heartbeat();
		break;
	case BlinkPacket:
		blinkLED(1, 1000);
		break;
	case ResetRevisionPacket:
		lastMovementRevision = 0;
		lastOtaRevision = 0;
		break;

	case GetInfoPacket: {
		writeHeader(udp, revision, GetInfoPacket);

		writeBuffer->writeString(config->DroneName);
		writeBuffer->writeString(MODEL_NAME);

		char serialCode[32];
		getBuildSerialCode(serialCode, sizeof(serialCode));
		writeBuffer->writeString(serialCode);

		writeBuffer->writeString(BUILD_NAME);

		writeBuffer->write(uint8_t(BUILD_VERSION));

		rst_info* resetInfo = ESP.getResetInfoPtr();

		writeBuffer->write(uint8_t(resetInfo->reason));
		writeBuffer->write(uint8_t(resetInfo->exccause));
		writeBuffer->write(resetInfo->epc1);
		writeBuffer->write(resetInfo->epc2);
		writeBuffer->write(resetInfo->epc3);
		writeBuffer->write(resetInfo->excvaddr);
		writeBuffer->write(resetInfo->depc);

		writeBuffer->write(uint8_t(engine->getStopReason()));

		writeBuffer->writeString(sensor->getGyroName());
		writeBuffer->writeString(sensor->getMagnetometerName());
		writeBuffer->writeString(sensor->getBaroName());

		writeBuffer->write((uint8_t*)config, sizeof(Config));

		sendPacket(udp);
		break;
	}

	case SubscribeDataFeed:
		_dataFeedSubscriptor = udp->remoteIP();
		_dataFeedSubscribed = true;

		Log::debug("Network", "Client %s subscribed data", udp->remoteIP().toString().c_str());
		break;

	case UnsubscribeDataFeed:
		_dataFeedSubscribed = false;

		Log::debug("Network", "Client %s unsubscribed data", udp->remoteIP().toString().c_str());
		break;
	case CalibrateGyro:
		if (engine->state() == StateReset || engine->state() == StateStopped || engine->state() == StateIdle) {
			bool calibrateMagnet = readBuffer->readBoolean();
			if (readBuffer->getError())
				return;

			if (calibrateMagnet)
				sensor->getGyro()->beginCalibration(CalibrationMagnet);
			else
				sensor->getGyro()->beginCalibration(CalibrationGyro);
		}
		break;

	case Reset:
		if (engine->state() == StateReset || engine->state() == StateStopped || engine->state() == StateIdle)
			ESP.restart();
		break;
	case SetConfig: {
		if (readBuffer->getSize() - readBuffer->getPosition() != sizeof(Config)) {
			Log::error("Network", "[SetConfig] Packet size does not match config structure size");
			return;
		}
		readBuffer->read((uint8_t*)config, sizeof(Config));

		if (readBuffer->getError()) 
			return;

		Log::info("Network", "Config set");
		
		Log::setPrintToSerial(config->VerboseSerialLog);
		engine->updateTunings();

		saveConfig = config->SaveConfig;
		break;
	}
	case ClearStatus:
		engine->clearStatus();
		break;

	case BeginOTA: {
		if (!checkRevision(lastOtaRevision, revision))
			return;

		lastOtaRevision = revision;

		char* md5 = readBuffer->readString();
		uint32_t size = readBuffer->readUint32();

		if (readBuffer->getError())
			return;

		if (!engine->beginOTA()) {
			free(md5);
			return;
		}

		Log::info("Network", "OTA begin with size %u and md5 %s", size, md5);

		if (ESP.getFreeSketchSpace() < size) {
			Log::error("Network", "OTA begin failed (not enough free space)");
			engine->endOTA();
			free(md5);
			return;
		}

		if (!Update.begin(size, U_FLASH)) {
			Log::error("Network", "OTA begin failed");
			engine->endOTA();
			free(md5);
			return;
		}

		if (!Update.setMD5(md5))
			free(md5);

		break;
	}
	case DataOTA: {
		if (!checkRevision(lastOtaRevision, revision)) {
			sendAck(udp, revision);
			return;
		}

		lastOtaRevision = revision;

		if (engine->state() != StateOTA) {
			sendAck(udp, revision);
			return;
		}
		int32_t chunkSize = readBuffer->readInt32();
		uint8_t dataHash = readBuffer->readUint8();

		uint8_t* data = readBuffer->getBufferRegion(chunkSize);

		if (readBuffer->getError())
			return;

		uint8_t hash = 0;
		for (int32_t i = 0; i < chunkSize; i++) 
			hash ^= data[i];

		if (hash != dataHash) {
			Log::error("Network", "OTA data failed (wrong hash)");
			return;
		}

		Update.write(data, chunkSize);
		sendAck(udp, revision);
		break;
	}
	case EndOTA:
		if (!checkRevision(lastOtaRevision, revision))
			return;

		lastOtaRevision = revision;

		if (engine->state() == StateOTA) {
			if (Update.end(!readBuffer->readBoolean())) {
				Log::info("Network", "OTA update done");
				ESP.restart();
				return;
			}

			Log::info("Network", "OTA md5: %s", Update.md5String().c_str());
			Log::error("Network", "OTA update failed (%u)", Update.getError());

			engine->endOTA();
		}
		break;
	default: 
		Log::error("Network", "Unknown packet: %u", type);
		FaultManager::fault(FaultProtocol, "Network", "Invalid packet");
		break;
	}
}

void NetworkManager::handleData(WiFiUDP* udp) {
	if (!_dataFeedSubscribed)
		return;

	Profiler::begin("handleData()");
	sendDroneData(udp);
	sendLog(udp);
	sendDebugData(udp);
	Profiler::end();
}

void NetworkManager::sendDroneData(WiFiUDP* udp) {
	if (millis() - _lastDataSend >= CYCLE_DATA) {
		writeDataHeader(dataUDP, dataRevision++, DataDrone);

		writeBuffer->write(uint8_t(engine->state()));
		writeBuffer->write(uint16_t(servos->getFrontLeft()));
		writeBuffer->write(uint16_t(servos->getFrontRight()));
		writeBuffer->write(uint16_t(servos->getBackLeft()));
		writeBuffer->write(uint16_t(servos->getBackRight()));

		writeBuffer->write(sensor->getGyro()->inCalibration());

		writeBuffer->write(sensor->getGyro()->getRoll());
		writeBuffer->write(sensor->getGyro()->getPitch());
		writeBuffer->write(sensor->getGyro()->getYaw());

		GyroValues values = sensor->getGyro()->getValues();

		writeBuffer->write(values.GyroX);
		writeBuffer->write(values.GyroY);
		writeBuffer->write(values.GyroZ);

		writeBuffer->write(values.AccX);
		writeBuffer->write(values.AccY);
		writeBuffer->write(values.AccZ);

		writeBuffer->write(values.MagnetX);
		writeBuffer->write(values.MagnetY);
		writeBuffer->write(values.MagnetZ);

		writeBuffer->write(values.Temperature);

		BaroValues baroValues = sensor->getBaro()->getValues();
		writeBuffer->write(baroValues.Pressure);
		writeBuffer->write(baroValues.Humidity);
		writeBuffer->write(baroValues.Temperature);
		writeBuffer->write(sensor->getBaro()->getAltitude());

		writeBuffer->write(voltageReader->readVoltage());
		writeBuffer->write(WiFi.RSSI());

		sendData(udp);
		_lastDataSend = millis();
	}
}

void NetworkManager::sendLog(WiFiUDP* udp) {
	if (millis() - _lastLogSend > CYCLE_LOG) {
		if (Log::getBuffer() == NULL)
			return;

		while (Log::getBufferLines() > 0) {
			writeDataHeader(dataUDP, dataRevision++, DataLog);

			uint32_t messagesToSend = Log::getBufferLines();
			if (messagesToSend > 5)
				messagesToSend = 5;

			writeBuffer->write(messagesToSend);

			for (uint32_t i = 0; i < messagesToSend; i++) {
				char* msg = Log::popMessage();
				writeBuffer->writeString(msg);
				free(msg);
			}

			sendData(udp);
		}

		_lastLogSend = millis();
	}
}

void NetworkManager::sendDebugData(WiFiUDP* udp) {
	if (millis() - _lastDebugDataSend > CYCLE_DEBUG_DATA) {
		writeDataHeader(dataUDP, dataRevision++, DataDebug);

		writeBuffer->write(ESP.getFreeHeap());

		Profiler::write(writeBuffer);

		writeBuffer->write(engine->getPitchOutput());
		writeBuffer->write(engine->getRollOutput());
		writeBuffer->write(engine->getYawOutput());

		sendData(udp);
		_lastDebugDataSend = millis();
	}
}