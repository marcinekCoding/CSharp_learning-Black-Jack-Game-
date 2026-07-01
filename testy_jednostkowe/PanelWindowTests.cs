using Avalonia.Interactivity;
using MojeLogowanieGUI.Models;
using MojeLogowanieGUI.Views;

namespace testy_jednostkowe;

public class PanelWindowTests
{
    public PanelWindowTests()
    {
        PanelWindow.punkty = 0;
        PanelWindow.idx = 0;
    }

    private static List<Card> UtworzTalie(params (Rank rank, Suit suit)[] karty)
    {
        return karty.Select(k => new Card(k.rank, k.suit)).ToList();
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
    public void DodajPunkty_Zwieksza_Licznik_Punktow()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            PanelWindow.punkty = 0;
            var panel = new PanelWindow("gracz");

            panel.dodajPunkty(panel, new RoutedEventArgs());

            Assert.Equal(1, PanelWindow.punkty);
            Assert.Equal("1", panel.Points_box.Text);
        });
    }

    [Fact]
    public void Licz_Punkty_Pusta_Lista_Zwraca_Zero()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            Assert.Equal(0, panel.licz_punkty(new List<Card>()));
        });
    }

    [Fact]
    public void Licz_Punkty_Sumuje_Wartosci_Kart()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var karty = UtworzTalie(
                (Rank.King, Suit.Hearts),
                (Rank.Five, Suit.Clubs),
                (Rank.Ace, Suit.Spades));

            Assert.Equal(26, panel.licz_punkty(karty));
        });
    }

    [Fact]
    public void Is_21_Zwraca_Prawde_Gdy_Suma_Wynosi_21()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var karty = UtworzTalie(
                (Rank.King, Suit.Hearts),
                (Rank.Ace, Suit.Clubs));

            Assert.True(panel.is_21(karty));
        });
    }

    [Fact]
    public void Is_21_Zwraca_Falsz_Gdy_Suma_Nie_Wynosi_21()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var karty = UtworzTalie(
                (Rank.Ten, Suit.Hearts),
                (Rank.Five, Suit.Clubs));

            Assert.False(panel.is_21(karty));
        });
    }

    [Fact]
    public void Gra_Dalej_Zwraca_Falsz_Gdy_Gracz_Przekroczyl_21()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var karty = UtworzTalie(
                (Rank.King, Suit.Hearts),
                (Rank.Queen, Suit.Clubs),
                (Rank.Five, Suit.Spades));

            Assert.False(panel.gra_dalej(karty));
        });
    }

    [Fact]
    public void Gra_Dalej_Zwraca_Prawde_Gdy_Gracz_Ma_21_Lub_Mniej()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var karty = UtworzTalie(
                (Rank.Ten, Suit.Hearts),
                (Rank.Seven, Suit.Clubs));

            Assert.True(panel.gra_dalej(karty));
        });
    }

    [Theory]
    [InlineData(22, 20, 0)]
    [InlineData(20, 22, 1)]
    [InlineData(20, 18, 1)]
    [InlineData(18, 20, 0)]
    [InlineData(20, 20, 2)]
    public void Check_Winner_Zwraca_Oczekiwany_Wynik(int sumaGracza, int sumaKrupiera, int oczekiwanyWynik)
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var gracz = UtworzTalieDlaSummy(sumaGracza);
            var krupier = UtworzTalieDlaSummy(sumaKrupiera);

            Assert.Equal(oczekiwanyWynik, panel.check_winner(gracz, krupier));
        });
    }

    [Fact]
    public void Dobierz_Karte_Dodaje_Karte_Z_Talii()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            PanelWindow.idx = 0;
            var panel = new PanelWindow("gracz");
            var gracz = new List<Card>();
            var talia = UtworzTalie((Rank.Two, Suit.Hearts), (Rank.Three, Suit.Clubs));

            panel.dobierz_karte(gracz, talia);

            Assert.Single(gracz);
            Assert.Equal(talia[0], gracz[0]);
            Assert.Equal(1, PanelWindow.idx);
        });
    }

    [Fact]
    public void Wstep_Bj_Rozdaje_3_Karty_Graczowi_I_1_Krupierowi()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            PanelWindow.idx = 0;
            var panel = new PanelWindow("gracz");
            var gracz = new List<Card>();
            var krupier = new List<Card>();
            var talia = UtworzTalie(
                (Rank.Two, Suit.Hearts),
                (Rank.Three, Suit.Clubs),
                (Rank.Four, Suit.Diamonds),
                (Rank.Five, Suit.Spades));

            panel.wstep_bj(gracz, talia, krupier);

            Assert.Equal(2, gracz.Count);
            Assert.Single(krupier);
            Assert.Equal(3, PanelWindow.idx);
        });
    }

    [Fact]
    public void Tasuj_Karty_Zachowuje_Te_Same_Karty()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");
            var talia = UtworzTalie(
                (Rank.Two, Suit.Hearts),
                (Rank.Three, Suit.Clubs),
                (Rank.Four, Suit.Diamonds),
                (Rank.Five, Suit.Spades));
            var oryginalnaKolejnosc = talia.Select(k => k.ToString()).ToList();

            panel.tasuj_karty(talia);

            Assert.Equal(oryginalnaKolejnosc.Count, talia.Count);
            Assert.Equal(
                oryginalnaKolejnosc.OrderBy(k => k),
                talia.Select(k => k.ToString()).OrderBy(k => k));
        });
    }

    [Fact]
    public void Black_Jack_Rozdaje_Karty_Na_Start()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            PanelWindow.idx = 0;
            var panel = new PanelWindow("gracz");

            panel.Black_Jack(panel, new RoutedEventArgs());

            Assert.Equal(3, PanelWindow.idx);
            Assert.Equal(2, panel.PlayerCards_text.Text?.Split("  ", StringSplitOptions.RemoveEmptyEntries).Length);
            Assert.Equal(1, panel.DealerCards_text.Text?.Split("  ", StringSplitOptions.RemoveEmptyEntries).Length);
        });
    }

    [Fact]
    public void DobierzKarte_Click_Bez_Gry_Ustawia_Komunikat()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");

            panel.DobierzKarte_Click(panel, new RoutedEventArgs());

            Assert.Equal("Rozpocznij grę!", panel.Verdict_box.Text);
        });
    }

    [Fact]
    public void Gracz_Wygrywa_Ustawia_Werdykt()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");

            panel.gracz_wygrywa();

            Assert.Equal("Gracz wygrywa", panel.Verdict_box.Text);
            Assert.True(panel.ResultOverlay.IsVisible);
            Assert.Equal("🎉 Gracz wygrywa!", panel.ResultOverlay_text.Text);
        });
    }

    [Fact]
    public void Gracz_Przegrywa_Ustawia_Werdykt()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");

            panel.gracz_przegrywa();

            Assert.Equal("Gracz przegrywa", panel.Verdict_box.Text);
            Assert.True(panel.ResultOverlay.IsVisible);
            Assert.Equal("💀 Gracz przegrywa!", panel.ResultOverlay_text.Text);
        });
    }

    [Fact]
    public void Gracz_Remisuje_Ustawia_Werdykt()
    {
        AvaloniaTestSetup.RunOnUiThread(() =>
        {
            var panel = new PanelWindow("gracz");

            panel.gracz_remisuje();

            Assert.Equal("Remis!", panel.Verdict_box.Text);
            Assert.True(panel.ResultOverlay.IsVisible);
            Assert.Equal("🤝 Remis!", panel.ResultOverlay_text.Text);
        });
    }

    private static List<Card> UtworzTalieDlaSummy(int docelowaSuma)
    {
        if (docelowaSuma <= 10)
        {
            return UtworzTalie(((Rank)docelowaSuma, Suit.Hearts));
        }

        if (docelowaSuma == 20)
        {
            return UtworzTalie((Rank.King, Suit.Hearts), (Rank.Queen, Suit.Clubs));
        }

        if (docelowaSuma == 22)
        {
            return UtworzTalie((Rank.King, Suit.Hearts), (Rank.Queen, Suit.Clubs), (Rank.Two, Suit.Spades));
        }

        if (docelowaSuma == 18)
        {
            return UtworzTalie((Rank.King, Suit.Hearts), (Rank.Eight, Suit.Clubs));
        }

        throw new ArgumentOutOfRangeException(nameof(docelowaSuma), docelowaSuma, "Nieobsługiwana suma testowa.");
    }
}
