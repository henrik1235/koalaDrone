﻿namespace DroneControl
{
    partial class FlightControl {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox deviceGroupBox;
            System.Windows.Forms.GroupBox dataGroupBox;
            System.Windows.Forms.GroupBox inputConfigGroupBox;
            System.Windows.Forms.Label yawExpText;
            System.Windows.Forms.Label pitchExpText;
            System.Windows.Forms.Label rollExpText;
            System.Windows.Forms.GroupBox inputGraphsGroupBox;
            System.Windows.Forms.Label thrustExpText;
            System.Windows.Forms.Label thrustBaseText;
            this.calibrateButton = new System.Windows.Forms.Button();
            this.searchDeviceButton = new System.Windows.Forms.Button();
            this.deviceBatteryLabel = new System.Windows.Forms.Label();
            this.deviceConnectionLabel = new System.Windows.Forms.Label();
            this.inputDeviceComboBox = new System.Windows.Forms.ComboBox();
            this.rollLabel = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.yawLabel = new System.Windows.Forms.Label();
            this.thrustLabel = new System.Windows.Forms.Label();
            this.pidDataLabel = new System.Windows.Forms.Label();
            this.yawExpTextBox = new System.Windows.Forms.NumericUpDown();
            this.pitchExpTextBox = new System.Windows.Forms.NumericUpDown();
            this.rollExpTextBox = new System.Windows.Forms.NumericUpDown();
            this.deadZoneCheckBox = new System.Windows.Forms.CheckBox();
            this.inputCurves = new DroneControl.QuadGraphControl();
            this.searchTimer = new System.Windows.Forms.Timer(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.thrustExpTextBox = new System.Windows.Forms.NumericUpDown();
            this.thrustBaseTextBox = new System.Windows.Forms.NumericUpDown();
            deviceGroupBox = new System.Windows.Forms.GroupBox();
            dataGroupBox = new System.Windows.Forms.GroupBox();
            inputConfigGroupBox = new System.Windows.Forms.GroupBox();
            yawExpText = new System.Windows.Forms.Label();
            pitchExpText = new System.Windows.Forms.Label();
            rollExpText = new System.Windows.Forms.Label();
            inputGraphsGroupBox = new System.Windows.Forms.GroupBox();
            thrustExpText = new System.Windows.Forms.Label();
            thrustBaseText = new System.Windows.Forms.Label();
            deviceGroupBox.SuspendLayout();
            dataGroupBox.SuspendLayout();
            inputConfigGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yawExpTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchExpTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollExpTextBox)).BeginInit();
            inputGraphsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thrustExpTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thrustBaseTextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // deviceGroupBox
            // 
            deviceGroupBox.Controls.Add(this.calibrateButton);
            deviceGroupBox.Controls.Add(this.searchDeviceButton);
            deviceGroupBox.Controls.Add(this.deviceBatteryLabel);
            deviceGroupBox.Controls.Add(this.deviceConnectionLabel);
            deviceGroupBox.Controls.Add(this.inputDeviceComboBox);
            deviceGroupBox.Location = new System.Drawing.Point(10, 14);
            deviceGroupBox.Name = "deviceGroupBox";
            deviceGroupBox.Size = new System.Drawing.Size(215, 154);
            deviceGroupBox.TabIndex = 25;
            deviceGroupBox.TabStop = false;
            deviceGroupBox.Text = "Input Device";
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(134, 123);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 23);
            this.calibrateButton.TabIndex = 13;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // searchDeviceButton
            // 
            this.searchDeviceButton.Location = new System.Drawing.Point(11, 21);
            this.searchDeviceButton.Name = "searchDeviceButton";
            this.searchDeviceButton.Size = new System.Drawing.Size(75, 23);
            this.searchDeviceButton.TabIndex = 12;
            this.searchDeviceButton.Text = "Search";
            this.searchDeviceButton.UseVisualStyleBackColor = true;
            this.searchDeviceButton.Click += new System.EventHandler(this.searchDeviceButton_Click);
            // 
            // deviceBatteryLabel
            // 
            this.deviceBatteryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deviceBatteryLabel.AutoSize = true;
            this.deviceBatteryLabel.Location = new System.Drawing.Point(6, 120);
            this.deviceBatteryLabel.Name = "deviceBatteryLabel";
            this.deviceBatteryLabel.Size = new System.Drawing.Size(76, 13);
            this.deviceBatteryLabel.TabIndex = 11;
            this.deviceBatteryLabel.Text = "Device battery";
            // 
            // deviceConnectionLabel
            // 
            this.deviceConnectionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deviceConnectionLabel.AutoSize = true;
            this.deviceConnectionLabel.Location = new System.Drawing.Point(6, 133);
            this.deviceConnectionLabel.Name = "deviceConnectionLabel";
            this.deviceConnectionLabel.Size = new System.Drawing.Size(95, 13);
            this.deviceConnectionLabel.TabIndex = 10;
            this.deviceConnectionLabel.Text = "Device connected";
            // 
            // inputDeviceComboBox
            // 
            this.inputDeviceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.inputDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputDeviceComboBox.FormattingEnabled = true;
            this.inputDeviceComboBox.Location = new System.Drawing.Point(11, 50);
            this.inputDeviceComboBox.Name = "inputDeviceComboBox";
            this.inputDeviceComboBox.Size = new System.Drawing.Size(198, 21);
            this.inputDeviceComboBox.Sorted = true;
            this.inputDeviceComboBox.TabIndex = 8;
            this.inputDeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.inputDeviceComboBox_SelectedIndexChanged);
            // 
            // dataGroupBox
            // 
            dataGroupBox.Controls.Add(this.rollLabel);
            dataGroupBox.Controls.Add(this.pitchLabel);
            dataGroupBox.Controls.Add(this.yawLabel);
            dataGroupBox.Controls.Add(this.thrustLabel);
            dataGroupBox.Controls.Add(this.pidDataLabel);
            dataGroupBox.Location = new System.Drawing.Point(231, 14);
            dataGroupBox.Name = "dataGroupBox";
            dataGroupBox.Size = new System.Drawing.Size(223, 154);
            dataGroupBox.TabIndex = 26;
            dataGroupBox.TabStop = false;
            dataGroupBox.Text = "Data";
            // 
            // rollLabel
            // 
            this.rollLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rollLabel.AutoSize = true;
            this.rollLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.rollLabel.Location = new System.Drawing.Point(90, 19);
            this.rollLabel.Name = "rollLabel";
            this.rollLabel.Size = new System.Drawing.Size(70, 14);
            this.rollLabel.TabIndex = 18;
            this.rollLabel.Text = "Roll: {0}";
            // 
            // pitchLabel
            // 
            this.pitchLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.pitchLabel.Location = new System.Drawing.Point(83, 33);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(77, 14);
            this.pitchLabel.TabIndex = 17;
            this.pitchLabel.Text = "Pitch: {0}";
            // 
            // yawLabel
            // 
            this.yawLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.yawLabel.AutoSize = true;
            this.yawLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.yawLabel.Location = new System.Drawing.Point(97, 47);
            this.yawLabel.Name = "yawLabel";
            this.yawLabel.Size = new System.Drawing.Size(63, 14);
            this.yawLabel.TabIndex = 19;
            this.yawLabel.Text = "Yaw: {0}";
            // 
            // thrustLabel
            // 
            this.thrustLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thrustLabel.AutoSize = true;
            this.thrustLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.thrustLabel.Location = new System.Drawing.Point(76, 61);
            this.thrustLabel.Name = "thrustLabel";
            this.thrustLabel.Size = new System.Drawing.Size(84, 14);
            this.thrustLabel.TabIndex = 20;
            this.thrustLabel.Text = "Thrust: {0}";
            // 
            // pidDataLabel
            // 
            this.pidDataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pidDataLabel.AutoSize = true;
            this.pidDataLabel.Font = new System.Drawing.Font("Consolas", 9F);
            this.pidDataLabel.Location = new System.Drawing.Point(6, 87);
            this.pidDataLabel.Name = "pidDataLabel";
            this.pidDataLabel.Size = new System.Drawing.Size(63, 14);
            this.pidDataLabel.TabIndex = 21;
            this.pidDataLabel.Text = "PID data";
            // 
            // inputConfigGroupBox
            // 
            inputConfigGroupBox.Controls.Add(this.thrustBaseTextBox);
            inputConfigGroupBox.Controls.Add(thrustBaseText);
            inputConfigGroupBox.Controls.Add(this.thrustExpTextBox);
            inputConfigGroupBox.Controls.Add(thrustExpText);
            inputConfigGroupBox.Controls.Add(this.yawExpTextBox);
            inputConfigGroupBox.Controls.Add(yawExpText);
            inputConfigGroupBox.Controls.Add(this.pitchExpTextBox);
            inputConfigGroupBox.Controls.Add(pitchExpText);
            inputConfigGroupBox.Controls.Add(this.rollExpTextBox);
            inputConfigGroupBox.Controls.Add(rollExpText);
            inputConfigGroupBox.Controls.Add(this.deadZoneCheckBox);
            inputConfigGroupBox.Location = new System.Drawing.Point(10, 174);
            inputConfigGroupBox.Name = "inputConfigGroupBox";
            inputConfigGroupBox.Size = new System.Drawing.Size(444, 127);
            inputConfigGroupBox.TabIndex = 27;
            inputConfigGroupBox.TabStop = false;
            inputConfigGroupBox.Text = "Input Config";
            // 
            // yawExpTextBox
            // 
            this.yawExpTextBox.DecimalPlaces = 2;
            this.yawExpTextBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.yawExpTextBox.Location = new System.Drawing.Point(194, 68);
            this.yawExpTextBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.yawExpTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.yawExpTextBox.Name = "yawExpTextBox";
            this.yawExpTextBox.Size = new System.Drawing.Size(62, 20);
            this.yawExpTextBox.TabIndex = 43;
            this.yawExpTextBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.yawExpTextBox.ValueChanged += new System.EventHandler(this.OnInputConfigChange);
            // 
            // yawExpText
            // 
            yawExpText.AutoSize = true;
            yawExpText.Location = new System.Drawing.Point(140, 70);
            yawExpText.Name = "yawExpText";
            yawExpText.Size = new System.Drawing.Size(48, 13);
            yawExpText.TabIndex = 42;
            yawExpText.Text = "Yaw exp";
            // 
            // pitchExpTextBox
            // 
            this.pitchExpTextBox.DecimalPlaces = 2;
            this.pitchExpTextBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.pitchExpTextBox.Location = new System.Drawing.Point(194, 42);
            this.pitchExpTextBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pitchExpTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.pitchExpTextBox.Name = "pitchExpTextBox";
            this.pitchExpTextBox.Size = new System.Drawing.Size(62, 20);
            this.pitchExpTextBox.TabIndex = 41;
            this.pitchExpTextBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.pitchExpTextBox.ValueChanged += new System.EventHandler(this.OnInputConfigChange);
            // 
            // pitchExpText
            // 
            pitchExpText.AutoSize = true;
            pitchExpText.Location = new System.Drawing.Point(137, 44);
            pitchExpText.Name = "pitchExpText";
            pitchExpText.Size = new System.Drawing.Size(51, 13);
            pitchExpText.TabIndex = 40;
            pitchExpText.Text = "Pitch exp";
            // 
            // rollExpTextBox
            // 
            this.rollExpTextBox.DecimalPlaces = 2;
            this.rollExpTextBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.rollExpTextBox.Location = new System.Drawing.Point(194, 16);
            this.rollExpTextBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.rollExpTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.rollExpTextBox.Name = "rollExpTextBox";
            this.rollExpTextBox.Size = new System.Drawing.Size(62, 20);
            this.rollExpTextBox.TabIndex = 39;
            this.rollExpTextBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.rollExpTextBox.ValueChanged += new System.EventHandler(this.OnInputConfigChange);
            // 
            // rollExpText
            // 
            rollExpText.AutoSize = true;
            rollExpText.Location = new System.Drawing.Point(143, 18);
            rollExpText.Name = "rollExpText";
            rollExpText.Size = new System.Drawing.Size(45, 13);
            rollExpText.TabIndex = 38;
            rollExpText.Text = "Roll exp";
            // 
            // deadZoneCheckBox
            // 
            this.deadZoneCheckBox.AutoSize = true;
            this.deadZoneCheckBox.Checked = true;
            this.deadZoneCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deadZoneCheckBox.Location = new System.Drawing.Point(11, 19);
            this.deadZoneCheckBox.Name = "deadZoneCheckBox";
            this.deadZoneCheckBox.Size = new System.Drawing.Size(80, 17);
            this.deadZoneCheckBox.TabIndex = 36;
            this.deadZoneCheckBox.Text = "Dead Zone";
            this.deadZoneCheckBox.UseVisualStyleBackColor = true;
            this.deadZoneCheckBox.CheckedChanged += new System.EventHandler(this.OnInputConfigChange);
            // 
            // inputGraphsGroupBox
            // 
            inputGraphsGroupBox.Controls.Add(this.inputCurves);
            inputGraphsGroupBox.Location = new System.Drawing.Point(10, 307);
            inputGraphsGroupBox.Name = "inputGraphsGroupBox";
            inputGraphsGroupBox.Size = new System.Drawing.Size(400, 400);
            inputGraphsGroupBox.TabIndex = 28;
            inputGraphsGroupBox.TabStop = false;
            inputGraphsGroupBox.Text = "Input Graphs";
            // 
            // inputCurves
            // 
            this.inputCurves.BaseLine = 0D;
            this.inputCurves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputCurves.LeftBottomName = "Yaw";
            this.inputCurves.LeftTopName = "Roll";
            this.inputCurves.Location = new System.Drawing.Point(3, 16);
            this.inputCurves.Name = "inputCurves";
            this.inputCurves.RightBottomName = "Thrust";
            this.inputCurves.RightTopName = "Pitch";
            this.inputCurves.ShowBaseLine = true;
            this.inputCurves.ShowHalfScaling = false;
            this.inputCurves.Size = new System.Drawing.Size(394, 381);
            this.inputCurves.TabIndex = 37;
            this.inputCurves.ValueMax = 500D;
            this.inputCurves.ValueMin = -500D;
            // 
            // searchTimer
            // 
            this.searchTimer.Enabled = true;
            this.searchTimer.Interval = 2500;
            this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 16;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // thrustExpTextBox
            // 
            this.thrustExpTextBox.DecimalPlaces = 2;
            this.thrustExpTextBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.thrustExpTextBox.Location = new System.Drawing.Point(194, 94);
            this.thrustExpTextBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.thrustExpTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.thrustExpTextBox.Name = "thrustExpTextBox";
            this.thrustExpTextBox.Size = new System.Drawing.Size(62, 20);
            this.thrustExpTextBox.TabIndex = 45;
            this.thrustExpTextBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.thrustExpTextBox.ValueChanged += new System.EventHandler(this.OnInputConfigChange);
            // 
            // thrustExpText
            // 
            thrustExpText.AutoSize = true;
            thrustExpText.Location = new System.Drawing.Point(131, 96);
            thrustExpText.Name = "thrustExpText";
            thrustExpText.Size = new System.Drawing.Size(57, 13);
            thrustExpText.TabIndex = 44;
            thrustExpText.Text = "Thrust exp";
            // 
            // thrustBaseTextBox
            // 
            this.thrustBaseTextBox.DecimalPlaces = 2;
            this.thrustBaseTextBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.thrustBaseTextBox.Location = new System.Drawing.Point(341, 94);
            this.thrustBaseTextBox.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.thrustBaseTextBox.Name = "thrustBaseTextBox";
            this.thrustBaseTextBox.Size = new System.Drawing.Size(62, 20);
            this.thrustBaseTextBox.TabIndex = 47;
            this.thrustBaseTextBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.thrustBaseTextBox.ValueChanged += new System.EventHandler(this.OnInputConfigChange);
            // 
            // thrustBaseText
            // 
            thrustBaseText.AutoSize = true;
            thrustBaseText.Location = new System.Drawing.Point(272, 96);
            thrustBaseText.Name = "thrustBaseText";
            thrustBaseText.Size = new System.Drawing.Size(63, 13);
            thrustBaseText.TabIndex = 46;
            thrustBaseText.Text = "Thrust base";
            // 
            // FlightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(inputConfigGroupBox);
            this.Controls.Add(inputGraphsGroupBox);
            this.Controls.Add(dataGroupBox);
            this.Controls.Add(deviceGroupBox);
            this.DoubleBuffered = true;
            this.Name = "FlightControl";
            this.Size = new System.Drawing.Size(464, 719);
            deviceGroupBox.ResumeLayout(false);
            deviceGroupBox.PerformLayout();
            dataGroupBox.ResumeLayout(false);
            dataGroupBox.PerformLayout();
            inputConfigGroupBox.ResumeLayout(false);
            inputConfigGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.yawExpTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchExpTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollExpTextBox)).EndInit();
            inputGraphsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thrustExpTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thrustBaseTextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox inputDeviceComboBox;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.Label rollLabel;
        private System.Windows.Forms.Label yawLabel;
        private System.Windows.Forms.Label thrustLabel;
        private System.Windows.Forms.Label pidDataLabel;
        private System.Windows.Forms.Button searchDeviceButton;
        private System.Windows.Forms.Label deviceBatteryLabel;
        private System.Windows.Forms.Label deviceConnectionLabel;
        private System.Windows.Forms.Timer searchTimer;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.CheckBox deadZoneCheckBox;
        private QuadGraphControl inputCurves;
        private System.Windows.Forms.NumericUpDown yawExpTextBox;
        private System.Windows.Forms.NumericUpDown pitchExpTextBox;
        private System.Windows.Forms.NumericUpDown rollExpTextBox;
        private System.Windows.Forms.NumericUpDown thrustBaseTextBox;
        private System.Windows.Forms.NumericUpDown thrustExpTextBox;
    }
}
