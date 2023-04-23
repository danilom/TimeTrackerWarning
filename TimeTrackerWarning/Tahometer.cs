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
    class Tahometer : ITimeTracker
    {
        public TimeTrackingState CheckState()
        {
            var procs = Process.GetProcessesByName("tahometer");
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

            if (Utils.ContainsBitmap(Resources.TahometerRunning, notificationIcons))
            {
                return TimeTrackingState.Active;
            }
            if (Utils.ContainsBitmap(Resources.TahometerStopped, notificationIcons))
            {
                return TimeTrackingState.Inactive;
            }
            return TimeTrackingState.Unknown;
        }

    }

}
