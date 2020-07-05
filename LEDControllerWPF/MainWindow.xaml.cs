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

            // Don't open settings in debug mode
            if (MyDebug.isDebug)
            {
                _isUsedBefore = true;
            }

            gameHandling = new GameHandling();

            dataPort = new DataPort();
            
            ledSelector = new LedSelector(LedStackPanel, ledCanvas);
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

        // Open new window when settings button is clicked
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("selected er: " + dataPort.SelectedPort);
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        // Get the name of the checkbox that is clicked
        private void LedStackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseWasDownOn = e.Source as FrameworkElement;
            if (mouseWasDownOn != null)
            {
                string elementName = mouseWasDownOn.Name;

                CheckBox hej = mouseWasDownOn as CheckBox;

                if (MyDebug.isDebug)
                {
                    Console.WriteLine(elementName);
                }
            }
        }

        private Point _startPoint;
        private Rectangle _rect;

        // Create rectangle when left mouse is clicked
        private void LedCanvas_OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            // Remove rectangle if it is stuck in canvas bounds. Can only be 1 rectangle at a time
            var rectangles = ledCanvas.Children.OfType<Rectangle>().ToList();
            if (rectangles.Count > 0)
            {
                foreach (var rect in rectangles)
                {
                    ledCanvas.Children.Remove(rect);
                }
            }

            _startPoint = e.GetPosition(ledCanvas);

            _rect = new Rectangle
            {
                Stroke = Brushes.LightBlue,
                Fill = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230)), // Same color as light blue, but with alpha
                StrokeThickness = 2
            };
            Canvas.SetLeft(_rect, _startPoint.X);
            Canvas.SetTop(_rect, _startPoint.Y);
            ledCanvas.Children.Add(_rect);
        }

        // Resize rectangle when mouse move
        private void LedCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || _rect == null)
                return;

            var pos = e.GetPosition(ledCanvas);

            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);

            var w = Math.Max(pos.X, _startPoint.X) - x;
            var h = Math.Max(pos.Y, _startPoint.Y) - y;

            _rect.Width = w;
            _rect.Height = h;

            Canvas.SetLeft(_rect, x);
            Canvas.SetTop(_rect, y);
        }

        // Remove the rectangle from canvas when mouse up
        private void LedCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var rectangles = ledCanvas.Children.OfType<Rectangle>().ToList();
            foreach (var rect in rectangles)
            {
                Point topLeft = new Point(Canvas.GetLeft(rect), Canvas.GetTop(rect));
                Point bottomRight = rect.TranslatePoint(new Point(rect.ActualWidth, rect.ActualHeight), ledCanvas);

                if (MyDebug.isDebug)
                {
                    Console.WriteLine($"top:{topLeft.X};{topLeft.Y} bottom:{bottomRight.X};{bottomRight.Y}");
                }
                ledCanvas.Children.Remove(rect);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
