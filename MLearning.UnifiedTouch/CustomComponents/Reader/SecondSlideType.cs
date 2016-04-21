using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class SecondSlideType : UIView
	{
		LOSlideSource source;
		public LOSlideSource Source
		{
			get { return source; }
			set { source = value; initComponent(); }
		}

		UIColor titlecolor;
		public UIColor TitleColor
		{
			get { return titlecolor; }
			set { titlecolor = value; }
		}

		UIColor contentcolor;
		public UIColor ContentColor
		{
			get { return contentcolor; }
			set { contentcolor = value; }
		}

		UILabel title, paragraph;
		UIScrollView contentScroll;
		UIImageView leftAvatar;
		IconSlideBar iconBar;

		public event moveReaderScrollToLeft MoveScrollToLeft;
		public event moveReaderScrollToRight MoveScrollToRight;
		public event moveReaderScrollDown MoveScrollDown;


		public SecondSlideType (int pos) : base()
		{
			var frame = Constants.ScreenFrame;
			frame.Y = pos * Constants.DeviceHeight;
			Frame = frame;


			contentScroll = new UIScrollView (new CGRect (0, 0, Constants.DeviceWidth, Constants.HeightForScrollReader)) 
			{
				Bounces = false,
				BackgroundColor = UIColor.Clear
			};

			title = Constants.makeLabel (new CGRect (509, 150, 380, 140), UIColor.White, UITextAlignment.Right, Font.Regular, Constants.SlidesTitleSize);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (title);

			paragraph = Constants.makeLabel (new CGRect (509, title.Frame.Y + title.Frame.Height + Constants.TextSeparationInReader, 380, 20), UIColor.White, UITextAlignment.Right, Font.Light, Constants.SlidesParagraphSize);
			paragraph.Lines = 0;
			paragraph.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (paragraph);

			leftAvatar = new UIImageView (new CGRect (90, 144, 376, 480));
			leftAvatar.Image = UIImage.FromBundle ("iOS Resources/slides/leftimg.png");

			Add (leftAvatar);
			Add (contentScroll);
		}

		void initComponent()
		{
			iconBar = new IconSlideBar();
			Add(iconBar);

			if (source.Type != 0)
			{
				if (source.Style.ColorNumber != 0)
					iconBar.ImageUrl = "iOS Resources/ricons/estilo" + source.Style.ID + "_color" + source.Style.ColorNumber + "-0" + source.Type + ".png";
				else
					iconBar.ImageUrl = "iOS Resources/ricons/tema5_colorblanco-0" + source.Type + ".png";
				iconBar.LineColor = source.Style.TitleColor;

				//arrows
				var arrowUrl = "iOS Resources/arrows/side_blanco.png";
				if(source.Style.ColorNumber != 0)
					arrowUrl = "iOS Resources/arrows/side" + source.Style.ID + "_color" + source.Style.ColorNumber + ".png";

				var leftArrow = Constants.getLeftArrowForSlide (arrowUrl);
				leftArrow.TouchUpInside += (object sender, EventArgs e) => 
				{
					MoveScrollToLeft ();
				};
				Add (leftArrow);

				var rightArrow = Constants.getRightArrowForSlide (arrowUrl);
				rightArrow.TouchUpInside += (object sender, EventArgs e) => 
				{
					MoveScrollToRight ();
				};
				Add (rightArrow);

				//bottomArrow
				arrowUrl = "iOS Resources/arrows/bottom_blanco.png";
				if(source.Style.ColorNumber != 0)
					arrowUrl = "iOS Resources/arrows/bottom" + source.Style.ID + "_color" + source.Style.ColorNumber + ".png";

				var bottomArrow = Constants.getBottomArrowForSlide (arrowUrl);
				bottomArrow.TouchUpInside += (object sender, EventArgs e) => 
				{
					MoveScrollDown ();
				};
				Add (bottomArrow);
			}

			if (source.Title != null)
				title.Text = source.Title.ToUpper();
			else
				title.Text = "";

			paragraph.Text = source.Paragraph;

			int expectedHeight = Constants.resizeUILabelHeight (title.Text, title.Font, title.Frame.Width);
			title.Frame = new CGRect (title.Frame.X, title.Frame.Y, title.Frame.Width, expectedHeight);

			/*resize height according to text*/

			expectedHeight = Constants.resizeUILabelHeight (paragraph.Text, paragraph.Font, paragraph.Frame.Width);
			paragraph.Frame = new CGRect (paragraph.Frame.X, title.Frame.Y + title.Frame.Height + Constants.TextSeparationInReader, paragraph.Frame.Width, expectedHeight);


			contentScroll.ContentSize = new CGSize (contentScroll.Frame.Width, title.Frame.Y + title.Frame.Height + Constants.TextSeparationInReader + expectedHeight + Constants.TextSeparationInReader / 2);
			title.TextColor = source.Style.TitleColor;
			paragraph.TextColor = source.Style.ContentColor;
			BackgroundColor = Source.Style.Background;

		}


	}
}

