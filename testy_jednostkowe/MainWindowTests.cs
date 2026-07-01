using Avalonia.Interactivity;
using MojeLogowanieGUI.Views;

namespace testy_jednostkowe;

public class MainWindowTests
{
    [Fact]
    public void ZalogujPrzycisk_Click_Poprawne_Dane_Ustawia_Access_Granted()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new MainWindow();
            okno.LoginBox.Text = "marcinek";
            okno.PasswordBox.Text = "1778";

            okno.ZalogujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Access granted...", okno.MessageBlock.Text);
        });
    }

    [Fact]
    public void ZalogujPrzycisk_Click_Login_Niezalezny_Od_Wielkosci_Liter()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new MainWindow();
            okno.LoginBox.Text = "Marcinek";
            okno.PasswordBox.Text = "1778";

            okno.ZalogujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Access granted...", okno.MessageBlock.Text);
        });
    }

    [Fact]
    public void ZalogujPrzycisk_Click_Bledne_Dane_Ustawia_Komunikat_Bledu()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new MainWindow();
            okno.LoginBox.Text = "zly_login";
            okno.PasswordBox.Text = "zle_haslo";

            okno.ZalogujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Get the fuck out", okno.MessageBlock.Text);
        });
    }

    [Fact]
    public void ZalogujPrzycisk_Click_Bledne_Haslo_Ustawia_Komunikat_Bledu()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new MainWindow();
            okno.LoginBox.Text = "marcinek";
            okno.PasswordBox.Text = "0000";

            okno.ZalogujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Get the fuck out", okno.MessageBlock.Text);
        });
    }
}
