
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using MLearning.Core.ViewModels;
using MLearningDB;

namespace MLearning.Touch
{
	public partial class PersonCell : MvxTableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("PersonCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PersonCell");

		public PersonCell (IntPtr handle) : base (handle)
		{
			this.DelayBind (() => {
				var set = this.CreateBindingSet <PersonCell, MLearning.Core.ViewModels.MainViewModel.user_by_circle_wrapper>();
				set.Bind (PeopleName).To (person => person.user.name);
				set.Apply();
				PeopleImage.Image = (UIImage.FromFile ("iOS Resources/muro/greencircle.png"));

			});

			var bgView = new UIView ();
			bgView.BackgroundColor = Constants.SearchBarColor;
			BackgroundView = bgView;

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public static PersonCell Create ()
		{
			return (PersonCell)Nib.Instantiate (null, null) [0];
		}
	}
}

