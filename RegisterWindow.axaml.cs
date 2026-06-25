using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MojeLogowanieGUI;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
    }

    public void RejestrujPrzycisk_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e){
        string haslo1 = PasswordBox.Text ?? "";
        string haslo2 = PasswordBoxV2.Text ?? "";
        string login_wpisany = LoginBox.Text ?? "";

        if(haslo1 == haslo2 && login_wpisany != "" && haslo1 != "")
        {
            PanelWindow ekranGlowny = new PanelWindow(login_wpisany);
            ekranGlowny.Show();
            this.Close();
        }else{
            MessageBlock.Text = "zle wprowadzilas haslo suko...";
        }
    }
}