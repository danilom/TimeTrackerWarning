namespace TimeTrackerWarning
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.checkAppStateTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.blinkLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.snoozeTimer = new System.Windows.Forms.Timer(this.components);
            this.bSnooze = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkAppStateTimer
            // 
            this.checkAppStateTimer.Enabled = true;
            this.checkAppStateTimer.Interval = 5000;
            this.checkAppStateTimer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(22, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1157, 282);
            this.label1.TabIndex = 0;
            this.label1.Text = "Checking...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blinkLabelTimer
            // 
            this.blinkLabelTimer.Interval = 300;
            this.blinkLabelTimer.Tick += new System.EventHandler(this.blinkLabelTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Time Tracker Warning";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // snoozeTimer
            // 
            this.snoozeTimer.Interval = 1000;
            this.snoozeTimer.Tick += new System.EventHandler(this.snoozeTimer_Tick);
            // 
            // bSnooze
            // 
            this.bSnooze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSnooze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSnooze.ForeColor = System.Drawing.Color.White;
            this.bSnooze.Location = new System.Drawing.Point(829, 9);
            this.bSnooze.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.bSnooze.Name = "bSnooze";
            this.bSnooze.Size = new System.Drawing.Size(244, 74);
            this.bSnooze.TabIndex = 1;
            this.bSnooze.Text = "Snooze for 10 min";
            this.bSnooze.UseVisualStyleBackColor = true;
            this.bSnooze.Click += new System.EventHandler(this.bSnooze_Click);
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.ForeColor = System.Drawing.Color.White;
            this.bClose.Location = new System.Drawing.Point(1084, 9);
            this.bClose.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(108, 74);
            this.bClose.TabIndex = 2;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(1201, 316);
            this.ControlBox = false;
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bSnooze);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Opacity = 0.4D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Time Tracker Warning";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer checkAppStateTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer blinkLabelTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer snoozeTimer;
        private System.Windows.Forms.Button bSnooze;
        private System.Windows.Forms.Button bClose;
    }
}

