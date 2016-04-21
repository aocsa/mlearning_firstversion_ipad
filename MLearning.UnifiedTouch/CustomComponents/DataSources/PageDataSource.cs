using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UIKit;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class PageDataSource : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		private string name;
		public string Name 
		{
			get { return name;	}
			set 
			{
				name = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Name"));
			}
		}

		private string description;
		public string Description 
		{
			get { return description; }
			set 
			{
				description = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Description"));
			}
		}

		private UIImage imageContent;
		public UIImage ImageContent 
		{
			get { return imageContent; }
			set 
			{
				imageContent = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("ImageSource"));
			}
		}
			
		private UIColor borderColor;
		public UIColor BorderColor 
		{
			get { return borderColor; }
			set 
			{
				borderColor = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("BorderColor"));
			}
		}
			
	}
}

