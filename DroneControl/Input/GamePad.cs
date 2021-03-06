﻿using DroneLibrary.Diagnostics;
using SharpDX.DirectInput;
using System;

namespace DroneControl.Input
{
    public class GamePad : IInputDevice
    {
        private DirectInput directInput;
        private Joystick device;

        private JoystickState currentState;
        private JoystickState lastState;

        public bool IsConnected
        {
            get
            {
                return currentState != null;
            }
        }

        public string Name
        {
            get
            {
                return device.Information.InstanceName;
            }
        }

        public BatteryInfo Battery
        {
            get
            {
                return new BatteryInfo(false, BatteryLevel.Empty);
            }
        }

        public bool CanCalibrate
        {
            get { return IsConnected; }
        }

        public bool HasError { get; private set; }

        public GamePad(DirectInput directInput, DeviceInstance deviceInstance)
        {
            this.directInput = directInput;
            this.device = new Joystick(directInput, deviceInstance.InstanceGuid);
            this.device.Acquire();
            UpdateState();
        }

        private bool UpdateState()
        {
            try
            {
                if (!IsConnected)
                    device.Acquire();
                currentState = device.GetCurrentState();
            }
            catch (SharpDX.SharpDXException)
            {
                currentState = null;
            }
            return currentState != null;
        }

        public void Calibrate()
        {
            if (IsConnected)
                device.RunControlPanel();
        }

        public void Update(InputManager manager)
        {
            try
            {
                if (!UpdateState())
                    return;

                if (CheckButtonPressed(2))
                    manager.SendClear();

                if (CheckButtonPressed(1))
                    manager.StopDrone();

                if (CheckButtonPressed(0))
                    manager.ArmDrone();

                if (CheckButtonReleased(0))
                    manager.DisarmDrone();

                float deadZone = 0.075f;
                if (!manager.DeadZone)
                    deadZone = 0;

                const int maxValue = UInt16.MaxValue / 2;
                TargetData target = new TargetData();
                target.Roll = DeadZone.Compute(currentState.X - maxValue, maxValue, deadZone);
                target.Pitch = DeadZone.Compute(currentState.Y - maxValue, maxValue, deadZone);
                target.Yaw = DeadZone.Compute(currentState.RotationZ - maxValue, maxValue, deadZone);
                target.Thrust = DeadZone.Compute(UInt16.MaxValue - currentState.Z, UInt16.MaxValue, deadZone);

                manager.SendTargetData(target);

                lastState = currentState;
                HasError = false;
            }
            catch(Exception e)
            {
                HasError = true;
                Log.Error(e);
            }
        }

        private bool CheckButtonReleased(int button)
        {
            if (lastState == null)
                return false;
            if (button >= currentState.Buttons.Length)
                return false;

            bool current = currentState.Buttons[button];
            bool last = lastState.Buttons[button];
            return !current && last;
        }

        private bool CheckButtonPressed(int button)
        {
            if (lastState == null)
                return false;
            if (button >= currentState.Buttons.Length)
                return false;

            bool current = currentState.Buttons[button];
            bool last = lastState.Buttons[button];
            return current && !last;
        }

        public void Dispose()
        {
            if (device != null)
            {
                device.Unacquire();
                device.Dispose();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is GamePad)
                return Equals((GamePad)obj);
            return false;
        }

        public bool Equals(IInputDevice other)
        {
            GamePad o = other as GamePad;
            if (o == null)
                return false;
            return o.device.Information.InstanceGuid == device.Information.InstanceGuid;
        }

        public override int GetHashCode()
        {
            return device.Information.InstanceGuid.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
