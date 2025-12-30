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
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save settings: {ex.Message}", "OK");
        }
    }
}