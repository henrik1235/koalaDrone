// DroneEngine.h

#ifndef _DRONEENGINE_h
#define _DRONEENGINE_h


#ifdef _VSARDUINO_H_ //Kompatibilitšt mit visual micro
#include "arduino.h"
#include "ServoManager.h"
#include "Gyro.h"
#include "MathHelper.h"
#include "Log.h"

#define byte unsigned char
#else
#include "arduino.h"
#include "ServoManager.h"
#include "Gyro.h"
#include "MathHelper.h"
#include "Log.h"
#endif

#define PHYSICS_CALC_DELAY_MS 20

enum DroneState {
	State_Idle,
	State_Armed,
	State_Flying
};

class DroneEngine
{
 protected:
	 Config* config;
	 long lastPhysicsCalc;
	 long lastYawTargetCalc;
	 long lastMovementUpdate;
	 long maxMovementUpdateInterval = 200;

	 DroneState _state;

	 Gyro* gyro;
	 ServoManager* servos;

	 float maxTilt;
	 float maxRotationSpeed;

	 float targetVerticalSpeed;
	 float targetPitch;
	 float targetRoll;
	 float targetYaw;
	 float targetRotationSpeed;

	 float frontLeftRatio;
	 float frontRightRatio;
	 float backLeftRatio;
	 float backRightRatio;

 public:
	explicit DroneEngine(Gyro* gyro, ServoManager* servos, Config* config);

	void arm();
	void disarm();
	void stop();

	DroneState state() const;
	
	void handle();

	void setRawServoValues(int fl, int fr, int bl, int br, bool forceWrite = false) const;
	void setRawServoValues(int all, bool forceWrite = false) const;

	void setMaxTilt(float tilt);
	void setMaxRotationSpeed(float rotaionSpeed);

	float getMaxTilt() const;
	float getMaxRotationSpeed() const;

	void setTargetMovement(float pitch, float roll, float yaw);
	void setTargetPitch(float pitch);
	void setTargetRoll(float roll);
	void setTargetRotarySpeed(float yaw);
	void setTargetVerticalSpeed(float vertical);

	float getTargetPitch() const;
	float getTargetRoll() const;
	float getTargetYaw() const;
	float getTargetRotarySpeed() const;
	float getTargetVerticalSpeed() const;

	float getFrontLeftRatio() const;
	float getFrontRightRatio() const;
	float getBackLeftRatio() const;
	float getBackRightRatio() const;
};

#endif

