// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace LocationBasedAlert
{
	[Register ("LocationBasedAlertViewController")]
	partial class LocationBasedAlertViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton alertButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField destinationText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel distanceText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField radiusText { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (alertButton != null) {
				alertButton.Dispose ();
				alertButton = null;
			}
			if (destinationText != null) {
				destinationText.Dispose ();
				destinationText = null;
			}
			if (distanceText != null) {
				distanceText.Dispose ();
				distanceText = null;
			}
			if (radiusText != null) {
				radiusText.Dispose ();
				radiusText = null;
			}
		}
	}
}
