using Microsoft.Maui.Graphics.Text;
using Microsoft.UI.Xaml.Documents;
using Windows.UI.Text;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        private bool lightTheme;

        public MainPage()
        {
            InitializeComponent();
            lightTheme = Preferences.Default.Get("LightTheme", true);
        }

        private void ToggleThemeButton_Clicked(object sender, EventArgs e)
        {
            lightTheme = !lightTheme;
            applyTheme();
            Preferences.Default.Set("LightTheme", lightTheme);
        }


        private void applyTheme()
        {
            if (lightTheme)
            {
                Resources["PageBackgroundColor"] = Resources["LightBackgroundColor"];
                ToggleThemeButton.TextColor = Colors.Black;
                ToggleThemeButton.BackgroundColor = Colors.BlueViolet;
                

            }
            else
            {
                Resources["PageBackgroundColor"] = Resources["DarkBackgroundColor"];
                ToggleThemeButton.TextColor = Colors.LightGray;
                ToggleThemeButton.BackgroundColor = Colors.Teal;
            }
        }
    }
}
   
