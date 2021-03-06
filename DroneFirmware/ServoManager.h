#pragma once

#include <Arduino.h>

#include "Config.h"
#include "Log.h"
#include "MathHelper.h"
#include "Profiler.h"
#include "Servo.h"
#include "Model.h"


class ServoManager
{
protected:
	Config* config;

	bool attached;

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

	bool _dirty;

	int getValue(int servoValue);

	void internalAttach();

public:
	explicit ServoManager(Config* config);

	void attach();
	void detach();
	void waitForDetach();
	void waitForDetach(Servo servo);

	void setServos(int fl, int fr, int bl, int br);
	void setAllServos(int val);

	void handleTick();

	void calibrate();

	int getFrontLeft() const {
		return servoFLValue;
	}
	int getFrontRight() const {
		return servoFRValue;
	}
	int getBackLeft() const {
		return servoBLValue;
	}
	int getBackRight() const {
		return servoBRValue;
	}

	// Gibt zur�ck ob die Daten sich ge�ndert haben und setzt dann dirty wieder zur�ck
	bool dirty() {
		bool d = _dirty;
		_dirty = false;
		return d;
	}
};