using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kassa
{
    /// <summary>
    /// Interaktionslogik für Anmeldung.xaml
    /// </summary>
    public partial class Anmeldung : Window
    {
        public Anmeldung()
        {
            InitializeComponent();
            
            
        }

        private void btnAnmelden_Click(object sender, RoutedEventArgs e)
        {
            anmelden();
        }
        public int UserName()
        {
            string Text = tbuser.Text;
            int UserN = 0;
            try
            {
                UserN = Convert.ToInt32(Text);

                return UserN;
            }catch(FormatException ex)
            {
                wronginput();
            }
            return UserN;
        }
        public bool psswort(out string passwort)
        {
            passwort = pbpaswort.Password;
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[az])(?=.*[AZ]).{8,12}$");
            Match match;
            match = regex.Match(passwort);
            return match.Success;
        }
        public void controle(ref Dictionary<int, string> User, int UsName, string passwort)
        {
           

            if (User.ContainsKey(UsName))
            {
                if (User[UsName] == passwort)
                {
                    this.DialogResult = true;
                    this.Close();


                }
                else
                {
                    wronginput();
                }
            }
            else
            {
                wronginput();
            }
            
        }
        public string Anmelden
        {
            get { return tbuser.Text; }
        }

        private void tbuser_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                pbpaswort.Focus();
            }
        }
        private void anmelden()
        {
            Dictionary<int, string> User = new Dictionary<int, string>();
            bool check = psswort(out string passwort);
            int UsName = UserName();
            string query = "EXEC Pro_User";

            if (UsName != 0)
            {
                if (check)
                {
                    wronginput();
                }
                else
                {
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Datenbank(out string[] output, query);
                    for (int i = 0; i < output.Length - 1; i = i + 2)
                    {
                        User.Add(Convert.ToInt32(output[i]), output[i + 1]);
                    }
                    controle(ref User, UsName, passwort);
                }
            }
            else
            {
                wronginput();
            }
        }

        private void pbpaswort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                anmelden();
            }
        }
        private void wronginput()
        {
            errorMs.Text = "Das Passwort oder der User Name ist falsch!";
            pbpaswort.Clear();
            tbuser.Clear();
            tbuser.Focus();
        }

        private void passwortseebutton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            passwortsee.Source = new BitmapImage(new Uri(@"Bilder/look_password.png", UriKind.Relative));
            pbpaswort.Password = showpasswort.Text;
            showpasswort.Visibility = Visibility.Collapsed;
            pbpaswort.Visibility = Visibility.Visible;
        }

        private void passwortseebutton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passwortsee.Source = new BitmapImage(new Uri(@"Bilder/password_see.png", UriKind.Relative));
            showpasswort.Text = pbpaswort.Password;
            showpasswort.Visibility = Visibility.Visible;
            pbpaswort.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
   
}
