using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MLReader
{
    public class LOPageSource
    {
        public LOPageSource()
        { 
        }


        private BitmapImage _cover;

        public BitmapImage Cover
        {
            get { return _cover; }
            set { _cover = value; }
        }


        private string  _pagetitle;

        public string  PageTitle
        {
            get { return _pagetitle; }
            set { _pagetitle = value; }
        }


        private string _pagedescription;

        public string PageDescription
        {
            get { return _pagedescription; }
            set { _pagedescription = value; }
        }
        


        private List<LOSlideSource> _slides;

        public List<LOSlideSource> Slides
        {
            get { return _slides; }
            set { _slides = value; }
        }


        public int Index { get; set; }

        public int PageIndex { get; set; }

        //section
        public int StackIndex { get; set; }

        //chapter
        public int LOIndex { get; set; }
    }
}
