using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public delegate void ControlDownElementSelectedEventHandler (object sender, int index);

	public class ControlDownElement : UIView
	{
		public event ControlDownElementSelectedEventHandler ControlDownElementSelected;

		int index;
		public int Index
		{
			set { index = value; }
			get { return index; }
		}

		ChapterDataSource source;
		public ChapterDataSource Source
		{
			set { source = value; updateValues(); }
			get { return source; }
		}
			
		bool isSelected;
		public bool IsSelected 
		{
			get { return isSelected; }
			set { isSelected = value; }
		}

		UILabel textName;
		public UILabel TextName 
		{
			get { return textName; }
			set { textName = value; }
		}

		public ControlDownElement (int pos)
		{
			Frame = new CGRect (pos * 198 + pos * 2, 0, 198, 88);
			BackgroundColor = UIColor.FromRGBA (0, 0, 0, 200);

			TextName = Constants.makeLabel (new CGRect (25, 14, 148, 60), UIColor.White, UITextAlignment.Center, Font.Bold, 14);
			TextName.LineBreakMode = UILineBreakMode.WordWrap;
			TextName.Lines = 0;

			TextName.Text = "Capítulo " + pos;
			Add (TextName);
			var tap = new UITapGestureRecognizer (handleTap);
			AddGestureRecognizer (tap);

		}

		public void Select()
		{
			IsSelected = true;
			animateToColor(Source.ChapterColor.ColorWithAlpha (0.7f));
		}

		public void Unselect()
		{
			IsSelected = false;
			animateToColor(UIColor.FromRGBA(0, 0, 0, 175));
		}

		void animateToColor(UIColor c)
		{
			UIView.Animate (
				duration: 0.35f,
				animation: () => 
				{
					BackgroundColor = c;
				}
			);
		}

		void updateValues()
		{
			if (Source.Title != null)
				TextName.Text = Source.Title;
			else
				TextName.Text = "No text found";
		}

		void handleTap (UITapGestureRecognizer tap)
		{
			ControlDownElementSelected (this, Index);
		}
	}
}

