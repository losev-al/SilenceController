using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    /// <summary>
    /// Делегат для события начала интервала
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void IntervalStartedHandler(object sender, IntervalStartedArgs e);
}
