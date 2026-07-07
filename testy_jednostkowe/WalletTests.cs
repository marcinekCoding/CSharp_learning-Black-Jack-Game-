using MojeLogowanieGUI.Models;

namespace testy_jednostkowe;

public class WalletTests
{
    public WalletTests()
    {
        Wallet.Reset();
    }

    [Fact]
    public void TryPlaceBet_Odrzuca_Stawke_Wieksza_Niz_Saldo()
    {
        Wallet.Reset(50);

        Assert.False(Wallet.TryPlaceBet(100));
        Assert.Equal(50, Wallet.Balance);
        Assert.Equal(0, Wallet.CurrentBet);
    }

    [Fact]
    public void TryPlaceBet_Akceptuje_Poprawna_Stawke()
    {
        Wallet.Reset(100);

        Assert.True(Wallet.TryPlaceBet(30));

        Assert.Equal(70, Wallet.Balance);
        Assert.Equal(30, Wallet.CurrentBet);
    }

    [Fact]
    public void ResolveWin_Dodaje_Double_Stawke()
    {
        Wallet.Reset(100);
        Wallet.TryPlaceBet(20);

        Wallet.ResolveWin();

        Assert.Equal(120, Wallet.Balance);
    }

    [Fact]
    public void ResolveDraw_Zwraca_Stawke()
    {
        Wallet.Reset(100);
        Wallet.TryPlaceBet(20);

        Wallet.ResolveDraw();

        Assert.Equal(100, Wallet.Balance);
    }

    [Fact]
    public void ResolveBlackjack_Dodaje_Wyplate_2_5x()
    {
        Wallet.Reset(100);
        Wallet.TryPlaceBet(20);

        Wallet.ResolveBlackjack();

        Assert.Equal(130, Wallet.Balance);
    }
}
