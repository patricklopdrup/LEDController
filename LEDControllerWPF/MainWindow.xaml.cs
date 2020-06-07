using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
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
using LEDControllerWPF.Settings;


namespace LEDControllerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataPort dataPort;
        private GameHandling gameHandling;
        private LedSelector ledSelector;
        private bool _isUsedBefore = false;

        

        public MainWindow()
        {
            InitializeComponent();
            /*ShowInTaskbar = true;
            WindowState = System.Windows.WindowState.Minimized;*/

            gameHandling = new GameHandling();

            dataPort = new DataPort();
            
            ledSelector = new LedSelector(LedStackPanel);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MyColor color = new MyColor(ColorPalette);

            if (!_isUsedBefore)
            {
                SettingsButton_Click(this, new RoutedEventArgs());
                _isUsedBefore = true;
            }
        }

        private void ColorPalette_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("selected er: " + dataPort.SelectedPort);
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void LedStackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseWasDownOn = e.Source as FrameworkElement;
            if (mouseWasDownOn != null)
            {
                string elementName = mouseWasDownOn.Name;
                Console.WriteLine(elementName);
            }
        }
    }
}
