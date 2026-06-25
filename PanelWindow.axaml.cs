using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MojeLogowanieGUI;

public partial class PanelWindow : Window
{
    public static int punkty = 0;

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
}