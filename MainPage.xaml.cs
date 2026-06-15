namespace LocationHeatMapApp;

public partial class MainPage : ContentPage
{
    LocationDatabase db;

    public MainPage()
    {
        InitializeComponent();

        db = new LocationDatabase();
    }

    private async void SaveLocation_Clicked(object sender, EventArgs e)
    {
        try
        {
            var location = await Geolocation.Default.GetLocationAsync();

            if (location != null)
            {
                LocationPoint point = new LocationPoint();

                point.Latitude = location.Latitude;
                point.Longitude = location.Longitude;
                point.TimeRecorded = DateTime.Now.ToString();

                await db.SaveLocation(point);

                ResultLabel.Text =
                    "Location Saved\n" +
                    "Latitude: " + point.Latitude +
                    "\nLongitude: " + point.Longitude;
            }
        }
        catch (Exception ex)
        {
            ResultLabel.Text = ex.Message;
        }
    }
}