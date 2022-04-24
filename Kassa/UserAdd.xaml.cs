using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaktionslogik für UserAdd.xaml
    /// </summary>
    public partial class UserAdd : Window
    {
        public UserAdd(string user)
        {
            InitializeComponent();
            RechteDB(user);
        }
        private void RechteDB(string user)
        {
            List<string> rechte = new List<string>();
            string query = "SELECT Rechte FROM Rechte";
            MainWindow mainWindow = new MainWindow();
            mainWindow.Datenbank(out string[] output, query);
            for (int i = 0; i < output.Length-1; i++)
            {
                rechte.Add(output[i]);
            }
            string test = mainWindow.tbuid.Text;
            mainWindow.Rechte(ref rechte, user);
            rechteaswahl.ItemsSource = rechte;
        }

        private void useradd_Click(object sender, RoutedEventArgs e)
        {
            string user = UserID.Text;
            int userid;
            string vorname = firstname.Text;
            string nachname = lastname.Text;
            string recht = rechteaswahl.Text;
            DateTime date = DateTime.Now;
            string datetime;
            string passwort = "Firma123";
            string query;
            MainWindow mainWindow = new MainWindow();
            if (int.TryParse(user, out userid))
            {
                if (vorname != "")
                {
                    if (vorname.Length <= 50)
                    {
                        if (nachname != "")
                        {
                            if (nachname.Length <= 50)
                            {
                                query = $"SELECT RechteID FROM Rechte WHERE Rechte = '{recht}'";
                                mainWindow.Datenbank(out string[] output, query);
                                datetime = date.ToString("dd MMM yyyy");
                                query = $"INSERT KUser VALUES ({userid}, '{vorname}', '{nachname}', '{passwort}', '{datetime}', {output[0]})";
                                mainWindow.Datenbank(out output, query);
                                string test = mainWindow.tbuid.Text;
                                mainWindow.UserA();
                            }
                            else
                            {
                                MessageBox.Show("Der Nachname darf maximal 50 Zeichen lang sein!");
                                lastname.Focus();
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("Der Nachname ist nicht korekt!");
                            lastname.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Der Vorname darf maximal 50 Zeichen lang sein!");
                        firstname.Focus();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Der Vorname ist nicht korekt!");
                    firstname.Focus();
                }
            }
            else
            {
                MessageBox.Show("User ID ist nicht korekt!");
                UserID.Focus();
            }

        }
    }
}
