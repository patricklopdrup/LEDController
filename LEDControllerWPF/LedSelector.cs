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
    class LedSelector
    {
        private StackPanel _st;
        private Canvas _canvas;
        private int _ledNum = 91;

        public LedSelector(StackPanel stackPanel, Canvas canvas)
        {
            _st = stackPanel;
            _canvas = canvas;

            CreateCheckBoxes();
        }

        // Creates checkboxes in a stackpanel
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
