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
        List<ITimeTracker> _trackers = new List<ITimeTracker>();

        DateTime _startTime;

 
        // Measure today's time
        DateTime _today;
        TimeSpan _timeWorkedToday;
        TimeSpan _timeWorkedYesterday;
        const double _hourlyRate = 30;

        public MainForm()
        {
            _trackers.Add(new ODesk());
            _trackers.Add(new Tahometer());

            _startTime = DateTime.Now;
            _today = DateTime.Now.Date;
            InitializeComponent();
        }

        bool IsTracking()
        {
            foreach (var tracker in _trackers)
            {
                if (tracker.CheckState() == TimeTrackingState.Active) { return true; }
            }
            return false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            bool isWorkActive = IsWorkActive();
            bool isTracking = IsTracking();

            this.Visible = isWorkActive && !isTracking;
            blinkLabelTimer.Enabled = this.Visible;

            if (isTracking)
            {
                if (_today != DateTime.Now.Date)
                {
                    _today = DateTime.Now.Date;
                    _timeWorkedYesterday = _timeWorkedToday;
                    _timeWorkedToday = TimeSpan.Zero;
                }

                _timeWorkedToday = _timeWorkedToday + TimeSpan.FromMilliseconds(checkODeskTimer.Interval);
                UpdateTimeAndEarnings();
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
            this.Visible = IsWorkActive() && !IsTracking();
        }

        bool IsWorkActive()
        {
            return true;
            //return HasProcessWithTitlePart(
            //    "flowol4", "robotmesh", "ConsoleTest", "PlaySerialScript", "FireBreath");
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


    }
}
