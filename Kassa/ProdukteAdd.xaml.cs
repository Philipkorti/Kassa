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
                            mainWindow.Data(out string[] output, query);

                            if (int.TryParse(output[0], out int zahl) && zahl == 1)
                            {
                                errorms.Text = "Das Produkt exestiert bereits!";
                            }
                            else
                            {
                                query = $"SELECT ID FROM Produkte WHERE Name = '{produktName}'";
                                mainWindow.Data(out output, query);
                                query = $"INSERT INTO Lager VALUES ({output[0]}, {intproduktlager}, null)";
                                mainWindow.Data(out output, query);
                                this.Close();
                            }

                        }
                        else
                        {
                            errorms.Text = "Der Preis von dem Produkt darf nicht 0 sein!";
                            preis.Focus();
                        }
                    }
                    else
                    {
                        errorms.Text = "Der Name vom Produkt passt nicht!";
                        name.Focus();
                    }
                }
                else
                {
                    errorms.Text = "Die Eingabe vom Lagerbestand war nicht richtig!";
                    lager.Focus();
                }
            }
            else
            {
                errorms.Text = "Die Eingabe vom Preis war nicht richtig!";
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
                tbname.Text = "Name";
            }
        }

        private void name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                name.Text = "Name";
                name.Foreground = Brushes.LightGray;
                tbname.Text = "";
            }
        }

        private void preis_GotFocus(object sender, RoutedEventArgs e)
        {
            if (preis.Text == "Preis")
            {
                preis.Text = "";
                preis.Foreground = Brushes.Black;
                tbpreis.Text = "Preis";
            }
        }

        private void preis_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(preis.Text))
            {
                preis.Text = "Preis";
                preis.Foreground = Brushes.LightGray;
                tbpreis.Text = "";
            }
        }

        private void lager_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lager.Text == "Lager")
            {
                lager.Text = "";
                lager.Foreground = Brushes.Black;
                tblager.Text = "Lager";
            }
        }

        private void lager_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lager.Text))
            {
                lager.Text = "Lager";
                lager.Foreground = Brushes.LightGray;
                tblager.Text = "";
            }
        }
    }
}
