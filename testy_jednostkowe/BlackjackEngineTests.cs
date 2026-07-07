using MojeLogowanieGUI.Models;
using MojeLogowanieGUI.Services;

namespace testy_jednostkowe;

public class BlackjackEngineTests
{
    private static List<Card> CreateHand(params (Rank rank, Suit suit)[] cards)
    {
        return cards.Select(c => new Card(c.rank, c.suit)).ToList();
    }

    [Fact]
    public void CalculateHandValue_Pusta_Lista_Zwraca_Zero()
    {
        Assert.Equal(0, BlackjackEngine.CalculateHandValue(new List<Card>()));
    }

    [Fact]
    public void CalculateHandValue_Sumuje_Wartosci_Kart()
    {
        var hand = CreateHand(
            (Rank.King, Suit.Hearts),
            (Rank.Five, Suit.Clubs),
            (Rank.Ace, Suit.Spades));

        Assert.Equal(26, BlackjackEngine.CalculateHandValue(hand));
    }

    [Fact]
    public void IsBlackjack_Zwraca_Prawde_Gdy_Dwie_Karty_Daja_21()
    {
        var hand = CreateHand(
            (Rank.King, Suit.Hearts),
            (Rank.Ace, Suit.Clubs));

        Assert.True(BlackjackEngine.IsBlackjack(hand));
    }

    [Fact]
    public void IsBlackjack_Zwraca_Falsz_Gdy_Suma_Nie_Wynosi_21()
    {
        var hand = CreateHand(
            (Rank.Ten, Suit.Hearts),
            (Rank.Five, Suit.Clubs));

        Assert.False(BlackjackEngine.IsBlackjack(hand));
    }

    [Fact]
    public void CanPlayerContinue_Zwraca_Falsz_Gdy_Gracz_Przekroczyl_21()
    {
        var engine = new BlackjackEngine();
        engine.PrepareNewRound();

        for (int i = 0; i < 10; i++)
            engine.HitPlayer();

        Assert.False(engine.CanPlayerContinue());
    }

    [Theory]
    [InlineData(22, 20, GameResult.PlayerLose)]
    [InlineData(20, 22, GameResult.PlayerWin)]
    [InlineData(20, 18, GameResult.PlayerWin)]
    [InlineData(18, 20, GameResult.PlayerLose)]
    [InlineData(20, 20, GameResult.Draw)]
    public void CheckWinner_Zwraca_Oczekiwany_Wynik(int playerSum, int dealerSum, GameResult expected)
    {
        var player = CreateHandForSum(playerSum);
        var dealer = CreateHandForSum(dealerSum);

        Assert.Equal(expected, BlackjackEngine.CheckWinner(player, dealer));
    }

    [Fact]
    public void DealInitialCards_Rozdaje_2_Karty_Graczowi_I_1_Krupierowi()
    {
        var engine = new BlackjackEngine();
        engine.PrepareNewRound();
        engine.DealInitialCards();

        Assert.Equal(2, engine.PlayerHand.Count);
        Assert.Single(engine.DealerHand);
        Assert.Equal(3, engine.DeckIndex);
    }

    [Fact]
    public void PrepareNewRound_Tworzy_Pelna_Talie_52_Kart()
    {
        var engine = new BlackjackEngine();
        engine.PrepareNewRound();

        while (engine.DeckIndex < 52)
            engine.HitPlayer();

        Assert.Equal(52, engine.PlayerHand.Count);
    }

    private static List<Card> CreateHandForSum(int targetSum)
    {
        if (targetSum <= 10)
            return CreateHand(((Rank)targetSum, Suit.Hearts));

        return targetSum switch
        {
            18 => CreateHand((Rank.King, Suit.Hearts), (Rank.Eight, Suit.Clubs)),
            20 => CreateHand((Rank.King, Suit.Hearts), (Rank.Queen, Suit.Clubs)),
            22 => CreateHand((Rank.King, Suit.Hearts), (Rank.Queen, Suit.Clubs), (Rank.Two, Suit.Spades)),
            _ => throw new ArgumentOutOfRangeException(nameof(targetSum), targetSum, "Nieobslugiwana suma testowa.")
        };
    }
}
