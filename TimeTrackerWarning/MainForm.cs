using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace TimeTrackerWarning
{
    public partial class MainForm : Form
    {
        ITimeTracker _tracker = new Tahometer();

        DateTime _startTime;

 
        // Measure today's time
        DateTime _today;
        TimeSpan _timeWorkedToday;
        TimeSpan _timeWorkedYesterday;
        const double _hourlyRate = 55;

        public MainForm()
        {
            _startTime = DateTime.Now;
            _today = DateTime.Now.Date;
            InitializeComponent();
        }

        TimeTrackingState _trackingState = TimeTrackingState.Unknown;
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
                    if (_today != DateTime.Now.Date)
                    {
                        _today = DateTime.Now.Date;
                        _timeWorkedYesterday = _timeWorkedToday;
                        _timeWorkedToday = TimeSpan.Zero;
                    }
                    _timeWorkedToday = _timeWorkedToday + TimeSpan.FromMilliseconds(checkODeskTimer.Interval);
                    UpdateTimeAndEarnings();

                    this.Visible = false;

                    blinkLabelTimer.Enabled = false;
                    break;
                case TimeTrackingState.Inactive:
                case TimeTrackingState.Unknown:
                    label1.Text = _trackingState == TimeTrackingState.Inactive ? 
                        "Time NOT tracked." : "State unknown";
                    this.BackColor = Color.Red;
                    this.Visible = true;
                    blinkLabelTimer.Enabled = true;
                    break;
                case TimeTrackingState.AppNotStarted:
                    label1.Text = "App NOT started.";
                    this.BackColor = Color.DarkRed;
                    this.ControlBox = true;

                    this.Visible = true;
                    blinkLabelTimer.Enabled = false;
                    break;

            }
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
            CheckStateAndUpdate();
        }

        bool HasProcessWithTitlePart(params String[] titles)
        {
            var ps = Process.GetProcesses();
            var names = ps.Select(p => p.MainWindowTitle.ToLowerInvariant());

            foreach (var t in titles)
            {
                var n = names.FirstOrDefault(x => x.Contains(t.ToLowerInvariant()));
                if (n != null)
                {
                    Console.WriteLine(n);
                    return true;
                }
            }
            return false;
        }


        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            checkODeskTimer.Enabled = true;
            showAgainTimer.Enabled = false;
        }


        void UpdateTimeAndEarnings()
        {
            var sb = new StringBuilder();

            sb.AppendFormat(@"Today: {0:hh\:mm}/${1:0}", 
                _timeWorkedToday,
                _timeWorkedToday.TotalHours * _hourlyRate);

            if (_timeWorkedYesterday > TimeSpan.Zero)
            {
                sb.AppendLine();
                sb.AppendFormat(@"Yesteday: {0:hh\:mm}", _timeWorkedYesterday);
            }
            if (_startTime > _today)
            {
                sb.AppendLine();
                sb.Append("(incomplete data)");
            }

            notifyIcon.Text = sb.ToString();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Yes: hide the warning for 10 minutes\n"
                + "No: exit the tracker app"
                , "Hide or exit?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                checkODeskTimer.Enabled = false;
                showAgainTimer.Enabled = true;
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void showAgainTimer_Tick(object sender, EventArgs e)
        {
            checkODeskTimer.Enabled = true;
            showAgainTimer.Enabled = false;
        }
    }
}
