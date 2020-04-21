using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    /// <summary>
    /// Аргументы события начала интервала
    /// </summary>
    public class IntervalStartedArgs
    {
        /// <summary>
        /// Длительность интервала
        /// </summary>
        public TimeSpan IntervalLength { get; set; }

        /// <summary>
        /// Номер итерации в серии
        /// </summary>
        public int IterationNumber { get; set; }
    }
}
