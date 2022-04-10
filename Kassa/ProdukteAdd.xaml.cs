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
            string query = "SELECT Name FROM Produkte";
            string produktName = name.Text;
            string produktPreis = preis.Text;
            string produktlager = lager.Text;
            double doubleproduktPreis;
            int intproduktlager;
            bool check = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Datenbank(out string[] output, query);
            foreach (string item in output)
            {
                if (item == produktName)
                {
                    check = false;
                }
            }
            if (check)
            {
                if (double.TryParse(produktPreis, out doubleproduktPreis))
                {
                    if (int.TryParse(produktlager, out intproduktlager))
                    {
                        query = $"INSERT INTO Produkte VALUES ('{produktName}', {produktPreis})";

                        if (produktName.Length <= 20)
                        {
                            if (doubleproduktPreis != 0)
                            {
                                mainWindow.Datenbank(out output, query);
                                query = $"SELECT ID FROM Produkte WHERE Name = '{produktName}'";
                                mainWindow.Datenbank(out output, query);
                                query = $"INSERT INTO Lager VALUES ({output[0]}, {intproduktlager}, null)";
                                mainWindow.Datenbank(out output, query);
                            }
                            else
                            {
                                MessageBox.Show("Der Preis von dem Produkt darf nicht 0 sein!");
                                preis.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Der Name von dem produkt ist zu lange!");
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
            else
            {
                MessageBox.Show("Dieses Produkt existiert bereits!");
            }
           
           
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
