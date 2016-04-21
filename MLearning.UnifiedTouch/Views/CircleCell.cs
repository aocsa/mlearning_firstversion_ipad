
using System;
using System.Drawing;

using Foundation;
using UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using MLearningDB;

namespace MLearning.UnifiedTouch
{
	public partial class CircleCell : MvxTableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("CircleCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("CircleCell");

		public CircleCell (IntPtr handle) : base (handle)
		{
			this.DelayBind (() => {
				var set = this.CreateBindingSet <CircleCell, circle_by_user>();
				set.Bind (CircleName).To (circle => circle.name);
				set.Apply();
				CircleImage.Image = (UIImage.FromFile ("iOS Resources/muro/greenpop.png"));

			});

			var bgView = new UIView ();
			bgView.BackgroundColor = Constants.SearchBarColor;
			BackgroundView = bgView;

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			var bgColorView = new UIView();
			bgColorView.BackgroundColor = Constants.BlueColor;
			SelectedBackgroundView = bgColorView;
		}

		public static CircleCell Create ()
		{
			return (CircleCell)Nib.Instantiate (null, null) [0];
		}
	}
}

