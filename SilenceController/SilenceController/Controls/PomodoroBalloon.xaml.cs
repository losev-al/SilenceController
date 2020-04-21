using SilenceController.PomodoroWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SilenceController.Controls
{
    /// <summary>
    /// Interaction logic for PomodoroBalloon.xaml
    /// </summary>
    public partial class PomodoroBalloon : UserControl
    {
        public PomodoroBalloon(string header, string content)
        {
            InitializeComponent();
            tbHeader.Text = header;
            tbContent.Visibility = Visibility.Visible;
            tbContent.Text = content;
            spActions.Visibility = Visibility.Collapsed;
        }

        public PomodoroBalloon(string header, BallonAction[] actions)
        {
            InitializeComponent();
            tbHeader.Text = header;
            tbContent.Visibility = Visibility.Collapsed;
            spActions.Visibility = Visibility.Visible;
            foreach (var action in actions)
            {
                Button btn = new Button();
                btn.Content = action.Caption;
                btn.Click += (sender, e) => action.Action?.Invoke();
                spActions.Children.Add(btn);
            }
        }
    }
}
