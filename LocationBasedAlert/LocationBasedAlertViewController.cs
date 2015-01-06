using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

// https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/LocationAwarenessPG/RegionMonitoring/RegionMonitoring.html

// https://developer.apple.com/library/mac/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/Chapters/IPhoneOSClientImp.html

// Stanford University address: 450 Serra Mall, Stanford, CA 94305

namespace LocationBasedAlert
{
	public partial class LocationBasedAlertViewController : UIViewController
	{

		CLLocationManager locationManager;

		UIUserNotificationSettings settings;

		public LocationBasedAlertViewController (IntPtr handle) : base (handle)
		{
		}
			
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			destinationText.ResignFirstResponder();
			radiusText.ResignFirstResponder();

			locationManager = new CLLocationManager ();

			// set the update threshold 
			locationManager.DistanceFilter = 10; // move 10 meters before updating

			// set the desired accuracy:
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest; // best accuracy

			// If the OS is iOS 8, ask for permission to location and send notification

			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				locationManager.RequestWhenInUseAuthorization ();
			
				// http://www.knowing.net/index.php/2014/07/03/local-notifications-in-ios-8-with-xamarin/

				settings = UIUserNotificationSettings.GetSettingsForTypes(
					UIUserNotificationType.Alert
					| UIUserNotificationType.Badge
					| UIUserNotificationType.Sound,
					new NSSet());
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			//locationManager.StartUpdatingLocation();

			if (CLLocationManager.LocationServicesEnabled) {
			
			    if (CLLocationManager.Status != CLAuthorizationStatus.Denied) {

					// handle button click. Create geofence with the provided information 

					alertButton.TouchUpInside += delegate(object sender, EventArgs e) {

						if ((!String.IsNullOrEmpty (destinationText.Text)) && (!String.IsNullOrEmpty (radiusText.Text))) {

							var geocoder = new CLGeocoder ();
							geocoder.GeocodeAddress (destinationText.Text, (CLPlacemark[] placemarks, NSError error) => {

								if ((placemarks != null) && (placemarks.Length > 0)) {

									Console.WriteLine("Destination: " + placemarks[0].Location.Coordinate.Latitude.ToString() + "," 
										+ placemarks[0].Location.Coordinate.Longitude.ToString());

									// get radius from input in meters 
									double radius = Convert.ToDouble (radiusText.Text)*1000;

									// adjust radius to maximum allowed value if exceeds
									// https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/LocationAwarenessPG/RegionMonitoring/RegionMonitoring.html

									if (radius > locationManager.MaximumRegionMonitoringDistance) {
										radius = locationManager.MaximumRegionMonitoringDistance;
										Console.WriteLine("Exceed maximum region monitoring distance. Radius has been adjusted to maximum allowed value " + radius.ToString() + " m.");
									}

									// Get current location and calculate distance to destination

									locationManager.StartUpdatingLocation();
									locationManager.LocationsUpdated += delegate (object sends, CLLocationsUpdatedEventArgs events) {
										CLLocation currentLocation = events.Locations [0];
										CLLocation destination = new CLLocation(placemarks[0].Location.Coordinate.Latitude, 
											placemarks[0].Location.Coordinate.Longitude);
										double distance = Math.Round(currentLocation.DistanceFrom (destination) / 1000, 3);
										distanceText.Text = distance.ToString ();
										Console.WriteLine("Current location: " + currentLocation.Coordinate.Latitude.ToString() + "," 
											+ currentLocation.Coordinate.Longitude.ToString());

									};

		

									// set the region to be monitored
									var region = new CLCircularRegion (new CLLocationCoordinate2D (placemarks[0].Location.Coordinate.Latitude, 
										placemarks[0].Location.Coordinate.Longitude), radius, "Destination");

									// start monitoring region
									// locationManager.StartMonitoring(region);


									// once enter region fire notification

									/*
									    locationManager.RegionEntered += (ob, ev) => {
									
										UIAlertView alert = new UIAlertView ("Alert", "You are within " + radius.ToString() + " m to destination", null, "OK", null);
										alert.Show ();
									
									}; */


										// Notification part

										var notification = new UILocalNotification ();

										// set the fire time. This works well
										//notification.FireDate = DateTime.Now.AddSeconds(15);

									    // only trigger once
									    notification.RegionTriggersOnce = true;

										// set notification region
										notification.Region = region;

										// configure the alert stuff
										notification.AlertAction = "Alert";
										notification.AlertBody = "Your are within " + radius.ToString() + " m to destination";

										// modify the badge
										notification.ApplicationIconBadgeNumber = 1;

										// set the sound to be the default sound
										notification.SoundName = UILocalNotification.DefaultSoundName;

									    

										// schedule it
										UIApplication.SharedApplication.ScheduleLocalNotification (notification);

									    locationManager.StartMonitoring(region);

										//Console.WriteLine("Alert has been set, Destination Coordinate: [" + placemarks[0].Location.Coordinate.Latitude.ToString() + "," 
										//	+ placemarks[0].Location.Coordinate.Longitude.ToString() + "], Radius: " + radius.ToString() + " m");



								} 
							});

						} else {
							Console.WriteLine ("No valid destination and alert radius are available");
						}

					};
	
				} else {
					Console.WriteLine ("App is not authorized to use location data");
				}

			} else {
				Console.WriteLine ("Location services not enabled");
			}

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

