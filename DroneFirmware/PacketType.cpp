#include "PacketType.h"

const char* getHelloPacketName(HelloPacketType type) {
	switch (type) {
	case HelloQuestion:
		return "Question";
	case HelloAnswer:
		return "Answer";
	}
	return "Unknown";
}

const char* getControlPacketName(ControlPacketType type) {
	switch (type) {
	case MovementPacket:
		return "Movement";
	case StopPacket:
		return "Stop";
	case ArmPacket:
		return "Arm";
	case BlinkPacket:
		return "Blink";
	case RawSetPacket:
		return "RawSet";
	case AckPacket:
		return "Ack";
	case PingPacket:
		return "Ping";
	case ResetRevisionPacket:
		return "ResetRevision";
	case GetInfoPacket:
		return "GetInfo";
	case SubscribeDataFeed:
		return "SubscribeData";
	case UnsubscribeDataFeed:
		return "UnsubscribeData";
	case CalibrateGyro:
		return "CalibrateGyro";
	case Reset:
		return "Reset";
	case SetConfig:
		return "SetConfig";
	case ClearStatus:
		return "ClearStatus";
	case BeginOTA:
		return "BeginOTA";
	case DataOTA:
		return "DataOTA";
	case EndOTA:
		return "EndOTA";
	}
	return "Unknown";
}

const char* getDataPacketName(DataPacketType type) {
	switch (type) {
	case DataDrone:
		return "Drone";
	case DataLog:
		return "Log";
	case DataDebugOutput:
		return "DebugOutput";
	case DataDebugProfiler:
		return "DebugProfiler";
	}
	return "Unknown";
}