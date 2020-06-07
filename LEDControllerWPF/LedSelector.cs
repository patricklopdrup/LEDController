using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LEDControllerWPF
{
    class LedSelector
    {
        private StackPanel _st;
        private int _ledNum = 91;

        public LedSelector(StackPanel stackPanel)
        {
            _st = stackPanel;
            CreateCheckBoxes();
        }

        private void CreateCheckBoxes()
        {
            for (int i = 0; i < _ledNum; i++)
            {
                _st.Children.Add(new CheckBox
                {
                    Name = "led" + i,
                    VerticalAlignment = VerticalAlignment.Center,
                    LayoutTransform = new ScaleTransform(0.5, 1),
                });
                
            }
        }

    }
}
