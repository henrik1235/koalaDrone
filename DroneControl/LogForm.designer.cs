﻿namespace DroneControl
{
    partial class LogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.clientPage = new System.Windows.Forms.TabPage();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.dronePage = new System.Windows.Forms.TabPage();
            this.droneLogTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.clientPage.SuspendLayout();
            this.dronePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.clientPage);
            this.tabControl1.Controls.Add(this.dronePage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(528, 390);
            this.tabControl1.TabIndex = 2;
            // 
            // clientPage
            // 
            this.clientPage.Controls.Add(this.logTextBox);
            this.clientPage.Location = new System.Drawing.Point(4, 22);
            this.clientPage.Name = "clientPage";
            this.clientPage.Padding = new System.Windows.Forms.Padding(3);
            this.clientPage.Size = new System.Drawing.Size(520, 364);
            this.clientPage.TabIndex = 0;
            this.clientPage.Text = "Client";
            this.clientPage.UseVisualStyleBackColor = true;
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logTextBox.Location = new System.Drawing.Point(3, 3);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(514, 358);
            this.logTextBox.TabIndex = 2;
            // 
            // dronePage
            // 
            this.dronePage.Controls.Add(this.droneLogTextBox);
            this.dronePage.Location = new System.Drawing.Point(4, 22);
            this.dronePage.Name = "dronePage";
            this.dronePage.Padding = new System.Windows.Forms.Padding(3);
            this.dronePage.Size = new System.Drawing.Size(520, 364);
            this.dronePage.TabIndex = 1;
            this.dronePage.Text = "Drone";
            this.dronePage.UseVisualStyleBackColor = true;
            // 
            // droneLogTextBox
            // 
            this.droneLogTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.droneLogTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droneLogTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.droneLogTextBox.Location = new System.Drawing.Point(3, 3);
            this.droneLogTextBox.Multiline = true;
            this.droneLogTextBox.Name = "droneLogTextBox";
            this.droneLogTextBox.ReadOnly = true;
            this.droneLogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.droneLogTextBox.Size = new System.Drawing.Size(514, 358);
            this.droneLogTextBox.TabIndex = 7;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 390);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogForm";
            this.Text = "Log";
            this.tabControl1.ResumeLayout(false);
            this.clientPage.ResumeLayout(false);
            this.clientPage.PerformLayout();
            this.dronePage.ResumeLayout(false);
            this.dronePage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage clientPage;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.TabPage dronePage;
        private System.Windows.Forms.TextBox droneLogTextBox;
    }
}