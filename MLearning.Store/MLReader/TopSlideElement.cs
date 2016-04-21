using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace MLReader
{
    public sealed partial class TopSlideElement : Grid, ISlideElement
    {

        double DeviceHeight = 900.0, DeviceWidth = 1600.0;


        public TopSlideElement()
        {
            init();
            Background = new SolidColorBrush(Colors.Transparent);
            //Opacity = 0.6;
            ManipulationMode = ManipulationModes.All;
        }

        public double GetSize()
        {
            return DeviceHeight;
        }

        public event ISlideElementSizeChangedEventHandler ISlideElementSizeChanged;

        private double _position;

        public double Position
        {
            get { return _position; }
            set { _position = value; }
        }

        void init()
        {
            Height = DeviceHeight;
            Width = DeviceWidth;
            Background = new SolidColorBrush(Colors.Transparent);
            if (ISlideElementSizeChanged != null)
                ISlideElementSizeChanged(this);
        }
    }
}
