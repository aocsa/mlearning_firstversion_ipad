using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;

namespace MLearning.Touch
{
	public enum Font
	{
		Regular = 0,
		Bold,
		Light
	}

	public class Constants
	{
		static RectangleF screenFrame = new RectangleF (0, 0, 1024, 768);
		public static RectangleF ScreenFrame {
			get { return screenFrame; }
		}

		static Dictionary<Font, string> fontName = new Dictionary<Font, string> ()
		{
			{ Font.Regular 	, "HelveticaNeue" },
			{ Font.Bold 	, "HelveticaNeue-Bold" },
			{ Font.Light 	, "HelveticaNeue-Light" }
		};
		public static Dictionary<Font, string> FontName {
			get { return fontName; }
		}

		static float mloCornerRadius = 2;
		public static float MloCornerRadius {
			get {return mloCornerRadius;}
		}


		static UIColor blueColor = UIColor.FromRGB (78, 177, 223);
		public static UIColor BlueColor {
			get {return blueColor;}
		}

		static UIColor searchBarColor = UIColor.FromRGB(246, 240, 234);
		public static UIColor SearchBarColor {
			get {return searchBarColor;}
		}

		static UIColor loginLabelColor = UIColor.FromRGB (147, 147, 147);
		public static UIColor LoginLabelColor {
			get {return loginLabelColor;}
		}

		static UIColor loginTextColor = UIColor.FromRGB (179, 179, 179);
		public static UIColor LoginTextColor {
			get {return loginTextColor;}
		}

		static int loginFormOffsetY = -130;
		public static int LoginFormOffsetY {
			get {return loginFormOffsetY;}
		}

		static CGColor[] mainViewGradientColors = new CGColor[2] { UIColor.FromRGB(62,57,53).CGColor, UIColor.FromRGB(30,27,30).CGColor};
		public static CGColor[] MainViewGradientColors {
			get {return mainViewGradientColors;}
		}

		static float rightViewX = 316;
		public static float RightViewX {
			get {return rightViewX;}
		}

		public static UILabel makeLabel (RectangleF frame, UIColor color, UITextAlignment alignment, Font font, int textSize)
		{
			var label = new UILabel (frame)
			{
				TextColor = color,
				TextAlignment = alignment,
			};
			label.Font = UIFont.FromName (FontName [font], textSize);
			return label;
		}

		public static UIImageView makeImageView (RectangleF frame, string resourcePath)
		{
			var image = new UIImageView (frame);
			image.Image = UIImage.FromFile (resourcePath);
			return image;
		}

		public static UITextField makeTextField (RectangleF frame, UIColor color, string placeholder, Font font, int textSize)
		{
			var textField = new UITextField (frame) 
			{
				TextColor = color,
				Placeholder = placeholder
			};
			textField.Font = UIFont.FromName (FontName [font], textSize);
			return textField;
		}
	}
}

