using System;
using System.Collections.Generic;
using System.Text;

namespace WCore.Services.Tasks
{
    /// <summary>
    /// Represents default values related to task services
    /// </summary>
    public static partial class WCoreTaskDefaults
    {
        /// <summary>
        /// Gets a running schedule task path
        /// </summary>
        public static string ScheduleTaskPath => "scheduletask/runtask";
    }
}
