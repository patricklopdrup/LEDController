using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LEDControllerWPF
{
    class MyColor
    {
        private Canvas canvas;
        private int phases = 6;
        private byte maxColor = 255;
        private byte _r, _g, _b;

        public MyColor(Canvas canvas)
        {
            this.canvas = canvas;
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
            //double hej = (maxColor / phaseLen);
            //Console.WriteLine($"hello: {offset} - real: {hej}");

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
            return (int) (lineNum / phaseLen);
        } 

        // add a vertical line on the canvas
        private void AddLine(int lineNum)
        {
            CalcRGBValues(lineNum);

            Console.WriteLine($"R:{_r} G:{_g} B:{_b}");

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

        public void GetBitmap()
        {
            BitmapSource bitmap = new BitmapImage();

        }
    }
}
