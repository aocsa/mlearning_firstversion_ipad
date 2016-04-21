using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch
{
	public class IconSlideBar : UIView
	{
		UIImageView icon;
		UIView topLine, bottomLine;
		public IconSlideBar () : base()
		{
			Frame = new CGRect (66, 0, 54, 768);

			icon = new UIImageView (new CGRect(0, 357, 54, 54)); 
			Add(icon);

			topLine = new UIView (new CGRect (25, 0, 4, 313));
			Add(topLine);

			bottomLine = new UIView (new CGRect (25, 455, 4, 313));
			Add(bottomLine);
		}

		string imageUrl;
		public string ImageUrl
		{
			get { return imageUrl; }
			set
			{
				imageUrl = value;
				icon.Image = UIImage.FromFile (imageUrl);
			}
		}


		UIColor lineColor;
		public UIColor LineColor
		{
			get { return lineColor; }
			set
			{
				lineColor = value;
				topLine.BackgroundColor = lineColor;
				bottomLine.BackgroundColor = lineColor;
			}
		}
	}
}

