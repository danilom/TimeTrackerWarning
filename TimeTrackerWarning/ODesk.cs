using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace TimeTrackerWarning
{
    class ODesk : ITimeTracker
    {
        public TimeTrackingState CheckState()
        {
            var procs = Process.GetProcessesByName("oDeskTeam");
            if (procs.Length == 0)
            {
                return TimeTrackingState.Inactive;
            }

            if (procs.Length == 0) { return TimeTrackingState.Inactive; }
            var handle = procs[0].MainWindowHandle;
            if (handle == IntPtr.Zero) { return TimeTrackingState.Inactive; }

            try
            {
                var app = AutomationElement.FromHandle(handle);
                return HasStartButton(app) ? TimeTrackingState.Inactive : TimeTrackingState.Active;
            }
            catch (Exception)
            {
                // TODO: log
                return TimeTrackingState.Unknown;
            }
        }

        bool HasStartButton(AutomationElement targetApp)
        {
            if (targetApp == null) { return false; }

            // The control type we're looking for; in this case 'Document'
            PropertyCondition isButton =
                new PropertyCondition(
                AutomationElement.ControlTypeProperty,
                ControlType.Button);

            var results = new List<AutomationElement>();
            foreach (AutomationElement ae in targetApp.FindAll(TreeScope.Descendants, isButton))
            {
                results.Add(ae);
            }

            return results.Select(x => x.Current.Name).Contains("Start");
        }


    }
}
