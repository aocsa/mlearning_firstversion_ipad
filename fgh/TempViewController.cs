using System;
using UIKit;
using CoreGraphics;

namespace fgh
{
	public class TempViewController : UIViewController
	{
		public TempViewController ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var tmp = new UIManipulableView ();
			tmp.setFrame (new CGRect (100, 100, 500, 500));
			tmp.BackgroundColor = UIColor.Green;
			Add (tmp);

			var otro = new UIManipulableView ();
			otro.setFrame (new CGRect (100, 100, 200, 200));
			otro.BackgroundColor = UIColor.Red;
			tmp.Add (otro);
		}
	}
}

