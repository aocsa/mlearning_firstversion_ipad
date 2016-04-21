// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MLearning.UnifiedTouch
{
	[Register ("PersonCell")]
	partial class PersonCell
	{
		[Outlet]
		UIKit.UIImageView PeopleImage { get; set; }

		[Outlet]
		UIKit.UILabel PeopleName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PeopleImage != null) {
				PeopleImage.Dispose ();
				PeopleImage = null;
			}

			if (PeopleName != null) {
				PeopleName.Dispose ();
				PeopleName = null;
			}
		}
	}
}
