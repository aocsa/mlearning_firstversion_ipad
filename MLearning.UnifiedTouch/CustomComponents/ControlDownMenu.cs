using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class ControlDownMenu : UIScrollView
	{
		BookDataSource source;
		public BookDataSource Source 
		{
			get { return source;}
			set { source = value; initStack (); }
		}

		int currentIndex;
		public int CurrentIndex {
			get { return currentIndex;}
			set { currentIndex = value;}
		}

		List<ControlDownElement> elements;
		public List<ControlDownElement> Elements 
		{
			get { return elements;}
			set { elements = value;}
		}

		public event ControlDownElementSelectedEventHandler ControlDownElementSelected;


		public ControlDownMenu () : base()
		{
			Frame = new CGRect (0, 680, 1024, 88);
			CurrentIndex = 0;
			Elements = new List<ControlDownElement> ();
			ShowsHorizontalScrollIndicator = false;
			ShowsVerticalScrollIndicator = false;
			Bounces = false;
			UserInteractionEnabled = true;
		}

		public void SelectElement(int index)
		{
			if (index != CurrentIndex && Elements.Count > 0)
			{
				if (CurrentIndex >= 0)
					Elements[CurrentIndex].Unselect();
				CurrentIndex = index;

				Elements[CurrentIndex].Select();
			}

		}

		void initStack()
		{
			for (int i = 0; i < Source.Chapters.Count; i++)
			{
				var elem = new ControlDownElement(i);
				elem.Index = i;
				elem.Source = Source.Chapters[i];
				elem.ControlDownElementSelected += ControlDown_ElementSelected;
				Add(elem);
				Elements.Add (elem);
				if (i == 0) elem.Select();
			}

			ContentSize = new CGSize (Elements.Count * 198 + (Elements.Count - 1) * 2, 88);
		}

		void ControlDown_ElementSelected(object sender, int index)
		{
			SelectElement(index);
			ControlDownElementSelected(this, index);
		}

	}
}

