using Avalonia.Interactivity;
using MojeLogowanieGUI.Models;
using MojeLogowanieGUI.Services;
using MojeLogowanieGUI.Views;

namespace testy_jednostkowe;

public class PanelWindowTests
{
    public PanelWindowTests()
    {
        PanelWindow.ClickerScore = 0;
        Wallet.Reset();
    }

    [Fact]
    public void Konstruktor_Ustawia_Nazwe_Uzytkownika()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("testuser");
            Assert.Equal("Your username: testuser", panel.UserLogin.Text);
        });
    }

    [Fact]
    public void DodajPunkty_Zwieksza_Licznik_Clickera()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            PanelWindow.ClickerScore = 0;
            var panel = new PanelWindow("gracz");

            panel.dodajPunkty(panel, new RoutedEventArgs());

            Assert.Equal(1, PanelWindow.ClickerScore);
            Assert.Equal("1", panel.Points_box.Text);
        });
    }

    [Fact]
    public void Black_Jack_Ustawia_Stan_Oczekiwania_Na_Zaklad()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");

            panel.Black_Jack(panel, new RoutedEventArgs());

            Assert.Equal("Czekamy na obstawienie", panel.PlayerCards_text.Text);
            Assert.False(panel.hit_btn.IsEnabled);
            Assert.False(panel.stop_btn.IsEnabled);
        });
    }

    [Fact]
    public void DobierzKarte_Click_Bez_Postawionego_Zakladu_Ustawia_Komunikat()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");

            panel.DobierzKarte_Click(panel, new RoutedEventArgs());

            Assert.Equal("Najpierw postaw zaklad!", panel.Verdict_box.Text);
        });
    }

    [Fact]
    public void PlaceBet_Click_Z_Poprawna_Stawka_Rozpoczyna_Gre()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            Wallet.Reset(100);
            var panel = new PanelWindow("gracz");
            panel.Black_Jack(panel, new RoutedEventArgs());
            panel.BetInput_box.Text = "20";

            panel.PlaceBet_Click(panel, new RoutedEventArgs());

            Assert.Equal(80, Wallet.Balance);
            Assert.Equal(20, Wallet.CurrentBet);
            Assert.Equal(2, panel.PlayerCards_text.Text?.Split("  ", StringSplitOptions.RemoveEmptyEntries).Length);
            Assert.False(panel.PlaceBet_btn.IsEnabled);
        });
    }
}
