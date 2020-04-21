using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilenceController.PomodoroWorker
{
    public class PomodoroWorker
    {
        #region Поля
        /// <summary>
        /// Таймер на котором все крутится
        /// </summary>
        Timer _timer;

        /// <summary>
        /// Статус
        /// </summary>
        PomodoroStatuses _status = PomodoroStatuses.Idle;

        /// <summary>
        /// Время старта интервала
        /// </summary>
        TimeSpan _intervalStart;

        /// <summary>
        /// Количество помидорок выполненных в серии
        /// </summary>
        int _currentSeriesPomodoroCount = 0;
        #endregion

        #region Свойства
        /// <summary>
        /// Длительность рабочего интервала
        /// </summary>
        public TimeSpan WorkInterval { get; set; }

        /// <summary>
        /// Длительность интервала отдыха
        /// </summary>
        public TimeSpan BreakInterval { get; set; }

        /// <summary>
        /// Сколько циклов в серии
        /// </summary>
        public int WorkCycles { get; set; }
        #endregion

        #region События
        /// <summary>
        /// Событие вызываемое при начале интервала работы
        /// </summary>
        public event IntervalStartedHandler WorkIntervalStarted;

        /// <summary>
        /// Событие вызываемое при окончании интервала работы
        /// </summary>
        public event IntervalEndedHandler WorkIntervalEnded;

        /// <summary>
        /// Событие вызываемое при начале интервала отдыха
        /// </summary>
        public event IntervalStartedHandler BreakIntervalStarted;

        /// <summary>
        /// Событие вызываемое при окончании интервала отдыха
        /// </summary>
        public event IntervalEndedHandler BreakIntervalEnded;

        /// <summary>
        /// Событие вызываемое при начале интервала Ожидания
        /// </summary>
        public event Action IdleIntervalStarted;

        /// <summary>
        /// Событие говорящие о том, что реакция пользователя отработана
        /// </summary>
        public event Action SeriesEnded;
        #endregion

        public PomodoroWorker(TimeSpan workInterval, TimeSpan breakInterval, int workCycles)
        {
            WorkInterval = workInterval;
            BreakInterval = breakInterval;
            WorkCycles = workCycles;

            _timer = new Timer(TickWorker, null, new TimeSpan(-1), new TimeSpan(0, 0, 1));
        }

        private void TickWorker(object state)
        {
            switch (_status)
            {
                case PomodoroStatuses.Idle:
                    break;
                case PomodoroStatuses.WorkInterval:
                    if (CheckEndInterval(WorkInterval))
                    {
                        _status = PomodoroStatuses.AfterWorkInterval;
                        WorkIntervalEnded?.Invoke(
                            this,
                            new IntervalEndedArgs() { Actions = new[]
                                {
                                    _currentSeriesPomodoroCount < 4 ? new BallonAction() { Caption = "Отдых", Action = StartBreakInterval } : new BallonAction() { Caption = "Завершить", Action = () => SeriesEnded?.Invoke() },
                                    new BallonAction() { Caption = "Отмена", Action = InterruptSeries }
                                }
                            });
                    }
                    break;
                case PomodoroStatuses.AfterWorkInterval:
                    break;
                case PomodoroStatuses.BreakInterval:
                    if (CheckEndInterval(BreakInterval))
                    {
                        _status = PomodoroStatuses.AfterBreakInterval;
                        BreakIntervalEnded?.Invoke(
                            this,
                            new IntervalEndedArgs()
                            {
                                Actions = new[]
                                {
                                    new BallonAction() { Caption = "Помидорка", Action = StartWorkInterval },
                                    new BallonAction() { Caption = "Отмена", Action = InterruptSeries }
                                }
                            });
                    }
                    break;
                case PomodoroStatuses.AfterBreakInterval:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Начало помидорки (рабочего интервала)
        /// </summary>
        private void StartWorkInterval()
        {
            _currentSeriesPomodoroCount++;
            _intervalStart = DateTime.Now.TimeOfDay;
            _status = PomodoroStatuses.WorkInterval;
            WorkIntervalStarted?.Invoke(this, new IntervalStartedArgs() { IntervalLength = WorkInterval, IterationNumber = _currentSeriesPomodoroCount });
        }

        /// <summary>
        /// Начало перерыва в работе
        /// </summary>
        private void StartBreakInterval()
        {
            _intervalStart = DateTime.Now.TimeOfDay;
            _status = PomodoroStatuses.BreakInterval;
            BreakIntervalStarted?.Invoke(this, new IntervalStartedArgs() { IntervalLength = BreakInterval, IterationNumber = _currentSeriesPomodoroCount });
        }

        /// <summary>
        /// Прерывает серию
        /// </summary>
        private void InterruptSeries()
        {
            _status = PomodoroStatuses.Idle;
            IdleIntervalStarted();
        }

        /// <summary>
        /// Проверка на очончание интервала
        /// </summary>
        /// <param name="workInterval"></param>
        /// <returns></returns>
        private bool CheckEndInterval(TimeSpan workInterval)
        {
            bool result = false;
            var div = DateTime.Now.TimeOfDay.Subtract(_intervalStart);
            if (div >= workInterval)
            {
                result = true;
            }
            else if (div.TotalSeconds < 0)
            {
                throw new Exception("Отрицательной разницы между текущим временем и началом интервала быть не должно");
            }
            return result;
        }

        /// <summary>
        /// Запуск серии из помидорок и перерывов
        /// </summary>
        public void StartSeries()
        {
            _currentSeriesPomodoroCount = 0;

            StartWorkInterval();
        }
    }
}
