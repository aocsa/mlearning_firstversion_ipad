using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using MLearning.UnifiedTouch.CustomComponents;
using Foundation;

namespace MLearning.UnifiedTouch
{
	public delegate void ItemHasChanged (object sender, int prevChapter, int chapter, int section, int page);
	public class LOReaderScroll : UIScrollView
	{


		List<LOPageSource> source;
		public List<LOPageSource> Source
		{
			get { return source; }
			set { source = value; loadSource(); }
		}


		int chapterIndex;
		public int ChapterIndex
		{
			get { return chapterIndex; }
			set { chapterIndex = value; }
		}


		int sectionIndex;
		public int SectionIndex
		{
			get { return sectionIndex; }
			set { sectionIndex = value; }
		}

		int pageIndex;
		public int PageIndex
		{
			get { return pageIndex; }
			set { pageIndex = value; }
		}

		int numberOfItems;
		public int NumberOfItems 
		{
			get { return numberOfItems; }
			set { numberOfItems = value; }
		}

		int currentIndex;
		public int CurrentIndex 
		{
			get { return currentIndex; }
			set { currentIndex = value; }
		}

		List<LOReaderScrollElement> elements;
		List<IStackItem> stackItems;
		bool itemChanged;
		int prevIndex;
		public event ItemHasChanged ItemHasChanged;
		IStackItem currentItem;
		static bool isVisible;
		public static bool IsVisible 
		{
			get { return isVisible; }
			set { isVisible = value; }
		}
			
		UIButton backArrow;
		public UIButton BackArrow 
		{
			get { return backArrow; }
			set { backArrow = value; }
		}

		public LOReaderScroll (List<IStackItem> items) : base ()
		{
			stackItems = items;
			Frame = Constants.ScreenFrame;
			PagingEnabled = true;
			Bounces = false;
			elements = new List<LOReaderScrollElement> ();
			itemChanged = false;
			Scrolled += HandleScrolled;
			isVisible = false;

			//gestures
			var pinchRecognizer = new UIPinchGestureRecognizer (handlePinchPanRotate);
			pinchRecognizer.CancelsTouchesInView = false;
			pinchRecognizer.DelaysTouchesBegan = false;
			pinchRecognizer.DelaysTouchesEnded = false;
			AddGestureRecognizer (pinchRecognizer);

			var rotationRecognizer = new UIRotationGestureRecognizer (handlePinchPanRotate);
			rotationRecognizer.CancelsTouchesInView = false;
			rotationRecognizer.DelaysTouchesBegan = false;
			rotationRecognizer.DelaysTouchesEnded = false;
			AddGestureRecognizer (rotationRecognizer);

			var panRecognizer = new UIPanGestureRecognizer (handlePinchPanRotate);
			panRecognizer.CancelsTouchesInView = false;
			panRecognizer.DelaysTouchesBegan = false;
			panRecognizer.DelaysTouchesEnded = false;
			panRecognizer.MinimumNumberOfTouches = 2;
			panRecognizer.MaximumNumberOfTouches = 2;
			AddGestureRecognizer (panRecognizer);

			pinchRecognizer.WeakDelegate = this;
			rotationRecognizer.WeakDelegate = this;
			panRecognizer.WeakDelegate = this;


			backArrow = new UIButton (UIButtonType.Custom) { Frame = new CGRect (14, 25, 37, 35) };
			backArrow.SetBackgroundImage (UIImage.FromFile ("iOS Resources/slides/back_icon.png"), UIControlState.Normal);
			backArrow.TouchUpInside += (object sender, EventArgs e) => 
			{
				SetInvisible();
				currentItem.AnchorPointUpdated = false;
				currentItem.moveToOriginalFrameAnimated (true);
			};
			backArrow.Layer.ZPosition = 110;
		}
			
		[Export("gestureRecognizer:shouldRecognizeSimultaneouslyWithGestureRecognizer:")]
		public virtual bool ShouldRecognizeSimultaneously (UIGestureRecognizer g1, UIGestureRecognizer g2)
		{
			if (g1.View != this)
				return false;
			if (g1.View != g2.View)
				return false;
			return true;
		}


		void handlePinchPanRotate (UIGestureRecognizer gesture)
		{
			switch (gesture.State) 
			{
			case UIGestureRecognizerState.Began:
				ScrollEnabled = false;
				SetInvisible ();
				currentItem.handlePinchPanRotate (gesture);
				break;
			case UIGestureRecognizerState.Ended:
				currentItem.AnchorPointUpdated = false;
				currentItem.moveToOriginalFrameAnimated (true);
				Frame = Constants.ScreenFrame;
				ScrollEnabled = true;
				RemoveFromSuperview ();
				break;
			default: 
				currentItem.handlePinchPanRotate (gesture);
				break;
			}
		}

		void HandleScrolled (object sender, EventArgs e)
		{
			if (!Tracking) 
			{
				if (updateCurrentElement ()) 
				{
					ItemHasChanged (this, stackItems[prevIndex].Chapter, stackItems[currentIndex].Chapter, stackItems [currentIndex].Section, stackItems [currentIndex].Page);

				}
			}
		}

		bool updateCurrentElement()
		{
			int item = (int) Math.Round (ContentOffset.X / Constants.DeviceWidth);
			if (item >= NumberOfItems || item < 0) 
			{
				itemChanged = false;
				return itemChanged;
			}
			if (item != currentIndex) 
			{
				prevIndex = currentIndex;
				currentIndex = item;
				stackItems [prevIndex].moveToOriginalFrameAnimated (false);

				currentItem = stackItems [currentIndex];
				currentItem.moveToFullScreenWindowAnimated (false);
				itemChanged = true;
			}
			else
				itemChanged = false;
			return itemChanged;
		}


		void loadSource()
		{
			numberOfItems = source.Count;

			for (int i = 0; i < numberOfItems; i++) 
			{
				LOReaderScrollElement element = new LOReaderScrollElement (i);
				element.Source = source [i];
				element.MoveScrollToLeft += () => 
				{
					if(currentIndex > 0)
						SetContentOffset (new CGPoint ((currentIndex - 1) * Constants.DeviceWidth, 0), true); 
				};

				element.MoveScrollToRight += () => 
				{
					if(currentIndex < numberOfItems - 1)
						SetContentOffset (new CGPoint ((currentIndex + 1) * Constants.DeviceWidth, 0), true); 
				};
				Add (element);
				elements.Add (element);
			}
			ContentSize = new CGSize (Constants.DeviceWidth * numberOfItems, Constants.DeviceHeight);
		}
			

		public override void SetContentOffset (CGPoint contentOffset, bool animated)
		{
			base.SetContentOffset (contentOffset, animated);
			currentIndex = (int)Math.Round (ContentOffset.X / Constants.DeviceWidth);
			currentItem = stackItems [currentIndex];
		}

		public void SetVisible()
		{
			isVisible = true;
			Alpha = 1;
			foreach (var item in stackItems) 
			{
				item.Alpha = 0;

			}
		}

		public void SetInvisible()
		{
			backArrow.RemoveFromSuperview ();
			isVisible = false;
			Alpha = 0;
			foreach (var item in stackItems) 
			{
				item.Alpha = 1;

			}
		}
	}
}

