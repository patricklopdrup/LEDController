using System.Windows;

namespace LEDControllerWPF.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new DataPort();
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //todo: save settings

            this.Close();
        }
    }
}
