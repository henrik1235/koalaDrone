#include "DroneEngine.h"

DroneEngine::DroneEngine(SensorHAL* sensor, ServoManager* servos, Config* config) {
	this->sensor = sensor;
	this->servos = servos;
	this->config = config;

	this->lastHeartbeat = 0;
	this->lastMovementUpdate = 0;

	this->pitchPID = NULL;
	this->rollPID = NULL;
	this->yawPID = NULL;

	this->anglePitchPID = NULL;
	this->angleRollPID = NULL;
	this->angleYawPID = NULL;

	createPID();

	_state = StateReset;
	_stopReason = None;
	servos->setAllServos(config->ServoMin);
}

void DroneEngine::createPID() {
	if (rollPID)
		delete rollPID;
	if (pitchPID)
		delete pitchPID;
	if (yawPID)
		delete yawPID;
	if (angleRollPID)
		delete angleRollPID;
	if (anglePitchPID)
		delete anglePitchPID;
	if (angleYawPID)
		delete angleYawPID;

	rollPID = createPID(config->RollPid, 200, &rollOutput);
	pitchPID = createPID(config->PitchPid, 200, &pitchOutput);
	yawPID = createPID(config->YawPid, 300, &yawOutput);

	angleRollPID = createPID(config->AngleRoll, 200, &angleRollOutput);
	anglePitchPID = createPID(config->AnglePitch, 200, &anglePitchOutput);
	angleYawPID = createPID(config->AngleYaw, 20, &angleYawOutput);
}

PID* DroneEngine::createPID(PIDSettings settings, double limit, double* output) {
	PID* pid = new PID(&pidInput, output, &pidSetpoint, settings.Kp, settings.Ki, settings.Kd, DIRECT);
	pid->SetMode(AUTOMATIC);
	pid->SetSampleTime(CYCLE_PID);
	pid->SetOutputLimits(-limit, limit);
	return pid;
}

bool DroneEngine::isGyroSafe() {
	return abs(sensor->getIMU()->getRoll()) <= config->SafeRoll && abs(sensor->getIMU()->getPitch()) <= config->SafePitch;
}

void DroneEngine::arm() {
	if (_state == StateIdle) {
		if (!sensor->getGyro()->isCalibrated()) {
			blinkLED(5, 300);
			Log::info("Engine", "Can not arm motors, gyro is not calibrated");
			return;
		}
		if (!sensor->getIMU()->isCalibrated()) {
			blinkLED(5, 300);
			Log::info("Engine", "Can not arm motors, imu is not calibrated");
			return;
		}
		if (!sensor->getGyro()->isDataOK()) {
			blinkLED(5, 300);
			Log::info("Engine", "Can not arm motors, gyro data is not valid");
			return;
		}
		if (!isGyroSafe()) {
			blinkLED(5, 300);
			Log::info("Engine", "Can not arm motors, gyro is not safe");
			return;
		}
		if (config->OnlyArmWhenStill) {
			bool still = true;
			if (sensor->getGyro()->isMoving()) {
				Log::info("Engine", "Can not arm motors, not still (moving)");
				still = false;
			}
			if (!sensor->getIMU()->isFlat()) {
				Log::info("Engine", "Can not arm motors, not still (not flat)");
				still = false;
			}

			if (!still) {
				blinkLED(5, 300);
				return;
			}
		}

		servos->setAllServos(config->ServoMin);
		_state = StateArmed;
		blinkLED(3, 600);

		Log::info("Engine", "Armed motors");
	}
}

void DroneEngine::disarm() {
	if (areMotorsRunning()) {
		servos->setAllServos(config->ServoMin);

		_state = StateIdle;
		Log::info("Engine", "Disarmed motors");
	}
}

void DroneEngine::fly() {
	// entweder schon im Flying Zustand
	// oder nicht Armed
	if (_state != StateArmed)
		return;

	if (thrust > config->MaxThrustForFlying)
		return;

	rollOutput = 0;
	pitchOutput = 0;
	yawOutput = 0;

	angleRollOutput = 0;
	anglePitchOutput = 0;
	angleYawOutput = 0;

	createPID();

	_state = StateFlying;
	_stopReason = None;

	blinkLED(3, 550);
	Log::info("Engine", "Flying");
}

void DroneEngine::stop(StopReason reason) {
	if (_state == StateOTA)
		return;

	disarm();
	blinkLED(5, 250);

	_stopReason = reason;
	_state = StateStopped;
	Log::info("Engine", "Stopped! reason: %d", reason);
}

void DroneEngine::clearStatus() {
	if (_state == StateReset || _state == StateStopped) {
		_state = StateIdle;
		Log::info("Engine", "Status cleared");
	}
}

bool DroneEngine::beginOTA() {
	if (_state != StateFlying) {
		disarm();

		_state = StateOTA;
		Log::info("Engine", "Now in OTA state");
		return true;
	}
	return false;
}

void DroneEngine::endOTA() {
	if (_state == StateOTA) {
		_state = StateIdle;
		Log::info("Engine", "Stopped OTA state");
	}
}

void DroneEngine::handle() {
	Profiler::begin("DroneEngine::handle()");

	if (areMotorsRunning()) {
		blinkLED(1, 800);

		if (millis() - lastHeartbeat >= config->MaximumNetworkTimeout) {
			stop(NoPing);
			Profiler::end();
			return;
		}

		if (!config->IgnoreSafeOrientationWhileFlying && !isGyroSafe()) {
			stop(InvalidGyro);
			Profiler::end();
			return;
		}

		if (_state == StateFlying) {
			if (millis() - lastMovementUpdate >= config->MaximumNetworkTimeout) {
				stop(NoData);
				Profiler::end();
				return;
			}

			handleInternal();
		}
	}
	Profiler::end();
}

void DroneEngine::handleInternal() {
	int values[4];

	GyroValues gyroValues = sensor->getGyro()->getValues();

	const float cmdScale = (1.0f / 500.0f);
	float sensitivity = cmdScale * config->InputScale;

	float rollCmd = targetGyroX * sensitivity;
	float pitchCmd = targetGyroY * sensitivity;
	float yawCmd = targetGyroZ * sensitivity;

	if (config->EnableStabilization) {
		if (sensor->getIMU()->isDataOK()) {
			float angleSensivitiy = cmdScale * config->StabInputScale;

			// Reset new targetYaw
			if (yawCmd != 0 || thrust <= config->MaxThrustForFlying)
				targetYaw = sensor->getIMU()->getYaw();

			calculatePID(angleRollPID, sensor->getIMU()->getRoll(), targetGyroX * angleSensivitiy);
			calculatePID(anglePitchPID, sensor->getIMU()->getPitch(), targetGyroY * angleSensivitiy);
			calculatePID(angleYawPID, MathHelper::angleDifference(sensor->getIMU()->getYaw(), targetYaw), 0);

			rollCmd = angleRollOutput;
			pitchCmd = anglePitchOutput;

			// Only stabilise yaw when no cmd is given
			if (yawCmd == 0)
				yawCmd = angleYawOutput;
		}
		else
			FaultManager::fault(FaultInvalidSensorData, "DroneEngine", "hasValidImuData()");
	}

	// Add trims
	rollCmd += config->RollTrim * sensitivity;
	pitchCmd += config->PitchTrim * sensitivity;
	yawCmd += config->YawTrim * sensitivity;


	if (sensor->getGyro()->isDataOK()) {
		calculatePID(rollPID, gyroValues.GyroX, rollCmd);
		calculatePID(pitchPID, gyroValues.GyroY, pitchCmd);
		calculatePID(yawPID, gyroValues.GyroZ, yawCmd);
	}
	else {
		FaultManager::fault(FaultInvalidSensorData, "DroneEngine", "hasValidGyroData()");

		rollOutput = 0;
		pitchOutput = 0;
		yawOutput = 0;
	}

	for (int i = 0; i < 4; i++) {
		if (thrust > config->MaxThrustForFlying) {
			int value = (int)(config->ServoMin + thrust + Quadcopter::mixMotor(config, i, rollOutput, pitchOutput, yawOutput));
			values[i] = _max(config->ServoIdle, value);
		}
		else
			values[i] = config->ServoMin;
	}	

	servos->setServos(values[0], values[1], values[2], values[3]);

	if (memcmp(values, lastValues, sizeof(lastValues)) != 0)
	{
		Profiler::restart("DroneEngine::data()");
		memcpy(lastValues, values, sizeof(lastValues));
	}
}

void DroneEngine::updateTunings() {
	updateTuning(rollPID, config->RollPid);
	updateTuning(pitchPID, config->PitchPid);
	updateTuning(yawPID, config->YawPid);

	updateTuning(angleRollPID, config->AngleRoll);
	updateTuning(anglePitchPID, config->AnglePitch);
	updateTuning(angleYawPID, config->AngleYaw);
}

void DroneEngine::updateTuning(PID* pid, PIDSettings settings) {
	pid->SetTunings(settings.Kp, settings.Ki, settings.Kd);
}

void DroneEngine::calculatePID(PID* pid, float input, float setpoint) {
	this->pidInput = input;
	this->pidSetpoint = setpoint;
	pid->Compute();
}


void DroneEngine::setRawServoValues(int fl, int fr, int bl, int br) const {
	if(_state == StateArmed)
		servos->setServos(fl, fr, bl, br);
}

void DroneEngine::setRawServoValues(int all) const {
	setRawServoValues(all, all, all, all);
}

void DroneEngine::heartbeat() {
	lastHeartbeat = millis();
}

DroneState DroneEngine::state() const {
	return _state;
}

StopReason DroneEngine::getStopReason() const {
	return _stopReason;
}

bool DroneEngine::isStateIdle() const {
	return state() == StateReset || state() == StateStopped || state() == StateIdle;
}

bool DroneEngine::areMotorsRunning() const {
	return state() == StateArmed || state() == StateFlying;
}

void DroneEngine::setTargetMovement(int16_t roll, int16_t pitch, int16_t yaw, int16_t thrust) {
	if (!areMotorsRunning())
		return;

	// Werte in richtigen Bereich bringen und setzen
	this->targetGyroX = MathHelper::clampValue(roll, -500, 500);
	this->targetGyroY = MathHelper::clampValue(pitch, -500, 500);
	this->targetGyroZ = MathHelper::clampValue(yaw, -500, 500);
	this->thrust = MathHelper::clampValue(thrust, 0, config->ServoMax - config->ServoMin);

	// in den Fliegen Modus gehen
	fly();
	lastMovementUpdate = millis();

	Profiler::restart("DroneEngine::input()");
}

int DroneEngine::getTargetGyroX() const {
	return targetGyroX;
}

int DroneEngine::getTargetGyroY() const {
	return targetGyroY;
}

int DroneEngine::getTargetGyroZ() const {
	return targetGyroZ;
}

int DroneEngine::getThrust() const {
	return thrust;
}

float DroneEngine::getRollOutput() const {
	return rollOutput;
}

float DroneEngine::getPitchOutput() const {
	return pitchOutput;
}

float DroneEngine::getYawOutput() const {
	return yawOutput;
}

float DroneEngine::getAngleRollOutput() const
{
	return angleRollOutput;
}

float DroneEngine::getAnglePitchOutput() const
{
	return anglePitchOutput;
}

float DroneEngine::getAngleYawOutput() const
{
	return angleYawOutput;
}