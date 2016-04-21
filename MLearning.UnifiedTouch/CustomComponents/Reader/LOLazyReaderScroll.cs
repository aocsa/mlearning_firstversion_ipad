using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

namespace MLearning.UnifiedTouch.CustomComponents
{
	public class LOLazyReaderScroll : UIScrollView
	{
		List<IStackItem> itemsList;
		List<LOPageSource> source;
		LOReaderScrollElement [] elements;

		public List<LOPageSource> Source
		{
			get { return source; }
			set 
			{ 
				numberOfItems = value.Count;
				elements = new LOReaderScrollElement[value.Count];
				ContentSize = new CGSize(Constants.DeviceWidth * value.Count, Constants.DeviceHeight);
				source = value;
			}
		}

		int chapterIndex;
		public int ChapterIndex
		{
			get { return chapterIndex; }
			set { chapterIndex = value; }
		}


		int sectionIndex;
		public int SectionIndex
		{
			get { return sectionIndex; }
			set { sectionIndex = value; }
		}

		int pageIndex;
		public int PageIndex
		{
			get { return pageIndex; }
			set { pageIndex = value; }
		}

		int numberOfItems;
		public int NumberOfItems 
		{
			get { return numberOfItems; }
			set { numberOfItems = value; }
		}

		int currentIndex;
		public int CurrentIndex 
		{
			get { return currentIndex; }
			set { currentIndex = value; }
		}

		bool itemChanged;
		int prevIndex;

		static bool isVisible;
		public static bool IsVisible {
			get {
				return isVisible;
			}
			set {
				isVisible = value;
			}
		}

		private static LOLazyReaderScroll Instance = null;


		private LOLazyReaderScroll (List<IStackItem> items) : base ()
		{
			itemsList = items;
			Frame = Constants.ScreenFrame;
			PagingEnabled = true;
			Bounces = false;
			itemChanged = false;
			Scrolled += HandleScrolled;
		}

		public static LOLazyReaderScroll getInstance (List<IStackItem> items)
		{
			if (Instance == null)
				Instance = new LOLazyReaderScroll (items);
			return Instance;
		}

		void HandleScrolled (object sender, EventArgs e)
		{

			loadVisiblePages ();

		}


		void loadPage (int page)
		{
			if (page < 0 || page >= elements.Length)
				return;

			var scrollElement = elements [page];
			if (scrollElement == null) 
			{
				LOReaderScrollElement element = new LOReaderScrollElement (page);
				element.Source = source [page];
				Add (element);
				elements[page] = element;
			}
		}


		void purgePage (int page)
		{
			if (page < 0 || page >= elements.Length)
				return;
			var scrollElement = elements [page];
			if (scrollElement != null) 
			{
				scrollElement.RemoveFromSuperview ();
				elements [page] = null;
			}
		}
	

		public void loadVisiblePages ()
		{
			nfloat pageWidth = Frame.Width;
			int page = (int) Math.Floor ((ContentOffset.X * 2 + pageWidth) / (pageWidth * 2));
			int firstPage = page - 1;
			int lastPage = page + 1;

			for (int i = 0; i < firstPage; i++)
				InvokeOnMainThread ( () => purgePage(i));

			for (int i = firstPage; i <= lastPage; i++)
				InvokeOnMainThread ( () => loadPage(i));

			for (int i = lastPage + 1; i < Source.Count; i++)
				InvokeOnMainThread ( () => purgePage(i));

		}

		bool updateCurrentElement()
		{
			int item = (int) Math.Round (ContentOffset.X / Constants.DeviceWidth);

			if (item >= NumberOfItems || item < 0) 
			{
				itemChanged = false;
				return itemChanged;
			}
			if (item != currentIndex) 
			{
				prevIndex = currentIndex;
				currentIndex = item;
				itemChanged = true;
			}
			else
				itemChanged = false;
			return itemChanged;
		}

		public override void SetContentOffset (CGPoint contentOffset, bool animated)
		{
			base.SetContentOffset (contentOffset, animated);
			currentIndex = (int)Math.Round (ContentOffset.X / Constants.DeviceWidth);
		}

	}
}

