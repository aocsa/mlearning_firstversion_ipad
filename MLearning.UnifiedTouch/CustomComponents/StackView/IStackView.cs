using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public delegate void StackItemsMovingEventHandler (object sender, int index, nfloat delta);
	public delegate void StackItemsMovingFinishEventHandler (object sender, int index, nfloat delta, bool isChange);
	public delegate void StackItemCreated (object sender, int index);

	public class IStackView : UIView
	{
		List <IStackItem> itemsVector;
		List<float> angles;

		int chapter;
		public int Chapter
		{
			set { chapter = value; }
			get { return chapter; }
		}

		int section;
		public int Section
		{
			set { section = value; }
			get { return section; }
		}

		int stackNumber;
		public int StackNumber
		{
			set
			{
				stackNumber = value;
			}
			get { return stackNumber; }
		}

		int numberOfItems;
		public int NumberOfItems
		{
			set { numberOfItems = value; }
			get { return numberOfItems; }
		}
			

		SectionDataSource source;
		public SectionDataSource Source
		{
			set
			{
				source = value;
				initComponent();
			}
			get { return source; }
		}

		IStackItem selectedItem;
		public IStackItem SelectedStackItem
		{
			get { return selectedItem; }
		}

		CGPoint firstCenter;
		public CGPoint FirstCenter 
		{
			get { return firstCenter; }
			set { firstCenter = value; }
		}

		bool isStack = false;
		public bool IsStack 
		{
			get { return isStack; }
			set { isStack = value; }
		}

		public override CGRect Frame 
		{
			get { return base.Frame; }
			set 
			{
				base.Frame = value;
				firstCenter = Center;
			}
		}
			
		public event StackItemsMovingEventHandler StackViewsUpdate;
		public event StackItemsMovingFinishEventHandler StackViewsDidFinish;
		public event StackItemCreated StackItemCreated;

		public event Constants.StackItemFullAnimationCompletedEventHandler StackItemFullAnimationCompleted;
		public event Constants.StackItemFullAnimationStartedEventHandler StackItemFullAnimationStarted;
		public event Constants.StackItemThumbAnimationCompletedEventHandler StackItemThumbAnimationCompleted;
		public event Constants.StackItemThumbAnimationStartedEventHandler StackItemThumbAnimationStarted;


		public IStackView (float stackViewXPosition) : base ()
		{
			itemsVector = new List<IStackItem> ();
			Frame = new CGRect (stackViewXPosition, 0, Constants.ThumbWidth, Constants.ControlHeight);

			initProperties ();

			var pinch = new UIPinchGestureRecognizer (handlePinch);
			AddGestureRecognizer (pinch);

			var tapScroll = new UITapGestureRecognizer (handleTapScroll);
			AddGestureRecognizer (tapScroll);

		}

		#region Initialization
		void initProperties ()
		{
			stackNumber = 0;
			numberOfItems = 0;

			angles = new List<float>();
			angles.Add (8.0f);
			angles.Add(-8.0f);
			angles.Add( 0.0f);
		}
		#endregion

		#region StackGestures
		void handleTapScroll (UITapGestureRecognizer tap)
		{
			if (isStack) 
			{
				animateToShowItems (0.5f);
			}
		}
	
		nfloat lastScale = 1.0f;
		void handlePinch(UIPinchGestureRecognizer p)
		{
			var scale = 1.0f - (lastScale - p.Scale);

			if (p.State == UIGestureRecognizerState.Began) 
			{
				for (int i = 1; i < itemsVector.Count; i++) 
				{
					var image = itemsVector [i];
					var transform = CGAffineTransform.MakeRotation (toRadians(angles[(i-1) % 3]));
					UIView.Animate (duration: 0.25f,
						animation: () => 
						{
							image.Transform = transform;

						});
				}
				//hideInfo
				foreach (var item in itemsVector)
					item.hideInfo ();

			}
			if (p.State == UIGestureRecognizerState.Changed) 
			{
				float grow = scale > 1 ? -3 : 4;

				for (int i = 1; i < itemsVector.Count; i++) 
				{
					var image = itemsVector [i];
					var tmp = image.Center;
					if ( scale == 1 || (scale < 1 && tmp.X - grow * i < itemsVector[0].Center.X
						|| (scale > 1 && itemsVector[0].Center.X + Constants.ItemSeparation + Constants.ItemFrameWidth < itemsVector[1].Center.X ) )
					)
						break; 
					tmp.X -= grow * i;
					image.Center = tmp;

					if(i == NumberOfItems - 1)
						StackViewsUpdate (this, StackNumber, grow * i);


				}

				lastScale = p.Scale;

			}
			if (p.State == UIGestureRecognizerState.Ended) 
			{

				if (lastScale < 1)
					animateToStack (0.5f);
				else
					animateToShowItems (0.5f);
			}

		}

		public void animateToShowItems(float duration)
		{
				UIView.Animate (duration: duration,
					animation: () => 
					{
						for (int i = 1; i < itemsVector.Count; i++) 
						{

							var image = itemsVector [i];
							//tmp represents the center of the first item in stack (the center is fixed)
							var tmp = new CGPoint (95.5f, 128);

							image.Transform = CGAffineTransform.MakeIdentity ();

							tmp.X += ((Constants.ItemFrameWidth + Constants.ItemSeparation) * (i));
							image.Center = tmp;

							if (i == itemsVector.Count - 1) 
							{
								var fullItemWidth = (Constants.ItemFrameWidth + Constants.ItemSeparation);
								var PrevCenterOfLastItem = fullItemWidth / 2.0f;
								var LastCenterOfLastItem = (NumberOfItems * fullItemWidth) - (fullItemWidth / 2.0f);
								
								if(isStack)
									StackViewsDidFinish (this, StackNumber, PrevCenterOfLastItem - LastCenterOfLastItem, true);
								else
									StackViewsDidFinish (this, StackNumber, PrevCenterOfLastItem - LastCenterOfLastItem, false);

							}

						}

						foreach (var image in itemsVector)
							image.UserInteractionEnabled = true;

					},
					completion: () => 
					{
						//showInfo
						foreach (var item in itemsVector)
							item.showInfo ();
					});

				var newFrame = Frame;
				newFrame.Width = (Constants.ItemFrameWidth + Constants.ItemSeparation) * NumberOfItems;
				Frame = newFrame;

				isStack = false;
				
		}

		public void animateToStack(float duration)
		{

				UIView.Animate (duration: duration,
					animation: () => {

						for (int i = 1; i < itemsVector.Count; i++) {
							var image = itemsVector [i];

							var tmp = itemsVector [0].Center;
							image.Center = tmp; 

							if (i == itemsVector.Count - 1) {
								var fullItemWidth = (Constants.ItemFrameWidth + Constants.ItemSeparation);
								var PrevCenterOfLastItem = (NumberOfItems * fullItemWidth) - (fullItemWidth / 2.0f);
								var LastCenterOfLastItem = fullItemWidth / 2.0f;
								
								if(!isStack)
									StackViewsDidFinish (this, StackNumber, PrevCenterOfLastItem - LastCenterOfLastItem, true);
								else
									StackViewsDidFinish (this, StackNumber, PrevCenterOfLastItem - LastCenterOfLastItem, false);
							}


						}
						foreach (var image in itemsVector)
							image.UserInteractionEnabled = false;


					});

				var newFrame = Frame;
				newFrame.Width = Constants.ItemFrameWidth + Constants.ItemSeparation;
				Frame = newFrame;
				isStack = true;

		}

		float toRadians (float angle)
		{
			return (float) (angle * Math.PI / 180);
		}

		#endregion



		void initComponent()
		{
			if (source != null)
			{
				numberOfItems = source.Pages.Count;
				float stackItemXPosition = 0;
				//_numberofitems = 6;
				for (int i = 0; i < numberOfItems; i++)
				{
					var sitem = new IStackItem(stackItemXPosition);
					Console.WriteLine (sitem.Frame);
					sitem.InitialAngle = angles[i % 3];

					sitem.Chapter = Chapter;
					sitem.Section = Section;
					sitem.Page = i;
					sitem.Source = source.Pages[i];

					sitem.Layer.ZPosition = i + 1;
					sitem.StackItemFullAnimationStarted += HandleStackItemFullAnimationStarted;
					sitem.StackItemFullAnimationCompleted += HandleStackItemFullAnimationCompleted;
					sitem.StackItemThumbAnimationStarted += HandleStackItemThumbAnimationStarted;
					sitem.StackItemThumbAnimationCompleted += HandleStackItemThumbAnimationCompleted;


					Add(sitem);
					itemsVector.Add(sitem);
					StackItemCreated (sitem, sitem.IndexInBook);

					stackItemXPosition += (float)sitem.Frame.Width + Constants.ItemSeparation;
				}
				if (itemsVector.Count > 0)
					selectedItem = itemsVector[0];

				CGRect tmp = Frame;
				tmp.Width = stackItemXPosition;
				Frame = tmp;
			}
		}


		void HandleStackItemThumbAnimationCompleted (object sender, int chapter, int section, int page)
		{
			StackItemThumbAnimationCompleted (sender, chapter, section, page);
		}

		void HandleStackItemThumbAnimationStarted (object sender, int chapter, int section, int page)
		{
			StackItemThumbAnimationStarted (sender, chapter, section, page);
		}

		void HandleStackItemFullAnimationCompleted (object sender, int chapter, int section, int page)
		{
			StackItemFullAnimationCompleted (sender, chapter, section, page);
		}

		void HandleStackItemFullAnimationStarted (object sender, int chapter, int section, int page)
		{
			StackItemFullAnimationStarted (sender, chapter, section, page);
			selectedItem = (IStackItem)sender;
		}
			

		public void LoadDataSource()
		{
			for (int i = 1; i < itemsVector.Count; i++)
				itemsVector[i].LoadThumbSource();
		}

		public void LoadFirst()
		{
			if (itemsVector.Count > 0)
			{
				itemsVector[0].LoadThumbSource();
			}
		}

	}
}

