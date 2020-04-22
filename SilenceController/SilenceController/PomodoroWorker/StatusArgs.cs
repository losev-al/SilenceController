using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    public class StatusArgs
    {
        /// <summary>
        /// Строка с сообщением о текущем состоянии
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Кнопки для показа во всплывающем окне
        /// </summary>
        public BallonAction[] Actions { get; set; }
    }
}
