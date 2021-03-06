#pragma once

#define MODEL_TWO 1
#define MODEL_NAME "koalaDrone r2"

//#define PIN_FRONT_LEFT 12
//#define PIN_FRONT_RIGHT 14
//#define PIN_BACK_LEFT 27
//#define PIN_BACK_RIGHT 26

#define PIN_FRONT_LEFT 27
#define PIN_FRONT_RIGHT 26
#define PIN_BACK_LEFT 12
#define PIN_BACK_RIGHT 14

#define PIN_LED0 13
#define PIN_BATTERY 36

#define BATTERY_MAX_VALUE (4095.0f)
#define BATTERY_MAX_VOLTAGE (3.9f) //(22.155f)

#define DEFAULT_SERVO_MIN 1000
#define DEFAULT_SERVO_MAX 2000
#define DEFAULT_SERVO_IDLE 1050

#define MEMORY_I2C_ENABLE true
#define MEMORY_I2C_SIZE 8192
#define MEMORY_I2C_PAGE_SIZE 32

#define SWAP_GYRO_XY true
#define NEGATE_GYRO_X true
#define NEGATE_GYRO_Y true
#define NEGATE_GYRO_Z false