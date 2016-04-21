using MonoTouch.UIKit;
using System.Drawing;


namespace MLearning.Touch.CustomComponents
{
	public class LoadingOverlay : UIView 

	{
		UIActivityIndicatorView activitySpinner;

		public LoadingOverlay (RectangleF frame, float alpha) : base (frame)
		{
			BackgroundColor = UIColor.Black;
			Alpha = alpha;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
				
			float centerX = Frame.Width / 2;
			float centerY = Frame.Height / 2;

			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new RectangleF (
				centerX - (activitySpinner.Frame.Width / 2) ,
				centerY - activitySpinner.Frame.Height - 20 ,
				activitySpinner.Frame.Width ,
				activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();

		}

		public LoadingOverlay (RectangleF frame) : base (frame)
		{
			BackgroundColor = UIColor.Clear;

			float centerX = Frame.Width / 2;
			float centerY = Frame.Height / 2;

			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
			activitySpinner.Frame = new RectangleF (
				centerX - (activitySpinner.Frame.Width / 2) ,
				centerY - activitySpinner.Frame.Height - 20 ,
				activitySpinner.Frame.Width ,
				activitySpinner.Frame.Height);
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();
		}


		public void Hide ()
		{
			UIView.Animate (
				0.5,
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); Alpha = 1; }
			);
		}
	};
}