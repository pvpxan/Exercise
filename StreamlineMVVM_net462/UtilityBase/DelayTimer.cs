using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace StreamlineMVVM
{
    public class DelayTimer : IDisposable
    {
        private Timer timer = null;
        public Action ElapsedAction { get; set; }

        public DelayTimer()
        {
            try
            {
                timer = new Timer(1000);
                timer.AutoReset = false;
                timer.Elapsed += timerElapsed;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error attempting to initialize DelayTimer.", Ex);
            }
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            if (ElapsedAction != null)
            {
                ElapsedAction();
            }
        }

        public void SetDelay(int delay)
        {
            if (timer != null && timer.Enabled)
            {
                return;
            }

            timer.Interval = delay;
        }

        public void StartDelay()
        {
            if (timer == null)
            {
                return;
            }

            try
            {
                timer.Stop();
                timer.Start();
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error attempting to restart DelayTimer.", Ex);
            }
        }

        public void StopDelay()
        {
            if (timer == null)
            {
                return;
            }

            try
            {
                timer.Stop();
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error attempting to stop DelayTimer.", Ex);
            }
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
