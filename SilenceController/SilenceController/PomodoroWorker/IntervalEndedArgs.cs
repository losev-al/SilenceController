using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    /// <summary>
    /// Аргументы события вызываемого при окончании интервала
    /// </summary>
    public class IntervalEndedArgs
    {
        /// <summary>
        /// Кнопки для показа во всплывающем окне
        /// </summary>
        public BallonAction[] Actions { get; set; }
    }
}
