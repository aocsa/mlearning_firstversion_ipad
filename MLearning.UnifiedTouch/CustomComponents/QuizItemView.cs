using System;
using UIKit;
using System.Drawing;

namespace MLearning.UnifiedTouch.CustomComponents
{

	public class QuizItemView : UIView
	{
		UIImageView quizImage;
		public UIImageView QuizImage {
			get {return quizImage;}
			set {quizImage = value;}
		}

		UILabel quizLabel;
		public UILabel QuizLabel {
			get {return quizLabel;}
			set {quizLabel = value;}
		}

		int index;
		public int Index {
			get {return index;}
			set {index = value;}
		}

		public QuizItemView (int pos, string imagePath, string text) : base ()
		{
			Index = pos;
			Frame = new RectangleF (0, pos * 52, 316, 52);

			QuizImage = Constants.makeImageView (new RectangleF (22, 16, 20, 20), imagePath);
			Add (QuizImage);

			QuizLabel = Constants.makeLabel (new RectangleF (54, 16, 240, 20), UIColor.White, UITextAlignment.Left, Font.Regular, 14);
			//QuizLabel.AdjustsFontSizeToFitWidth = true;
			QuizLabel.Text = text;
			Add (QuizLabel);

			BackgroundColor = UIColor.Clear;
			UserInteractionEnabled = true;
		}
	}
}

