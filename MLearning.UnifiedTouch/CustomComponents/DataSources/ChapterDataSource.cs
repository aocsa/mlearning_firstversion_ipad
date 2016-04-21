using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UIKit;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class ChapterDataSource : INotifyPropertyChanged
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

		private string author;
		public string Author 
		{
			get { return author; }
			set 
			{
				author = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Author"));
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

		private ObservableCollection<SectionDataSource> sections = new ObservableCollection<SectionDataSource> ();
		public ObservableCollection<SectionDataSource> Sections 
		{
			get { return sections; }
			set 
			{
				sections = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Sections"));
			}
		}

		private UIColor chapterColor;
		public UIColor ChapterColor 
		{
			get { return chapterColor; }
			set 
			{
				chapterColor = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("ChapterColor"));
			}
		}

		private UIColor temporalColor;
		public UIColor TemporalColor 
		{
			get { return temporalColor; }
			set 
			{
				temporalColor = value;
				foreach (var item in sections)
					item.TemporalColor = temporalColor;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("TemporalColor"));
			}
		}

		private UIImage backgroundImage;
		public UIImage BackgroundImage 
		{
			get { return backgroundImage; }
			set 
			{
				backgroundImage = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("BackgroundImage"));
			}
		}
	}
}

