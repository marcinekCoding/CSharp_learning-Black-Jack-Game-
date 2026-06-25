using Avalonia.Controls;

namespace MojeLogowanieGUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ZalogujPrzycisk_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string login_wpisany = LoginBox.Text?.ToLower() ?? "";
        string haslo_wpisane = PasswordBox.Text ?? "";

        if(login_wpisany == "marcinek" && haslo_wpisane=="1778")
        {
            MessageBlock.Text = "Access granted...";
             PanelWindow ekranGlowny = new PanelWindow(login_wpisany);
            ekranGlowny.Show();
            this.Close();
            
        }else{
            MessageBlock.Text = "Get the fuck out";
        }
    }

    public void ZarejestrujPrzycisk_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e){
        RegisterWindow rejestracja = new RegisterWindow();
        rejestracja.Show();
        this.Close();
    }
}
