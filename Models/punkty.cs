using System;
namespace MojeLogowanieGUI.Models;

public static class Punkty{

public static double user_points {get;}= 100;
public static int bet {get;} = 0;


public static void init_points()
{
    user_points = 100;
}

public static void becik(int bet_value)
{
    if(bet_value>user_points)
        {
            return;
        }
    bet = bet_value;  
    user_points-=bet;  

}

public static void bet_won()
    {
        user_points+=2*bet;
    }

public static void bj_win()
    {
        user_points+=bet;
        user_points+=1.5*bet;
    }
}
