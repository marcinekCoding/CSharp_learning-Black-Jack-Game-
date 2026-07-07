using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MojeLogowanieGUI.Models;
using MojeLogowanieGUI.Services;

namespace MojeLogowanieGUI.Views;

public partial class PanelWindow : Window
{
    private readonly BlackjackEngine _engine = new();
    private RoundState _roundState;

    /// <summary>Mini-gra clicker po lewej stronie panelu (osobne od salda blackjacka).</summary>
    public static int ClickerScore;

    public PanelWindow(string login)
    {
        InitializeComponent();
        UserLogin.Text = "Your username: " + login;
        SetWaitingForBetUi();
        RefreshBettingUi();
    }

    // ── Clicker (mini-gra) ──────────────────────────────────────────────

    public void dodajPunkty(object sender, RoutedEventArgs e)
    {
        ClickerScore++;
        Points_box.Text = ClickerScore.ToString();
    }

    // ── Przyciski gry ─────────────────────────────────────────────────

    public void Black_Jack(object sender, RoutedEventArgs e)
    {
        _engine.PrepareNewRound();
        ClearTableUi();
        ResultOverlay.IsVisible = false;
        Wallet.ClearBet();
        SetWaitingForBetUi();
        ShowBetValidationMessage("Postaw zaklad, aby rozpoczac runde.");
        PlayerCards_text.Text = "Czekamy na obstawienie";
    }

    public void PlaceBet_Click(object sender, RoutedEventArgs e)
    {
        int? betValue = TryReadBetFromInput();
        if (betValue is null)
        {
            ShowBetValidationMessage("Wpisz poprawna stawke (liczba calkowita > 0).");
            return;
        }

        if (!Wallet.TryPlaceBet(betValue.Value))
        {
            ShowBetValidationMessage("Zaklad odrzucony. Sprawdz saldo i sprobuj ponownie.");
            SetWaitingForBetUi();
            RefreshBettingUi();
            return;
        }

        SetPlayingUi();
        RefreshBettingUi();
        ShowBetValidationMessage("Zaklad przyjety. Rozpoczynam gre...");
        StartRoundAfterBet();
    }

    public void DobierzKarte_Click(object sender, RoutedEventArgs e)
    {
        if (_roundState != RoundState.Playing)
        {
            Verdict_box.Text = "Najpierw postaw zaklad!";
            return;
        }

        if (_engine.PlayerHand.Count == 0)
        {
            Verdict_box.Text = "Rozpocznij gre!";
            return;
        }

        _engine.HitPlayer();
        RefreshGameUi();

        if (!_engine.CanPlayerContinue())
        {
            ShowPlayerLoses();
            FinishRound();
        }
    }

    public async void NieDobieraj(object sender, RoutedEventArgs e)
    {
        if (_roundState != RoundState.Playing)
            return;

        int dealerSum = BlackjackEngine.CalculateHandValue(_engine.DealerHand);
        if (dealerSum == 0)
            return;

        while (dealerSum <= 16)
        {
            _engine.HitDealer();
            dealerSum = BlackjackEngine.CalculateHandValue(_engine.DealerHand);
            RefreshGameUi();
            await System.Threading.Tasks.Task.Delay(4000);
        }

        ResolveRound(_engine.CheckWinner());
    }

    public void KolejnaGra_Click(object sender, RoutedEventArgs e)
    {
        ResultOverlay.IsVisible = false;
        ClearBetInput();
        Black_Jack(this, new RoutedEventArgs());
    }

    public void Wyjdz_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    // ── Flow rundy ────────────────────────────────────────────────────

    private void StartRoundAfterBet()
    {
        _engine.DealInitialCards();
        RefreshGameUi();

        if (BlackjackEngine.IsBlackjack(_engine.PlayerHand))
        {
            Wallet.ResolveBlackjack();
            ShowPlayerWins(isBlackjack: true);
            FinishRound();
        }
    }

    private void ResolveRound(GameResult result)
    {
        switch (result)
        {
            case GameResult.PlayerWin:
                Wallet.ResolveWin();
                ShowPlayerWins();
                break;
            case GameResult.PlayerLose:
                ShowPlayerLoses();
                break;
            case GameResult.Draw:
                Wallet.ResolveDraw();
                ShowDraw();
                break;
            default:
                Verdict_box.Text = "Nieoczekiwany wynik";
                break;
        }

        FinishRound();
    }

    private void FinishRound()
    {
        SetRoundFinishedUi();
        RefreshBettingUi();
    }

    // ── Komunikaty wyniku ─────────────────────────────────────────────

    private void ShowPlayerWins(bool isBlackjack = false)
    {
        Verdict_box.Text = isBlackjack ? "Blackjack!" : "Gracz wygrywa";
        string overlayText = isBlackjack ? "🃏 Blackjack!" : "🎉 Gracz wygrywa!";
        ShowResultOverlay(overlayText, "#55efc4", Wallet.CurrentBet);
    }

    private void ShowPlayerLoses()
    {
        Verdict_box.Text = "Gracz przegrywa";
        ShowResultOverlay("💀 Gracz przegrywa!", "#e17055", -Wallet.CurrentBet);
    }

    private void ShowDraw()
    {
        Verdict_box.Text = "Remis!";
        ShowResultOverlay("🤝 Remis!", "#fdcb6e", 0);
    }

    private void ShowResultOverlay(string resultText, string color, int moneyDelta)
    {
        ResultOverlay_text.Text = resultText;
        ResultOverlay_text.Foreground = Avalonia.Media.Brush.Parse(color);
        ResultOverlay_score.Text =
            $"Gracz: {BlackjackEngine.CalculateHandValue(_engine.PlayerHand)} pts  |  " +
            $"Krupier: {BlackjackEngine.CalculateHandValue(_engine.DealerHand)} pts";
        ShowRoundMoneyResult(moneyDelta);
        ResultOverlay.IsVisible = true;
    }

    // ── UI helpers ────────────────────────────────────────────────────

    private void RefreshGameUi()
    {
        int playerSum = BlackjackEngine.CalculateHandValue(_engine.PlayerHand);
        int dealerSum = BlackjackEngine.CalculateHandValue(_engine.DealerHand);

        PlayerPoints_box.Text = playerSum.ToString();
        KrupierPoints_box.Text = dealerSum.ToString();
        PlayerCards_text.Text = string.Join("  ", _engine.PlayerHand);
        DealerCards_text.Text = string.Join("  ", _engine.DealerHand);

        if (_engine.LastDrawnCard is not null)
        {
            LastCard_text.Text = _engine.LastDrawnCard.ToString();
        }
    }

    private void ClearTableUi()
    {
        Verdict_box.Text = "";
        PlayerCards_text.Text = "";
        DealerCards_text.Text = "";
        PlayerPoints_box.Text = "";
        KrupierPoints_box.Text = "";
        LastCard_text.Text = "🂠";
    }

    private void RefreshBettingUi()
    {
        Balance_box.Text = Wallet.Balance.ToString("0");
    }

    private void SetWaitingForBetUi()
    {
        _roundState = RoundState.WaitingForBet;
        hit_btn.IsEnabled = false;
        stop_btn.IsEnabled = false;
        PlaceBet_btn.IsEnabled = true;
    }

    private void SetPlayingUi()
    {
        _roundState = RoundState.Playing;
        hit_btn.IsEnabled = true;
        stop_btn.IsEnabled = true;
        PlaceBet_btn.IsEnabled = false;
    }

    private void SetRoundFinishedUi()
    {
        _roundState = RoundState.Finished;
        hit_btn.IsEnabled = false;
        stop_btn.IsEnabled = false;
        PlaceBet_btn.IsEnabled = false;
    }

    private void ShowBetValidationMessage(string message)
    {
        BetStatus_text.Text = message;
    }

    private int? TryReadBetFromInput()
    {
        if (!int.TryParse(BetInput_box.Text, out int bet) || bet <= 0)
            return null;

        return bet;
    }

    private void ShowRoundMoneyResult(int delta)
    {
        ResultOverlay_money.Text = delta switch
        {
            > 0 => $"+{delta} pkt",
            < 0 => $"{delta} pkt",
            _ => "0 pkt"
        };
    }

    private void ClearBetInput()
    {
        BetInput_box.Text = "";
    }
}
