#pragma once

#include <Arduino.h>

class MemoryAdapter {
protected:
	explicit MemoryAdapter();

public:
	virtual ~MemoryAdapter();

	virtual bool begin() = 0;
	virtual void writeByte(uint32_t address, uint8_t val) = 0;
	virtual void write(uint32_t address, uint8_t* data, size_t length) = 0;
	virtual uint8_t readByte(uint32_t address) = 0;
	virtual void read(uint32_t address, uint8_t* data, size_t length) = 0;
	virtual bool end() = 0;
};