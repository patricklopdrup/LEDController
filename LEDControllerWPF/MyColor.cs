using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LEDControllerWPF.Annotations;

namespace LEDControllerWPF
{
    class MyColor : INotifyPropertyChanged
    {
        private Canvas canvas;
        private int phases = 6;
        private byte maxColor = 255;
        private byte _r, _g, _b;
        private Slider _rSlider, _gSlider, _bSlider;
        private Border _previewColorBlock;

        private SolidColorBrush _previewColor;
        private SolidColorBrush _chosenColor;
        public SolidColorBrush PreviewColor
        {
            get => _previewColor;
            set
            {
                _previewColor = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ChosenColor
        {
            get => _chosenColor;
            set
            {
                _chosenColor = value;
                OnPropertyChanged();
            }
        }

        public MyColor(Canvas canvas, Slider rSlider, Slider gSlider, Slider bSlider, Border previewColorBlock)
        {
            this.canvas = canvas;
            _rSlider = rSlider;
            _gSlider = gSlider;
            _bSlider = bSlider;
            _previewColorBlock = previewColorBlock;
            DrawLines();
        }

        public void DrawLines()
        {
            // add a line for each pixel in the canvas' width
            for (var i = 0; i < canvas.Width; i++)
            {
                AddLine(i);
            }
        }

        // changing the global _r, _g, _b values
        private void CalcRGBValues(int lineNum)
        {
            double phaseLen = canvas.Width / phases;
            // cannot show every color so we jump according to the offset
            byte offset = (byte) (maxColor / phaseLen);

            if (MyDebug.isDebug)
            {
                double hej = (maxColor / phaseLen);
                Console.WriteLine($"offset: {offset} - real: {hej}");
                Console.WriteLine($"canvas width: {canvas.Width} og height: {canvas.Height}");
            }

            // x-axis for colors in the canvas
            switch (GetPhase(lineNum))
            {
                case 0:
                    _r = maxColor;
                    _g += offset;
                    _b = 0;
                    break;
                case 1:
                    _r -= offset;
                    _g = maxColor;
                    _b = 0;
                    break;
                case 2:
                    _r = 0;
                    _g = maxColor;
                    _b += offset;
                    break;
                case 3:
                    _r = 0;
                    _g -= offset;
                    _b = maxColor;
                    break;
                case 4:
                    _r += offset;
                    _g = 0;
                    _b = maxColor;
                    break;
                case 5:
                    _r = 255;
                    _g = 0;
                    _b -= offset;
                    break;
            }
        }

        // returns which of the 6 phases the lineNum is in
        private int GetPhase(int lineNum)
        {
            double phaseLen = canvas.Width / phases;
            if (MyDebug.isDebug)
            {
                Console.WriteLine($"phase len {phaseLen} on line {lineNum}");
                Console.WriteLine($"return: {(int) (lineNum / phaseLen)}");
            }
            return (int) (lineNum / phaseLen);
        }

        // add a vertical line on the canvas
        private void AddLine(int lineNum)
        {
            CalcRGBValues(lineNum);

            if (MyDebug.isDebug)
            {
                Console.WriteLine($"R:{_r} G:{_g} B:{_b}");
            }

            Color colorLine = new Color();
            colorLine = Color.FromRgb(_r,_g,_b);

            // make line from x1, y1 to x2, y2
            Line line = new Line();
            line.Opacity = 0.9;
            line.StrokeThickness = 2;
            line.X1 = lineNum % canvas.Width;
            line.Y1 = 0;
            line.X2 = lineNum;
            line.Y2 = canvas.ActualHeight;

            // Make an onClick listener on the lines
            line.MouseLeftButtonDown += new MouseButtonEventHandler(lineMouseDown);
            line.MouseMove += new MouseEventHandler(lineMouseMove);
            canvas.LostFocus += new RoutedEventHandler(lostFocus);

            LinearGradientBrush linearBrush = new LinearGradientBrush();
            linearBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

            // fade from actual color to white in a vertical manner
            GradientStop color1 = new GradientStop(colorLine, 0);
            linearBrush.GradientStops.Add(color1);

            GradientStop color2 = new GradientStop(Colors.White, 1.2);
            linearBrush.GradientStops.Add(color2);

            line.Stroke = linearBrush;

            // adding the line to the canvas
            canvas.Children.Add(line);
        }

        protected void lineMouseMove(object sender, MouseEventArgs e)
        {
            // Get y-pos to make offset
            var mouseYPos = e.GetPosition(canvas).Y;
            // Offset is between 0 and 1
            var offset = mouseYPos / canvas.ActualHeight;
            // Get the brush from the line we click on (the sender)
            var brush = ((Line)sender).Stroke;
            // Get the amount of gradientStops on the line
            var gradientStops = ((LinearGradientBrush)brush).GradientStops;
            // Gets the color from a point on the line
            var color = GetRelativeColor(gradientStops, offset);
            PreviewColor = new SolidColorBrush(color);
            if (MyDebug.isDebug)
            {
                Console.WriteLine($"y-pos:{mouseYPos} and offset:{offset}");
                Console.WriteLine($"color: {PreviewColor}");
            }

            // Move block of preview color near the mouse
            int textBlockOffset = 15;
            _previewColorBlock.RenderTransform = new TranslateTransform(
                e.GetPosition(canvas).X - (canvas.ActualWidth/2) + textBlockOffset, 
                e.GetPosition(canvas).Y - canvas.ActualHeight - textBlockOffset);
        }

        protected void lineMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get y-pos to make offset
            var mouseYPos = e.GetPosition(canvas).Y;
            // Offset is between 0 and 1
            var offset = mouseYPos / canvas.ActualHeight;
            // Get the brush from the line we click on (the sender)
            var brush = ((Line) sender).Stroke;
            // Get the amount of gradientStops on the line
            var gradientStops = ((LinearGradientBrush) brush).GradientStops;
            // Gets the color from a point on the line
            var color = GetRelativeColor(gradientStops, offset);
            ChosenColor = new SolidColorBrush(color);
            // Setting the 3 sliders to the correct position for RGB
            _rSlider.Value = color.R;
            _gSlider.Value = color.G;
            _bSlider.Value = color.B;

            if (MyDebug.isDebug)
            {
                Console.WriteLine($"y-pos:{mouseYPos} and offset:{offset}");
                Console.WriteLine($"color: {color}");
            }
        }

        protected void lostFocus(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            
        }

        // Taken from https://stackoverflow.com/a/9651053/13203546
        public Color GetRelativeColor(GradientStopCollection gsc, double offset)
        {
            var point = gsc.SingleOrDefault(f => f.Offset == offset);
            if (point != null) return point.Color;

            GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
            GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

            foreach (var gs in gsc)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                {
                    before = gs;
                }
                if (gs.Offset > offset && gs.Offset < after.Offset)
                {
                    after = gs;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
