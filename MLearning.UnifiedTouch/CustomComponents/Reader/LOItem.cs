using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace MLearning.UnifiedTouch
{
	public class LOItem : UIView
	{
		UILabel text;
		UIView bullet;

		string textContent;
		public string TextContent
		{
			get { return textContent; }
			set { 
				textContent = value;
				text.Text = value; 

				int expectedHeight = Constants.resizeUILabelHeight (text.Text, text.Font, text.Frame.Width);
				text.Frame = new CGRect (text.Frame.X, text.Frame.Y, text.Frame.Width, expectedHeight);
				Frame = new CGRect (Frame.X, Frame.Y, Frame.Width, expectedHeight);
			}
		}

		UIColor textColor;
		public UIColor TextColor
		{
			get { return textColor; }
			set { textColor = value; text.TextColor = value; }
		}


		UIColor bulletColor;
		public UIColor BulletColor
		{
			get { return bulletColor; }
			set { bulletColor = value; bullet.BackgroundColor = value; }
		}

		public LOItem (nfloat xPos, nfloat yPos) : base()
		{
			BackgroundColor = UIColor.Clear;
			Frame = new CGRect (xPos, yPos, 380, 20);
			bullet = new UIView (new CGRect (3, Constants.SlidesParagraphSize, 14, 14));
			bullet.Layer.CornerRadius = 7;
			bullet.BackgroundColor = UIColor.Black;
			Add (bullet);

			text = Constants.makeLabel(new CGRect (30, 12, 350, 20), UIColor.Black, UITextAlignment.Left, Font.Light, Constants.SlidesParagraphSize);
			text.Lines = 0;
			text.LineBreakMode = UILineBreakMode.WordWrap;
			Add (text);
		}
	}
}

