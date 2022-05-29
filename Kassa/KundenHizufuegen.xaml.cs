using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für KundenHizufuegen.xaml
    /// </summary>
    public partial class KundenHizufuegen : Window
    {
        public KundenHizufuegen()
        {
            InitializeComponent();
        }

        private void tbvorname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbvorname.Text == "Vorname")
            {
                tbvorname.Text = "";
                tbvorname.Foreground = Brushes.Black;
                textVorname.Text = "Vorname";
            }
        }

        private void tbvorname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbvorname.Text))
            {
                tbvorname.Text = "Vorname";
                tbvorname.Foreground = Brushes.LightGray;
                textVorname.Text = "";
            }
        }

        private void tbemail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbemail.Text == "Email")
            {
                tbemail.Text = "";
                tbemail.Foreground = Brushes.Black;
                textEmail.Text = "Email";
            }
        }

        private void tbemail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbemail.Text))
            {
                tbemail.Text = "Email";
                tbemail.Foreground = Brushes.LightGray;
                textEmail.Text = "";
            }
        }

        private void tbnachname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbnachname.Text == "Nachname")
            {
                tbnachname.Text = "";
                tbnachname.Foreground = Brushes.Black;
                textnachname.Text = "Nachname";
            }
        }

        private void tbnachname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbnachname.Text))
            {
                tbnachname.Text = "Nachname";
                tbnachname.Foreground = Brushes.LightGray;
                textnachname.Text = "";
            }
        }

        private void tbtelefone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbtelefone.Text == "Telefone")
            {
                tbtelefone.Text = "";
                tbtelefone.Foreground = Brushes.Black;
                texttelephone.Text = "Telefone";
            }
        }

        private void tbtelefone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbtelefone.Text))
            {
                tbtelefone.Text = "Telefone";
                tbtelefone.Foreground = Brushes.LightGray;
                texttelephone.Text = "";
            }
        }

        private void kundenadd_Click(object sender, RoutedEventArgs e)
        {
            Regex regemail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Regex regtel = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            Match match;
            string query;
            if (tbvorname.Text != " " && tbvorname.Text != "Vorname" && tbvorname.Text != null && tbvorname.Text.Length <= 50)
            {
                if (tbnachname.Text != " " && tbnachname.Text != "Nachname" && tbnachname != null && tbnachname.Text.Length <= 50)
                {
                    match = regemail.Match(tbemail.Text);
                    if (tbemail.Text != " " && tbemail.Text != "Email" && tbemail.Text != null && match.Success && tbemail.Text.Length <= 50)
                    {
                        match = regtel.Match(tbtelefone.Text);
                        if (tbtelefone.Text != " " && tbtelefone.Text != "Telefone" && tbtelefone.Text != null && match.Success && tbtelefone.Text.Length <= 50)
                        {
                            query = $"EXEC KundenAdd '{tbvorname.Text}', '{tbnachname.Text}', 0, '{tbemail.Text}', '{tbtelefone.Text}'";
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Data(out string[] output, query);
                            mainWindow.LesenKunden();
                            this.Close();
                        }
                        else
                        {
                            errorms.Text = "Die Telefonnummer darf nicht null sein oder sie muss diesen muster stimmen: xxx xxxxxxxxx";
                            tbtelefone.Focus();
                        }
                    }
                    else
                    {
                        errorms.Text = "Die Email ist null oder die Email ist nicht richtig geschrieben!";
                        tbemail.Focus();
                    }
                }
                else
                {
                    errorms.Text = "Der Nachname darf nicht null sein und darf maximal 50 zeichen lang sein!";
                    tbnachname.Focus();
                }
            }
            else
            {
                errorms.Text = "Der Vorname darf nicht null sein und darf maximal 50 zeichen lang sein!";
                tbvorname.Focus();
            }
        }

        private void abbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
