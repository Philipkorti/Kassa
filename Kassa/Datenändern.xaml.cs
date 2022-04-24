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
    /// Interaktionslogik für Datenändern.xaml
    /// </summary>
    public partial class Datenändern : Window
    {
        public Datenändern(int intid)
        {
            InitializeComponent();
            string query = "SELECT Vorname, Nachname, M_Pass FROM KUser";
            MainWindow mainWindow = new MainWindow();
            mainWindow.Datenbank(out string[] output, query);
            vorname.Text = output[0];
            nachname.Text = output[1];
            passwort.Text = output[2];
        }

        private void vornameändern_Click(object sender, RoutedEventArgs e)
        {
            tbvorname.Text = vorname.Text;
            tbvorname.Visibility = Visibility.Visible;
            vorname.Visibility = Visibility.Collapsed;
            tbvorname.Focus();
        }

        private void nachnameändern_Click(object sender, RoutedEventArgs e)
        {
            tbnachname.Text = nachname.Text;
            tbnachname.Visibility = Visibility.Visible;
            nachname.Visibility = Visibility.Collapsed;
            tbnachname.Focus();
        }

        private void passwortändern_Click(object sender, RoutedEventArgs e)
        {
            tbpasswort.Text = passwort.Text;
            tbpasswort.Visibility = Visibility.Visible;
            passwort.Visibility = Visibility.Collapsed;
            tbpasswort.Focus();
        }

        private void tbvorname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tbvorname.Visibility = Visibility.Collapsed;
                vorname.Visibility = Visibility.Visible;
                vorname.Text = tbvorname.Text;
            }
        }

        private void tbnachname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tbnachname.Visibility = Visibility.Collapsed;
                nachname.Visibility = Visibility.Visible;
                nachname.Text = tbnachname.Text;
            }
        }

        private void tbpasswort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                tbpasswort.Visibility = Visibility.Collapsed;
                passwort.Visibility = Visibility.Visible;
                passwort.Text = tbpasswort.Text;
            }
        }
        public void datenaendernreturn(out string[] edit)
        {
            edit = new string[3];

            edit[0] = vorname.Text;
            edit[1] = nachname.Text;
            edit[2] = passwort.Text;

        }
    }
}
