using System;
using System.Collections.Generic;
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


namespace LEDControllerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            CsgoHandling csgoHandling = new CsgoHandling();

            //DataPort dataPort = new DataPort();
            //dataPort.SendData();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MyColor color = new MyColor(ColorPalette);
        }

        private void ColorPalette_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

    }
}
