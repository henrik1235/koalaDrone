#include "ServoManager.h"

ServoManager::ServoManager(Config* config) {
	this->config = config;

	// prepare all pins
	pinMode(PIN_FRONT_LEFT, OUTPUT);
	pinMode(PIN_FRONT_RIGHT, OUTPUT);
	pinMode(PIN_BACK_LEFT, OUTPUT);
	pinMode(PIN_BACK_RIGHT, OUTPUT);
	digitalWrite(PIN_FRONT_LEFT, LOW);
	digitalWrite(PIN_FRONT_RIGHT, LOW);
	digitalWrite(PIN_BACK_LEFT, LOW);
	digitalWrite(PIN_BACK_RIGHT, LOW);

	servoFLValue = config->ServoMin;
	servoFRValue = config->ServoMin;
	servoBLValue = config->ServoMin;
	servoBRValue = config->ServoMin;

	attached = false;
	_dirty = true;
}

void ServoManager::internalAttach() {
	frontLeft.attach(PIN_FRONT_LEFT);
	frontRight.attach(PIN_FRONT_RIGHT);
	backLeft.attach(PIN_BACK_LEFT);
	backRight.attach(PIN_BACK_RIGHT);
	attached = true;
	yield();
}

void ServoManager::attach() {
	if (attached)
		return;

	Log::info("Servo", "attach()");
	setAllServos(config->ServoMin);
	internalAttach();
}

void ServoManager::detach() {
	if (!attached)
		return;

	Log::info("Servo", "detach()");

	setAllServos(config->ServoMin);

	frontLeft.detach();
	frontRight.detach();
	backLeft.detach();
	backRight.detach();

	attached = false;
	yield();
}

void ServoManager::waitForDetach() {
	Profiler::begin("ServoManager::waitForDetach()");
	if (attached)
		detach();

	delayMicroseconds(SERVO_REFRESH_INTERVAL);
	waitForDetach(frontLeft);
	waitForDetach(frontRight);
	waitForDetach(backLeft);
	waitForDetach(backRight);
	Profiler::end();
}

void ServoManager::waitForDetach(Servo servo) {
	uint32_t start = millis();
	while (millis() - start < 1000 && servo.attached()) 
		yield();
}

void ServoManager::handleTick() {
	if (!attached)
		return;

	int value = config->ServoMin;
	
	// alle 1000 Millisekunden f�r 150 Millisekunden kurz Motor drehen
	if (millis() % 1000 < 150)
		value = config->ServoIdle;

	if (servoFLValue == 1)
		frontLeft.writeMicroseconds(value);
	if (servoFRValue == 1)
		frontRight.writeMicroseconds(value);
	if (servoBLValue == 1)
		backLeft.writeMicroseconds(value);
	if (servoBRValue == 1)
		backRight.writeMicroseconds(value);
}

void ServoManager::calibrate() {
	if (attached)
		return;

	// wait 5000 ms until ESCs are ready
	delay(5000);

	// set values to maximum
	frontLeft.writeMicroseconds(config->ServoMax);
	frontRight.writeMicroseconds(config->ServoMax);
	backLeft.writeMicroseconds(config->ServoMax);
	backRight.writeMicroseconds(config->ServoMax);
	// attach them
	internalAttach();

	// wait 2000 ms until ESCs set the max value
	delay(2000);

	// set the minimum value and wait 2000 ms again
	setAllServos(config->ServoMin);
	delay(2000);
}

int ServoManager::getValue(int value) {
	if (value == 1)
		return value;

	value = MathHelper::clampValue(value, config->ServoMin, config->ServoMax);
	if (value > config->SafeServoValue)
		return config->SafeServoValue;
	return value;
}

void ServoManager::setServos(int fl, int fr, int bl, int br) {
	servoFLValue = getValue(fl);
	servoFRValue = getValue(fr);
	servoBLValue = getValue(bl);
	servoBRValue = getValue(br);

	_dirty = true;

	if (servoFLValue != 1)
		frontLeft.writeMicroseconds(servoFLValue);
	if (servoFRValue != 1)
		frontRight.writeMicroseconds(servoFRValue);
	if (servoBLValue != 1)
		backLeft.writeMicroseconds(servoBLValue);
	if (servoBRValue != 1)
		backRight.writeMicroseconds(servoBRValue);
}


void ServoManager::setAllServos(int val) {
	setServos(val, val, val, val);
}