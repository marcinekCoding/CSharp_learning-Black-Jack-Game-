using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MojeLogowanieGUI;

public partial class PanelWindow : Window
{
    public PanelWindow(string login)
    {
        InitializeComponent();
        UserLogin.Text = "Twoj login: "+ login;
    }
}