using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UIKit;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class BookDataSource : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;
		private string title;
		public string Title 
		{
			get { return title;	}
			set 
			{
				title = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Title"));
			}
		}

		private ObservableCollection<ChapterDataSource> chapters = new ObservableCollection<ChapterDataSource> ();
		public ObservableCollection<ChapterDataSource> Chapters 
		{
			get { return chapters; }
			set 
			{
				chapters = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Chapters"));
			}
		}

		private UIColor temporalColor;
		public UIColor TemporalColor 
		{
			get { return temporalColor; }
			set 
			{
				temporalColor = value;
				foreach (var item in chapters)
					item.TemporalColor = temporalColor;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("TemporalColor"));
			}
		}

	}
}

