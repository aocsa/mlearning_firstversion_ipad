using System;
using CoreAnimation;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Drawing;
using System.Collections.Generic;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class GradientComponent : CAGradientLayer
	{
		List< CGColor[] > colorsList;
		public List<CGColor[]> ColorsList {
			get {
				return colorsList;
			}
			set {
				colorsList = value;
			}
		}

		List< CABasicAnimation > animations;
		public List<CABasicAnimation> Animations {
			get {
				return animations;
			}
			set {
				animations = value;
			}
		}


		public GradientComponent (CGRect frame, float animationDuration) : base ()
		{

			Frame = frame;
			ColorsList = new List<CGColor[]> ();
			Animations = new List<CABasicAnimation> ();
			initializeColorList ();

			Colors = ColorsList [0];
	
			float stepAnimationDuration = animationDuration / ColorsList.Count;
			for (int i = 0; i < ColorsList.Count + 1; i++)
			{
				createAndAddAnimation (pos: i, duration: stepAnimationDuration,  beginTime: i * stepAnimationDuration);
			}

			CAAnimationGroup groupAnimation = new CAAnimationGroup ();
			groupAnimation.FillMode = CAFillMode.Forwards;
			groupAnimation.RemovedOnCompletion = false;
			groupAnimation.Animations = Animations.ToArray();
			groupAnimation.Duration = animationDuration;
			groupAnimation.RepeatCount = float.MaxValue;

			AddAnimation (groupAnimation, "groupAnimation");

		}

		public void initializeColorList ()
		{
			ColorsList.Add ( new CGColor[2] { UIColor.FromRGB (45, 189, 212).CGColor, UIColor.FromRGB (187, 210, 198).CGColor });
			ColorsList.Add ( new CGColor[2] { UIColor.FromRGB(224, 50, 115).CGColor	, UIColor.FromRGB(251, 147, 66).CGColor  });
			ColorsList.Add ( new CGColor[2] { UIColor.FromRGB(37, 191, 44).CGColor	, UIColor.FromRGB(216, 204, 121).CGColor });
			ColorsList.Add ( new CGColor[2] { UIColor.FromRGB(137, 25, 178).CGColor	, UIColor.FromRGB(237, 168, 152).CGColor });
			ColorsList.Add ( new CGColor[2] { UIColor.FromRGB (32, 98, 229).CGColor	, UIColor.FromRGB (30, 197, 206).CGColor });
		}

		public void createAndAddAnimation (int pos, float duration, float beginTime)
		{
			var max = ColorsList.Count;
			var animation = CABasicAnimation.FromKeyPath ("colors");
			animation.From = NSArray.FromObjects (ColorsList [pos % max]);
			animation.To = NSArray.FromObjects 	 (ColorsList [(pos + 1) % max]);
			animation.Duration = duration;
			animation.BeginTime = beginTime;
			animation.FillMode = CAFillMode.Forwards;
			animation.RemovedOnCompletion = false;

			Animations.Add (animation);
		}
	}
}

