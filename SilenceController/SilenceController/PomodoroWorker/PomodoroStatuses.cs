using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    /// <summary>
    /// Перечисление для статусов работы с помидором
    /// </summary>
    enum PomodoroStatuses
    {
        /// <summary>
        /// Ожидание
        /// </summary>
        Idle,
        /// <summary>
        /// Идет рабочий интервал (помидорка)
        /// </summary>
        WorkInterval,
        /// <summary>
        /// Рабочий интервал закончен, ожидание реакции пользователя
        /// </summary>
        AfterWorkInterval,
        /// <summary>
        /// Идет перерыв
        /// </summary>
        BreakInterval,
        /// <summary>
        /// Перерыв закончен, ожидание реакции пользователя
        /// </summary>
        AfterBreakInterval
    }
}
