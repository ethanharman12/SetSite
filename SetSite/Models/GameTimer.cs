using System;

namespace SetSite.Models
{
    public class GameTimer
    {
        public double SecondsElapsed { get; set; }
        public DateTime? StartTime { get; set; }
        
        public double GetCurrentSecondsElapsed()
        {
            if (StartTime != null)
            {
                var span = DateTime.Now.Subtract(StartTime.Value);

                return SecondsElapsed + span.TotalSeconds;
            }

            return SecondsElapsed;
        }
    }
}