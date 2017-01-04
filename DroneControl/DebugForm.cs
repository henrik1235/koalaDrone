﻿using DroneControl.Input;
using DroneLibrary;
using System;
using System.Text;
using System.Windows.Forms;

namespace DroneControl
{
    public partial class DebugForm : Form
    {
        public Drone Drone { get; private set; }
        public InputManager InputManager { get; private set; }

        public DebugForm(Drone drone, InputManager inputManager)
        {
            if (drone == null)
                throw new ArgumentNullException(nameof(drone));

            InitializeComponent();

            this.Drone = drone;
            this.Drone.OnDebugDataChange += Drone_OnDebugDataChange;
            this.InputManager = inputManager;

            UpdateDebugData(drone.DebugData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.Drone.OnDebugDataChange -= Drone_OnDebugDataChange;
            base.OnFormClosing(e);
        }

        private void UpdateDebugData(DebugData data)
        {
            StringBuilder profilerString = new StringBuilder();

            profilerString.AppendFormat("Free heap: {0}", Formatting.FormatDataSize(data.FreeHeapBytes));
            profilerString.AppendLine();

            if (data.Profiler.Entries != null)
            {
                for (int i = 0; i < data.Profiler.Entries.Length; i++)
                {
                    DebugProfiler.Entry entry = data.Profiler.Entries[i];
                    profilerString.AppendFormat("{0} {1}ms ({2}ms)",
                        entry.Name.PadLeft(25),
                        Formatting.FormatDecimal(entry.Time.TotalMilliseconds, 1, 4),
                        Formatting.FormatDecimal(entry.TimeMax.TotalMilliseconds, 1, 4));
                    profilerString.AppendLine();
                }

                profilerData.Text = profilerString.ToString();
            }
        }

        private void Drone_OnDebugDataChange(object sender, DebugDataChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<DebugDataChangedEventArgs>(Drone_OnDebugDataChange), sender, e);
                return;
            }

            UpdateDebugData(e.DebugData);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (!Drone.Data.State.IsIdle())
                return;
            Drone.SendReset();
        }

        private void blinkButton_Click(object sender, EventArgs e)
        {
            Drone.SendBlink();
        }

        private void recorderButton_Click(object sender, EventArgs e)
        {
            new RecordForm(Drone, InputManager).Show();
        }
    }
}
