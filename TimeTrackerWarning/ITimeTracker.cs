﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerWarning
{
    enum TimeTrackingState { 
        Starting,
        Unknown, 
        AppNotStarted, 
        Inactive, 
        Active 
    }

    interface ITimeTracker
    {
        TimeTrackingState CheckState();
    }
}
