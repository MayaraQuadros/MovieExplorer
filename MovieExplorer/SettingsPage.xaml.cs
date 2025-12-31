namespace MovieExplorer;

public partial class SettingsPage : ContentPage
{

    private AppSettings currentSettings;
	public SettingsPage()
	{
		InitializeComponent();

        // Load current settings from file
        currentSettings = SettingsManager.LoadSettings();

        // Initialize UI controls with current settings values
        SizeSlider.Value = currentSettings.TextSize;

        // Set up slider value changed events to update labels
        SizeSlider.ValueChanged += (s, e) =>
            SizeLabel.Text = $"{(int)e.NewValue}";
    }

    // Handles Theme button clicks. Updates the BackgroundColor setting.
    private void OnThemeSelected(object sender, EventArgs e)
    {
        if(sender is Button button)
        {
            string genre = button.Text;
            switch(genre)
            {
                case "Romance":
                    currentSettings.BackgroundColorFrame = "#b76f6a";
                    currentSettings.BackgroundColor = "#F2D6D3";
                    currentSettings.TextColor = "#000000";
                    currentSettings.FavouriteIcon = "romance_icon.png";

                    break;
                case "Western":
                    currentSettings.BackgroundColorFrame = "#D2B48C";
                    currentSettings.BackgroundColor = "#F5E9D6";
                    currentSettings.TextColor = "#000000";
                    currentSettings.FavouriteIcon = "western_icon.png";
                    break;
                case "Thriller":
                    currentSettings.BackgroundColorFrame = "#0B1C2D";
                    currentSettings.BackgroundColor = "#2E4057";
                    currentSettings.TextColor = "#FFFFFF";
                    currentSettings.FavouriteIcon = "thriller_icon.png";
                    break;
                case "Drama":
                    currentSettings.BackgroundColorFrame = "#8C8C8C";
                    currentSettings.BackgroundColor = "#E6E6E6";
                    currentSettings.TextColor = "#000000";
                    currentSettings.FavouriteIcon = "drama_icon.png";
                    break;
                case "Default":
                    currentSettings.BackgroundColorFrame = "#B8860B";
                    currentSettings.BackgroundColor = "#F2E6C7";
                    currentSettings.TextColor = "#000000";
                    currentSettings.FavouriteIcon = "love_icon.png";
                    break;
            }
        }
    }

    private async void OnSaved_Clicked(object sender, EventArgs e)
    {
        try
        {
            // TODO: Get the current values from the UI controls and update currentSettings
            // Get the current values from UI controls
           
            currentSettings.TextSize = SizeSlider.Value;
           

            // Save settings to file
            SettingsManager.SaveSettings(currentSettings);

            await DisplayAlert("Success", "Settings saved!", "OK");
            await Shell.Current.GoToAsync("//MoviesMainPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save settings: {ex.Message}", "OK");
        }
    }
}