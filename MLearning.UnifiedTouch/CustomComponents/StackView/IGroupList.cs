using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public delegate void ChapterChangedEventHandler (object sender, int chapter);

	public class IGroupList : UIScrollView, IUIScrollViewDelegate
	{
		#region Properties
		List<IStackList> listVector;

		BookDataSource source;
		public BookDataSource Source 
		{
			get { return source; }
			set 
			{
				source = value;
				loadControls ();
			}
		}

		int startIndex;
		public int StartIndex
		{
			get { return startIndex; }
			set { startIndex = value; }
		}

		public IStackItem SelectedStackItem
		{
			get { return listVector[SelectedChapter].SelectedStack.SelectedStackItem; }
		}

		int selectedChapter;
		public int SelectedChapter
		{
			set { selectedChapter = value; }
			get { return selectedChapter; }
		}

		int selectedSection;
		public int SelectedSection
		{
			set { selectedSection = value; }
			get { return selectedSection; }
		}

		int selectedPage;
		public int SelectedPage
		{
			set { selectedPage = value; }
			get { return selectedPage; }
		}

		int numberOfItems;
		public int NumberOfItems 
		{
			get { return numberOfItems; }
			set { numberOfItems = value; }
		}


		bool moveToRight, moveToLeft;
		int LimitToChangeChapter = 250;

		//used when the view must scroll to show current item position
		int FixedSizeToScroll = 400;

		public event ChapterChangedEventHandler ChapterHasChanged;

		public event Constants.StackItemFullAnimationCompletedEventHandler StackItemFullAnimationCompleted;
		public event Constants.StackItemFullAnimationStartedEventHandler StackItemFullAnimationStarted;
		public event Constants.StackItemThumbAnimationCompletedEventHandler StackItemThumbAnimationCompleted;
		public event Constants.StackItemThumbAnimationStartedEventHandler StackItemThumbAnimationStarted;

		public event StackItemCreated StackItemCreated;

		#endregion

		public IGroupList () : base (new CGRect (0, 254, Constants.DeviceWidth, Constants.ControlHeight))
		{
			listVector = new List<IStackList> ();
			initControls ();
			initProperties ();
		}

		#region Initialization
		void initControls ()
		{
			//groupPanel y groupScroll son reemplazados por el scroll general
			Scrolled += HandleScrolled;
		}

		void initProperties()
		{
			startIndex = 0;
			selectedChapter = 0;
			selectedSection = 0;
			selectedPage = 0;

			moveToLeft = true;
			moveToRight = false;

			ShowsHorizontalScrollIndicator = false;
			ShowsVerticalScrollIndicator = false;
		}
		#endregion


		void HandleScrolled (object sender, EventArgs e)
		{
			if (!Tracking) 
			{
				evaluateChapterBounds ();

				if (moveToLeft && SelectedChapter != 0)
					ChapterHasChanged (this, SelectedChapter - 1);
				else if (moveToRight && SelectedChapter != NumberOfItems - 1)
					ChapterHasChanged (this, SelectedChapter + 1);

			}
		}


		void evaluateChapterBounds()
		{

			if(ContentOffset.X <= (LimitToChangeChapter * -1))
				moveToLeft = true;
			else moveToLeft =false;

			if(ContentOffset.X + Frame.Width >= ContentSize.Width + LimitToChangeChapter)
				moveToRight = true;
			else moveToRight = false;

		}


		void loadControls ()
		{
			if (Source != null) 
			{
				BackgroundColor = UIColor.Clear;
				NumberOfItems = Source.Chapters.Count;

				float stackListXPosition = 0;

				for (int i = 0; i < NumberOfItems; i++) {
					IStackList list = new IStackList (stackListXPosition);
					list.StackItemCreated += HandleStackItemCreated;

					list.Chapter = i;
					list.DataSource = Source.Chapters [i];

					list.StackListWidthChanged += HandleStackListWidthChanged;

					list.StackItemFullAnimationStarted += HandleStackItemFullAnimationStarted;
					list.StackItemFullAnimationCompleted += HandleStackItemFullAnimationCompleted;
					list.StackItemThumbAnimationStarted += HandleStackItemThumbAnimationStarted;
					list.StackItemThumbAnimationCompleted += HandleStackItemThumbAnimationCompleted;

					Add (list);
					listVector.Add (list);
					stackListXPosition += (float)list.Frame.Width;
				}

				LoadList (startIndex);
				selectedChapter = startIndex;


				for (int i = 0; i < NumberOfItems; i++)
					LoadList (i);

				ContentSize = new CGSize (stackListXPosition, Frame.Height);

				//reset indexInBook so another IGroupList starts with its items in 0
				IStackItem.Index = 0;
			}
		}

		void HandleStackItemCreated (object sender, int index)
		{
			StackItemCreated (sender, index);
		}

		void HandleStackItemThumbAnimationCompleted (object sender, int chapter, int section, int page)
		{
			StackItemThumbAnimationCompleted (sender, chapter, section, page);
		}

		void HandleStackItemThumbAnimationStarted (object sender, int chapter, int section, int page)
		{
			selectedChapter = chapter;
			selectedPage = page;
			selectedSection = section;
			StackItemThumbAnimationStarted (sender, chapter, section, page);
		}

		void HandleStackItemFullAnimationCompleted (object sender, int chapter, int section, int page)
		{
			SetToItem(chapter, section, page);
			StackItemFullAnimationCompleted(sender, chapter, section, page);
		}

		void HandleStackItemFullAnimationStarted (object sender, int chapter, int section, int page)
		{
			selectedChapter = chapter;
			selectedPage = page;
			selectedSection = section;
			StackItemFullAnimationStarted (sender, chapter, section, page);
		}


		#region Methods

		public void SetToItem(int chapter, int section, int page)
		{
			selectedChapter = chapter;
			selectedSection = section;
			selectedPage = page;
			//reset stacks
			for (int i = 0; i < listVector.Count; i++)
				if (i == chapter)
					listVector[i].OpenStack(section);

		}

		public void ScrollToShowItem()
		{
			int indexOfPage = listVector [selectedChapter].indexOfPage (selectedSection, selectedPage);
			nfloat location = FixedSizeToScroll + (indexOfPage + 1) * (Constants.ItemFrameWidth + Constants.ItemSeparation);
			if (location > Constants.DeviceWidth)
				SetContentOffset (new CGPoint (location - Constants.DeviceWidth, 0), false);
		}

		public void LoadList(int number)
		{
			if (listVector.Count > 0 &&  !listVector[number].IsLoaded)
				listVector[number].LoadDataSource();
		}

		void HandleStackListWidthChanged (object sender, nfloat delta)
		{
			var tmp = ContentSize;
			tmp.Width -= delta;
			ContentSize = tmp;
		}

		public void animateToChapter (int chapter, bool animated)
		{
			var offsetChange = listVector [chapter].Frame.X;
			if (chapter < SelectedChapter) 
			{
				for (int i = 0; i < NumberOfItems; i++) 
				{
					var newFrame = listVector [i].Frame;
					newFrame.X -= offsetChange;
					UIView.Animate (animated ? 0.5f : 0, () => {
						//listVector [i].Frame = newFrame;
						listVector[i].Transform = CGAffineTransform.Translate (listVector[i].Transform,-offsetChange,0);

					});
				}
			}
			else 
			{
				for (int i = NumberOfItems - 1; i >= 0; i--) 
				{
					var newFrame = listVector [i].Frame;
					newFrame.X -= offsetChange;
					UIView.Animate (animated ? 0.5f : 0, () => {
						//listVector [i].Frame = newFrame;
						listVector[i].Transform = CGAffineTransform.Translate (listVector[i].Transform,-offsetChange,0);
						//listVector[i].Transform = CGAffineTransform.MakeIdentity();
					});

				}
			}
			SelectedChapter = chapter;

			ContentSize = new CGSize (listVector[chapter].Frame.Width, ContentSize.Height);
			SetContentOffset (new CGPoint (listVector [chapter].Frame.X, listVector [chapter].Frame.Y), false);
		}
		#endregion

	}
}