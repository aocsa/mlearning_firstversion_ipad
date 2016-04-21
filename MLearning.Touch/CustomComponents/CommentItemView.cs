using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace MLearning.Touch
{
	public class CommentItemView : UIView
	{
		UIImageView _image;
		public UIImageView Image {
			get {
				return _image;
			}
			set {
				_image = value;
			}
		}

		UILabel name;
		public UILabel Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		UILabel date;
		public UILabel Date {
			get {
				return date;
			}
			set {
				date = value;
			}
		}

		UILabel comment;
		public UILabel Comment {
			get {
				return comment;
			}
			set {
				comment = value;
			}
		}

		public CommentItemView (float height, string nameText, string dateText, string commentText) : base()
		{
			Frame = new RectangleF (0, height, 670, 65);
			Image = new UIImageView ( new RectangleF (32, 10, 50, 50) );
			Image.Layer.CornerRadius = 25f;
			Image.Layer.MasksToBounds = true;

			Add (Image);

			Name = Constants.makeLabel (new RectangleF (106, 10, 150, 17), UIColor.Black, UITextAlignment.Left, Font.Regular, 14);
			Name.Text = nameText;
			Name.AdjustsFontSizeToFitWidth = true;
			Add (Name);

			Date = Constants.makeLabel (new RectangleF (226, 10, 150, 17), UIColor.Gray, UITextAlignment.Left, Font.Regular, 10);
			Date.Text = dateText;
			Date.AdjustsFontSizeToFitWidth = true;
			Add (Date);

			Comment = Constants.makeLabel (new RectangleF (106, 30, 520 ,22), UIColor.Gray, UITextAlignment.Left, Font.Regular, 14);
			Comment.Text = commentText;
			Comment.Lines = 0;
			Comment.LineBreakMode = UILineBreakMode.WordWrap;
			Add (Comment);

			/*resize height according to text*/
			var nsText = new NSMutableAttributedString(Comment.Text);
			nsText.AddAttribute(UIStringAttributeKey.Font, Comment.Font, new NSRange(0, nsText.Length));

			var ctxt = new NSStringDrawingContext ();
			SizeF expectedSize = nsText.GetBoundingRect (new SizeF(520, float.MaxValue), 
				NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading,
				ctxt).Size;

			int expectedHeight = (int)Math.Round (expectedSize.Height);
			Comment.Frame = new RectangleF (106, 30, 520, expectedHeight);
			Frame = new RectangleF (0, height, 670, 50 + expectedHeight);
		}

		public float getHeight()
		{
			return Frame.Height;
		}
	}
}

