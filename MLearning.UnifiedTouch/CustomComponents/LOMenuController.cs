using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public delegate void closeViewController (object sender);

	public class LOMenuController : UIView
	{
		List<UIButton> buttons;
		public List<UIButton> Buttons 
		{
			get { return buttons;}
			set { buttons = value;}
		}

		bool isOpen;
		public bool IsOpen 
		{
			get { return isOpen;}
			set { isOpen = value;}
		}

		UIColor tempBackgroundColor;
		public UIColor TempBackgroundColor 
		{
			get { return tempBackgroundColor;}
			set { tempBackgroundColor = value;}
		}

		public event closeViewController CloseViewController;

		public LOMenuController () : base(new CGRect (52, 112, 640, 50))
		{
			Layer.CornerRadius = 16;
			IsOpen = true;
			//Buttons
			Buttons = new List<UIButton> ();
			var circularButton = new UIButton (UIButtonType.Custom) 
			{
				Frame = new CGRect (20, 11, 28, 28),
				AutoresizingMask = UIViewAutoresizing.None
			};
			circularButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/menu/CIRCULO.png"), UIControlState.Normal);
			Add (circularButton);

			var homeButton = new UIButton (UIButtonType.Custom) 
			{
				Frame = new CGRect (68, 11, 28, 28),
				AutoresizingMask = UIViewAutoresizing.None
			};
			homeButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/menu/HOME.png"), UIControlState.Normal);
			Add (homeButton);
			Buttons.Add (homeButton);

			float initPos = 170;
			var button1 = getMenuButton ("Perfil", initPos);
			Add (button1);
			Buttons.Add (button1);

			initPos += 84;
			var button2 = getMenuButton ("Showcase", initPos);
			Add (button2);
			Buttons.Add (button2);

			initPos += 95;
			var button3 = getMenuButton ("Artículo", initPos);
			Add (button3);
			Buttons.Add (button3);

			initPos += 80;
			var button4 = getMenuButton ("Archivos", initPos);
			Add (button4);
			Buttons.Add (button4);


			var shareButton = new UIButton (UIButtonType.Custom) 
			{
				Frame = new CGRect (546,11,28,28),
				AutoresizingMask = UIViewAutoresizing.None
			};
			shareButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/menu/SHARE.png"), UIControlState.Normal);
			Add (shareButton);
			Buttons.Add (shareButton);

			var helpButton = new UIButton (UIButtonType.Custom) 
			{
				Frame = new CGRect (592,11,28,28),
				AutoresizingMask = UIViewAutoresizing.None
			};
			helpButton.SetBackgroundImage (UIImage.FromFile ("iOS Resources/menu/HELP.png"), UIControlState.Normal);
			Add (helpButton);
			Buttons.Add (helpButton);

			//gestures
			circularButton.TouchUpInside += circularButtonTapped;
			homeButton.TouchUpInside += homeButtonTapped;
		}


		public void setColor (UIColor c)
		{
			BackgroundColor = c.ColorWithAlpha (0.7f);
			TempBackgroundColor = c.ColorWithAlpha (0.7f);
		}

		UIButton getMenuButton (string text, float pos)
		{
			var button = new UIButton (UIButtonType.RoundedRect) 
			{
				Frame = new CGRect(pos, 13, 80, 24),
				AutoresizingMask = UIViewAutoresizing.None
			};
			button.SetTitle (text, UIControlState.Normal);
			button.SetTitleColor (UIColor.White, UIControlState.Normal);
			button.TitleLabel.Font = UIFont.FromName (Constants.FontName[Font.Regular], 14);
			return button;
		}

		public void circularButtonTapped (object sender, EventArgs e)
		{
			if (isOpen)
				animateToClose();
			else animateToOpen();
			isOpen = !isOpen;
		}

		public void homeButtonTapped (object sender, EventArgs e)
		{
			CloseViewController (this);
		}

		void animateToClose ()
		{
			UIView.Animate (
				duration:0.4f,
				delay:0.0f,
				options:UIViewAnimationOptions.CurveEaseIn,
				animation:()=>
				{
					foreach (var b in Buttons)
						b.Alpha = 0;
				},
				completion:()=>
				{
					UIView.Animate 
					(
						duration: 0.4f,
						animation: ()=>
						{
							CGRect resized = Frame;
							resized.Width = 68;
							Frame = resized;
						},
						completion:()=>
						{
							UIView.Animate (
								duration: 0.4f,
								animation: () => 
								{ 
									BackgroundColor = UIColor.Clear; 
								}
							);
						}
					);
				});
		}

		void animateToOpen ()
		{
			UIView.Animate (
				duration:0.4f,
				delay:0.0f,
				options:UIViewAnimationOptions.CurveEaseIn,
				animation:()=>
				{
					BackgroundColor = TempBackgroundColor;
				},
				completion:()=>
				{
					UIView.Animate 
					(
						duration: 0.4f,
						animation: ()=>
						{
							CGRect resized = Frame;
							resized.Width = 640;
							Frame = resized;
						},
						completion:()=>
						{
							UIView.Animate (
								duration: 0.4f,
								animation: () => 
								{ 
									foreach (var b in Buttons)
									{
										b.Alpha = 1;
									}
								}
							);
						}
					);
				});
		}

		public void animateToColor(UIColor c)
		{
			UIView.Animate (
				duration: 0.6f,
				animation : () => 
				{
					BackgroundColor = c.ColorWithAlpha (0.7f);
					TempBackgroundColor = c.ColorWithAlpha (0.7f);
				},
				completion: () => 
				{
				}
			);

		}

	}
}

