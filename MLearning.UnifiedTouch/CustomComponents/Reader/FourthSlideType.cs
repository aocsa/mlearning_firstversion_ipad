using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class FourthSlideType : UIView
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

		UILabel title;
		UIScrollView contentScroll;
		UIImageView rightAvatar;

		public event moveReaderScrollToLeft MoveScrollToLeft;
		public event moveReaderScrollToRight MoveScrollToRight;
		public event moveReaderScrollDown MoveScrollDown;


		public FourthSlideType (int pos) : base()
		{
			var frame = Constants.ScreenFrame;
			frame.Y = pos * Constants.DeviceHeight;
			Frame = frame;


			contentScroll = new UIScrollView (new CGRect (0, 0, Constants.DeviceWidth, Constants.HeightForScrollReader)) 
			{
				Bounces = false,
				BackgroundColor = UIColor.Clear
			};

			title = Constants.makeLabel (new CGRect (170, 150, 380, 140), UIColor.White, UITextAlignment.Right, Font.Regular, Constants.SlidesTitleSize);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			contentScroll.Add (title);


			rightAvatar = new UIImageView (new CGRect (580, 144, 377, 480));
			rightAvatar.Image = UIImage.FromBundle ("iOS Resources/slides/rightimg.png");
			Add (rightAvatar);

			Add (contentScroll);
		}

		void initComponent()
		{
			var iconBar = new IconSlideBar();
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

			//ITEMS
			if (source.Title != null)
				title.Text = source.Title.ToUpper();
			else
				title.Text = "";

			int expectedHeight = Constants.resizeUILabelHeight (title.Text, title.Font, title.Frame.Width);
			title.Frame = new CGRect (title.Frame.X, title.Frame.Y, title.Frame.Width, expectedHeight);

			nfloat yPos = title.Frame.Y + title.Frame.Height + Constants.TextSeparationInReader;
			nfloat xPos = title.Frame.X;
			/*resize height according to text*/
			for (int i = 0; i < source.Itemize.Count; i++)
			{
				var item = new LOItem(xPos, yPos)
				{
					TextContent = source.Itemize[i].Text,
					TextColor = Source.Style.ContentColor,
					BulletColor = Source.Style.TitleColor
				};
				contentScroll.Add(item);
				yPos += item.Frame.Height;
			}


			contentScroll.ContentSize = new CGSize (contentScroll.Frame.Width, yPos + Constants.TextSeparationInReader);
			title.TextColor = source.Style.TitleColor;
			BackgroundColor = Source.Style.Background;
		}


	}
}

