#include "EEPROM_MemoryAdapter.h"

#ifdef HARDWARE_ESP8266
EEPROM_MemoryAdapter::EEPROM_MemoryAdapter(size_t size, uint16_t offset) {
	this->size = size;
	this->offset = offset;
}

bool EEPROM_MemoryAdapter::assertAddress(uint32_t address, size_t length) {
	if (address >= size) {
		Log::error("EEPROM", "Invalid value for address");
		return false;
	}
	if (address + length > size) {
		Log::error("EEPROM", "Invalid value for length");
		return false;
	}
	return true;
}

bool EEPROM_MemoryAdapter::begin() {
	EEPROM.begin(size);
	return true;
}

bool EEPROM_MemoryAdapter::end() {
	EEPROM.end();
	return true;
}

void EEPROM_MemoryAdapter::writeByte(uint32_t address, uint8_t val) {
	if (assertAddress(address, sizeof(uint8_t)))
		EEPROM.write(offset + address, val);
}

uint8_t EEPROM_MemoryAdapter::readByte(uint32_t address) {
	if (assertAddress(address, sizeof(uint8_t)))
		return EEPROM.read(offset + address); 
	return 0;
}

void EEPROM_MemoryAdapter::read(uint32_t address, uint8_t* data, size_t length) {
	if (assertAddress(address, length))
		memcpy(data, EEPROM.getDataPtr() + offset + address, length);
	else
		memset(data, 0, length); // Daten mit 0 bef�llen, damit der Leser bei Fehlern keine zuf�llige Werte bekommt
}

void EEPROM_MemoryAdapter::write(uint32_t address, uint8_t* data, size_t length) {
	if (assertAddress(address, length))
		memcpy(EEPROM.getDataPtr() + offset + address, data, length);
}
#endif