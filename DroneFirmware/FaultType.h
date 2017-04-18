#pragma once

#include <Arduino.h>

enum FaultType {
	FaultUnknown,			// unbekannter Fehler
	FaultInternal,			// interner Fehler ohne besondere Quelle

	FaultException,			// Exception
	FaultBug,				// m�glicherwei�e ein Softwarebug
	FaultProtocol,			// Fehler im Protokoll
	FaultInvalidSensorData,	// falsche Sensor Daten

	FaultHardware			// Hardware Fehler
};