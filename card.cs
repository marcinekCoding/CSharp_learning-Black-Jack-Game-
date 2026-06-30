using System;
namespace MojeLogowanieGUI;

public enum Suit
{
    Hearts,    // Kier ♥
    Diamonds,  // Karo ♦
    Clubs,     // Trefl ♣
    Spades     // Pik ♠
}
public enum Rank
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,   // J (Walet)
    Queen = 12,  // Q (Dama)
    King = 13,   // K (Król)
    Ace = 14     // A (As)
}

public class Card
{
    public Rank Rank { get; }
    public Suit Suit { get; }

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public int GetBlackjackValue()
    {
        if (Rank == Rank.Ace)
        {
            return 11;
        }
        if (Rank >= Rank.Jack)
        {
            return 10;
        }
        return (int)Rank;
    }

    public override string ToString()
    {
        string rankStr = Rank switch
        {
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            Rank.Ace => "A",
            _ => ((int)Rank).ToString()
        };

        string suitStr = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => Suit.ToString()
        };

        return $"{rankStr}{suitStr}";
    }
}
