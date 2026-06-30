using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HarfBuzzSharp;

namespace MojeLogowanieGUI;

public partial class PanelWindow : Window
{
    public static int punkty = 0;
    public static int idx = 0;
    private List<Card> talia = new List<Card>();
    private List<Card> gracz = new List<Card>();
    private List<Card> krupier = new List<Card>();

    
    public PanelWindow(string login)
    {
        InitializeComponent();
        UserLogin.Text = "Your username: "+ login;
    }
    public void dodajPunkty(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        punkty++;
        Points_box.Text = punkty.ToString();
    }

    public void tasuj_karty(List<Card> talia)
    {
        Random random = new Random();
        int list_size = talia.Count;

        for(int i=0;i < list_size*10;i++)
        {
        int idx1 = random.Next(list_size);        
        int idx2 = random.Next(list_size);        

        Card temp = talia[idx1];
        talia[idx1] = talia[idx2];
        talia[idx2] = temp;   
        }
       
    }

    public int licz_punkty(List<Card> lista)
    {
        int suma = 0;

        foreach (var karta in lista)
        {
            suma+=karta.GetBlackjackValue();
        }
        return suma;
    }
    public int check_winner(List<Card> gracz, List<Card> krupier)
    {
        // checks if the player is winner
        // 0 - player loose
        // 1 - player win
        // 2 - draw
        // 3 - undefined behavioiur
        int sum_gracz = licz_punkty(gracz);
        int sum_krupier = licz_punkty(krupier);

        //sparwdzanie wyniku 
        if(sum_gracz > 21) return 0;
        if(sum_gracz<sum_krupier && sum_krupier<=21) return 0;
        

        if(sum_gracz == sum_krupier && sum_gracz<=21) return 2;

        if(sum_gracz<=21 && sum_krupier<sum_gracz) return 1;
        if(sum_gracz<=21 && sum_krupier>21) return 1;
        return 3;
    }

    public bool is_21(List<Card> gracz)
    { 
        int sum_gracz = licz_punkty(gracz);
        
        if(sum_gracz==21) return true;
        return false;
    }

    public void dobierz_karte(List<Card>gracz,List<Card> talia)
    {
        gracz.Add(talia[idx]);
        idx++;
    }

    public void show_result()
    {
        int sum_gracz = licz_punkty(gracz);
        int sum_krupier = licz_punkty(krupier);
        KrupierPoints_box.Text = sum_krupier.ToString();
        PlayerPoints_box.Text = sum_gracz.ToString();

         PlayerCards_text.Text = string.Join("  ", gracz);
         DealerCards_text.Text = string.Join("  ", krupier);
    // Wyświetl ostatnio dobraną kartę (z talii — ta spod indeksu idx-1)
    if (idx > 0 && idx <= talia.Count)
    {
        LastCard_text.Text = talia[idx - 1].ToString();
    }

    }


    public void wstep_bj(List<Card>gracz,List<Card> talia,List<Card> krupier)
    {
        dobierz_karte(krupier,talia);
        dobierz_karte(gracz,talia);
        dobierz_karte(gracz,talia);


    }

    public void gracz_wygrywa()
    {
        Verdict_box.Text = "Gracz wygrywa";
        ShowResultOverlay("🎉 Gracz wygrywa!", "#55efc4");
    }
    public void gracz_przegrywa()
    {
        Verdict_box.Text = "Gracz przegrywa";
        ShowResultOverlay("💀 Gracz przegrywa!", "#e17055");
    }
    public void gracz_remisuje()
    {
        Verdict_box.Text = "Remis!";
        ShowResultOverlay("🤝 Remis!", "#fdcb6e");
    }

    public void ShowResultOverlay(string resultText, string color)
    {
        ResultOverlay_text.Text = resultText;
        ResultOverlay_text.Foreground = Avalonia.Media.Brush.Parse(color);
        ResultOverlay_score.Text = $"Gracz: {licz_punkty(gracz)} pts  |  Krupier: {licz_punkty(krupier)} pts";
        ResultOverlay.IsVisible = true;
    }

    public void KolejnaGra_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ResultOverlay.IsVisible = false;
        Black_Jack(sender, e);
    }

    public void Wyjdz_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Environment.Exit(0);
    }


    public void Black_Jack(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        talia.Clear();
        gracz.Clear();
        krupier.Clear();
        idx = 0;
        Verdict_box.Text = "";
        PlayerCards_text.Text = "";
        DealerCards_text.Text = "";
        LastCard_text.Text = "🂠";
        ResultOverlay.IsVisible = false;

        for (int i = 2; i < 15; i++) // Zaczynamy od 1, kończymy na 15
        {
            for(int j=0;j<4;j++){
                talia.Add(new Card((Rank)i, (Suit)j));
            }
            
        }

        tasuj_karty(talia);
        //wstep do gry
        wstep_bj(gracz,talia,krupier);
        //checking if there is an blackjack
        show_result();
        if(is_21(gracz)){
            gracz_wygrywa();
            return;
        }
        
        
        

    

    }
    public bool gra_dalej(List<Card> gracz)
    {
        int sum_gracz = licz_punkty(gracz);
        if(sum_gracz>21) return false;
        return true;

    }
    public void DobierzKarte_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (gracz.Count == 0 || talia.Count == 0)
        {
            Verdict_box.Text = "Rozpocznij grę!";
            return;
        }

        dobierz_karte(gracz, talia);
        show_result();
        if (!gra_dalej(gracz))
        {
            gracz_przegrywa();
            return;
        }
        
    }
    public async void NieDobieraj(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //teraz krupier zaczyna dobierac
        int sum_krupier = licz_punkty(krupier);
        if(sum_krupier==0) return;
        while(sum_krupier <= 16)
        {
            dobierz_karte(krupier,talia);
            sum_krupier = licz_punkty(krupier);
            show_result();
            await System.Threading.Tasks.Task.Delay(4000);
        }
        int ver = check_winner(gracz,krupier);
        switch (ver)
        {
            case 0: 
                gracz_przegrywa();
                break;
            case 1:
                gracz_wygrywa();
                break;
            case 2: 
                gracz_remisuje();
                break;
       
            default:
            break;
        }

    }

    
    
}