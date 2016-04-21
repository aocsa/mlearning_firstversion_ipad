using System;
using UIKit;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public enum Font
	{
		Regular = 0,
		Bold,
		Light
	}

	public class Constants
	{
		static CGRect screenFrame = new CGRect (0, 0, 1024, 768);
		public static CGRect ScreenFrame {
			get { return screenFrame; }
		}

		static CGRect firstItemFrame = new CGRect (0, 54, ItemFrameWidth, ItemFrameHeight);
		public static CGRect FirstItemFrame {
			get { return firstItemFrame; }

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

		public static UILabel makeLabel (CGRect frame, UIColor color, UITextAlignment alignment, Font font, int textSize)
		{
			var label = new UILabel (frame)
			{
				TextColor = color,
				TextAlignment = alignment,
			};
			label.Font = UIFont.FromName (FontName [font], textSize);
			return label;
		}

		public static UIImageView makeImageView (CGRect frame, string resourcePath)
		{
			var image = new UIImageView (frame);
			image.Image = UIImage.FromFile (resourcePath);
			return image;
		}

		public static UITextField makeTextField (CGRect frame, UIColor color, string placeholder, Font font, int textSize)
		{
			var textField = new UITextField (frame) 
			{
				TextColor = color,
				Placeholder = placeholder
			};
			textField.Font = UIFont.FromName (FontName [font], textSize);
			return textField;
		}
			
		public static UIColor GetColorByIndex(int i)
		{
			if (i % 6 == 0) return UIColor.FromRGBA (255, 71, 69, 255);
			if (i % 6 == 1) return UIColor.FromRGBA (114,173,66, 255);
			if (i % 6 == 2) return UIColor.FromRGBA (0,163,151, 255);
			if (i % 6 == 3) return UIColor.FromRGBA (244,195,56, 255);
			if (i % 6 == 4) return UIColor.FromRGBA (67, 202, 255, 255);
			if (i % 6 == 5) return UIColor.FromRGBA (0, 70, 205, 255);
			return UIColor.LightGray;
		}

		public static UIColor GetSecondColorByIndex(int i)
		{
			if (i % 6 == 0) return UIColor.FromRGBA (250,191,57, 255);
			if (i % 6 == 1) return UIColor.FromRGBA (195,216,72, 255);
			if (i % 6 == 2) return UIColor.FromRGBA (97,217,226, 255);
			if (i % 6 == 3) return UIColor.FromRGBA (247,82,149, 255);
			if (i % 6 == 4) return UIColor.FromRGBA (67, 202, 255, 255);
			if (i % 6 == 5) return UIColor.FromRGBA (0, 70, 205, 255);
			return UIColor.LightGray;
		}


		public static UIImageView getLogoByIndex (int i)
		{
			string logoName = "iOS Resources/loview/logo-0.png";
			if (i % 4 == 0) logoName = "iOS Resources/loview/logo-0.png";
			if (i % 4 == 1) logoName = "iOS Resources/loview/logo-1.png";
			if (i % 4 == 2) logoName = "iOS Resources/loview/logo-2.png";
			if (i % 4 == 3) logoName = "iOS Resources/loview/logo-3.png";

			return Constants.makeImageView (new CGRect (823, 100, 148, 91), logoName);
		}

		public static float DeviceHeight = 768.0f;
		public static float DeviceWidth = 1024.0f;
		public static float ThumbWidth = 181.0f;
		public static float ThumbHeight = 138.0f;
		public static float ItemFrameWidth = 191.0f;
		public static float ItemFrameHeight = 148.0f;
		public static float ItemSeparation = 32;
		public static float ItemStackWidth = 202.0f;
		public static float ItemStackHeight = 180.0f;
		public static float StackWidth = 244.0f;
		public static float StackHeight = 286.0f;
		public static float DeltaY = (float)((768 - 286) / 2 + 4); //translate of Y 
		public static float ThumbScale = 6.0f; // 1024.0 / 171.0 ;
		public static float ControlHeight = 426; // 1024.0 / 171.0 ;
		public static float StartWidth = 400;

		public static int SlidesParagraphSize = 30;
		public static int SlidesTitleSize = 35;
		public static float HeightForScrollReader = 725;
		public static int TextSeparationInReader = 20;

		public delegate void StackItemFullAnimationStartedTriggered(object sender);
		public delegate void StackItemFullAnimationCompletedTriggered(object sender);
		public delegate void StackItemThumbAnimationStartedTriggered(object sender);
		public delegate void StackItemThumbAnimationCompletedTriggered(object sender);	


		public delegate void StackItemFullAnimationStartedEventHandler(object sender, int chapter, int section, int page);
		public delegate void StackItemFullAnimationCompletedEventHandler(object sender, int chapter, int section, int page);
		public delegate void StackItemThumbAnimationStartedEventHandler(object sender, int chapter, int section, int page);
		public delegate void StackItemThumbAnimationCompletedEventHandler(object sender, int chapter, int section, int page);	


		public static async Task<UIImage> DownloadImageAsync(string imageUrl)
		{
			var httpclient = new HttpClient();
			Task <Byte[]> contentsTask = httpclient.GetByteArrayAsync (imageUrl);
			var contents = await contentsTask;
			return UIImage.LoadFromData(NSData.FromArray(contents));
		}

		public static int resizeUILabelHeight (string text, UIFont font, nfloat width)
		{
			var nsText = new NSMutableAttributedString(text);
			nsText.AddAttribute(UIStringAttributeKey.Font, font, new NSRange(0, nsText.Length));

			var ctxt = new NSStringDrawingContext ();
			var expectedSize = nsText.GetBoundingRect (new CGSize(width, float.MaxValue), 
				NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading,
				ctxt).Size;

			int expectedHeight = (int)Math.Round (expectedSize.Height);
			return expectedHeight;
		}

		public static UIButton getLeftArrowForSlide (string urlPath)
		{
			var leftArrow = new UIButton (UIButtonType.Custom) { Frame = new CGRect (5, 360, 28, 48) };
			leftArrow.SetBackgroundImage (UIImage.FromFile (urlPath), UIControlState.Normal);
			leftArrow.Transform = CGAffineTransform.Rotate (leftArrow.Transform, (float)Math.PI);
			return leftArrow;
		}

		public static UIButton getRightArrowForSlide (string urlPath)
		{
			var rightArrow = new UIButton (UIButtonType.Custom) { Frame = new CGRect (991, 360, 28, 48) };
			rightArrow.SetBackgroundImage (UIImage.FromFile (urlPath), UIControlState.Normal);
			return rightArrow;
		}

		public static UIButton getBottomArrowForSlide (string urlPath)
		{
			var bottomArrow = new UIButton (UIButtonType.Custom) { Frame = new CGRect (483, 730, 57, 33) };
			bottomArrow.SetBackgroundImage (UIImage.FromFile (urlPath), UIControlState.Normal);
			return bottomArrow;
		}
	}
}

