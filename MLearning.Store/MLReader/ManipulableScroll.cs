﻿using System;
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
using MLReader.ManipulablePages;

namespace MLReader
{
    public delegate void Animate2IndexEventHandler(object sender, int index, bool tobegin); 

    public sealed partial class ManipulableScroll : Grid, INotifyPropertyChanged
    {
        double DeviceHeight = 900.0, DeviceWidth = 1600.0;
        public event PropertyChangedEventHandler PropertyChanged;
        public event Animate2IndexEventHandler Animate2IndexEvent; 

        public ManipulableScroll()
        {
            init();
        }

        void init()
        {
            Width = DeviceWidth;
            Height = DeviceHeight;

            _elements = new List<ISlideElement>();

            //properties
            _translatedelta = 0.0;
            _thresholddelta = 0.0;
            _threshold = 0.0;

            //Scrol view
            _mainscroll = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollMode = ScrollMode.Disabled,
                Width = DeviceWidth,
                Height = DeviceHeight
            };
            Children.Add(_mainscroll);

            _paneltransform = new CompositeTransform();

            _contentpanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Width = DeviceWidth,
                RenderTransform = _paneltransform
            };
            _mainscroll.Content = _contentpanel;

            //manipulation
            Background = new SolidColorBrush(Colors.Transparent);
            ManipulationMode = ManipulationModes.All;
            ManipulationCompleted += ManipulableScroll_ManipulationCompleted;
            ManipulationInertiaStarting += ManipulableScroll_ManipulationInertiaStarting;
            ManipulationDelta += ManipulableScroll_ManipulationDelta;
            PointerPressed += ManipulableScroll_PointerPressed;
            PointerReleased += ManipulableScroll_PointerReleased;
            PointerCanceled += ManipulableScroll_PointerCanceled;
        }



        #region Controls and variables

        ScrollViewer _mainscroll;
        StackPanel _contentpanel;
        CompositeTransform _paneltransform;

        List<ISlideElement> _elements;
        int _currentindex, _pointers = 0;

        #endregion


        #region properties

        public List<ISlideElement> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }



        private LOPageSource _source;

        public LOPageSource Source
        {
            get { return _source; }
            set { _source = value; initdatasource(); }
        }

        private double _proportion = 1.0;

        public double Proportion
        {
            get { return _proportion; }
            set { _proportion = value; }
        }


        private double _threshold;

        public double Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }


        private double _thresholddelta;

        public double ThresholdDelta
        {
            get { return _thresholddelta; }
            set
            {
                _thresholddelta = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ThresholdDelta"));
            }
        }


        private double _translatedelta;

        public double TranslateDelta
        {
            get { return _translatedelta; }
            set
            {
                _translatedelta = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TranslateDelta"));
            }
        }


        private int _actualpage;

        public int ActualPage
        {
            get { return _actualpage; }
            set
            {
                _actualpage = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualPage"));
            }
        }


        #endregion


        #region fucntions

        

        bool _forcemanipulation2end = false, _deltatested = false;
        double _actualdelta = 0.0, _currenttranslate = 0.0;

        void ManipulableScroll_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _forcemanipulation2end = true;
        }

        void ManipulableScroll_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //_forcemanipulation2end = true;
        }

        void ManipulableScroll_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointers += 1;
        }


        bool _ismanipulationenable = true, _islocked = false;

        void ManipulableScroll_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_forcemanipulation2end || (e.IsInertial && TranslateDelta != 0.0)) e.Complete();
            if (_ismanipulationenable)
                if (_pointers < 2)
                {
                    if (_deltatested)
                    {
                        _actualdelta += e.Delta.Translation.Y;
                        if (_actualdelta > _threshold && _actualdelta < 0.0)
                            ThresholdDelta += e.Delta.Translation.Y;
                        else TranslateDelta += e.Delta.Translation.Y;
                    }
                    else
                    {
                        if (Math.Abs(e.Delta.Translation.X * 2) < Math.Abs(e.Delta.Translation.Y))
                        {
                            _deltatested = true;
                            _actualdelta += e.Delta.Translation.Y;
                            if (_actualdelta > _threshold && _actualdelta < 0.0)
                                ThresholdDelta += e.Delta.Translation.Y;
                            else TranslateDelta += e.Delta.Translation.Y;
                        }
                        else
                        {
                            _ismanipulationenable = false;
                            //_forcemanipulation2end = true;
                        }
                    }

                    _paneltransform.TranslateY = _currenttranslate + TranslateDelta;
                }
        }

        void ManipulableScroll_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            e.TranslationBehavior.DesiredDeceleration = 20.0 * 96.0 / (1000.0 * 1000.0); 
        }

        void ManipulableScroll_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

            if (_pointers < 2)
            {
                if (Math.Abs(TranslateDelta) > 0)
                {
                    if (TranslateDelta > 400.0) _currentindex--;

                    if (TranslateDelta < -400.0) _currentindex++;

                    animate2index();
                }

            }
            else
            {
            }

            _pointers = 0;
            _deltatested = false;
            _forcemanipulation2end = false;
            if(!_islocked)
                _ismanipulationenable = true;
        }

        #endregion



        void animate2index()
        {
            if (_currentindex < 0) _currentindex = 0;
            if (_currentindex >= _elements.Count) _currentindex = _elements.Count - 1;

            //ActualPage = _currentindex;

            bool tobegin = true;
            if (_currentindex == ActualPage)
                if (_translatedelta < 0) tobegin = false;

            if (Animate2IndexEvent != null)
                Animate2IndexEvent(this, _currentindex, tobegin);

            ActualPage = _currentindex;
            _currenttranslate = _elements[_currentindex].Position;

            Storyboard story = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(350);
            Storyboard.SetTarget(animation, _paneltransform);
            Storyboard.SetTargetProperty(animation, "TranslateY");
            story.Children.Add(animation);
            animation.To = _currenttranslate;
            story.Begin();

            if (tobegin)
            {
                _thresholddelta = 0.0;
                _actualdelta = 0.0;
            }
            else
            {
                _actualdelta = _thresholddelta;
            }
            _translatedelta = 0.0;

        }


        void initdatasource()
        {
            double pos = 0.0;
            for (int i = 0; i < _source.Slides.Count; i++)
            {
                var item = Source.Slides[i];
                if(item.Type == 1)
                {
                    SingleManipulableImage el = new SingleManipulableImage();
                    el.Source = item;
                    el.BorderImageSelected += (s, e) => { _ismanipulationenable = false; _islocked = true;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
                    };
                    el.BorderImageReleased += (s, e) => { _ismanipulationenable = true; _islocked = false; _pointers = 0;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Released"));
                    };
                    el.Position = pos;
                    _contentpanel.Children.Add(el);
                    _elements.Add(el);
                }

                if (item.Type == 4)
                {
                    ManipulableImageCollection el = new ManipulableImageCollection();
                    el.Source = item;
                    el.BorderImageSelected += (s, e) =>
                    {
                        _ismanipulationenable = false; _islocked = true;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
                    };
                    el.BorderImageReleased += (s, e) =>
                    {
                        _ismanipulationenable = true; _islocked = false; _pointers = 0;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("Released"));
                    };
                    el.Position = pos;
                    _contentpanel.Children.Add(el);
                    _elements.Add(el);
                }

                if (item.Type == 2)
                {
                    LeftAvatarSlide el = new LeftAvatarSlide();
                    //el.Source = item;
                    el.Position = pos;
                    _contentpanel.Children.Add(el);
                    _elements.Add(el);
                }


                if (item.Type == 3 || item.Type == 5 || item.Type == 17)
                {
                    RightAvatarSlide el = new RightAvatarSlide();
                    //el.Source = item;
                    el.Position = pos;
                    _contentpanel.Children.Add(el);
                    _elements.Add(el);
                }



                if (item.Type ==0 || item.Type == 6)
                {
                    TopSlideElement elem = new TopSlideElement();
                    elem.Position = pos;
                    _contentpanel.Children.Add(elem);
                    _elements.Add(elem);
                }
                pos -= 900.0;
            }

            _currentindex = 0;
            _actualpage = _currentindex;
            _threshold = _elements[_currentindex].GetSize() - DeviceHeight;
        }
    }
}
