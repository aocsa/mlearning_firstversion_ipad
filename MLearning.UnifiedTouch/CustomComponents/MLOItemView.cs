using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch.CustomComponents
{

	public class MLOItemView : UIView
	{
		bool isSelected;
		public bool IsSelected {
			get {return isSelected;}
			set {
				isSelected = value;
				this.BackgroundColor = isSelected ? Constants.BlueColor : UIColor.Clear;
			}
		}

		bool likesMe;
		public bool LikesMe {
			get {return likesMe;}
			set {
				likesMe = value;
				likesLabel.Text = likesMe ? "Te gusta" : "Me gusta";
			}
		}

		int index;
		public int Index {
			get {return index;}
			set {index = value;}
		}

		UIImageView mloImage;
		public UIImageView MloImage {
			get {return mloImage;}
			set {mloImage = value;}
		}

		IMLODelegate _delegate;
		public IMLODelegate Delegate {
			get {return _delegate;}
			set {_delegate = value;}
		}

		string title;
		public string Title {
			get {return title;}
			set {title = value;}
		}

		string author;
		public string Author {
			get {return author;}
			set {author = value;}
		}

		UIView mloContainer;
		public UIView MloContainer {
			get {return mloContainer;}
			set {mloContainer = value;}
		}

		UILabel likesLabel;
		public UILabel LikesLabel {
			get {return likesLabel;}
			set {likesLabel = value;}
		}

		UILabel descriptionLabel;
		public UILabel DescriptionLabel {
			get {return descriptionLabel;}
			set {descriptionLabel = value;}
		}

		UIView descriptionBackground;
		public UIView DescriptionBackground {
			get {return descriptionBackground;}
			set {descriptionBackground = value;}
		}

		public MLOItemView (int i, float posX, string title, string name, string lastname, bool lMe) : base ()
		{
			Index = i;
			Title = title;
			Author = name + " " + lastname;

			Frame = new CGRect (posX, 0, 122, 142);
			Layer.CornerRadius = 2;

			MloContainer = new UIView (new CGRect(3, 3, 116, 136));
			MloContainer.Layer.CornerRadius = Constants.MloCornerRadius;

			MloImage = new UIImageView (new CGRect (0, 0, 116, 136));
			MloImage.Layer.MasksToBounds = true;
			MloImage.Layer.CornerRadius = Constants.MloCornerRadius;
			MloContainer.Add (MloImage);

			DescriptionBackground = new UIView (new CGRect(0, 78, 116, 58));
			DescriptionBackground.BackgroundColor = UIColor.FromRGBA (0,0,0,80);
			DescriptionBackground.Layer.CornerRadius = Constants.MloCornerRadius;

			DescriptionLabel = Constants.makeLabel (new CGRect (5, 5, 116, 33), UIColor.White, UITextAlignment.Left, Font.Bold, 12);
			DescriptionLabel.Lines = 0;
			DescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			DescriptionLabel.AdjustsFontSizeToFitWidth = true;
			DescriptionLabel.Text = Title + " by " + Author;

			DescriptionBackground.Add (DescriptionLabel);

			LikesLabel = Constants.makeLabel (new CGRect (56, 38, 52, 16), UIColor.White, UITextAlignment.Center, Font.Regular, 9);
			LikesLabel.Layer.MasksToBounds = true;
			LikesLabel.Layer.CornerRadius = Constants.MloCornerRadius;
			LikesLabel.BackgroundColor = Constants.BlueColor;
			DescriptionBackground.Add (LikesLabel);

			MloContainer.Add (DescriptionBackground);
			Add (MloContainer);

			IsSelected = false;
			LikesMe = lMe;

			loadUserInteraction ();
		}

		void loadUserInteraction()
		{
			UserInteractionEnabled = true;
			LikesLabel.UserInteractionEnabled = true;

			var selecTap = new UITapGestureRecognizer (MLOSelected);
			MloContainer.AddGestureRecognizer (selecTap);

			var likeTap = new UITapGestureRecognizer (MLOLiked);
			LikesLabel.AddGestureRecognizer (likeTap);
		}

		void MLOSelected (UITapGestureRecognizer t)
		{
			IsSelected = true;
			Delegate.MLOItemSelected (Index);
		}

		void MLOLiked(UITapGestureRecognizer t)
		{
			LikesMe = !LikesMe;
			Delegate.MLOItemLiked (Index);
		}
	}
}