#pragma once

#include <Arduino.h>

#include "PacketBuffer.h"
#include "CycleTimes.h"

#define PROFILE_SIZE 48

struct ProfilerFunction {
	uint32_t index;
	char* name;
	uint32_t lastTime;
	uint32_t time;
	uint32_t maxTime;

	bool shouldReset;
	uint32_t currentTime;
};

class Profiler 
{
private:
	static ProfilerFunction* functions;
	static uint32_t length;

	static uint32_t* stack;
	static uint32_t stackCurrent;

	static long lastMaxReset;

	static ProfilerFunction* findFunction(const char* name);
	static uint32_t start(const char* name, bool reset);
	static void stop(ProfilerFunction* function, bool add);

public:
	static void init();

	static uint32_t start(const char* name);
	static void stop(const char* name);

	static void begin(const char* name);
	static void end();

	static void restart(const char* name);

	static void finishFrame();
	static void pushData(const char* name, uint32_t value);
	static void write(PacketBuffer* buffer);
};