// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace MLearning.Touch
{
	[Register ("CircleCell")]
	partial class CircleCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView CircleImage { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UILabel CircleName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CircleImage != null) {
				CircleImage.Dispose ();
				CircleImage = null;
			}

			if (CircleName != null) {
				CircleName.Dispose ();
				CircleName = null;
			}
		}
	}
}
