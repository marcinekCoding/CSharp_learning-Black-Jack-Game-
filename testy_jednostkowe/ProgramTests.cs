using Avalonia.Interactivity;
using MojeLogowanieGUI;
using MojeLogowanieGUI.Views;

namespace testy_jednostkowe;

public class ProgramTests
{
    [Fact]
    public void BuildAvaloniaApp_Zwraca_Skonfigurowany_AppBuilder()
    {
        var builder = Program.BuildAvaloniaApp();

        Assert.NotNull(builder);
    }

    [Fact]
    public void App_Initialize_Nie_Rzuca_Wyjatku()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var app = new App();
            var exception = Record.Exception(() => app.Initialize());

            Assert.Null(exception);
        });
    }
}
