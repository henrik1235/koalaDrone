#include "ConfigManager.h"

Config ConfigManager::loadConfig() {
	EEPROM_MemoryAdapter* adapter = new EEPROM_MemoryAdapter(1024, 64);

	adapter->begin();
	Config config = loadConfig(adapter);
	adapter->end();

	delete adapter;
	return config;
}

Config ConfigManager::loadConfig(MemoryAdapter* memory) {
	// wir nutzen erstes Byte um zu Erkennen ob schon Daten geschrieben wurden
	if (memory->readByte(0) != CONFIG_MAGIC_VALUE) {
		Log::info("Config", "Saved magic value does not match excepted magic value");
		return getDefault();
	}

	if (memory->readByte(1) != CONFIG_VERSION) {
		Log::info("Config", "Saved config version does not match excepted version");
		return getDefault();
	}

	// nach Magic Value folgt ein uint16_t f�r die Gr��e der Config
	uint8_t buffer[sizeof(uint16_t)];
	memory->read(2, buffer, sizeof(buffer));

	uint16_t size = BinaryHelper::readUint16(buffer, 0);

	// �ber die Gr��e erkennen wir ob sich die Structure ge�ndert hat
	if (size != sizeof(Config)) {
		Log::info("Config", "Config size does not match saved size");
		return getDefault();
	}

	// nach der Gr��e folgen unsere eigentliche Daten
	Config* config = (Config*)malloc(sizeof(Config));
	memory->read(4, (byte*)config, sizeof(Config));

	Log::info("Config", "Config loaded");
	return *config;
}

void ConfigManager::saveConfig(const Config config) {
	Profiler::begin("saveConfig()");
	EEPROM_MemoryAdapter* adapter = new EEPROM_MemoryAdapter(1024, 64);

	adapter->begin();
	saveConfig(adapter, config);
	adapter->end();

	yield();

	delete adapter;
	Profiler::end();
}

void ConfigManager::saveConfig(MemoryAdapter* memory, const Config config) {
	// Magic Value speichern
	memory->writeByte(0, CONFIG_MAGIC_VALUE);

	memory->writeByte(1, CONFIG_VERSION);

	// Gr��e der Config Structure speichern
	uint8_t buffer[sizeof(uint16_t)];
	BinaryHelper::writeUint16(buffer, 0, sizeof(Config));
	memory->write(2, buffer, sizeof(buffer));

	// eigentliche Daten speichern
	memory->write(4, (byte*)(&config), sizeof(Config));

	Log::info("Config", "Config saved");
}

Config ConfigManager::getDefault() {
	Config config;

	strncpy(config.DroneName, "koalaDrone", sizeof(config.DroneName));
	config.SaveConfig = true;

	strncpy(config.NetworkSSID, "", sizeof(config.NetworkSSID));
	strncpy(config.NetworkPassword, "", sizeof(config.NetworkPassword));

	strncpy(config.AccessPointPassword, "12345678", sizeof(config.AccessPointPassword));

	config.NetworkHelloPort = 4710;
	config.NetworkControlPort = 4711;
	config.NetworkDataPort = 4712;
	config.NetworkPacketBufferSize = 1024;
	config.MaximumNetworkTimeout = 1500;

	config.VerboseSerialLog = true;
	config.MaxTemperature = 60;

	config.ServoMin = 1000;
	config.ServoMax = 2000;
	config.ServoIdle = 1040;

	config.PinFrontLeft = 12;
	config.PinFrontRight = 13;
	config.PinBackLeft = 16;
	config.PinBackRight = 14;
	config.PinLed = 0;

	config.PitchPid.Kp = 1.3f;
	config.PitchPid.Ki = 0.0002f;
	config.PitchPid.Kd = 0.06f;

	config.RollPid.Kp = 1.3f;
	config.RollPid.Ki = 0.0002f;
	config.RollPid.Kd = 0.06f;

	config.YawPid.Kp = 4.0f;
	config.YawPid.Ki = 0.00008f;
	config.YawPid.Kd = 0.0f;

	config.SafePitch = 100;
	config.SafeRoll = 100;
	config.SafeServoValue = 1850;

	config.EnableStabilization = false;
	config.NegativeMixing = true;

	config.MaxThrustForFlying = 50;
	config.OnlyArmWhenStill = false;

	config.AngleStabilization.Kp = 5.0f;
	config.AngleStabilization.Ki = 0;
	config.AngleStabilization.Kd = 0;

	config.EnableImuAcc = true;
	config.EnableImuMag = false;
	config.GyroFilter = 0.02f;
	config.AccFilter = 0.2f;
	
	config.CalibrateServos = false;
	memset(&config.SensorCalibrationData, 0, sizeof(SensorCalibration));

	Log::info("Config", "Using default config");
	return config;
}
