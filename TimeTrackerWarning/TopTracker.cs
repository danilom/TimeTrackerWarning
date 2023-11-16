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
    // WARNING: ScreenScaling and bitmaps hardcoded for 1.5x scaling (Windows Setting)
    // Won't work in other resolutions
    class TopTracker : ITimeTracker
    {
        public TopTracker()
        {
            // Difficult to get, hardcode it for now
            Utils.ScreenScaling = 1.5;
        }

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

            //if (Utils.ContainsBitmap(Resources.TopTrackerRunning2, notificationIcons)
            if (Utils.ContainsBitmap(Resources.TopTrackerRunning150, notificationIcons))
            {
                return TimeTrackingState.Active;
            }
            //if (Utils.ContainsBitmap(Resources.TopTrackerStopped2, notificationIcons)
            if (Utils.ContainsBitmap(Resources.TopTrackerStopped150, notificationIcons))
            {
                return TimeTrackingState.Inactive;
            }
            return TimeTrackingState.Unknown;
          }

    }

}
