using System.Windows;

namespace LB5_LOBKOVA
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RacingControl.StartRace();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            RacingControl.PauseRace();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            RacingControl.ResetRace();
        }
    }
}
