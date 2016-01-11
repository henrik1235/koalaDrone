// ServoManager.h

#ifndef _SERVOMANAGER_h
#define _SERVOMANAGER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

#ifdef _VSARDUINO_H_ //Kompatibilit�t mit visual micro
#include <Servo/src/Servo.h>
#include "MathHelper.h"

#define byte unsigned char
#else
#include <Servo.h>
#include "MathHelper.h"
#endif

class ServoManager
{
 protected:
	 bool debug_output;

	 int servoOffValue;
	 int servoIdleValue;
	 int servoHoverValue;
	 int servoMaxValue;

	 // The values for the Servos
	 int servoFLValue;
	 int servoFRValue;
	 int servoBLValue;
	 int servoBRValue;

	 // The objects to control the Servos (ESCs)
	 Servo frontLeft;
	 Servo frontRight;
	 Servo backLeft;
	 Servo backRight;

	 bool _isArmed = false;

 public:
	 explicit ServoManager(int offValue, int idleValue,int hoverValue, int maxValue, bool debug_output);

	void init(int pinFL, int pinFR, int pinBL, int pinBR);
	void setServos(int fl, int fr, int bl, int br, bool forceWrite = false);
	void setAllServos(int val, bool forceWrite = false);
	void setRatio(float fl, float fr, float bl, float br);
	void setRationAll(float ratio);
	void armMotors();
	void disarmMotors();

	bool isArmed() const {
		return _isArmed;
	}
	int FL() const {
		return servoFLValue;
	}
	int FR() const {
		return servoFRValue;
	}
	int BL() const {
		return servoBLValue;
	}
	int BR() const {
		return servoBRValue;
	}
};


#endif
