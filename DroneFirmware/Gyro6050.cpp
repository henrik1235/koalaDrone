#include "Gyro6050.h"

Gyro6050::Gyro6050(SensorHAL* hal, Config* config) : Gyro(hal, config) {
	mpuOK = false;
}

const char* Gyro6050::getName() const {
	return "InvenSense MPU-6050";
}

const char* Gyro6050::getShortName() const {
	return "Gyro6050";
}

const char* Gyro6050::getMagnetometerName() const {
	return "";
}

boolean Gyro6050::isHardwareBased() const
{
	return true;
}

boolean Gyro6050::init() {
	Log::info("Gyro6050", "init()");

	if (!mpu.testConnection()) {
		Log::error("Gyro6050", "Connection failed");
		mpuOK = false;
		return mpuOK;
	}

	mpuOK = true;

	Log::debug("Gyro6050", "mpu.reset()");
	mpu.reset();
	mpu.resetSensors();

	Log::debug("Gyro6050", "mpu.initialize()");
	mpu.initialize();

	setSettings();

	Log::info("Gyro6050", "mpu.selfTest()");
	float result[6];
	if (mpu.selfTest(result))
		Log::info("Gyro6050", "Self test: PASSED");
	else {
		Log::error("Gyro6050", "Self test: FAIL");
		mpuOK = !config->IgnoreGyroSelfTest;
	}
	Log::debug("Gyro6050", "Self test result...");
	Log::debug("Gyro6050", "ax: %.2f %%, ay: %.2f %%, az: %.2f %%", result[0], result[1], result[2]);
	Log::debug("Gyro6050", "gx: %.2f %%, gy: %.2f %%, gz: %.2f %%", result[3], result[4], result[5]);

	Log::info("Gyro6050", "done with init");
	return mpuOK;
}

boolean Gyro6050::disable()
{
	return true;
}

bool Gyro6050::setSettings() {
	mpu.setClockSource(MPU6050_CLOCK_PLL_ZGYRO);
	mpu.setFullScaleAccelRange(MPU6050_ACCEL_FS_8);
	mpu.setFullScaleGyroRange(MPU6050_GYRO_FS_1000);
	if (config->GyroDLPF > MPU6050_DLPF_BW_5) {
		Log::error("Gyro6050", "Config.GyroDLPF is invalid (%d)", config->GyroDLPF);
		mpu.setDLPFMode(MPU6050_DLPF_BW_42);
	}
	else
		mpu.setDLPFMode(config->GyroDLPF);

	double accRange[4] = { 2, 4, 8, 16 }; // g
	double gyroRange[4] = { 250, 500, 1000, 2000 }; // degress/s

	accRes = accRange[mpu.getFullScaleAccelRange()] / 32768.0;
	gyroRes = gyroRange[mpu.getFullScaleGyroRange()] / 32768.0;
	return true;
}

bool Gyro6050::getValues(GyroValues* values) {
	if (!mpuOK)
		return false;

	Profiler::begin("Gyro6050::getValues()");

	if (mpu.getFullScaleGyroRange() != MPU6050_GYRO_FS_1000) {
		Log::error("Gyro6050", "FullScaleGyroRange set on chip does not match desired setting");
		FaultManager::fault(FaultHardware, "Gyro6050", "getFullScaleGyroRange()");
		setSettings();
		Profiler::end();
		return false;
	}

	int16_t ax, ay, az;
	int16_t gx, gy, gz;
	if (!mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz)) {
		FaultManager::fault(FaultHardware, "Gyro6050", "getMotion6()");
		Profiler::end();
		return false;
	}
	yield();

	values->AccX = ax * accRes;
	values->AccY = ay * accRes;
	values->AccZ = az * accRes;

	values->GyroX = gx * gyroRes;
	values->GyroY = gy * gyroRes;
	values->GyroZ = gz * gyroRes;

	values->MagnetX = 0;
	values->MagnetY = 0;
	values->MagnetZ = 0;

	values->Temperature = mpu.getTemperature() / 340.0f + 36.53f;
	yield();

	Profiler::end();
	return true;
}

bool Gyro6050::hasMagnetometer() const {
	return false;
}

bool Gyro6050::hasIMU() const {
	return false;
}