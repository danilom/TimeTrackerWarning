using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace TimeTrackerWarning
{
    public partial class MainForm : Form
    {
        ITimeTracker _tracker = new TopTracker();

        // Settings
        readonly string TITLE = "Time Tracker Warning";
        readonly TimeSpan _snoozeDuration = new TimeSpan(0, 10, 0);
        DateTime _snoozeEndTime = DateTime.MaxValue;

        public MainForm()
        {
            this.Text = TITLE;
            InitializeComponent();
            UpdateTrayTooltip();
            this.bSnooze.Text = "Snooze for " + _snoozeDuration.TotalMinutes + " min";
        }

        TimeTrackingState _trackingState = TimeTrackingState.Starting;
        private void timer_Tick(object sender, EventArgs e)
        {
            CheckStateAndUpdate();
        }
        private void CheckStateAndUpdate()
        {
            var newState = _tracker.CheckState();
            if (newState == _trackingState) { return; }
            _trackingState = newState;

            switch (_trackingState)
            {
                case TimeTrackingState.Active:
                    label1.Text = "Active";
                    this.Visible = false; // Window not visible, no need to bother the user
                    blinkLabelTimer.Enabled = false;
                    break;
                case TimeTrackingState.Inactive:
                case TimeTrackingState.Unknown:
                    label1.Text = _trackingState == TimeTrackingState.Inactive ? 
                        "Time NOT tracked." : "State unknown";
                    this.BackColor = Color.Red;
                    this.Visible = true;

                    blinkLabelTimer.Enabled = true;
                    label1.Visible = true;
                    break;
                case TimeTrackingState.AppNotStarted:
                    label1.Text = "App NOT started.";
                    this.BackColor = Color.DarkRed;
                    this.Visible = true;

                    blinkLabelTimer.Enabled = false;
                    break;
            }

            UpdateTrayTooltip();

            // Always show the label on state change
            label1.Visible = true;
        }


        private void blinkLabelTimer_Tick(object sender, EventArgs e)
        {
            label1.Visible = !label1.Visible;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var sb = Screen.AllScreens.Last().WorkingArea;
            this.Location = new Point(sb.Right - this.Width, sb.Top + 150);
            //this.SetBounds((int)(sb.X + 0.5 * sb.Width), sb.Y + 20, sb.Width / 2, sb.Height / 3);
            this.Visible = false;
            //CheckStateAndUpdate();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            SnoozeEnd();
        }

        void UpdateTrayTooltip()
        {
            var sb = new StringBuilder();

            sb.AppendLine(TITLE);
            
            if (snoozeTimer.Enabled)
            {
                sb.AppendLine("Snooze until: " + (_snoozeEndTime));
            }

            // Max 64
            var s = sb.ToString();
            if (s.Length >= 64) { s = s.Substring(0, 63); }

            notifyIcon.Text = s;
        }

        private void snoozeTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now > _snoozeEndTime)
            {
                SnoozeEnd();
            }
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Close " + TITLE + "?"
                ,TITLE, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void bSnooze_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Snooze " + TITLE + " for " + _snoozeDuration.TotalMinutes + " minutes?"
                , TITLE, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                SnoozeStart();
            }
        }

        void SnoozeStart()
        {
            checkAppStateTimer.Enabled = false;
            snoozeTimer.Enabled = true;
            _snoozeEndTime = DateTime.Now + _snoozeDuration;
            this.Visible = false;
            UpdateTrayTooltip();
        }

        // Reactivate the window after a snooze
        void SnoozeEnd()
        {
            this.Visible = true;
            checkAppStateTimer.Enabled = true;
            snoozeTimer.Enabled = false;
            _snoozeEndTime = DateTime.MaxValue;
            UpdateTrayTooltip();
        }

    }
}
