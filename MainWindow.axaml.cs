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
        string login_wpisany = LoginBox.Text;
        string haslo_wpisane = PasswordBox.Text;

        if(login_wpisany == "marcinek" && haslo_wpisane=="1778")
        {
            MessageBlock.Text = "wpuszczam do systemu...";
             PanelWindow ekranGlowny = new PanelWindow(login_wpisany);
            ekranGlowny.Show();
            this.Close();
            
        }else{
            MessageBlock.Text = "wypierdalaj z systemu";
        }
    }

    public void ZarejestrujPrzycisk_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e){
        RegisterWindow rejestracja = new RegisterWindow();
        rejestracja.Show();
        this.Close();
    }
}
