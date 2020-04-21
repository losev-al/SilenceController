using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    /// <summary>
    /// Делегат для события окончания интервала
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void IntervalEndedHandler(object sender, IntervalEndedArgs e);
}
