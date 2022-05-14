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
    /// Interaktionslogik für ProdukteAdd.xaml
    /// </summary>
    public partial class ProdukteAdd : Window
    {
        public ProdukteAdd()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query;
            string produktName = name.Text;
            string produktPreis = preis.Text;
            string produktlager = lager.Text;
            double doubleproduktPreis;
            int intproduktlager;
            MainWindow mainWindow = new MainWindow();
            if (double.TryParse(produktPreis, out doubleproduktPreis))
            {
                if (int.TryParse(produktlager, out intproduktlager))
                {

                    if (produktName.Length <= 20 && produktName != "Name" && produktName.Length >=2)
                    {
                        if (doubleproduktPreis != 0)
                        {
                            query = $"EXEC AddP '{produktName}', {produktPreis}";
                            mainWindow.Datenbank(out string[] output, query);

                            if (int.TryParse(output[0], out int zahl) && zahl == 1)
                            {
                                MessageBox.Show("Das Produkt exestiert bereits!");
                            }
                            else
                            {
                                query = $"SELECT ID FROM Produkte WHERE Name = '{produktName}'";
                                mainWindow.Datenbank(out output, query);
                                query = $"INSERT INTO Lager VALUES ({output[0]}, {intproduktlager}, null)";
                                mainWindow.Datenbank(out output, query);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Der Preis von dem Produkt darf nicht 0 sein!");
                            preis.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Der Name vom Produkt passt nicht!");
                        name.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Die Eingabe vom Lagerbestand war nicht richtig!");
                    lager.Focus();
                }
            }
            else
            {
                MessageBox.Show("Die Eingabe vom Preis war nicht richtig!");
                preis.Focus();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (name.Text == "Name")
            {
                name.Text = "";
                name.Foreground = Brushes.Black;
            }
        }

        private void name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                name.Text = "Name";
                name.Foreground = Brushes.LightGray;
            }
        }

        private void preis_GotFocus(object sender, RoutedEventArgs e)
        {
            if (preis.Text == "Preis")
            {
                preis.Text = "";
                preis.Foreground = Brushes.Black;
            }
        }

        private void preis_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(preis.Text))
            {
                preis.Text = "Preis";
                preis.Foreground = Brushes.LightGray;
            }
        }

        private void lager_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lager.Text == "Lager")
            {
                lager.Text = "";
                lager.Foreground = Brushes.Black;
            }
        }

        private void lager_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lager.Text))
            {
                lager.Text = "Lager";
                lager.Foreground = Brushes.LightGray;
            }
        }
    }
}
