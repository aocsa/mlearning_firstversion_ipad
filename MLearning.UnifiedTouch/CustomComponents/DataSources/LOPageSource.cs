using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using System.ComponentModel;
using Foundation;

namespace MLearning.UnifiedTouch
{
	public class LOPageSource : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		UIImage cover;
		public UIImage Cover 
		{
			get { return cover; }
			set 
			{
				cover = value; 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Cover"));
			}
		}

		byte[] coverBytes;
		public byte[] CoverBytes
		{
			get { return coverBytes; }
			set
			{
				coverBytes = value;
				Cover = UIImage.LoadFromData ((NSData.FromArray (coverBytes)));
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("CoverBytes"));
			}
		}

		string pageTitle;
		public string PageTitle 
		{
			get { return pageTitle; }
			set { pageTitle = value; }
		}

		string pageDescription;
		public string PageDescription 
		{
			get { return pageDescription; }
			set { pageDescription = value; }
		}

		List <LOSlideSource> slides;
		public List<LOSlideSource> Slides 
		{
			get { return slides; }
			set { slides = value; }
		}

		int index;
		public int Index
		{
			get { return index; }
			set { index = value; }
		}

		int pageIndex;
		public int PageIndex 
		{
			get { return pageIndex; }
			set { pageIndex = value; }
		}

		int stackIndex;
		public int StackIndex 
		{
			get { return stackIndex; }
			set { stackIndex = value; }
		}

		int lOIndex;
		public int LOIndex 
		{
			get { return lOIndex; }
			set { lOIndex = value; }
		}

		public LOPageSource ()
		{
			CGRect rect = new CGRect(0, 0, 1, 1);
			UIGraphics.BeginImageContext (rect.Size);
			CGContext context = UIGraphics.GetCurrentContext ();
			context.SetFillColor (UIColor.LightGray.CGColor);
			context.FillRect (rect);
			cover = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
		}
	}
}

