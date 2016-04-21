using System;
using System.ComponentModel;

namespace MLearning.UnifiedTouch
{
	public class LOItemSource : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		string text;
		public string Text 
		{
			get { return text; }
			set 
			{
				text = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Text"));

			}
		}

		string imageUrl;
		public string ImageUrl 
		{
			get { return imageUrl; }
			set 
			{
				imageUrl = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ImageUrl"));

			}
		}

		public LOItemSource ()
		{
		}
	}
}

