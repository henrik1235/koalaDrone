#ifndef _GYRO9250_h
#define _GYRO9250_h

#include "Build.h"
#include "Gyro.h"
#include "CycleTimes.h"

#include <Wire.h>
#include <I2Cdev.h>
#include <MPU9250.h>

class Gyro9250 : public Gyro
{
protected:
	bool mpuOK;

	MPU9250 mpu;
	double accRes;
	double gyroRes;
	double magRes;

	void getValues(GyroValues* values);
public:
	explicit Gyro9250(Config* config);

	char* name();
	char* magnetometerName();

	bool init();
	void reset();

	float getTemperature();

	bool hasMagnetometer() const;
	bool hasIMU() const;
};

#endif