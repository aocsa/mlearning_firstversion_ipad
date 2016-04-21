using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using System.Net.Http;

namespace MLearning.UnifiedTouch
{
	public class ThirdSlideType : UIView
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

		UILabel title, name1, name2, name3;
		UIManipulableView view1, view2, view3;
		UIImageView image1, image2, image3;

		public event moveReaderScrollToLeft MoveScrollToLeft;
		public event moveReaderScrollToRight MoveScrollToRight;
		public event moveReaderScrollDown MoveScrollDown;


		public ThirdSlideType (int pos) : base()
		{
			var frame = Constants.ScreenFrame;
			frame.Y = pos * Constants.DeviceHeight;
			Frame = frame;


			title = Constants.makeLabel (new CGRect (112, 58, 800, 140), UIColor.White, UITextAlignment.Center, Font.Regular, Constants.SlidesTitleSize);
			title.Lines = 0;
			title.LineBreakMode = UILineBreakMode.WordWrap;
			Add (title);

			view1 = new UIManipulableView ();
			view1.setFrame (new CGRect (190, 250, 208, 190));
			view1.Layer.CornerRadius = 4;
			view1.UserInteractionEnabled = false;

			image1 = new UIImageView (new CGRect (5, 5, 198, 180));
			image1.ContentMode = UIViewContentMode.ScaleAspectFill;
			image1.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			image1.ClipsToBounds = true;
			view1.Add (image1);

			view2 = new UIManipulableView ();
			view2.setFrame (new CGRect (468, 250, 208, 190));
			view2.Layer.CornerRadius = 4;
			view2.UserInteractionEnabled = false;

			image2 = new UIImageView (new CGRect (5, 5, 198, 180));
			image2.ContentMode = UIViewContentMode.ScaleAspectFill;
			image2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			image2.ClipsToBounds = true;
			view2.Add (image2);


			view3 = new UIManipulableView ();
			view3.setFrame (new CGRect (746, 250, 208, 190));
			view3.Layer.CornerRadius = 4;
			view3.UserInteractionEnabled = false;

			image3 = new UIImageView (new CGRect (5, 5, 198, 180));
			image3.ContentMode = UIViewContentMode.ScaleAspectFill;
			image3.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			image3.ClipsToBounds = true;
			view3.Add (image3);


			name1 = Constants.makeLabel (new CGRect (190, 455, 208, 200), UIColor.White, UITextAlignment.Left, Font.Regular, 14);
			name1.Lines = 0;
			name1.LineBreakMode = UILineBreakMode.WordWrap;
			Add (name1);

			name2 = Constants.makeLabel (new CGRect (468, 455, 208, 200), UIColor.White, UITextAlignment.Left, Font.Regular, 14);
			name2.Lines = 0;
			name2.LineBreakMode = UILineBreakMode.WordWrap;
			Add (name2);

			name3 = Constants.makeLabel (new CGRect (746, 455, 208, 200), UIColor.White, UITextAlignment.Left, Font.Regular, 14);
			name3.Lines = 0;
			name3.LineBreakMode = UILineBreakMode.WordWrap;
			Add (name3);

			Add (view1);
			Add (view2);
			Add (view3);
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

			if (source.Title != null)
				title.Text = source.Title.ToUpper();
			else
				title.Text = "";
			title.TextColor = source.Style.TitleColor;
			BackgroundColor = source.Style.Background;

			for (int i = 0; i < source.Itemize.Count; i++) 
			{
				var item = source.Itemize [i];
				switch (i)
				{
				case 0:
					name1.Text = item.Text;
					name1.SizeToFit ();
					name1.TextColor = source.Style.ContentColor;
					view1.BackgroundColor = source.Style.TitleColor;

						Constants.DownloadImageAsync (item.ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
							try { image1.Image = task.Result; }
							catch{ }
						}));

					view1.UserInteractionEnabled = true;

					break;
				case 1:
					name2.Text = item.Text;
					name2.SizeToFit ();

					name2.TextColor = source.Style.ContentColor;
					view2.BackgroundColor = source.Style.TitleColor;

						Constants.DownloadImageAsync (item.ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
							try { image2.Image = task.Result; }
							catch{ }
						}));
					
					view2.UserInteractionEnabled = true;

					break;
				case 2:
					name3.Text = item.Text;
					name3.SizeToFit ();

					name3.TextColor = source.Style.ContentColor;
					view3.BackgroundColor = source.Style.TitleColor;

						Constants.DownloadImageAsync (item.ImageUrl).ContinueWith ((task) => InvokeOnMainThread (() => {
							try { image3.Image = task.Result; }
							catch{ }
						}));

					view3.UserInteractionEnabled = true;

					break;

				}
			}
		}


	}
}

