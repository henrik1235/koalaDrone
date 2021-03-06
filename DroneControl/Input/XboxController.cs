﻿using DroneLibrary.Diagnostics;
using SharpDX.XInput;
using System;

namespace DroneControl.Input
{
    public class XboxController : IInputDevice
    {
        private Controller controller;

        private bool firstUpdate = true;
        private State currentState, lastState;

        public bool IsConnected
        {
            get { return controller.IsConnected; }
        }

        public string Name
        {
            get { return string.Format("Xbox Controller [{0}]", controller.UserIndex);  }
        }

        public BatteryInfo Battery
        {
            get
            {
                if (!controller.IsConnected)
                    return new BatteryInfo();

                BatteryInformation info = controller.GetBatteryInformation(BatteryDeviceType.Gamepad);

                bool hasBattery = info.BatteryType == BatteryType.Alkaline || info.BatteryType == BatteryType.Nimh;
                return new BatteryInfo(hasBattery, (BatteryLevel)(int)info.BatteryLevel);
            }
        }

        public bool CanCalibrate
        {
            get { return false; }
        }

        public bool HasError { get; private set; }

        public XboxController(Controller controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            this.controller = controller;
        }

        public void Dispose()
        {
        }

        public void Calibrate()
        {
        }

        public void Update(InputManager manager)
        {
            try
            {
                if (!IsConnected)
                    return;

                currentState = controller.GetState();

                if (CheckButtonPressed(GamepadButtonFlags.A))
                    manager.SendClear();

                if (CheckButtonPressed(GamepadButtonFlags.B))
                    manager.StopDrone();

                if (CheckButtonPressed(GamepadButtonFlags.Y))
                    manager.ToogleArmStatus();

                float deadZone = 0.075f;
                if (!manager.DeadZone)
                    deadZone = 0;

                TargetData target = new TargetData();
                target.Roll = DeadZone.Compute(currentState.Gamepad.RightThumbX, short.MaxValue, deadZone);
                target.Pitch = -DeadZone.Compute(currentState.Gamepad.RightThumbY, short.MaxValue, deadZone);
                target.Yaw = DeadZone.Compute(currentState.Gamepad.LeftThumbX, short.MaxValue, deadZone);
                target.Thrust = DeadZone.Compute(currentState.Gamepad.LeftThumbY + short.MaxValue, short.MaxValue * 2, deadZone);

                float x = GetButtonValue(GamepadButtonFlags.DPadRight) - GetButtonValue(GamepadButtonFlags.DPadLeft);
                float y = -GetButtonValue(GamepadButtonFlags.DPadDown) - GetButtonValue(GamepadButtonFlags.DPadUp);
                target.Roll += x * 0.1f;
                target.Pitch += y * 0.1f;


                manager.SendTargetData(target);

                lastState = currentState;
                firstUpdate = false;

                HasError = false;
            }
            catch(Exception e)
            {
                HasError = true;
                Log.Error(e);
            }
        }

        private float GetButtonValue(GamepadButtonFlags button)
        {
            if (currentState.Gamepad.Buttons.HasFlag(button))
                return 1;
            return 0;
        }

        private bool CheckButtonPressed(GamepadButtonFlags button)
        {
            if (firstUpdate)
                return false;

            bool current = currentState.Gamepad.Buttons.HasFlag(button);
            bool last = lastState.Gamepad.Buttons.HasFlag(button);
            return current && !last;
        }

        public override bool Equals(object other)
        {
            if (other is XboxController)
                return Equals((XboxController)other);
            return false;
        }

        public bool Equals(IInputDevice other)
        {
            return Equals((object)other);
        }

        public bool Equals(XboxController other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return controller.UserIndex == other.controller.UserIndex;
        }

        public override int GetHashCode()
        {
            return controller.UserIndex.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
