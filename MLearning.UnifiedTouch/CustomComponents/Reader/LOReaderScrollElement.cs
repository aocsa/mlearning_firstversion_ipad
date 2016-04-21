using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch
{
	public class LOReaderScrollElement : UIScrollView
	{

		LOPageSource source;
		public LOPageSource Source 
		{
			get { return source; }
			set 
			{
				source = value;
				loadElement ();
			}
		}

		CoverTextSlide cover;
		public event moveReaderScrollToLeft MoveScrollToLeft;
		public event moveReaderScrollToRight MoveScrollToRight;


		public LOReaderScrollElement (int position) : base()
		{
			PagingEnabled = true;
			Bounces = false;

			var elementFrame = Constants.ScreenFrame;
			elementFrame.X = Constants.DeviceWidth * position;
			Frame = elementFrame;

			cover = new CoverTextSlide ();
			Add (cover);
			ContentSize = new CGSize (Constants.DeviceWidth, Constants.DeviceHeight);
		}

		void loadElement()
		{


			for (int i = 0; i < source.Slides.Count; i++) 
			{
				if (i == 0) 
				{
					cover.Source = source.Slides [0];
					cover.CoverImage = source.Cover;
					Source.PropertyChanged += (sender, e) => 
					{
						if (e.PropertyName == "Cover")
							cover.CoverImage = source.Cover;
					};
				}
				else 
				{
					switch (source.Slides [i].Type) 
					{
					case 1: 
						FirstSlideType slide1 = new FirstSlideType(i) { Source = source.Slides[i] };
						slide1.MoveScrollToLeft += HandleMoveScrollToLeft;
						slide1.MoveScrollToRight += HandleMoveScrollToRight;
						slide1.MoveScrollDown += HandleMoveScrollDown;
						Add(slide1);
						break;
					case 2:
						SecondSlideType slide2 = new SecondSlideType (i) { Source = source.Slides [i] };
						slide2.MoveScrollToLeft += HandleMoveScrollToLeft;
						slide2.MoveScrollToRight += HandleMoveScrollToRight;
						slide2.MoveScrollDown += HandleMoveScrollDown;
						Add(slide2);
						break;
						//4 y 3 inverted on purpose (mistake naming classes)
					case 4:
						ThirdSlideType slide3 = new ThirdSlideType(i) { Source = source.Slides[i] };
						slide3.MoveScrollToLeft += HandleMoveScrollToLeft;
						slide3.MoveScrollToRight += HandleMoveScrollToRight;
						slide3.MoveScrollDown += HandleMoveScrollDown;
						Add(slide3);
						break;
					case 3:
						FourthSlideType slide4 = new FourthSlideType(i) { Source = source.Slides[i] };
						slide4.MoveScrollToLeft += HandleMoveScrollToLeft;
						slide4.MoveScrollToRight += HandleMoveScrollToRight;
						slide4.MoveScrollDown += HandleMoveScrollDown;
						Add(slide4);
						break;
					default:
						Add (new UIView (){ BackgroundColor = UIColor.LightGray, Frame = new CGRect (0, i * Constants.DeviceHeight, Constants.DeviceWidth, Constants.DeviceHeight)});
						break;
					}


				}
			}

			ContentSize = new CGSize (Constants.DeviceWidth, Constants.DeviceHeight * source.Slides.Count);

		}

		void HandleMoveScrollToLeft ()
		{
			MoveScrollToLeft ();
		}

		void HandleMoveScrollToRight ()
		{
			MoveScrollToRight ();
		}

		void HandleMoveScrollDown ()
		{
			int currentPos = (int) (ContentOffset.Y / Constants.DeviceHeight);
			if (currentPos < source.Slides.Count - 1)
				SetContentOffset (new CGPoint (ContentOffset.X, ContentOffset.Y + Constants.DeviceHeight), true);
		}
	}
}

