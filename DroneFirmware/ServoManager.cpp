// 
// 
// 

#include "ServoManager.h"

ServoManager::ServoManager(Config* config) {
	this->config = config;

	servoFLValue = config->ServoMin;
	servoFRValue = config->ServoMin;
	servoBLValue = config->ServoMin;
	servoBRValue = config->ServoMin;
}


void ServoManager::init(int pinFL, int pinFR, int pinBL, int pinBR) {
	frontLeft.attach(pinFL);
	frontRight.attach(pinFR);
	backLeft.attach(pinBL);
	backRight.attach(pinBR);

	setAllServos(config->ServoMin);
}

void ServoManager::setServos(int fl, int fr, int bl, int br, bool forceWrite) {
	servoFLValue = forceWrite ? fl : MathHelper::clampValue(fl, config->ServoMin, config->ServoMax);
	servoFRValue = forceWrite ? fr : MathHelper::clampValue(fr, config->ServoMin, config->ServoMax);
	servoBLValue = forceWrite ? bl : MathHelper::clampValue(bl, config->ServoMin, config->ServoMax);
	servoBRValue = forceWrite ? br : MathHelper::clampValue(br, config->ServoMin, config->ServoMax);

	frontLeft.writeMicroseconds(servoFLValue);
	frontRight.writeMicroseconds(servoFRValue);
	backLeft.writeMicroseconds(servoBLValue);
	backRight.writeMicroseconds(servoBRValue);

	if(config->VerboseSerialLog) {
		Serial.print("$ Set Servos to: ");
		Serial.print(servoFLValue);
		Serial.print(", ");
		Serial.print(servoFRValue);
		Serial.print(", ");
		Serial.print(servoBLValue);
		Serial.print(", ");
		Serial.println(servoBRValue);
	}
}


void ServoManager::setAllServos(int val, bool forceWrite) {
	setServos(val, val, val, val, forceWrite);
}

void ServoManager::setRatio(float fl, float fr, float bl, float br) {
	int targetFL = MathHelper::mapRatio(fl, config->ServoMin, config->ServoMax, config->ServoHover);
	int targetFR = MathHelper::mapRatio(fr, config->ServoMin, config->ServoMax, config->ServoHover);
	int targetBL = MathHelper::mapRatio(bl, config->ServoMin, config->ServoMax, config->ServoHover);
	int targetBR = MathHelper::mapRatio(br, config->ServoMin, config->ServoMax, config->ServoHover);

	setServos(targetFL, targetFR, targetBL, targetBR);
}

void ServoManager::setRationAll(float ratio) {
	setRatio(ratio, ratio, ratio, ratio);
}


void ServoManager::armMotors() {
	if(!isArmed()) {
		setAllServos(config->ServoIdle);
		_isArmed = true;
	}
}

void ServoManager::disarmMotors() {
	setAllServos(config->ServoMin);
	_isArmed = false;
}