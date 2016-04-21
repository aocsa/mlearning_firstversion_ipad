using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

namespace MLearning.UnifiedTouch.CustomComponents
{

	public delegate void StackListWidthChangedEventHandler(object sender, nfloat delta);


	public class IStackList : UIView
	{

		public event StackListWidthChangedEventHandler StackListWidthChanged;

		int chapter;
		public int Chapter 
		{
			get { return chapter; }
			set { chapter = value; }
		}

		public ChapterDataSource dataSource;
		public ChapterDataSource DataSource 
		{
			get {
				return dataSource;
			}
			set {
				dataSource = value;
				loadControls ();
			}
		}

		bool isLoaded;
		public bool IsLoaded
		{
			get { return isLoaded; }
		}
			
		IStackView selectedStack;
		public IStackView SelectedStack
		{
			get { return selectedStack; }
		}

		int numberOfStacks;
		public int NumberOfStacks 
		{
			get { return numberOfStacks; }
			set { numberOfStacks = value; }
		}

		public event Constants.StackItemFullAnimationCompletedEventHandler StackItemFullAnimationCompleted;
		public event Constants.StackItemFullAnimationStartedEventHandler StackItemFullAnimationStarted;
		public event Constants.StackItemThumbAnimationCompletedEventHandler StackItemThumbAnimationCompleted;
		public event Constants.StackItemThumbAnimationStartedEventHandler StackItemThumbAnimationStarted;
		public event StackItemCreated StackItemCreated;

		List <IStackView> stacksVector;
		ChapterHeaderControl headerControl;

		public IStackList (float stackListXPosition) : base()
		{
			stacksVector = new List<IStackView> ();
			Frame = new CGRect (stackListXPosition, 0, Constants.StartWidth, Constants.ControlHeight);
			initControls ();

			isLoaded = false;
		}

		#region Initialization
		void initControls()
		{
			headerControl = new ChapterHeaderControl();
			Add (headerControl);
		}
		#endregion

		void loadControls()
		{
			if (dataSource != null)
			{
				NumberOfStacks = dataSource.Sections.Count;
				float stackViewXPosition = 400;
				for (int i = 0; i < NumberOfStacks; i++)
				{
					var stack = new IStackView(stackViewXPosition);
					stack.StackItemCreated += HandleStackItemCreated;

					stack.StackNumber = i;
					stack.Chapter = Chapter;
					stack.Section = i;

					stack.Source = dataSource.Sections[i];


					stack.StackViewsUpdate += HandleStackViewsUpdate;
					stack.StackViewsDidFinish += HandleStackViewsDidFinish;

					stack.StackItemFullAnimationStarted += HandleStackItemFullAnimationStarted;
					stack.StackItemFullAnimationCompleted += HandleStackItemFullAnimationCompleted;
					stack.StackItemThumbAnimationStarted += HandleStackItemThumbAnimationStarted;
					stack.StackItemThumbAnimationCompleted += HandleStackItemThumbAnimationCompleted;

					Add(stack);
					stacksVector.Add(stack);

					stackViewXPosition += (float)stack.Frame.Width;
				}

				//header
				headerControl.Author = dataSource.Author;
				headerControl.Title = dataSource.Title;
				headerControl.DescriptionText = dataSource.Description;
				headerControl.ChapterColor = dataSource.ChapterColor;

				CGRect tmp = Frame;
				tmp.Width = stackViewXPosition;
				Frame = tmp;
			}
		}

		void HandleStackItemCreated (object sender, int index)
		{
			StackItemCreated (sender, index);
		}


		public void OpenStack(int index)
		{
			if (stacksVector[index].IsStack)
					stacksVector[index].animateToShowItems(0);

			selectedStack = stacksVector[index];
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
			selectedStack = stacksVector[section];
		}

		void HandleStackViewsDidFinish (object sender, int index, nfloat delta, bool isChange)
		{
			for (int i = index + 1; i < NumberOfStacks; i++) 
			{

				var stackView = stacksVector [i];
				if (isChange) 
				{
					UIView.Animate (duration: 0.5f,
						animation: () => {
							var tmp = stackView.FirstCenter;

							//La diferencia en centros 
							tmp.X -= delta;
							stackView.Center = tmp;
							stackView.FirstCenter = tmp;

						},
						completion: () => 
						{

						}
					);
				}
				else 
				{
					UIView.Animate (duration: 0.5f,
						animation: () => {
							stackView.Center = stackView.FirstCenter;
						},
						completion: () => {

						}
					);
				}
			}
			if (isChange) 
			{
				var aux = Frame;
				aux.Width -= delta;
				Frame = aux;
				StackListWidthChanged (this, delta);
			}
		}

		void HandleStackViewsUpdate (object sender, int index, nfloat delta)
		{
			for (int i = index + 1; i < NumberOfStacks; i++) 
			{
				var stackView = stacksVector [i];

				var tmp = stackView.Center;
				tmp.X -= delta;
				stackView.Center = tmp;
			}
		}


		public void LoadDataSource()
		{
			isLoaded = true;
			for (int i = 0; i < stacksVector.Count; i++)
				stacksVector [i].LoadFirst ();
			for (int i = 0; i < stacksVector.Count; i++)
				stacksVector[i].LoadDataSource();
		}

		public int indexOfPage(int section, int page)
		{
			//index should consider if views are stacked
			int index = 0;
			for (int i = 0; i < stacksVector.Count; i++) 
			{
				if (stacksVector [i].StackNumber < section)
					index += stacksVector [i].IsStack ? 1 : stacksVector [i].NumberOfItems;
				else
					break;
			}
			index += page;
			return index;
		}
	}
}

