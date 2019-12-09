using System;
using System.Collections.Generic;
using System.Timers;
using EAS_Development_Interfaces;

namespace SystemPlugin.EventSources
{
    public class DateTime : IEventSource
    {
        public bool IsRunning => _timer.Enabled;
        private readonly Timer _timer;
        public DateTime()
        {
            _timer = new Timer();
            _timer.Elapsed += OnElapsedTime;
            _timer.Interval = 1000;
            _timer.Enabled = true;
            _timer.Start();
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            EventFired?.Invoke(source, new DictionaryEventArgs()
            {
                Values = new Dictionary<string, object>()
                {
                    {"Time",System.DateTime.Now }
                }
            });
        }

        public string Name => "Clock";
        public string Description => "Event Fires After Given interval";
        public event EventHandler<DictionaryEventArgs> EventFired;
        public List<string> AvailableValues => new List<string>(){"Time"};
    }
}
