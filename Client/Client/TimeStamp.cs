using System;

namespace Client
{
    /// <summary>
    /// Get current time to stamp client/server activity
    /// </summary>
    class TimeStamp
    {
        /// <summary>
        /// Return string holding the time stamp.
        /// </summary>
        public string GetCurrentTimeStamp =>
            DateTime.Now.ToString("[hh:mm:ss] ");
    }
}
