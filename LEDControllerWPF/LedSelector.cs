using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LEDControllerWPF
{
    // Class so we are able to send which led is clicked on
    class LedSelectEventArgs : EventArgs
    {
        public int StartNum { get; set; }
        public int EndNum { get; set; }
    }

    // Hold information of the LED(s) that is clicked
    class LedObject
    {
        public int StartNum { get; set; }
        public int EndNum { get; set; }

        public string GetSequence()
        {
            // if more than 1 LED is selected, fx led0 to led7: "0-7"
            return EndNum == -1 ? StartNum.ToString() : $"{StartNum}-{EndNum}";
        }
    }

    class LedSelector
        {
        public EventHandler<LedSelectEventArgs> LedClicked;

        private StackPanel _st;
        private Canvas _canvas;
        private int _ledNum = 91;

        public LedSelector(StackPanel stackPanel, Canvas canvas)
        {
            _st = stackPanel;
            _canvas = canvas;

            CreateCheckBoxes();
        }

        {
        private void CreateCheckBoxes()
        {
            for (int i = 0; i < _ledNum; i++)
            {
                _st.Children.Add(new ToggleButton()
                {
                    Name = "led" + i,
                    ClickMode = ClickMode.Press,
                    Cursor = Cursors.Hand,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 15,
                    Width = 10,
                    //IsEnabled = true,
                    //IsChecked = true,
                    //Background = Brushes.Transparent,
                    Margin = new Thickness(0.2,0,0.2,0)
                    //LayoutTransform = new ScaleTransform(0.7, 1.1)
                });
            }

            if (MyDebug.isDebug)
            {
                double winWidth = SystemParameters.PrimaryScreenWidth;
                Console.WriteLine($"stackpanel width: {winWidth}");
            }
        }

        private void GetCheckboxesPos()
        {
            List<Point> checkboxPos = new List<Point>();
            var boxes = _st.Children.OfType<CheckBox>().ToList();
            for (int i = 0; i < boxes.Count; i++)
            {
                var pos = boxes[i].TranslatePoint(new Point(0,0), _st);
                Console.WriteLine($"hej: {pos.ToString()}");
                checkboxPos.Add(pos);
            }

            Console.WriteLine($"Amount of boxes: {boxes.Count}");
            foreach (var point in checkboxPos)
            {
                Console.WriteLine($"x:{point.X} ; y:{point.Y}");
            }
        }

    }
}
