using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerWarning.Properties;

namespace TimeTrackerWarning
{
    class TopTracker : ITimeTracker
    {
        public TimeTrackingState CheckState()
        {
            var procs = Process.GetProcessesByName("TopTracker");
            if (procs.Length == 0) 
            { 
                return TimeTrackingState.AppNotStarted; 
            }

            // Check the icon in the system tray
            Bitmap notificationIcons;
            try
            {
                notificationIcons = Utils.GetNotificationIconsImage();
            }
            catch (Exception)
            {
                return TimeTrackingState.Unknown;
            }

            if (Utils.ContainsBitmap(Resources.TopTrackerRunning, notificationIcons))
            {
                return TimeTrackingState.Active;
            }
            if (Utils.ContainsBitmap(Resources.TopTrackerStopped, notificationIcons))
            {
                return TimeTrackingState.Inactive;
            }
            return TimeTrackingState.Unknown;
        }

    }

}
