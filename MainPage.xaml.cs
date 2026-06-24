using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using LocationHeatMapApp.Models;
using LocationHeatMapApp.Services;
using System;
using System.Threading.Tasks;

namespace LocationHeatMapApp
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();

            BackgroundWorkerInitializePosition();
        }

        private async void BackgroundWorkerInitializePosition()
        {
            try
            {
                var location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                {
                    LocationMap.MoveToRegion(
                        MapSpan.FromCenterAndRadius(
                            new Location(location.Latitude, location.Longitude),
                            Distance.FromMiles(0.5)
                        )
                    );
                }
            }
            catch (Exception)
            {
            }
        }

        private async void OnLogLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(8)
                );

                var location = await Geolocation.Default.GetLocationAsync(request);
                await DisplayAlert(
                                  "Location",
                                  $"{location?.Latitude}, {location?.Longitude}",
                                  "OK");

                if (location != null)
                {
                    var record = new LocationPoint
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.Now
                    };

                    await _databaseService.SaveLocationAsync(record);
                    await DisplayAlert("Success", "Location point recorded.", "OK");

                    await GenerateHeatmapOverlayAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Location Error", ex.Message, "OK");
            }
        }

        private async void OnRefreshHeatmapClicked(object sender, EventArgs e)
        {
            await GenerateHeatmapOverlayAsync();
        }

        private async Task GenerateHeatmapOverlayAsync()
        {
            LocationMap.MapElements.Clear();

            var trackHistory = await _databaseService.GetLocationsAsync();

            foreach (var point in trackHistory)
            {
                var circleOverlay = new Circle
                {
                    Center = new Location(point.Latitude, point.Longitude),
                    Radius = new Distance(60),
                    StrokeColor = Color.FromRgba(255, 0, 0, 180),
                    FillColor = Color.FromRgba(255, 0, 0, 120)
                };

                LocationMap.MapElements.Add(circleOverlay);
            }
        }
    }
}