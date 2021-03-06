﻿namespace DroneControl
{
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.SplitContainer dronePingSplitContainer;
            System.Windows.Forms.SplitContainer motorsInfoSplitContainer;
            System.Windows.Forms.SplitContainer motorsSensorSplitContainer;
            System.Windows.Forms.TabControl infoTabControl;
            System.Windows.Forms.TabPage infoTabPage;
            System.Windows.Forms.TabPage settingsTabPage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.infoButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.wifiRssiLabel = new System.Windows.Forms.Label();
            this.graphsButton = new System.Windows.Forms.Button();
            this.debugButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.logButton = new System.Windows.Forms.Button();
            this.statusButton = new System.Windows.Forms.Button();
            this.statusArmedLabel = new System.Windows.Forms.Label();
            this.pingLabel = new System.Windows.Forms.Label();
            this.ipInfoLabel = new System.Windows.Forms.Label();
            this.mainViewTabs = new System.Windows.Forms.TabControl();
            this.manualControlPage = new System.Windows.Forms.TabPage();
            this.motorControl1 = new DroneControl.MotorControl();
            this.flightControlPage = new System.Windows.Forms.TabPage();
            this.flightControl1 = new DroneControl.FlightControl();
            this.sensorControl1 = new DroneControl.SensorControl();
            this.droneInfoPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.droneSettingsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.timer = new System.Windows.Forms.Timer(this.components);
            dronePingSplitContainer = new System.Windows.Forms.SplitContainer();
            motorsInfoSplitContainer = new System.Windows.Forms.SplitContainer();
            motorsSensorSplitContainer = new System.Windows.Forms.SplitContainer();
            infoTabControl = new System.Windows.Forms.TabControl();
            infoTabPage = new System.Windows.Forms.TabPage();
            settingsTabPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(dronePingSplitContainer)).BeginInit();
            dronePingSplitContainer.Panel1.SuspendLayout();
            dronePingSplitContainer.Panel2.SuspendLayout();
            dronePingSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(motorsInfoSplitContainer)).BeginInit();
            motorsInfoSplitContainer.Panel1.SuspendLayout();
            motorsInfoSplitContainer.Panel2.SuspendLayout();
            motorsInfoSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(motorsSensorSplitContainer)).BeginInit();
            motorsSensorSplitContainer.Panel1.SuspendLayout();
            motorsSensorSplitContainer.Panel2.SuspendLayout();
            motorsSensorSplitContainer.SuspendLayout();
            this.mainViewTabs.SuspendLayout();
            this.manualControlPage.SuspendLayout();
            this.flightControlPage.SuspendLayout();
            infoTabControl.SuspendLayout();
            infoTabPage.SuspendLayout();
            settingsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // dronePingSplitContainer
            // 
            dronePingSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            dronePingSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            dronePingSplitContainer.IsSplitterFixed = true;
            dronePingSplitContainer.Location = new System.Drawing.Point(0, 0);
            dronePingSplitContainer.Name = "dronePingSplitContainer";
            dronePingSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // dronePingSplitContainer.Panel1
            // 
            dronePingSplitContainer.Panel1.Controls.Add(this.infoButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.settingsButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.wifiRssiLabel);
            dronePingSplitContainer.Panel1.Controls.Add(this.graphsButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.debugButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.stopButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.logButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.statusButton);
            dronePingSplitContainer.Panel1.Controls.Add(this.statusArmedLabel);
            dronePingSplitContainer.Panel1.Controls.Add(this.pingLabel);
            dronePingSplitContainer.Panel1.Controls.Add(this.ipInfoLabel);
            // 
            // dronePingSplitContainer.Panel2
            // 
            dronePingSplitContainer.Panel2.Controls.Add(motorsInfoSplitContainer);
            dronePingSplitContainer.Size = new System.Drawing.Size(1047, 737);
            dronePingSplitContainer.SplitterDistance = 51;
            dronePingSplitContainer.TabIndex = 18;
            // 
            // infoButton
            // 
            this.infoButton.Location = new System.Drawing.Point(426, 4);
            this.infoButton.Name = "infoButton";
            this.infoButton.Size = new System.Drawing.Size(60, 23);
            this.infoButton.TabIndex = 22;
            this.infoButton.Text = "Info";
            this.infoButton.UseVisualStyleBackColor = true;
            this.infoButton.Click += new System.EventHandler(this.infoButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(360, 4);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(60, 23);
            this.settingsButton.TabIndex = 21;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // wifiRssiLabel
            // 
            this.wifiRssiLabel.AutoSize = true;
            this.wifiRssiLabel.Location = new System.Drawing.Point(123, 30);
            this.wifiRssiLabel.Name = "wifiRssiLabel";
            this.wifiRssiLabel.Size = new System.Drawing.Size(53, 13);
            this.wifiRssiLabel.TabIndex = 20;
            this.wifiRssiLabel.Text = "Wifi RSSI";
            // 
            // graphsButton
            // 
            this.graphsButton.Location = new System.Drawing.Point(302, 4);
            this.graphsButton.Name = "graphsButton";
            this.graphsButton.Size = new System.Drawing.Size(52, 23);
            this.graphsButton.TabIndex = 19;
            this.graphsButton.Text = "Graphs";
            this.graphsButton.UseVisualStyleBackColor = true;
            this.graphsButton.Click += new System.EventHandler(this.graphButton_Click);
            // 
            // debugButton
            // 
            this.debugButton.Location = new System.Drawing.Point(244, 4);
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(52, 23);
            this.debugButton.TabIndex = 18;
            this.debugButton.Text = "Debug";
            this.debugButton.UseVisualStyleBackColor = true;
            this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.BackColor = System.Drawing.Color.DarkRed;
            this.stopButton.ForeColor = System.Drawing.Color.White;
            this.stopButton.Location = new System.Drawing.Point(960, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 17;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // logButton
            // 
            this.logButton.Location = new System.Drawing.Point(199, 4);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(39, 23);
            this.logButton.TabIndex = 16;
            this.logButton.Text = "Log";
            this.logButton.UseVisualStyleBackColor = true;
            this.logButton.Click += new System.EventHandler(this.logButton_Click);
            // 
            // statusButton
            // 
            this.statusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusButton.Location = new System.Drawing.Point(879, 5);
            this.statusButton.Name = "statusButton";
            this.statusButton.Size = new System.Drawing.Size(75, 23);
            this.statusButton.TabIndex = 15;
            this.statusButton.Text = "Status";
            this.statusButton.UseVisualStyleBackColor = true;
            this.statusButton.Click += new System.EventHandler(this.statusToogleButton_Click);
            // 
            // statusArmedLabel
            // 
            this.statusArmedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusArmedLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.statusArmedLabel.Location = new System.Drawing.Point(680, 10);
            this.statusArmedLabel.Name = "statusArmedLabel";
            this.statusArmedLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusArmedLabel.Size = new System.Drawing.Size(193, 13);
            this.statusArmedLabel.TabIndex = 14;
            this.statusArmedLabel.Text = "Status: {0}";
            this.statusArmedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.statusArmedLabel.Click += new System.EventHandler(this.statusArmedLabel_Click);
            // 
            // pingLabel
            // 
            this.pingLabel.AutoSize = true;
            this.pingLabel.ForeColor = System.Drawing.Color.Red;
            this.pingLabel.Location = new System.Drawing.Point(12, 30);
            this.pingLabel.Name = "pingLabel";
            this.pingLabel.Size = new System.Drawing.Size(61, 13);
            this.pingLabel.TabIndex = 13;
            this.pingLabel.Text = "Ping: {0}ms";
            // 
            // ipInfoLabel
            // 
            this.ipInfoLabel.AutoSize = true;
            this.ipInfoLabel.Location = new System.Drawing.Point(12, 9);
            this.ipInfoLabel.Name = "ipInfoLabel";
            this.ipInfoLabel.Size = new System.Drawing.Size(98, 13);
            this.ipInfoLabel.TabIndex = 12;
            this.ipInfoLabel.Text = "Connected to \"{0}\"";
            // 
            // motorsInfoSplitContainer
            // 
            motorsInfoSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            motorsInfoSplitContainer.Location = new System.Drawing.Point(0, 0);
            motorsInfoSplitContainer.Name = "motorsInfoSplitContainer";
            // 
            // motorsInfoSplitContainer.Panel1
            // 
            motorsInfoSplitContainer.Panel1.Controls.Add(motorsSensorSplitContainer);
            // 
            // motorsInfoSplitContainer.Panel2
            // 
            motorsInfoSplitContainer.Panel2.Controls.Add(infoTabControl);
            motorsInfoSplitContainer.Size = new System.Drawing.Size(1047, 682);
            motorsInfoSplitContainer.SplitterDistance = 657;
            motorsInfoSplitContainer.TabIndex = 18;
            // 
            // motorsSensorSplitContainer
            // 
            motorsSensorSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            motorsSensorSplitContainer.Location = new System.Drawing.Point(0, 0);
            motorsSensorSplitContainer.Name = "motorsSensorSplitContainer";
            motorsSensorSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // motorsSensorSplitContainer.Panel1
            // 
            motorsSensorSplitContainer.Panel1.Controls.Add(this.mainViewTabs);
            // 
            // motorsSensorSplitContainer.Panel2
            // 
            motorsSensorSplitContainer.Panel2.Controls.Add(this.sensorControl1);
            motorsSensorSplitContainer.Size = new System.Drawing.Size(657, 682);
            motorsSensorSplitContainer.SplitterDistance = 273;
            motorsSensorSplitContainer.TabIndex = 0;
            // 
            // mainViewTabs
            // 
            this.mainViewTabs.Controls.Add(this.manualControlPage);
            this.mainViewTabs.Controls.Add(this.flightControlPage);
            this.mainViewTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainViewTabs.Location = new System.Drawing.Point(0, 0);
            this.mainViewTabs.Name = "mainViewTabs";
            this.mainViewTabs.SelectedIndex = 0;
            this.mainViewTabs.Size = new System.Drawing.Size(657, 273);
            this.mainViewTabs.TabIndex = 15;
            // 
            // manualControlPage
            // 
            this.manualControlPage.Controls.Add(this.motorControl1);
            this.manualControlPage.Location = new System.Drawing.Point(4, 22);
            this.manualControlPage.Name = "manualControlPage";
            this.manualControlPage.Padding = new System.Windows.Forms.Padding(3);
            this.manualControlPage.Size = new System.Drawing.Size(649, 247);
            this.manualControlPage.TabIndex = 0;
            this.manualControlPage.Text = "Manual Control";
            this.manualControlPage.UseVisualStyleBackColor = true;
            // 
            // motorControl1
            // 
            this.motorControl1.Location = new System.Drawing.Point(6, 3);
            this.motorControl1.Name = "motorControl1";
            this.motorControl1.Size = new System.Drawing.Size(364, 132);
            this.motorControl1.TabIndex = 3;
            // 
            // flightControlPage
            // 
            this.flightControlPage.Controls.Add(this.flightControl1);
            this.flightControlPage.Location = new System.Drawing.Point(4, 22);
            this.flightControlPage.Name = "flightControlPage";
            this.flightControlPage.Padding = new System.Windows.Forms.Padding(3);
            this.flightControlPage.Size = new System.Drawing.Size(649, 247);
            this.flightControlPage.TabIndex = 1;
            this.flightControlPage.Text = "Flight Control";
            this.flightControlPage.UseVisualStyleBackColor = true;
            // 
            // flightControl1
            // 
            this.flightControl1.AutoScroll = true;
            this.flightControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flightControl1.Location = new System.Drawing.Point(3, 3);
            this.flightControl1.Name = "flightControl1";
            this.flightControl1.Size = new System.Drawing.Size(643, 241);
            this.flightControl1.TabIndex = 0;
            // 
            // sensorControl1
            // 
            this.sensorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sensorControl1.Location = new System.Drawing.Point(0, 0);
            this.sensorControl1.Name = "sensorControl1";
            this.sensorControl1.Size = new System.Drawing.Size(657, 405);
            this.sensorControl1.TabIndex = 17;
            // 
            // infoTabControl
            // 
            infoTabControl.Controls.Add(infoTabPage);
            infoTabControl.Controls.Add(settingsTabPage);
            infoTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            infoTabControl.Location = new System.Drawing.Point(0, 0);
            infoTabControl.Name = "infoTabControl";
            infoTabControl.SelectedIndex = 0;
            infoTabControl.Size = new System.Drawing.Size(386, 682);
            infoTabControl.TabIndex = 0;
            // 
            // infoTabPage
            // 
            infoTabPage.Controls.Add(this.droneInfoPropertyGrid);
            infoTabPage.Location = new System.Drawing.Point(4, 22);
            infoTabPage.Name = "infoTabPage";
            infoTabPage.Padding = new System.Windows.Forms.Padding(3);
            infoTabPage.Size = new System.Drawing.Size(378, 656);
            infoTabPage.TabIndex = 0;
            infoTabPage.Text = "Info";
            infoTabPage.UseVisualStyleBackColor = true;
            // 
            // droneInfoPropertyGrid
            // 
            this.droneInfoPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droneInfoPropertyGrid.HelpVisible = false;
            this.droneInfoPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.droneInfoPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.droneInfoPropertyGrid.Name = "droneInfoPropertyGrid";
            this.droneInfoPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.droneInfoPropertyGrid.Size = new System.Drawing.Size(372, 650);
            this.droneInfoPropertyGrid.TabIndex = 0;
            // 
            // settingsTabPage
            // 
            settingsTabPage.Controls.Add(this.droneSettingsPropertyGrid);
            settingsTabPage.Location = new System.Drawing.Point(4, 22);
            settingsTabPage.Name = "settingsTabPage";
            settingsTabPage.Padding = new System.Windows.Forms.Padding(3);
            settingsTabPage.Size = new System.Drawing.Size(378, 656);
            settingsTabPage.TabIndex = 1;
            settingsTabPage.Text = "Settings";
            settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // droneSettingsPropertyGrid
            // 
            this.droneSettingsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droneSettingsPropertyGrid.HelpVisible = false;
            this.droneSettingsPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.droneSettingsPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.droneSettingsPropertyGrid.Name = "droneSettingsPropertyGrid";
            this.droneSettingsPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.droneSettingsPropertyGrid.Size = new System.Drawing.Size(372, 650);
            this.droneSettingsPropertyGrid.TabIndex = 0;
            this.droneSettingsPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.droneSettingsPropertyGrid_PropertyValueChanged);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 737);
            this.Controls.Add(dronePingSplitContainer);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Drone Control";
            dronePingSplitContainer.Panel1.ResumeLayout(false);
            dronePingSplitContainer.Panel1.PerformLayout();
            dronePingSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(dronePingSplitContainer)).EndInit();
            dronePingSplitContainer.ResumeLayout(false);
            motorsInfoSplitContainer.Panel1.ResumeLayout(false);
            motorsInfoSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(motorsInfoSplitContainer)).EndInit();
            motorsInfoSplitContainer.ResumeLayout(false);
            motorsSensorSplitContainer.Panel1.ResumeLayout(false);
            motorsSensorSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(motorsSensorSplitContainer)).EndInit();
            motorsSensorSplitContainer.ResumeLayout(false);
            this.mainViewTabs.ResumeLayout(false);
            this.manualControlPage.ResumeLayout(false);
            this.flightControlPage.ResumeLayout(false);
            infoTabControl.ResumeLayout(false);
            infoTabPage.ResumeLayout(false);
            settingsTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.PropertyGrid droneInfoPropertyGrid;
        private System.Windows.Forms.PropertyGrid droneSettingsPropertyGrid;
        private SensorControl sensorControl1;
        private FlightControl flightControl1;
        private System.Windows.Forms.TabPage flightControlPage;
        private MotorControl motorControl1;
        private System.Windows.Forms.TabPage manualControlPage;
        private System.Windows.Forms.TabControl mainViewTabs;
        private System.Windows.Forms.Label ipInfoLabel;
        private System.Windows.Forms.Label pingLabel;
        private System.Windows.Forms.Label statusArmedLabel;
        private System.Windows.Forms.Button statusButton;
        private System.Windows.Forms.Button logButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button debugButton;
        private System.Windows.Forms.Button graphsButton;
        private System.Windows.Forms.Label wifiRssiLabel;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button infoButton;
    }
}