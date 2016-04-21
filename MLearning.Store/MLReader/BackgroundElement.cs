using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MLReader
{
    public sealed partial class BackgroundElement : Grid , ISlideElement
    {
        double DeviceHeight = 900.0, DeviceWidth = 1600.0; 

        public BackgroundElement()
        {
            init();
        }
        

        public event ISlideElementSizeChangedEventHandler ISlideElementSizeChanged;

        public double GetSize()
        {
            return DeviceHeight;
        }

        private double _position;

        public double Position
        {
            get { return _position; }
            set { _position = value; }
        }


        Image _backimage;

        void init()
        {
            Height = DeviceHeight;
            Width = DeviceWidth;

            _backimage = new Image() { Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill, Width = DeviceWidth, Height = DeviceHeight };
            Children.Add(_backimage);
            Background = new SolidColorBrush(Colors.White);
        }


        private LOSlideSource _source;

        public LOSlideSource Source
        {
            get { return _source; }
            set { _source = value; initsource(); }
        }
        

        void initsource()
        {
            if (_source.ImageUrl != null)
                _backimage.Source = new BitmapImage(new Uri(_source.ImageUrl));

            Background = new SolidColorBrush(_source.Style.BackgroundColor);
        }
    }
}
