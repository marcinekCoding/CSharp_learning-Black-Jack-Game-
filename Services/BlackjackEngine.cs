using System;
using System.Collections.Generic;
using MojeLogowanieGUI.Models;

namespace MojeLogowanieGUI.Services;

/// <summary>
/// Logika gry w blackjacka: talia, rozdawanie, liczenie kart i wynik rundy.
/// </summary>
public class BlackjackEngine
{
    private readonly List<Card> _deck = new();
    private readonly List<Card> _playerHand = new();
    private readonly List<Card> _dealerHand = new();
    private int _deckIndex;

    public IReadOnlyList<Card> PlayerHand => _playerHand;
    public IReadOnlyList<Card> DealerHand => _dealerHand;
    public int DeckIndex => _deckIndex;
    public Card? LastDrawnCard { get; private set; }

    public void PrepareNewRound()
    {
        ClearHands();
        BuildDeck();
        ShuffleDeck();
    }

    public void ClearHands()
    {
        _playerHand.Clear();
        _dealerHand.Clear();
        _deckIndex = 0;
        LastDrawnCard = null;
    }

    public void BuildDeck()
    {
        _deck.Clear();
        for (int rank = 2; rank < 15; rank++)
        {
            for (int suit = 0; suit < 4; suit++)
            {
                _deck.Add(new Card((Rank)rank, (Suit)suit));
            }
        }
    }

    public void ShuffleDeck()
    {
        var random = new Random();
        int count = _deck.Count;

        for (int i = 0; i < count * 10; i++)
        {
            int index1 = random.Next(count);
            int index2 = random.Next(count);

            Card temp = _deck[index1];
            _deck[index1] = _deck[index2];
            _deck[index2] = temp;
        }
    }

    public static int CalculateHandValue(IReadOnlyList<Card> hand)
    {
        int sum = 0;
        foreach (var card in hand)
        {
            sum += card.GetBlackjackValue();
        }
        return sum;
    }

    public static bool IsBlackjack(IReadOnlyList<Card> hand)
    {
        return hand.Count == 2 && CalculateHandValue(hand) == 21;
    }

    public bool CanPlayerContinue()
    {
        return CalculateHandValue(_playerHand) <= 21;
    }

    public static GameResult CheckWinner(IReadOnlyList<Card> playerHand, IReadOnlyList<Card> dealerHand)
    {
        int playerSum = CalculateHandValue(playerHand);
        int dealerSum = CalculateHandValue(dealerHand);

        if (playerSum > 21) return GameResult.PlayerLose;
        if (playerSum < dealerSum && dealerSum <= 21) return GameResult.PlayerLose;
        if (playerSum == dealerSum && playerSum <= 21) return GameResult.Draw;
        if (playerSum <= 21 && dealerSum < playerSum) return GameResult.PlayerWin;
        if (playerSum <= 21 && dealerSum > 21) return GameResult.PlayerWin;
        return GameResult.Undefined;
    }

    public GameResult CheckWinner() => CheckWinner(_playerHand, _dealerHand);

    public void DrawCardTo(IList<Card> hand)
    {
        if (_deckIndex >= _deck.Count)
            return;

        Card card = _deck[_deckIndex];
        hand.Add(card);
        LastDrawnCard = card;
        _deckIndex++;
    }

    public void DealInitialCards()
    {
        DrawCardTo(_dealerHand);
        DrawCardTo(_playerHand);
        DrawCardTo(_playerHand);
    }

    public void HitPlayer() => DrawCardTo(_playerHand);
    public void HitDealer() => DrawCardTo(_dealerHand);
}
