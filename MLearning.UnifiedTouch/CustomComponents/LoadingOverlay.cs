using UIKit;
using CoreGraphics;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class LoadingOverlay : UIView 
	{
		UIActivityIndicatorView activitySpinner;

		public LoadingOverlay (CGRect frame, float alpha) : base (frame)
		{
			BackgroundColor = UIColor.Clear;
			Alpha = alpha;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
				
			float centerX = (float) Frame.Width / 2;
			float centerY = (float) Frame.Height / 2;

			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new CGRect (
				centerX - (float) (activitySpinner.Frame.Width / 2) ,
				centerY - (float) activitySpinner.Frame.Height - 20 ,
				activitySpinner.Frame.Width ,
				activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();

		}

		public LoadingOverlay (CGRect frame) : base (frame)
		{
			BackgroundColor = UIColor.Clear;

			float centerX = (float) Frame.Width / 2;
			float centerY = (float) Frame.Height / 2;

			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
			activitySpinner.Frame = new CGRect (
				centerX - (float) (activitySpinner.Frame.Width / 2) ,
				centerY - (float) activitySpinner.Frame.Height - 20 ,
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