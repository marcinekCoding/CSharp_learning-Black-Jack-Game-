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
    public static int idx = 3;
    private List<int> talia = new List<int>();
    private List<int> gracz = new List<int>();
    private List<int> krupier = new List<int>();

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

    public void tasuj_karty(List<int> talia)
    {
        Random random = new Random();
        int list_size = talia.Count;

        for(int i=0;i < list_size*3;i++)
        {
        int idx1 = random.Next(list_size);        
        int idx2 = random.Next(list_size);        

        // 3. Klasyczny mechanizm zamiany (swap) elementów miejscami przy użyciu zmiennej tymczasowej
        int temp = talia[idx1];
        talia[idx1] = talia[idx2];
        talia[idx2] = temp;   
        }
       
    }

    public int licz_punkty(List<int> lista)
    {
        int suma = 0;

        foreach (var karta in lista)
        {
            suma+=karta;
        }
        return suma;
    }
    public int check_winner(List<int> gracz, List<int> krupier)
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
        return 3;
    }

    public bool is_21(List<int> gracz)
    {
        int sum_gracz = 0;
        foreach(int i in gracz)
        {
            sum_gracz+=i;
        }
        if(sum_gracz==21) return true;
        return false;
    }

    public void dobierz_karte(List<int>gracz,List<int> talia)
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

    }


    public void wstep_bj(List<int>gracz,List<int> talia,List<int> krup)
    {
        krup.Add(talia[0]);
        gracz.Add(talia[1]);
        gracz.Add(talia[2]);

    }

    public void gracz_wygrywa()
    {
        Verdict_box.Text = "Gracz wygrywa";
    }
    public void gracz_przegrywa()
    {
        Verdict_box.Text = "Gracz przegrywa";
    }
    public void gracz_remisuje()
    {
        Verdict_box.Text = "Remis!";
    }


    public void Black_Jack(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        talia.Clear();
        gracz.Clear();
        krupier.Clear();
        idx = 3;
        Verdict_box.Text = "";

        for (int i = 1; i < 15; i++) // Zaczynamy od 1, kończymy na 15
        {
            talia.Add(i);
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
    public bool gra_dalej(List<int> gracz)
    {
        int sum_gracz = 0;

        foreach (var karta in gracz)
        {
            sum_gracz+=karta;
        }
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
    public void NieDobieraj(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //teraz krupier zaczyna dobierac
        int sum_krupier = licz_punkty(krupier);
        if(sum_krupier==0) return;
        while(sum_krupier <= 16)
        {
            dobierz_karte(krupier,talia);
            sum_krupier = licz_punkty(krupier);
            show_result();
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