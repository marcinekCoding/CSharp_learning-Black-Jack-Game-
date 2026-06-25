using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
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
            if (haslo1.Length < 5)
            {
            MessageBlock.Text = "Too short password";
            return;
                
            }
            if (!password_validation(haslo1))
            {
            MessageBlock.Text = "masz niespełniony jeden z dwóch warunków:\n * co najmniej jedna '@' \n * co najmniej dwie liczby";
            return;
            }
            PanelWindow ekranGlowny = new PanelWindow(login_wpisany);
            ekranGlowny.Show();
            this.Close();
        }else{
            MessageBlock.Text = "Passwords are not the same";
        }
    }
    public bool password_validation(string s)
    {
        int size = s.Length;

        if(size < 5 ) return false;

        int monkey_iterator = 0;
        int digit_iterator = 0 ;
        foreach(char c in s)
        {
            if(c == '@')
            {
                monkey_iterator++;
            }else if (char.IsDigit(c))
            {
                digit_iterator++;
            }
        }
        
        if(monkey_iterator < 1 || digit_iterator<2) return false;

        return true;


    }
}