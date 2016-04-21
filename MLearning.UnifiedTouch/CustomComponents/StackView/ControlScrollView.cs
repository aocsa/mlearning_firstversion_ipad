using System;
using UIKit;
using CoreGraphics;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class ControlScrollView : UIView
	{
		#region Properties
		List<UIImageView> imagesList;
		public List<UIImageView> ImagesList 
		{
			get { return imagesList; }
			set { imagesList = value; }
		}

		List<UIImageView> logosList;
		public List<UIImageView> LogosList 
		{
			get { return logosList; }
			set { logosList = value; }
		}

		int itemsNumber;
		public int ItemsNumber
		{
			set	{ itemsNumber = value; }
			get { return itemsNumber; }
		}

		int currentItem;
		public int CurrentItem
		{
			set	{ currentItem = value; }
			get { return currentItem; }
		}
		int actualIndex;
		public int ActualIndex
		{
			set	{ actualIndex = value; }
			get { return actualIndex; }
		}
			
		private BookDataSource source;
		public BookDataSource Source
		{
			get { return source; }
			set { source = value; ItemsNumber = source.Chapters.Count; loadScroll(); }
		}

		#endregion

		public ControlScrollView () : base()
		{
			ImagesList = new List<UIImageView> ();
			LogosList = new List<UIImageView> ();
			Frame = Constants.ScreenFrame;
			BackgroundColor = UIColor.Clear;
			//actual is used to know which image should be faded
			//current item is used to know which image should be shown when animating
			ActualIndex = 0;
			CurrentItem = 0;
		}

		void loadScroll()
		{
			for (int i = 0; i < ItemsNumber; i++)
			{
				var img = new UIImageView ();
				img.Image = source.Chapters[i].BackgroundImage;
				img.Frame = Constants.ScreenFrame;
				img.ContentMode = UIViewContentMode.ScaleToFill;
				img.Alpha = 0;
				Add (img);
				ImagesList.Add(img);
			}

			var alphaView = new UIView (Constants.ScreenFrame);
			alphaView.BackgroundColor = UIColor.Black;
			alphaView.Alpha = 0.4f;
			Add (alphaView);

			for (int i = 0; i < ItemsNumber; i++) 
			{
				var logo = Constants.getLogoByIndex (i);
				logo.Alpha = 0;
				Add (logo);
				LogosList.Add (logo);
			}



			ImagesList[0].Alpha = 1;
			LogosList [0].Alpha = 1;
		}

		public void setToIndex(int index)
		{
			animateImage(0, ActualIndex);
			animateImage(1, index);
			ActualIndex = index;
		}

		void animateImage(float alpha, int index)
		{
			UIView.Animate (
				duration: 0.65f,
				delay: 0.0f,
				options: UIViewAnimationOptions.CurveEaseInOut,
				animation: ()=>
				{
					ImagesList[index].Alpha = alpha;
					LogosList [index].Alpha = alpha;
				},
				completion: ()=> {}
			);
		}
	}
}

