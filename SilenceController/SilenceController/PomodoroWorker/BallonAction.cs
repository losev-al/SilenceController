using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    /// <summary>
    /// Действия доступные 
    /// </summary>
    public class BallonAction
    {
        /// <summary>
        /// Заголовок для показа на кнопке
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Действие которое надо выполнить
        /// </summary>
        public Action Action { get; set; }
    }
}
