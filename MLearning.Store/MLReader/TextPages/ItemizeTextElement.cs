using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MLReader
{

    public sealed partial class ItemizeTextElement : Grid, ISlideElement
    {

        public ItemizeTextElement()
        {
            init();
            SizeChanged += ItemizeTextElement_SizeChanged;
            //Background = new SolidColorBrush(Colors.Violet);
        }

        void ItemizeTextElement_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (this.ActualHeight != _actualheight)
            {
                _actualheight = ActualHeight;
                if (ISlideElementSizeChanged != null)
                    ISlideElementSizeChanged(this);
            }
        }


        public event ISlideElementSizeChangedEventHandler ISlideElementSizeChanged;

        StackPanel _contentpanel, _itemspanel;
        TextBlock _titleblock;
        double _titleheight = 0.0, _itemsheight = 0.0, _actualheight = 0.0;
        void init()
        {
            Width = 1600.0;
            Height = 900.0;
            _actualheight = 900.0;

            _contentpanel = new StackPanel() { Orientation = Orientation.Vertical };
            _contentpanel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            _contentpanel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            _contentpanel.RenderTransform = new CompositeTransform() { TranslateX = 320 };
            _contentpanel.Width = 586.0;
            
            Children.Add(_contentpanel);

            Grid header = new Grid() { Width = 100.0, Height = 214.0 };
            Grid footer = new Grid() { Width = 100.0, Height = 214.0 };
            Grid separation = new Grid() { Width = 100.0, Height = 66.0 };
            _titleblock = new TextBlock() { TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap, FontSize = 56 };
            _titleblock.LayoutUpdated += _titleblock_LayoutUpdated;

            _itemspanel = new StackPanel() { Orientation = Orientation.Vertical };
            _itemspanel.LayoutUpdated += _itemspanel_LayoutUpdated;
            _itemspanel.Width = 560.0;

            //childrens of content
            _contentpanel.Children.Add(header);
            _contentpanel.Children.Add(_titleblock);
            _contentpanel.Children.Add(separation);
            _contentpanel.Children.Add(_itemspanel);
            _contentpanel.Children.Add(footer);
        }

        void _titleblock_LayoutUpdated(object sender, object e)
        {
            _titleheight = _titleblock.ActualHeight;
            this.Height = 2 * 182 + 66 + _titleheight + _itemsheight;
        }

        void _itemspanel_LayoutUpdated(object sender, object e)
        {
            _itemsheight = _itemspanel.ActualHeight;
            Height = 2 * 182 + 66 + _titleheight + _itemsheight;
        }


        #region Properties

        private LOSlideSource _source;

        public LOSlideSource Source
        {
            get { return _source; }
            set { _source = value; initcomponent(); }
        }

        private double _position;

        public double Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Color _titlecolor;

        public Color TitleColor
        {
            get { return _titlecolor; }
            set { _titlecolor = value; }
        }

        private Color _contentcolor;

        public Color ContentColor
        {
            get { return _contentcolor; }
            set { _contentcolor = value; }
        }





        #endregion


        #region Functions


        public double GetSize()
        {
            return _actualheight;
        }


        void initcomponent()
        {
            if (Source != null)
            {
                _titleblock.Text = _source.Title.ToUpper();
                _titleblock.Foreground = new SolidColorBrush(Source.Style.TitleColor);
                for (int i = 0; i < _source.Itemize.Count; i++)
                {
                    _itemspanel.Children.Add(new Item_LO()
                    {
                        TextContetn = _source.Itemize[i].Text,
                        TextColor = Source.Style.ContentColor,
                        BulletColor = Source.Style.TitleColor
                    });
                    _itemspanel.Children.Add(new Grid() { Height = 12, Width = 10 });
                }

                //double h = 2 * 182 + 66 + _titleheight + _itemsheight;
                double h = 2 * 182 + 66 + _titleblock.DesiredSize.Height + _itemspanel.DesiredSize.Height;
                if (h > 900.0)
                    this.Height = h;
            }
        }



        #endregion

        public sealed partial class Item_LO : Grid
        {
            Border bullet;
            TextBlock tblock;

            public Item_LO()
            {
                init();
            }

            void init()
            {
                Width = 586.0;
                Height = 42;
                //border bullet
                bullet = new Border()
               {
                   // Background = new SolidColorBrush(c),
                   Width = 16,
                   Height = 16,
                   CornerRadius = new Windows.UI.Xaml.CornerRadius(8),
                   HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left,
                   VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top,
                   RenderTransform = new CompositeTransform() { TranslateX = 5, TranslateY = 12 }
               };
                this.Children.Add(bullet);


                StackPanel panel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right,
                    Width = 540
                };
                Children.Add(panel);
                //text
                tblock = new TextBlock();
                //tblock.LayoutUpdated += tblock_LayoutUpdated;
                tblock.SizeChanged += tblock_SizeChanged;
                tblock.TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap;
                tblock.FontSize = 33;//28;
                tblock.FontWeight = Windows.UI.Text.FontWeights.Light;
                tblock.Width = 540.0;
                //tblock.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                //tblock.Text = content;
                panel.Children.Add(tblock);
            }

            void tblock_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
            {

                if (tblock.ActualHeight > 42)
                    this.Height = tblock.ActualHeight;
            }

            void tblock_LayoutUpdated(object sender, object e)
            {
                if (tblock.ActualHeight > 42)
                    this.Height = tblock.ActualHeight;
            }

            private string _textcontent;

            public string TextContetn
            {
                get { return _textcontent; }
                set { _textcontent = value; tblock.Text = value; tblock.UpdateLayout(); }
            }

            private Color _textcolor;

            public Color TextColor
            {
                get { return _textcolor; }
                set { _textcolor = value; tblock.Foreground = new SolidColorBrush(_textcolor); }
            }


            private Color _bulletcolor;

            public Color BulletColor
            {
                get { return _bulletcolor; }
                set { _bulletcolor = value; bullet.Background = new SolidColorBrush(_bulletcolor); }
            }


        }
    }
}
