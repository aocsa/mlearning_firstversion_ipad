using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MLReader
{
    public sealed partial class LOPageViewer : Grid, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        public LOPageViewer()
        {
            init();
        }


        #region Controls and variables

        ManipulableScroll _manipulablescroll;
        MultiTextScroll _textscroll;
        BackgroundScroll _backscroll;

        int _currentIndex;

        #endregion

        void init()
        {
            Width = 1600.0;
            Height = 900.0;

            _backscroll = new BackgroundScroll();
            Children.Add(_backscroll);
            _textscroll = new MultiTextScroll();
            _textscroll.ISlideElementSizeChanged += _textscroll_ISlideElementSizeChanged;
            Children.Add(_textscroll);
            _manipulablescroll = new ManipulableScroll();
            _manipulablescroll.PropertyChanged += _manipulablescroll_PropertyChanged;
            _manipulablescroll.Animate2IndexEvent += _manipulablescroll_Animate2IndexEvent;
            Children.Add(_manipulablescroll);
        }

        void _manipulablescroll_Animate2IndexEvent(object sender, int index, bool tobegin)
        {
            _currentIndex = index;
            _textscroll.Animate2Index(_currentIndex, tobegin);
            _backscroll.Animate2Index(_currentIndex);
            computeThresholds();
         
        }

        void _textscroll_ISlideElementSizeChanged(object sender)
        {
            computeThresholds();
        }

        void computeThresholds()
        {
            if (_manipulablescroll.Elements.Count > 0)
                _manipulablescroll.Threshold = 900.0 - _textscroll.Elements[_currentIndex].GetSize(); ;
        }

        void computeThresholds1()
        {
            double maxsize = 900.0;
            if (_backscroll.Elements[_currentIndex].GetSize() > maxsize) maxsize = _backscroll.Elements[_currentIndex].GetSize();
            if (_textscroll.Elements[_currentIndex].GetSize() > maxsize) maxsize = _textscroll.Elements[_currentIndex].GetSize();
            if (_manipulablescroll.Elements.Count > 0)
                if (_manipulablescroll.Elements[_currentIndex].GetSize() > maxsize) maxsize = _manipulablescroll.Elements[_currentIndex].GetSize();

            _backscroll.Threshold = -1.0 * _backscroll.Elements[_currentIndex].GetSize() + 900.0;
            _backscroll.Proportion = 0.0; //_backscroll.Threshold / maxsize;
             
            /**if (_textscroll.Elements.Count > 0)
            {
                _textscroll.Threshold = 900.0 - maxsize;
                double th = -1.0 * _textscroll.Elements[_currentIndex].GetSize() + 900.0;
                double div = Math.Abs(maxsize - 900);
                if (div > 0) _textscroll.Proportion = Math.Abs(th / div);
                else _textscroll.Proportion = 0.0;
            }

            if (_manipulablescroll.Elements.Count > 0)
            {
                _manipulablescroll.Threshold = 900.0 - maxsize;
                double th =-1.0 * _manipulablescroll.Elements[_currentIndex].GetSize() + 900.0;
                double div = Math.Abs(maxsize - 900);
                if (div > 0) _manipulablescroll.Proportion = Math.Abs(th / div);
                else _manipulablescroll.Proportion = 0.0;
            }*/

            _textscroll.Proportion = 1.0;

            _manipulablescroll.Threshold = 900.0 - maxsize;
            _textscroll.Proportion = 0.0;

        }


        private LOPageSource _source;

        public LOPageSource Source
        {
            get { return _source; }
            set { _source = value; initsource(); }
        }


        void initsource()
        {
            _backscroll.Source = _source;
            _textscroll.Source = _source;
            _manipulablescroll.Source = _source;
            _currentIndex = 0;
        }


        void _manipulablescroll_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThresholdDelta")
            {
                _textscroll.ThresholdDelta = _manipulablescroll.ThresholdDelta;
            }

            if (e.PropertyName == "TranslateDelta")
            {
                _textscroll.TranslateDelta = _manipulablescroll.TranslateDelta;
                _backscroll.TranslateDelta = _manipulablescroll.TranslateDelta;
            }

            if (e.PropertyName == "ActualPage_1")
            {
                _currentIndex = _manipulablescroll.ActualPage;
                _textscroll.Animate2Index(_currentIndex, false);
                _backscroll.Animate2Index(_currentIndex);
                computeThresholds();
            }

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




       
    }
}
