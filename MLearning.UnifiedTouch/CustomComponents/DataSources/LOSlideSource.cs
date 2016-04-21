using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MLearning.UnifiedTouch
{
	public class LOSlideSource : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		int type;
		public int Type 
		{
			get { return type; }
			set 
			{
				type = value; 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Type"));
			}
		}

		LOSlideStyle style;
		public LOSlideStyle Style 
		{
			get { return style; }
			set 
			{ style = value; }
		}

		string title;
		public string Title 
		{
			get { return title; }
			set 
			{
				title = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Title"));

			}
		}

		string author;
		public string Author 
		{
			get { return author; }
			set 
			{ 
				author = value; 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Author"));

			}
		}

		string paragraph;
		public string Paragraph 
		{
			get { return paragraph; }
			set 
			{
				paragraph = value; 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Paragraph"));

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

		string videoUrl;
		public string VideoUrl 
		{
			get { return videoUrl; }
			set 
			{
				videoUrl = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("VideoUrl"));

			}
		}

		ObservableCollection <LOItemSource> itemize;
		public ObservableCollection<LOItemSource> Itemize 
		{
			get { return itemize; }
			set 
			{
				itemize = value; 
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Itemize"));

			}
		}

		public LOSlideSource ()
		{
		}
	}
}

