using MojeLogowanieGUI.Models;

namespace testy_jednostkowe;

public class CardTests
{
    [Fact]
    public void Konstruktor_Ustawia_Rank_I_Suit()
    {
        var karta = new Card(Rank.Five, Suit.Hearts);

        Assert.Equal(Rank.Five, karta.Rank);
        Assert.Equal(Suit.Hearts, karta.Suit);
    }

    [Theory]
    [InlineData(Rank.Two, 2)]
    [InlineData(Rank.Five, 5)]
    [InlineData(Rank.Ten, 10)]
    public void GetBlackjackValue_Zwraca_Wartosc_Nominalna_Dla_Kart_2_10(Rank rank, int oczekiwanaWartosc)
    {
        var karta = new Card(rank, Suit.Clubs);

        Assert.Equal(oczekiwanaWartosc, karta.GetBlackjackValue());
    }

    [Theory]
    [InlineData(Rank.Jack)]
    [InlineData(Rank.Queen)]
    [InlineData(Rank.King)]
    public void GetBlackjackValue_Zwraca_10_Dla_Figur(Rank rank)
    {
        var karta = new Card(rank, Suit.Spades);

        Assert.Equal(10, karta.GetBlackjackValue());
    }

    [Fact]
    public void GetBlackjackValue_Zwraca_11_Dla_As()
    {
        var karta = new Card(Rank.Ace, Suit.Diamonds);

        Assert.Equal(11, karta.GetBlackjackValue());
    }

    [Theory]
    [InlineData(Rank.Jack, Suit.Hearts, "J♥")]
    [InlineData(Rank.Queen, Suit.Diamonds, "Q♦")]
    [InlineData(Rank.King, Suit.Clubs, "K♣")]
    [InlineData(Rank.Ace, Suit.Spades, "A♠")]
    [InlineData(Rank.Ten, Suit.Hearts, "10♥")]
    [InlineData(Rank.Seven, Suit.Clubs, "7♣")]
    public void ToString_Zwraca_Poprawny_Format(Rank rank, Suit suit, string oczekiwany)
    {
        var karta = new Card(rank, suit);

        Assert.Equal(oczekiwany, karta.ToString());
    }
}
