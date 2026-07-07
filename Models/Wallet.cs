namespace MojeLogowanieGUI.Models;

/// <summary>
/// Saldo gracza i obsługa zakładów w blackjacku.
/// </summary>
public static class Wallet
{
    public static double Balance { get; private set; } = 100;
    public static int CurrentBet { get; private set; }

    public static void Reset(double startingBalance = 100)
    {
        Balance = startingBalance;
        CurrentBet = 0;
    }

    public static bool TryPlaceBet(int amount)
    {
        if (amount <= 0 || amount > Balance)
            return false;

        CurrentBet = amount;
        Balance -= amount;
        return true;
    }

    public static void ResolveWin()
    {
        Balance += 2 * CurrentBet;
    }

    public static void ResolveDraw()
    {
        Balance += CurrentBet;
    }

    public static void ResolveBlackjack()
    {
        Balance += CurrentBet + (int)(1.5 * CurrentBet);
    }

    public static void ClearBet()
    {
        CurrentBet = 0;
    }
}
