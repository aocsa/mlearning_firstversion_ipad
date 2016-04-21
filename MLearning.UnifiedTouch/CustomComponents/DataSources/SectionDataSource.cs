using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UIKit;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class SectionDataSource : INotifyPropertyChanged
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

		private ObservableCollection<PageDataSource> pages = new ObservableCollection<PageDataSource> ();
		public ObservableCollection<PageDataSource> Pages 
		{
			get { return pages; }
			set 
			{
				pages = value;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("Pages"));
			}
		}


		private UIColor temporalColor;
		public UIColor TemporalColor 
		{
			get { return temporalColor; }
			set 
			{
				temporalColor = value;
				foreach (var item in pages)
					item.BorderColor = temporalColor;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("TemporalColor"));
			}
		}

		private UIColor sectionColor;
		public UIColor SectionColor 
		{
			get { return sectionColor; }
			set 
			{
				sectionColor = value;
				foreach (var item in pages)
					item.BorderColor = sectionColor;
				if (PropertyChanged != null)
					PropertyChanged (this, new PropertyChangedEventArgs ("SectionColor"));
			}
		}
	}
}

