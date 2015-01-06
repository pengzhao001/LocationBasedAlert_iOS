using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LocationBasedAlert
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		//private LocationBasedAlertViewController viewController;
		//private UIWindow window;

		// when app is in background

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			//window = new UIWindow(UIScreen.MainScreen.Bounds);

			//viewController = new LocationBasedAlertViewController();
			//window.RootViewController = viewController;

			//window.MakeKeyAndVisible();

			// check for a notification
			if (options != null)
			{
				// check for a local notification
				if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
				{
					var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
					if (localNotification != null)
					{
						new UIAlertView(localNotification.AlertAction, localNotification.AlertBody, null, "OK", null).Show();
						// reset our badge
						UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
					}
				}
			}

			return true;
		}

		// when app is in foreground

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// show an alert
			new UIAlertView(notification.AlertAction, notification.AlertBody, null, "OK", null).Show();

			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

			Console.WriteLine ("Notification Fired");
		}
			
		// class-level declarations
		
		public override UIWindow Window {
			get;
			set;
		}
		
		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		
		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
		}
	}
}

