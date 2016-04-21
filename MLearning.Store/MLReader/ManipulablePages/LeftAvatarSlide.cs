using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MLReader.ManipulablePages
{
    public sealed partial class LeftAvatarSlide : Grid, ISlideElement
    {
        public LeftAvatarSlide()
        {
            Width = 1600.0;
            Height = 900.0;
            if (ISlideElementSizeChanged != null)
                ISlideElementSizeChanged(this);

            Image img = new Image() {
                Width = 590,
                Height =747,
                RenderTransform = new TranslateTransform(){ X=282, Y=147},
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                Source =  new BitmapImage(new Uri("ms-appx:///leftimg.png")) };
            Children.Add(img);

            Background = new SolidColorBrush(Colors.Transparent);
            ManipulationMode = ManipulationModes.All;
        }

        public double GetSize()
        {
            return 1600.0;
        }

        double _position = 0;
        public double Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public event ISlideElementSizeChangedEventHandler ISlideElementSizeChanged;
    }
}
