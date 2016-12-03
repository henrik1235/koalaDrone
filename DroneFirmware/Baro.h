#ifndef _BARO_h
#define _BARO_h

#include "arduino.h"
#include "MathHelper.h"
#include "SensorCalibration.h"
#include "Log.h"
#include "Profiler.h"

struct BaroValues {
	float Temperature;
	float Pressure;
	float Humidity;
};

class SensorHAL;

class Baro
{
private:
	bool firstSample;
	uint32_t lastSample;
	BaroValues last;

	BaroValues values;
	float altitude;

protected:
	Config* config;
	SensorCalibration* calibration;

	virtual bool getValues(BaroValues* values) = 0;

public:
	explicit Baro(Config* config);
	virtual ~Baro();

	virtual const char* name() = 0;

	virtual bool init() = 0;
	virtual void reset() = 0;

	void update();

	BaroValues getValues() const;
	float getAltitude() const;
};

#endif

