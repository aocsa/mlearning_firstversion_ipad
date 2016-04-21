using System;
using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class ChapterHeaderControl : UIView
	{
		UILabel titleLabel, authorLabel, descriptionLabel, tagsLabel;

		string title;
		public string Title 
		{
			get { return title; }
			set { title = value.ToUpper(); titleLabel.Text = value.ToUpper ();}
		}

		string author;
		public string Author 
		{
			get { return author; }
			set { author = value.ToUpper(); authorLabel.Text = value.ToUpper (); }
		}

		string description;
		public string DescriptionText 
		{
			get { return description; }
			set { description = value; descriptionLabel.Text = value;}
		}

		UIColor chapterColor;
		public UIColor ChapterColor 
		{
			get { return chapterColor; }
			set { chapterColor = value; setColor (); }
		}

		public ChapterHeaderControl () : base (new CGRect(65, 0, 270, 336))
		{
			titleLabel = Constants.makeLabel (new CGRect (0, 0, 234, 68), UIColor.White, UITextAlignment.Left, Font.Light, 21);
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Lines = 0;
			titleLabel.Text = "Experiencia de Usuario";
			Add (titleLabel);

			tagsLabel = Constants.makeLabel (new CGRect (0, 75, 234, 38), UIColor.White, UITextAlignment.Left, Font.Bold, 23);
			tagsLabel.LineBreakMode = UILineBreakMode.WordWrap;
			tagsLabel.Text = "INMERSIVA";
			Add (tagsLabel);

			authorLabel = Constants.makeLabel (new CGRect (0, 120, 234, 44), UIColor.White, UITextAlignment.Left, Font.Regular, 21);
			authorLabel.LineBreakMode = UILineBreakMode.WordWrap;
			authorLabel.Text = "Sensacional e intuitivo";
			Add (authorLabel);

			descriptionLabel = Constants.makeLabel (new CGRect (0, 171, 234, 165), UIColor.White, UITextAlignment.Left, Font.Regular, 16);
			descriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
			descriptionLabel.Lines = 0;
			descriptionLabel.Text = "\"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat\";";
			Add (descriptionLabel);

			var verticalLine = new UIView (new CGRect (265, 0, 1, 336));
			verticalLine.BackgroundColor = UIColor.Gray;
			Add (verticalLine);
		}

		void setColor()
		{
			tagsLabel.TextColor = ChapterColor;
		}
	}
}

