using System;
using UIKit;

namespace MLearning.UnifiedTouch
{
	public class LOSlideStyle
	{
		int id = 0;
		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		int colorNumber;
		public int ColorNumber
		{
			get { return colorNumber; }
			set { colorNumber = value; }
		}

		UIColor titleColor;
		public UIColor TitleColor 
		{
			get { return titleColor; }
			set { titleColor = value; }
		}

		UIColor contentColor;
		public UIColor ContentColor 
		{
			get { return contentColor; }
			set { contentColor = value; }
		}

		UIColor background;
		public UIColor Background 
		{
			get { return background; }
			set { background = value; }
		}

		UIColor borderColor;
		public UIColor BorderColor 
		{
			get { return borderColor; }
			set { borderColor = value; }
		}

		public LOSlideStyle ()
		{
		}
	}
}

