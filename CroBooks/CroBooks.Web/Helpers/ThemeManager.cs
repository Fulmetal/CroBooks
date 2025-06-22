using MudBlazor;

namespace CroBooks.Web.Helpers
{
    public static class ThemeManager
    {
        public static MudTheme DefaultTheme()
        {
            //var primary = Colors.Blue.Default;
            //var onPrimary = Colors.Shades.White;
            //var primaryContainer = 


            MudTheme defaultTheme = new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = Colors.Blue.Default,
                    PrimaryLighten = Colors.Blue.Lighten4,
                    Secondary = Colors.Blue.Lighten4,
                    
                    AppbarBackground = Colors.Red.Default,
                },
                PaletteDark = new PaletteDark()
                {
                    Primary = Colors.Blue.Lighten1
                },

                LayoutProperties = new LayoutProperties()
                {
                    DrawerWidthLeft = "260px",
                    DrawerWidthRight = "300px"
                }
            };

            return defaultTheme;
        }

    }
}
