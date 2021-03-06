﻿using DroneControl.Input;
using DroneLibrary;
using DroneLibrary.Data;
using DroneLibrary.Diagnostics;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DroneControl
{
    public partial class MainForm : Form
    {
        private Drone drone;

        private LogForm logForm;
        private DebugForm debugForm;
        private GraphForm graphForm;
        private SettingsForm settingsForm;
        private InfoForm infoForm;

        public InputManager InputManager
        {
            get { return flightControl1.InputManager; }
        }

        private long tickCount;
        private bool dataDirty = true;

        private Stopwatch flyTimer = new Stopwatch();

        public MainForm(Drone drone)
        {
            if (drone == null)
                throw new ArgumentNullException(nameof(drone));

            InitializeComponent();

            this.drone = drone;

            Log.Info("Booting main form with drone {0}", drone.Address);

            timer.Interval = 250;
            timer.Tick += Timer_Tick;
            timer.Start();

            drone.OnInfoChange += Drone_OnInfoChange;
            drone.OnPingChange += Drone_OnPingChange;
            drone.OnDataChange += Drone_OnDataChange;
            drone.OnSettingsChange += Drone_OnSettingsChange;

            motorControl1.Init(drone);
            flightControl1.Init(drone);
            sensorControl1.Init(drone);

            ipInfoLabel.Text = string.Format("Connecting to \"{0}\"", drone.Address);
            UpdateInfo(drone.Info);
            UpdateUI();
            UpdateSettings(drone.Settings);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (timer != null)
                timer.Stop();

            if (drone != null)
            {
                drone.OnInfoChange -= Drone_OnInfoChange;
                drone.OnPingChange -= Drone_OnPingChange;
                drone.OnDataChange -= Drone_OnDataChange;
                drone.OnSettingsChange -= Drone_OnSettingsChange;

                drone.Disconnect();
            }

            base.OnFormClosed(e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            drone.SendPing();
            if (tickCount % 50 == 0)
                drone.SendGetInfo();
            drone.ResendPendingPackets();

            tickCount++;

            UpdateUI();
        }

        private Form ShowForm(Form form, Func<Form> onClosed)
        {
            if (onClosed == null)
                throw new ArgumentNullException(nameof(onClosed));

            if (form == null || form.IsDisposed)
                form = onClosed();

            if (!form.Visible)
                form.Show();
            form.BringToFront();
            return form;
        }

        private void UpdateUI()
        {
            if (!dataDirty)
                return;

            UpdatePing();
            UpdateData();

            dataDirty = false;
        }

        private void UpdatePing()
        {
            if (!drone.IsConnected)
            {
                pingLabel.Text = "Not connected";
                ipInfoLabel.Text = string.Format("IP-Address: \"{0}\"", drone.Address);
            }
            else
            {
                pingLabel.Text = string.Format("Ping: {0}ms", drone.Ping);
                ipInfoLabel.Text = string.Format("Connected to \"{0}\"", drone.Address);
            }

            if (!drone.IsConnected || drone.Ping > 50)
                pingLabel.ForeColor = Color.Red;
            else
                pingLabel.ForeColor = Color.Green;
        } 

        private void UpdateInfo(DroneInfo info)
        {
            if (string.IsNullOrWhiteSpace(info.Name))
                Text = string.Format("DroneControl - {0}", drone.Address);
            else
                Text = string.Format("DroneControl - {0}", info.Name);

            droneInfoPropertyGrid.SelectedObject = info;
        }


        private void UpdateData()
        {
            try
            {
                SuspendLayout();

                DroneData data = drone.Data;
                if (!drone.IsConnected)
                {
                    statusArmedLabel.ForeColor = Color.Red;
                    statusArmedLabel.Text = "Status: not connected";
                    if (data.State != DroneState.Unknown)
                        statusArmedLabel.Text += string.Format(" (last: {0})", data.State);

                    statusButton.Enabled = false;
                    wifiRssiLabel.Visible = false;
                }
                else
                {
                    statusButton.Enabled = true;

                    switch (drone.Data.State)
                    {
                        case DroneState.Unknown:
                            statusButton.Enabled = false;
                            statusArmedLabel.ForeColor = Color.DarkRed;
                            statusButton.Text = "Unknown";
                            break;
                        case DroneState.Stopped:
                        case DroneState.Reset:
                            statusArmedLabel.ForeColor = Color.Green;
                            statusButton.Text = "Clear";
                            break;
                        case DroneState.Idle:
                            statusArmedLabel.ForeColor = Color.DarkGreen;
                            statusButton.Text = "Arm";
                            break;
                        case DroneState.Armed:
                        case DroneState.Flying:
                            statusArmedLabel.ForeColor = Color.DarkOrange;
                            statusButton.Text = "Disarm";
                            break;
                    }

                    if (drone.IsConnected && data.State.AreMotorsRunning() && !flyTimer.IsRunning)
                        flyTimer.Start();
                    else if (!data.State.AreMotorsRunning() && flyTimer.IsRunning)
                        flyTimer.Stop();

                    if (flyTimer.ElapsedMilliseconds > 0)
                        statusArmedLabel.Text = string.Format("Status: {0} ({1:00}:{2:00})", data.State, (int)flyTimer.Elapsed.TotalMinutes, flyTimer.Elapsed.Seconds);
                    else
                        statusArmedLabel.Text = $"Status: {data.State}";

                    // RSSI ist immer unter 0, wenn die Drohne mit einem Netzwerk verbunden ist
                    wifiRssiLabel.Visible = data.WifiRssi < 0;

                    if (wifiRssiLabel.Visible)
                    {
                        StringBuilder wifiText = new StringBuilder();
                        wifiText.Append("WiFi signal: ");
                        wifiText.Append(data.WifiRssi);
                        wifiText.Append("dBm ");

                        if (data.WifiRssi > -40)
                        {
                            wifiText.Append("very good");
                            wifiRssiLabel.ForeColor = Color.DarkGreen;
                        }
                        else if (data.WifiRssi > -70)
                        {
                            wifiText.Append("good");
                            wifiRssiLabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            wifiText.Append("bad");
                            wifiRssiLabel.ForeColor = Color.DarkRed;
                        }

                        wifiRssiLabel.Text = wifiText.ToString();
                    }
                }

                ResumeLayout();
            }
            catch(Exception e)
            {
                ErrorHandler.HandleException(drone, e);
            }
        }

        private void UpdateSettings(DroneSettings settings)
        {
            droneSettingsPropertyGrid.SelectedObject = settings;
        }

        private void Drone_OnInfoChange(object sender, InfoChangedEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<InfoChangedEventArgs>(Drone_OnInfoChange), this, eventArgs);
                return;
            }

            UpdateInfo(eventArgs.Info);
        }

        private void Drone_OnDataChange(object sender, DataChangedEventArgs eventArgs)
        {
            dataDirty = true;
        }

        private void Drone_OnPingChange(object sender, PingChangedEventArgs e)
        {
            dataDirty = true;
        }

        private void Drone_OnSettingsChange(object sender, SettingsChangedEventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<SettingsChangedEventArgs>(Drone_OnSettingsChange), this, eventArgs);
                return;
            }

            UpdateSettings(eventArgs.Settings);
        }

        private void statusToogleButton_Click(object sender, EventArgs e)
        {
            switch(drone.Data.State)
            {
                case DroneState.Reset:
                case DroneState.Stopped:
                    Log.Info("Clear status by user input with status button");
                    drone.SendClearStatus();
                    break;
                case DroneState.Idle:
                    Log.Info("Arm by user input with status button");
                    drone.SendArm();
                    break;
                case DroneState.Armed:
                case DroneState.Flying:
                    Log.Info("Disarm by user input with status button");
                    drone.SendDisarm();
                    break;
            }

        }

        private void logButton_Click(object sender, EventArgs e)
        {
            ShowForm(logForm, () => (logForm = new LogForm(drone)));
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Log.Info("Stopping because stop button is clicked");
            drone.SendStop();
        }

        private void droneSettingsPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                DroneSettings settings = (DroneSettings)droneSettingsPropertyGrid.SelectedObject;
                drone.SendConfig(settings);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageBox.Show(ex.Message, "Error setting settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void debugButton_Click(object sender, EventArgs e)
        {
            ShowForm(debugForm, () => (debugForm = new DebugForm(drone, InputManager)));
        }

        private void graphButton_Click(object sender, EventArgs e)
        {
            ShowForm(graphForm, () => (graphForm = new GraphForm(drone, flightControl1)));
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            ShowForm(settingsForm, () => (settingsForm = new SettingsForm(drone)));
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            ShowForm(infoForm, () => (infoForm = new InfoForm()));
        }

        private void statusArmedLabel_Click(object sender, EventArgs e)
        {
            if (drone.IsConnected && drone.Data.State.AreMotorsRunning())
                return;
            if (flyTimer.ElapsedMilliseconds == 0)
                return;

            if (MessageBox.Show("Do you want to reset the fly timer?", "Fly timer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                flyTimer.Reset();
        }
    }
}
