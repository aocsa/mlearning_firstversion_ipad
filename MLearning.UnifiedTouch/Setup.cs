using UIKit;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.CrossCore;
using MLearning.Core.File;
using MLearning.UnifiedTouch.File;

namespace MLearning.UnifiedTouch
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}


		protected override void InitializeLastChance()
		{
			Mvx.RegisterSingleton<IAsyncStorageService>(new AsyncStorageTouchService());
			base.InitializeLastChance();
		}


		protected override IMvxApplication CreateApp ()
		{
			return new Core.App();
		}

		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}
	}
}