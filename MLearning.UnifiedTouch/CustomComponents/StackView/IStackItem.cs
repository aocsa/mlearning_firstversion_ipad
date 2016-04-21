using System;
using UIKit;
using CoreGraphics;
using System.ComponentModel;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class IStackItem : UIManipulableView
	{
		#region Properties

		static int index = 0;
		public static int Index 
		{
			get { return index; }
			set { index = value; }
		}

		int indexInBook;
		public int IndexInBook 
		{
			get { return indexInBook; }
			set { indexInBook = value; }
		}

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

		int page;
		public int Page
		{
			set { page = value; }
			get { return page; }
		}

		float initialAngle;
		public float InitialAngle
		{
			set { initialAngle = value; }
			get { return initialAngle; }
		}

		PageDataSource source;
		public PageDataSource Source
		{
			set	{ source = value; }
			get { return source; }
		}
			
		UIImageView thumbImage;
		UILabel titleText;
		UILabel descriptionText;
		UIView panel;

		public event Constants.StackItemFullAnimationCompletedEventHandler StackItemFullAnimationCompleted;
		public event Constants.StackItemFullAnimationStartedEventHandler StackItemFullAnimationStarted;
		public event Constants.StackItemThumbAnimationCompletedEventHandler StackItemThumbAnimationCompleted;
		public event Constants.StackItemThumbAnimationStartedEventHandler StackItemThumbAnimationStarted;

		float AnimationShowInfoDuration = 0.5f;
		CGRect showPanelFrame = new CGRect (10, 160, 171, 116);
		CGRect hidePanelFrame = new CGRect (10, 10, 171, 116);

		#endregion

		public IStackItem (float stackItemXPosition) : base ()
		{
			ContentMode = UIViewContentMode.TopLeft;
			setFrame (new CGRect (stackItemXPosition, 54, Constants.ItemFrameWidth, Constants.ItemFrameHeight));

			//imageFeatures
			thumbImage = new UIImageView (new CGRect (5, 5, Constants.ThumbWidth, Constants.ThumbHeight));
			thumbImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			thumbImage.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			thumbImage.ClipsToBounds = true;
			BackgroundColor = UIColor.Red;
			Layer.CornerRadius = 4;
			Add (thumbImage);

			panel = new UIView (showPanelFrame);

			titleText = Constants.makeLabel (new CGRect(30, 0, 111,30), UIColor.White, UITextAlignment.Center, Font.Regular, 12);
			titleText.Text = "Nombre de Item";
			titleText.LineBreakMode = UILineBreakMode.WordWrap;
			titleText.Lines = 2;
			panel.Add (titleText);

			descriptionText = Constants.makeLabel (new CGRect(0, 30, 171, 100), UIColor.White, UITextAlignment.Center, Font.Regular, 10);
			descriptionText.Text = "Descripcion de Item";
			descriptionText.LineBreakMode = UILineBreakMode.WordWrap;
			descriptionText.Lines = 0;
			panel.Add (descriptionText);

			Add (panel);
			IndexInBook = index++;

			initProperties ();
		}


		void initProperties()
		{
			initialAngle = 0;
			source = null;

			StackItemFullAnimationCompletedTriggered += (object sender) => 
			{
				hideInfo();
				StackItemFullAnimationCompleted (this, chapter, section, page);
			};

			StackItemFullAnimationStartedTriggered += (object sender) => 
			{
				hideInfo();
				StackItemFullAnimationStarted (this, chapter, section, page);
			};

			StackItemThumbAnimationCompletedTriggered += (object sender) => 
			{
				showInfo();
				StackItemThumbAnimationCompleted (this, chapter, section, page);
			};

			StackItemThumbAnimationStartedTriggered += (object sender) => 
			{
				hideInfo();
				StackItemThumbAnimationStarted (this, chapter, section, page);
			};

			StackItemPinchPanRotateStarted += (object sender) => 
			{
				hideInfo();
			};
		}

	

		#region Public Methods
		public void showInfo()
		{
			UIView.Animate (AnimationShowInfoDuration, 
				() =>
				{
					panel.Alpha = 1;
					panel.Frame = showPanelFrame;
				}
			);

		}

		public void hideInfo()
		{
			panel.Alpha = 0;
			panel.Frame = hidePanelFrame;
		}


		public void LoadThumbSource()
		{
			if (source.ImageContent != null)
				thumbImage.Image = source.ImageContent;
			titleText.Text = source.Name;
			descriptionText.Text = source.Description;
			descriptionText.SizeToFit ();
			BackgroundColor = source.BorderColor;
			source.PropertyChanged += DataSourcePropertyChanged;
		}

	
		void DataSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ImageSource")
			{
				if (source.ImageContent != null)
					thumbImage.Image = source.ImageContent;
			}

			if (e.PropertyName == "BorderColor")
			{
				UIView.Animate (
					duration: 0.65f,
					animation: () => {
						BackgroundColor = source.BorderColor;
					}
				);
			}
		}
		#endregion

	}
}

