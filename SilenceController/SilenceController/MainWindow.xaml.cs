using SilenceController.Configurations;
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

        DateTime _lastAction = DateTime.Now;

        Timer _timer;

        private void CheckTime(object state)
        {
            var fromLastActionSeconds = DateTime.Now.TimeOfDay.Subtract(_lastAction.TimeOfDay).TotalSeconds;
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

            _timer = new Timer(CheckTime, null, new TimeSpan(-1), new TimeSpan(0, 0, 1));
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
    }
}
