using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch
{
	public class CoverTextSlide : UIView
	{
		UILabel title;
		UILabel content;
		UIImageView backImage;

		UIImage coverImage;
		public UIImage CoverImage 
		{
			get { return coverImage; }
			set 
			{
				coverImage = value;
				backImage.Image = value;
				backImage.ContentMode = UIViewContentMode.ScaleAspectFill;
				backImage.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				backImage.ClipsToBounds = true;
			}
		}

		private LOSlideSource source;
		public LOSlideSource Source
		{
			get { return source; }
			set { source = value; initComponent(); }
		}

		public CoverTextSlide () : base ()
		{
			Frame = Constants.ScreenFrame;
			BackgroundColor = UIColor.Clear;

			backImage = new UIImageView (Constants.ScreenFrame);
			Add (backImage);

			title = Constants.makeLabel (new CGRect (450, 450, 500, 130), UIColor.White, UITextAlignment.Right, Font.Regular, 47);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			Add (title);

			content = Constants.makeLabel (new CGRect (450, 600, 500, 130), UIColor.White, UITextAlignment.Right, Font.Regular, 23);
			content.Lines = 0;
			content.LineBreakMode = UILineBreakMode.WordWrap;
			Add (content);

			var verticalLine = new UIView (new CGRect (450, 590, 500, 2));
			verticalLine.BackgroundColor = UIColor.Gray;
			Add (verticalLine);
		}

		void initComponent()
		{
			title.Text = source.Title.ToUpper();
			content.Text = source.Paragraph;
			title.TextColor = Source.Style.TitleColor;
			content.TextColor = Source.Style.ContentColor;
		}
	}
}

