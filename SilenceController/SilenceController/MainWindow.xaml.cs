using SilenceController.Configurations;
using SilenceController.Controls;
using SilenceController.PomodoroWorker;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SilenceController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] startupPaths = null;
        string[] killPaths = null;
        DateTime[] startupTimes = null;
        DateTime[] killTimes = null;
        /// <summary>
        /// Признак, вызывать отключение приложений по таймеру или нет
        /// </summary>
        bool useSchedule = false;

        DateTime _lastAction = DateTime.Now;

        Timer _timer;

        MediaPlayer _player = null;

        /// <summary>
        /// Воркер управляющий помидорками
        /// </summary>
        PomodoroWorker.PomodoroWorker _pomodoroWorker = null;

        private void CheckTime(object state)
        {
            var fromLastActionSeconds = DateTime.Now.TimeOfDay.Subtract(_lastAction.TimeOfDay).TotalSeconds;
            if (useSchedule)
            {
                if (fromLastActionSeconds > 5 || fromLastActionSeconds < 0)
                {
                    foreach (var time in startupTimes)
                    {
                        if (DateTime.Now.TimeOfDay.Subtract(time.TimeOfDay).TotalSeconds > 0 && DateTime.Now.TimeOfDay.Subtract(time.TimeOfDay).TotalSeconds < 5)
                        {
                            MenuItemRun_Click(null, null);
                            _lastAction = DateTime.Now;
                        }
                    }

                    foreach (var time in killTimes)
                    {
                        if (DateTime.Now.TimeOfDay.Subtract(time.TimeOfDay).TotalSeconds > 0 && DateTime.Now.TimeOfDay.Subtract(time.TimeOfDay).TotalSeconds < 5)
                        {
                            MenuItemKill_Click(null, null);
                            _lastAction = DateTime.Now;
                        }
                    }
                } 
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            StartupPaths startupPathsSection = (StartupPaths)cfg.Sections["StartupPaths"];
            if (startupPathsSection != null)
            {
                startupPaths = startupPathsSection.PathItems.Cast<PathElement>().Select(p => p.Value).ToArray();
            }
            else
            {
                throw new Exception("Отсутствует секция путей запуска");
            }

            KillPaths killPathsSection = (KillPaths)cfg.Sections["KillPaths"];
            if (killPathsSection != null)
            {
                killPaths = killPathsSection.PathItems.Cast<PathElement>().Select(p => p.Value).ToArray();
            }
            else
            {
                throw new Exception("Отсутствует секция путей остановки");
            }

            StartupTimes startupTimesSection = (StartupTimes)cfg.Sections["StartupTimes"];
            if (startupTimesSection != null)
            {
                startupTimes = startupTimesSection.TimeItems.Cast<TimeElement>().Select(p => DateTime.Parse(p.Value)).ToArray();
            }
            else
            {
                throw new Exception("Отсутствует секция времен запуска");
            }

            KillTimes killTimesSection = (KillTimes)cfg.Sections["KillTimes"];
            if (killTimesSection != null)
            {
                killTimes = killTimesSection.TimeItems.Cast<TimeElement>().Select(p => DateTime.Parse(p.Value)).ToArray();
            }
            else
            {
                throw new Exception("Отсутствует секция времен останова");
            }

            ApplicationMode applicationMode = (ApplicationMode)cfg.Sections["ApplicationMode"];
            useSchedule = applicationMode.UseSchedule.Value;

            Pomodoro pomodoro = (Pomodoro)cfg.Sections["Pomodoro"];
            _pomodoroWorker = new PomodoroWorker.PomodoroWorker(pomodoro.WorkInterval, pomodoro.BreakInterval, pomodoro.WorkCycles);
            _pomodoroWorker.WorkIntervalStarted += PomodoroWorker_WorkIntervalStarted;
            _pomodoroWorker.WorkIntervalEnded += PomodoroWorker_WorkIntervalEnded;
            _pomodoroWorker.BreakIntervalStarted += PomodoroWorker_BreakIntervalStarted;
            _pomodoroWorker.BreakIntervalEnded += PomodoroWorker_BreakIntervalEnded;
            _pomodoroWorker.IdleIntervalStarted += PomodoroWorker_IdleIntervalStarted;
            _pomodoroWorker.SeriesEnded += PomodoroWorker_SeriesEnded;

            _timer = new Timer(CheckTime, null, new TimeSpan(-1), new TimeSpan(0, 0, 1));

            _player = new MediaPlayer();
            _player.Open(new Uri(@"Audio\sound_21576.mp3", UriKind.Relative));
        }

        private void PomodoroWorker_IdleIntervalStarted()
        {
            myNotifyIcon.ShowCustomBalloon(new PomodoroBalloon("Серия прервана", $"В следующий раз получится лучше!"), System.Windows.Controls.Primitives.PopupAnimation.Slide, 5000);
            _player.Stop();
        }

        private void PomodoroWorker_BreakIntervalEnded(object sender, IntervalEndedArgs e)
        {
            SmartInvoke(() => myNotifyIcon.ShowCustomBalloon(new PomodoroBalloon("Отдых закончен", e.Actions), System.Windows.Controls.Primitives.PopupAnimation.Slide, null));            
        }

        private void PomodoroWorker_BreakIntervalStarted(object sender, IntervalStartedArgs e)
        {
            myNotifyIcon.ShowCustomBalloon(new PomodoroBalloon($"{e.IterationNumber} отдых начат", $"Интервал: {e.IntervalLength}"), System.Windows.Controls.Primitives.PopupAnimation.Slide, 5000);
        }

        private void PomodoroWorker_SeriesEnded()
        {
            myNotifyIcon.ShowCustomBalloon(new PomodoroBalloon("Серия закончена", $"Приятного отдыха"), System.Windows.Controls.Primitives.PopupAnimation.Slide, 5000);
        }

        private void PomodoroWorker_WorkIntervalEnded(object sender, PomodoroWorker.IntervalEndedArgs e)
        {
            SmartInvoke(() =>
            {
                myNotifyIcon.ShowCustomBalloon(new PomodoroBalloon("Помидор закончен", e.Actions), System.Windows.Controls.Primitives.PopupAnimation.Slide, null);
                _player.Play();
            });
        }

        private void SmartInvoke(Action action)
        {
            if (Thread.CurrentThread != this.Dispatcher.Thread)
            {
                this.Dispatcher.BeginInvoke((Action)(() => SmartInvoke(action)));
            }
            else
            {
                action?.Invoke();
            }
        }

        private void PomodoroWorker_WorkIntervalStarted(object sender, PomodoroWorker.IntervalStartedArgs e)
        {
            myNotifyIcon.ShowCustomBalloon(new PomodoroBalloon($"{e.IterationNumber} помидор начат", $"Интервал: {e.IntervalLength}"), System.Windows.Controls.Primitives.PopupAnimation.Slide, 5000);
        }

        private void MenuItemRun_Click(object sender, RoutedEventArgs e)
        {
            foreach (var path in startupPaths)
            {
                Process.Start(Environment.ExpandEnvironmentVariables(path));
            }
        }

        private void MenuItemKill_Click(object sender, RoutedEventArgs e)
        {
            foreach (var path in killPaths)
            {
                var procs = Process.GetProcessesByName(path);
                foreach (var item in procs)
                {
                    item.Kill();
                }
            }
        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PomodoroSeriesStart_Click(object sender, RoutedEventArgs e)
        {
            _pomodoroWorker.StartSeries();
        }
    }
}
