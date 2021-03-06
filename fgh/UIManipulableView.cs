﻿using System;
using UIKit;
using CoreGraphics;
using Foundation;
using CoreAnimation;

namespace fgh
{
	public class UIManipulableView : UIView
	{

		/***+*
		WAY TO USE
		
			var container = new UIView (new CGRect (0,0,300,400));
			Add (container);
			var temp = new UIManipulableView ();
			temp.setFrame( new CGRect (10, 20, 300, 300) );
			container.Add (temp);

			var t2 = new UIImageView (new CGRect (0, 0, 300, 300));
			t2.Image = UIImage.FromFile("fondo.png");
			t2.ContentMode = UIViewContentMode.ScaleAspectFill;
			t2.BackgroundColor = UIColor.Black;
			t2.Layer.BorderColor = UIColor.Black.CGColor;
			t2.Layer.BorderWidth = 1;
			t2.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			t2.ClipsToBounds = true;
			temp.Add (t2);
			
		******/
		#region Properties
		UITapGestureRecognizer tapRecognizer;
		UIPanGestureRecognizer panRecognizer;
		CGAffineTransform scaleTransform;
		CGAffineTransform rotateTransform;
		CGAffineTransform panTransform;
		CGRect initialFrame;
		int initialIndex;
		bool fullScreen;
		bool anchorPointUpdated;


		UIView initialSuperview;
		bool beingDragged, gesturesEnded, scaleActive, keepShadow;

		bool allowSingleTapSwitch;
		public bool AllowSingleTapSwitch {
			get {
				return allowSingleTapSwitch;
			}
			set {
				allowSingleTapSwitch = value;
			}
		}

		#endregion

		#region Constants
		float kPSAnimationDuration = 0.75f;
		float kPSShadowFadeDuration = 0.45f;
		float kPSAnimationMoveToOriginalPositionDuration = 0.75f;
		float kPSFullscreenAnimationBounce = 20;
		float kPSEmbeddedAnimationBounceMultiplier = 0.05f;
		#endregion

		public UIManipulableView () : base ()
		{
			initView ();

		}

		public UIManipulableView (CGRect frame) : base (frame)
		{
			initView ();
		}

		void initView ()
		{
			UserInteractionEnabled = true;
			MultipleTouchEnabled = true;

			scaleTransform = CGAffineTransform.MakeIdentity ();
			rotateTransform = CGAffineTransform.MakeIdentity ();
			panTransform = CGAffineTransform.MakeIdentity ();
			initialIndex = 0;
			allowSingleTapSwitch = true;
			keepShadow = false;

			UIPinchGestureRecognizer pinchRecognizer = new UIPinchGestureRecognizer (handlePinchPanRotate);
			pinchRecognizer.CancelsTouchesInView = false;
			pinchRecognizer.DelaysTouchesBegan = false;
			pinchRecognizer.DelaysTouchesEnded = false;
			AddGestureRecognizer (pinchRecognizer);

			UIRotationGestureRecognizer rotationRecognizer = new UIRotationGestureRecognizer (handlePinchPanRotate);
			rotationRecognizer.CancelsTouchesInView = false;
			rotationRecognizer.DelaysTouchesBegan = false;
			rotationRecognizer.DelaysTouchesEnded = false;
			AddGestureRecognizer (rotationRecognizer);

			panRecognizer = new UIPanGestureRecognizer (handlePinchPanRotate);
			panRecognizer.CancelsTouchesInView = false;
			panRecognizer.DelaysTouchesBegan = false;
			panRecognizer.DelaysTouchesEnded = false;
			panRecognizer.MinimumNumberOfTouches = 2;
			panRecognizer.MaximumNumberOfTouches = 2;
			AddGestureRecognizer (panRecognizer);

			tapRecognizer = new UITapGestureRecognizer (handleTap);
			tapRecognizer.CancelsTouchesInView = false;
			tapRecognizer.DelaysTouchesBegan = false;
			tapRecognizer.DelaysTouchesEnded = false;
			AddGestureRecognizer (tapRecognizer);


			/** DELEGATE METHODS **/
			pinchRecognizer.WeakDelegate = this;
			rotationRecognizer.WeakDelegate = this;
			panRecognizer.WeakDelegate = this;
			tapRecognizer.WeakDelegate = this;


		}


		[Export("gestureRecognizer:shouldRecognizeSimultaneouslyWithGestureRecognizer:")]
		public virtual bool ShouldRecognizeSimultaneously (UIGestureRecognizer g1, UIGestureRecognizer g2)
		{
			if (g1.View != this)
				return false;
			if (g1.View != g2.View)
				return false;
			return true;
		}

		[Export("gestureRecognizer:shouldReceiveTouch:")]
		public virtual bool ShouldReceiveTouch (UIGestureRecognizer recognizer, UITouch touch)
		{
			if ((recognizer.GetType () == typeof(UITapGestureRecognizer)) && (touch.View.GetType () == typeof(UIButton)))
				return false;
			Alpha = 1;
			return true;
		}


		public void setFrame (CGRect frame)
		{
			Frame = frame;
			var root = getRootView ();
			if (root == null)
				initialFrame = Frame;
			if (!(Superview == root))
				initialFrame = Frame;
		}


		UIView getRootView ()
		{
			return UIApplication.SharedApplication.KeyWindow;
		}

		CGRect getWindowsBounds ()
		{
			return UIScreen.MainScreen.Bounds;
		}

		CGRect superviewCorrectedInitialFrame ()
		{
			var rootView = getRootView ();
			var superviewCorrectedInitialFrame = rootView.ConvertRectFromView (initialFrame, initialSuperview);
			return superviewCorrectedInitialFrame;
		}

		bool detachViewToWindow (bool enable)
		{
			bool viewChanged = false;
			var rootView = getRootView ();
			if (enable && initialSuperview == null) 
			{
				initialIndex = Array.IndexOf (Superview.Subviews, this);
				initialSuperview = Superview;
				var newFrame = Superview.ConvertRectToView (initialFrame, rootView);
				rootView.Add (this);
				setFrame (newFrame);
				viewChanged = true;
			}
			else if(!enable) 
			{
				if (initialSuperview != null) 
				{
					initialSuperview.InsertSubview (this, initialIndex);
					viewChanged = true;
				}
				setFrame (initialFrame);
				initialSuperview = null;
			}
			return viewChanged;
		}


		void updateShadowPath ()
		{
			Layer.ShadowPath = UIBezierPath.FromRect (Bounds).CGPath;
		}

		void applyShadowAnimated (bool animated)
		{
			if (keepShadow)
				return;
			if (animated) 
			{
				CABasicAnimation anim = CABasicAnimation.FromKeyPath ("shadowOpacity");
				anim.From = NSNumber.FromFloat (0.0f);
				anim.To = NSNumber.FromFloat (1.0f);
				anim.Duration = kPSShadowFadeDuration;
				Layer.AddAnimation (anim, "shadowOpacity");
			}
			else
				Layer.RemoveAnimation ("shadowOpacity");

			updateShadowPath();
			Layer.ShadowOpacity = 1.0f;

		}

		void removeShadowAnimated (bool animated)
		{
			if (keepShadow)
				return;
			if (animated) 
			{
				CABasicAnimation anim = CABasicAnimation.FromKeyPath ("shadowOpacity");
				anim.From = NSNumber.FromFloat (1.0f);
				anim.To = NSNumber.FromFloat (0.0f);
				anim.Duration = kPSShadowFadeDuration;
				Layer.AddAnimation (anim, "shadowOpacity");
			}
			else
				Layer.RemoveAnimation ("shadowOpacity");

			Layer.ShadowOpacity = 0.0f;

		}


		void setBeingDragged (bool newBeingDragged)
		{
			if (newBeingDragged != beingDragged) 
			{
				beingDragged = newBeingDragged;

				if (beingDragged)
					applyShadowAnimated (true);
				else
					removeShadowAnimated (false);

			}
		}

		void moveViewToOriginalPositionAnimated (bool animated, bool bounces)
		{
			float bounceY = (float)panTransform.y0 * kPSEmbeddedAnimationBounceMultiplier * -1;
			float bounceX = (float)panTransform.x0 * kPSEmbeddedAnimationBounceMultiplier * -1;

			fullScreen = false;

		
			UIView.Animate (duration: animated ? kPSAnimationMoveToOriginalPositionDuration : 0,
				delay: 0,
				options: UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.AllowUserInteraction,
				animation: () => 
				{
					rotateTransform = CGAffineTransform.MakeIdentity();
					scaleTransform = CGAffineTransform.MakeIdentity();
					panTransform = CGAffineTransform.MakeIdentity();
					Transform = CGAffineTransform.MakeIdentity();

					var correctedInitialFrame = superviewCorrectedInitialFrame();
					if (bounces)
					{
						if(Math.Abs (bounceX) > 0 || Math.Abs (bounceY) > 0 )
						{
							float widthDifference = (float)(Frame.Width - correctedInitialFrame.Width) * 0.05f;
							float heightDifference = (float)(Frame.Height - correctedInitialFrame.Height) * 0.05f;

							var targetFrame = new CGRect (correctedInitialFrame.X + bounceX + widthDifference / 2.0f,
								correctedInitialFrame.Y + bounceY + heightDifference / 2.0f,
								correctedInitialFrame.Width + (widthDifference * -1),
								correctedInitialFrame.Height + (heightDifference * -1));
							setFrame (targetFrame);
						}
						else
						{
							var targetFrame = new CGRect (correctedInitialFrame.X + 3,
								correctedInitialFrame.Y + 3,
								correctedInitialFrame.Width - 6,
								correctedInitialFrame.Height - 6);
							setFrame (targetFrame);
						}
					}
					else
					{
						setFrame (correctedInitialFrame);
					}
				},
				completion: () => 
				{
					if(bounces)
					{
						UIView.Animate ( duration: kPSAnimationMoveToOriginalPositionDuration / 2,
							delay: 0,
							options: UIViewAnimationOptions.AllowUserInteraction,
							animation: () =>
							{
								var correctedInitialFrame = superviewCorrectedInitialFrame ();
								setFrame (correctedInitialFrame);
							},
							completion: () =>
							{
								if(!beingDragged)
									detachViewToWindow (false);
							}
						);
					}


				}
			);

		}


		void moveToFullScreenAnimated (bool animated, bool bounces)
		{
			bool viewChanged = detachViewToWindow (true);
			fullScreen = true;



			UIView.Animate (
				duration: animated ? kPSAnimationDuration : 0,
				delay: 0,
				options: (viewChanged ? 0 : UIViewAnimationOptions.BeginFromCurrentState) | UIViewAnimationOptions.AllowUserInteraction,
				animation: () => 
				{
					scaleTransform = CGAffineTransform.MakeIdentity();
					rotateTransform = CGAffineTransform.MakeIdentity();
					panTransform = CGAffineTransform.MakeIdentity();
					Transform = CGAffineTransform.MakeIdentity();

					var windowsBounds = getWindowsBounds();
					if (bounces)
					{
						setFrame (
							new CGRect (windowsBounds.X - kPSFullscreenAnimationBounce,
								windowsBounds.Y - kPSFullscreenAnimationBounce,
								windowsBounds.Width + kPSFullscreenAnimationBounce * 2,
								windowsBounds.Height + kPSFullscreenAnimationBounce * 2)
						);
					}
					else
					{
						setFrame (windowsBounds);
					}
				},
				completion: () => 
				{
					if(bounces)
					{
						var windowsBounds = getWindowsBounds();
						detachViewToWindow(true);
						UIView.Animate (
							duration: kPSAnimationDuration / 2,
							delay: 0,
							options: UIViewAnimationOptions.AllowUserInteraction,
							animation: () => 
							{
								setFrame (windowsBounds);
							},
							completion: () => 
							{
								anchorPointUpdated = false;
							
							}
						);
					}
					else
					{
						anchorPointUpdated = false;

					}


				}
			);
		}


		void alignViewAnimated (bool animated, bool bounces)
		{
			if (Frame.Width > getWindowsBounds ().Width)
				moveToFullScreenAnimated (animated, bounces);
			else
				moveViewToOriginalPositionAnimated (animated, bounces);
		}


		void resetGestureRecognizers()
		{
			foreach (var gesture in GestureRecognizers) 
			{
				gesture.Enabled = false;
				gesture.Enabled = true;
			}
		}

		void startedGesture (UIGestureRecognizer gesture)
		{
			detachViewToWindow (true);
			UIPinchGestureRecognizer pinch = (gesture.GetType () == typeof(UIPinchGestureRecognizer)) ? 
				(UIPinchGestureRecognizer)gesture : null;
			gesturesEnded = false;
			if (pinch != null)
				scaleActive = true;
		}

		void endedGesture (UIGestureRecognizer gesture)
		{
			if (gesturesEnded)
				return;

			UIPinchGestureRecognizer pinch = (gesture.GetType () == typeof(UIPinchGestureRecognizer)) ? 
				(UIPinchGestureRecognizer)gesture : null;

			if (scaleActive && pinch == null)
				return;

			gesturesEnded = true;
			if (pinch != null) 
			{
				scaleActive = false;

				if (pinch.Velocity >= 2.0f)
					moveToFullScreenAnimated (true, true);
				else
					alignViewAnimated (true, true);
			}
			else
				alignViewAnimated (true, true);
		}

		void modifiedGesture(UIGestureRecognizer gesture)
		{
			if (gesture.GetType () == typeof(UIPinchGestureRecognizer))
			{
				var pinch = (UIPinchGestureRecognizer) gesture;
				scaleTransform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity(), pinch.Scale, pinch.Scale);
			}
			else if (gesture.GetType () == typeof(UIRotationGestureRecognizer)) 
			{
				var rotate = (UIRotationGestureRecognizer) gesture;
				rotateTransform = CGAffineTransform.Rotate(CGAffineTransform.MakeIdentity(), rotate.Rotation);
			}
			else if (gesture.GetType () == typeof(UIPanGestureRecognizer)) 
			{
				var pan = (UIPanGestureRecognizer) gesture;
				CGPoint translation = pan.TranslationInView (Superview);
				panTransform = CGAffineTransform.Translate(CGAffineTransform.MakeIdentity(), translation.X, translation.Y);
			}

			Transform = CGAffineTransform.Multiply(CGAffineTransform.Multiply(scaleTransform, rotateTransform), panTransform);

		}


		void adjustAnchorPointForGestureRecognizer (UIGestureRecognizer gesture)
		{
			if (!anchorPointUpdated) 
			{
				UIView piece = gesture.View;
				CGPoint locationInView = gesture.LocationInView (piece);
				CGPoint locationInSuperview = gesture.LocationInView (piece.Superview);

				piece.Layer.AnchorPoint = new CGPoint (locationInView.X / piece.Bounds.Width,
					locationInView.Y / piece.Bounds.Height);
				piece.Center = locationInSuperview;
				anchorPointUpdated = true;
			}
		}

		public void handleGesture (UIGestureRecognizer g)
		{
			handlePinchPanRotate (g);
		}

		void handlePinchPanRotate (UIGestureRecognizer gesture)
		{
			switch (gesture.State) 
			{
			case UIGestureRecognizerState.Began:
				adjustAnchorPointForGestureRecognizer (gesture);
				startedGesture (gesture);
				break;
			case UIGestureRecognizerState.Possible:
				break;
			case UIGestureRecognizerState.Cancelled:
				endedGesture (gesture);
				anchorPointUpdated = false;
				break;
			case UIGestureRecognizerState.Failed:
				anchorPointUpdated = false;
				break;
			case UIGestureRecognizerState.Changed:
				modifiedGesture (gesture);
				break;
			case UIGestureRecognizerState.Ended:
				anchorPointUpdated = false;
				endedGesture (gesture);
				break;
			}
		}


		void handleTap (UITapGestureRecognizer tap)
		{
			if (allowSingleTapSwitch) 
			{
				if (tap.State == UIGestureRecognizerState.Ended)
				{
					if(!fullScreen)
						moveToFullScreenWindowAnimated (true);
					else
						moveToOriginalFrameAnimated (true);
				}
			}
		}



		void moveToFullScreenWindowAnimated (bool animated)
		{
			if (fullScreen) return;
			moveToFullScreenAnimated (animated, true);
		}

		void moveToOriginalFrameAnimated (bool animated) 
		{
			if (!fullScreen) return;
			moveViewToOriginalPositionAnimated (animated, true);
		}


		public void setAllowSingleTapSwitch (bool allow)
		{
			if (allowSingleTapSwitch != allow)
			{
				allowSingleTapSwitch = allow;
				tapRecognizer.Enabled = allow;
			}
		}


	}
}

