using System;
using System.Collections.Generic;
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
    public delegate void LOReaderPagedChangedEventHandler(object sender);
    public delegate void LOReaderRightTappedEventHandler(object sender);
    public delegate void LOReaderAnimate2ThumbnailEventHandler(object sender);
    public sealed partial class LOReaderScroll : Grid
    {
        double DeviceHeight = 900.0, DeviceWidth = 1600.0;
        public LOReaderPagedChangedEventHandler LOReaderPagedChanged;
        public event LOReaderAnimate2ThumbnailEventHandler LOReaderAnimate2Thumbnail;

        public LOReaderScroll()
        {
            init();
        }

        ScrollViewer _mainscroll;
        StackPanel _contentpanel;
        CompositeTransform _ctrasnform;

        int _pointers = 0, _currentindex, _numberofitems;
        bool _forcemanipulation2end = false;
        double _initthreshold = 0.0, _finalthreshold = 0.0;
        double _currentposition = -0.0;

        void init()
        {
            Height = DeviceHeight;
            Width = DeviceWidth;
            Background = new SolidColorBrush(Colors.Transparent);

            _mainscroll = new ScrollViewer()
            {
                VerticalScrollMode = ScrollMode.Disabled,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollMode = ScrollMode.Disabled,
                Width = DeviceWidth,
                Height = DeviceHeight
            };
            Children.Add(_mainscroll);

            _contentpanel = new StackPanel()
            {
                ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.All,
                Height = DeviceHeight,
                Orientation = Orientation.Horizontal
            };
            _mainscroll.Content = _contentpanel;
             
            _ctrasnform = new CompositeTransform();
            _contentpanel.RenderTransform = _ctrasnform;

            //events
            ManipulationMode = ManipulationModes.All;
            PointerPressed += LOReaderView_PointerPressed;
            PointerCanceled += LOReaderView_PointerCanceled;
            PointerReleased += LOReaderView_PointerReleased;
            ManipulationStarted += LOReaderView_ManipulationStarted;
            ManipulationDelta += LOReaderView_ManipulationDelta;
            ManipulationCompleted += LOReaderView_ManipulationCompleted;
            ManipulationInertiaStarting += LOReaderView_ManipulationInertiaStarting;
            //rigth tapped
            RightTapped += LOReaderView_RightTapped;
        }

        void loadsource()
        {
            _numberofitems = _source.Count;
            for (int i = 0; i < _source.Count; i++)
            {
                LOReaderScrollElement elem = new LOReaderScrollElement();
                 elem.Source = _source[i];
                _contentpanel.Children.Add(elem);
                elem.PropertyChanged += elem_PropertyChanged;
            } 
            _finalthreshold = -1.0 * _numberofitems * DeviceWidth;
        }

        void elem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            {
                _ismanipulationenable = false;
                _islocked = true;
            }

            if (e.PropertyName == "Released")
            {
                _ismanipulationenable = true;
                _islocked = false;
                _pointers = 0;
            }
        }

        #region Event Functions

        private void LOReaderView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _pointers += 1;
        }

        private void LOReaderView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            _forcemanipulation2end = true;
        }

        private void LOReaderView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        private void LOReaderView_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }


        bool _deltatested = false;
        bool _ismanipulationenable = true, _islocked = false;
        double _page_translation = 0.0;
        int _lastindex = 0;

        private void LOReaderView_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_forcemanipulation2end || (e.IsInertial && _ismanipulationenable))
            {
                e.Complete();
            }

            if (_ismanipulationenable)
                if (_pointers < 2)
                {
                    if (_deltatested)
                    {
                        //_ctrasnform.TranslateX += e.Delta.Translation.X;
                        if (_currentposition < _initthreshold && _currentposition > _finalthreshold)
                        {
                            _currentposition += e.Delta.Translation.X;
                            _page_translation += e.Delta.Translation.X;
                        }
                        else
                        {
                            _currentposition += (e.Delta.Translation.X * 0.4);
                            _page_translation += (e.Delta.Translation.X * 0.4);
                        }

                    }
                    else
                    {
                        if (Math.Abs(e.Delta.Translation.Y / 2) < Math.Abs(e.Delta.Translation.X))
                        {
                            _deltatested = true;
                            _currentposition += e.Delta.Translation.X;
                            _page_translation += e.Delta.Translation.X;
                        }
                        else
                        {
                            _ismanipulationenable = false;
                            // _forcemanipulation2end = true;
                        }
                    }
                    _ctrasnform.TranslateX = _currentposition; 
                }
                else
                {
                    //manipulation for the element to thumb
                   //this.Opacity = 0.0; 
                }
        }

        private void LOReaderView_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_pointers > 1)
            {
            }
            else
            {
                if (Math.Abs(e.Velocities.Linear.X) > 3.5)
                {
                    if (e.Velocities.Linear.X > 0) _currentindex -= 1;
                    else _currentindex += 1;
                }
                else
                {
                    if (_currentposition < -1.0 * (DeviceWidth * _currentindex + DeviceWidth / 2.0))
                        _currentindex += 1;

                    if (_currentposition > -1.0 * (DeviceWidth * _currentindex - DeviceWidth / 2.0))
                        _currentindex -= 1;
                }
                animate2index(_currentindex);
            }

            _pointers = 0;
            _forcemanipulation2end = false;
            _deltatested = false;
            if (!_islocked)
                _ismanipulationenable = true;
        }

        private void LOReaderView_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {

        }

        private void LOReaderView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }


        void animate2index(int index)
        {
            if (_currentindex < 0) _currentindex = 0;
            if (_currentindex >= _numberofitems) _currentindex = _numberofitems - 1;
            _currentposition = -1.0 * DeviceWidth * _currentindex;
            animate2double(_currentposition);
            /**if (_lastindex == _currentindex) animatepage2double(0.0);
            if (_lastindex > _currentindex) { animatepage2double(1600.0); loadpageat(_currentindex); }
            if (_lastindex < _currentindex) { animatepage2double(-1600.0); loadpageat(_currentindex); }*/
            _forcemanipulation2end = false;
            _deltatested = false;
            _lastindex = _currentindex;
            _page_translation = 0.0;
            //update page
            ChapterIndex = _source[_currentindex].LOIndex;
            SectionIndex = _source[_currentindex].StackIndex;
            PageIndex = _source[_currentindex].PageIndex;
            if (LOReaderPagedChanged != null)
                LOReaderPagedChanged(this);


        }
         

        void animatepage2double(double to)
        {
            Storyboard story = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(350);

            Storyboard.SetTarget(animation, _ctrasnform);
            Storyboard.SetTargetProperty(animation, "TranslateX");

            animation.To = to;
            story.Children.Add(animation);
            story.Begin();
        }

        void animate2double(double to)
        {
            Storyboard story = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(350);

            Storyboard.SetTarget(animation, _ctrasnform);
            Storyboard.SetTargetProperty(animation, "TranslateX");

            animation.To = to;
            story.Children.Add(animation);
            story.Begin();
            story.Completed += story_Completed;

        }

        void story_Completed(object sender, object e)
        {
            _forcemanipulation2end = false;
            _deltatested = false;
        }


        #endregion

        #region properties and public functions

        public void SetVisible()
        {
            Opacity = 1.0;
        }

        private List<LOPageSource> _source;

        public List<LOPageSource> Source
        {
            get { return _source; }
            set { _source = value; loadsource(); }
        }


        private CompositeTransform _elementtransform = new CompositeTransform();

        public CompositeTransform ElementTransform
        {
            get { return _elementtransform; }
            set { _elementtransform = value; }
        }


        private int _chapterindex;

        public int ChapterIndex
        {
            get { return _chapterindex; }
            set { _chapterindex = value; }
        }


        private int _sectionindex;

        public int SectionIndex
        {
            get { return _sectionindex; }
            set { _sectionindex = value; }
        }

        private int _pageindex;

        public int PageIndex
        {
            get { return _pageindex; }
            set { _pageindex = value; }
        }


        public void SetAt(int lo, int stack, int page)
        {  
            int pos = 0;
            foreach (var item in _source)
            {
                if (item.LOIndex == lo && item.StackIndex == stack && item.PageIndex == page)
                {
                    pos = item.Index;
                    break;
                }
            }
            _currentindex = pos;
            if (_currentindex < 0) _currentindex = 0;
            if (_currentindex >= _numberofitems) _currentindex = _numberofitems - 1;
            _currentposition = -1.0 * DeviceWidth * _currentindex;
            //animate2double(_currentposition); 
            _ctrasnform.TranslateX = _currentposition;
            _forcemanipulation2end = false;
            _deltatested = false;
            _lastindex = _currentindex;
            _page_translation = 0.0;
            //update page
            ChapterIndex = _source[_currentindex].LOIndex;
            SectionIndex = _source[_currentindex].StackIndex;
            PageIndex = _source[_currentindex].PageIndex; 
        }


        #endregion


        
 
    }

}
