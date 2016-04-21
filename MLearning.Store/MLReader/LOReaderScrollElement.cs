using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace MLReader
{
    public sealed partial class LOReaderScrollElement : Grid, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        double DeviceHeight = 900.0, DeviceWidth = 1600.0;
        public LOReaderScrollElement()
        {
            init();
        }

        Grid _pagegrid;
        Image _backimage;
        CoverTextSlide _backtext;


        void init()
        {
            Background = new SolidColorBrush(Colors.Transparent);

            _backimage = new Image() { Stretch = Stretch.UniformToFill};
            Children.Add(_backimage);

            _backtext = new CoverTextSlide();
            Children.Add(_backtext);

            _pagegrid = new Grid() { Width = DeviceWidth , Height = DeviceHeight};
            Children.Add(_pagegrid);
            Canvas.SetZIndex(_pagegrid,-10);
        }


        private LOPageSource _source;

        public LOPageSource Source
        {
            get { return _source; }
            set { _source = value; loadelement(); }
        }


        void loadelement()
        {
            _backimage.Source = _source.Cover;
            _backtext.Source = _source.Slides[0];
            resetpage();
        }


        public void resetpage()
        {
            _pagegrid.Children.Clear();

            LOPageViewer page = new LOPageViewer();
            page.PropertyChanged += page_PropertyChanged;
            page.Source = _source;
            _pagegrid.Children.Add(page);
            Canvas.SetZIndex(_pagegrid, 10);
        }

        void page_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
            }

            if (e.PropertyName == "Released")
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Released"));
            }
        }


        public void clearpage()
        {

            _pagegrid.Children.Clear();
            Canvas.SetZIndex(_pagegrid, -10);
        }

    }
}
