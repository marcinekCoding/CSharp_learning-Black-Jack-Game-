using Avalonia.Interactivity;
using MojeLogowanieGUI.Views;

namespace testy_jednostkowe;

public class RegisterWindowTests
{
    [Theory]
    [InlineData("ab@12")]
    [InlineData("haslo@99")]
    [InlineData("x@y12abc")]
    public void Password_Validation_Akceptuje_Poprawne_Haslo(string haslo)
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            Assert.True(okno.password_validation(haslo));
        });
    }

    [Theory]
    [InlineData("ab@1")]
    [InlineData("1234")]
    [InlineData("abcde")]
    public void Password_Validation_Odrzuca_Za_Krotkie_Haslo(string haslo)
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            Assert.False(okno.password_validation(haslo));
        });
    }

    [Theory]
    [InlineData("abcde12")]
    [InlineData("haslo99")]
    public void Password_Validation_Odrzuca_Brak_Znaku_Malpy(string haslo)
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            Assert.False(okno.password_validation(haslo));
        });
    }

    [Theory]
    [InlineData("haslo@1")]
    [InlineData("abc@5")]
    public void Password_Validation_Odrzuca_Mniej_Niz_Dwie_Cyfry(string haslo)
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            Assert.False(okno.password_validation(haslo));
        });
    }

    [Theory]
    [InlineData("hasl o@12")]
    [InlineData("ha slo@99")]
    public void Password_Validation_Odrzuca_Spacje(string haslo)
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            Assert.False(okno.password_validation(haslo));
        });
    }

    [Fact]
    public void RejestrujPrzycisk_Click_Rozne_Hasla_Ustawia_Komunikat()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            okno.LoginBox.Text = "nowy_user";
            okno.PasswordBox.Text = "haslo@12";
            okno.PasswordBoxV2.Text = "inne@12";

            okno.RejestrujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Passwords are not the same", okno.MessageBlock.Text);
        });
    }

    [Fact]
    public void RejestrujPrzycisk_Click_Puste_Pola_Ustawia_Komunikat()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            okno.LoginBox.Text = "";
            okno.PasswordBox.Text = "";
            okno.PasswordBoxV2.Text = "";

            okno.RejestrujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Passwords are not the same", okno.MessageBlock.Text);
        });
    }

    [Fact]
    public void RejestrujPrzycisk_Click_Za_Krotkie_Haslo_Ustawia_Komunikat()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            okno.LoginBox.Text = "user";
            okno.PasswordBox.Text = "a@12";
            okno.PasswordBoxV2.Text = "a@12";

            okno.RejestrujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Equal("Too short password", okno.MessageBlock.Text);
        });
    }

    [Fact]
    public void RejestrujPrzycisk_Click_Niepoprawne_Haslo_Ustawia_Komunikat_Wymagan()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var okno = new RegisterWindow();
            okno.LoginBox.Text = "user";
            okno.PasswordBox.Text = "haslo12";
            okno.PasswordBoxV2.Text = "haslo12";

            okno.RejestrujPrzycisk_Click(okno, new RoutedEventArgs());

            Assert.Contains("Your password doesn't meet the requirements", okno.MessageBlock.Text);
        });
    }
}
