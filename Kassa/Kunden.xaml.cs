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
using Kassa.Classes;

namespace Kassa
{
    /// <summary>
    /// Interaktionslogik für Kunden.xaml
    /// </summary>
    public partial class Kunden : Window
    {
        List<Kundenanzeige> kundenList = new List<Kundenanzeige>();
        double preis;
        public Kunden(int gesamtpreis)
        {
            InitializeComponent();
            preis = Convert.ToDouble(gesamtpreis);
            LesenKunden();
        }

        private void hinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            KundenHizufuegen kundenHizufuegen = new KundenHizufuegen();
            kundenHizufuegen.ShowDialog();
            LesenKunden();
            MainWindow main = new MainWindow();
            main.LesenKunden();
        }
        private void DgKunden_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            int id = DgKunden.SelectedIndex;
            int count;
            double rabat;
            string query;
            if(id != -1)
            {
                count = kundenList[id].Punkte;
                count += Convert.ToInt32(preis / 2);
                if (count >= 200)
                {
                    count -= 200;
                    rabat = (preis / 100) * 10;
                    preis = preis - rabat;
                    
                }
                query = $"UPDATE Kunden SET Punkte = {count} WHERE KundenID = {kundenList[id].KundenID}";
                mainWindow.Data(out string[] output, query);
                MessageBox.Show("Der Gesamt Betrag beträgt: " + preis + "€");
                this.Close();
            }
            
        }

        private void suche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = suche.Text;
            string[,] sucharray = new string[kundenList.Count, 3];
            List<Kundenanzeige> listsuche = new List<Kundenanzeige>();
            listsuche.Clear();
            string item;
            if (input != null && input != "" && input != " ")
            {
                for (int i = 0; i < sucharray.GetLength(0); i++)
                {
                    sucharray[i, 0] = Convert.ToString(kundenList[i].KundenID);
                    sucharray[i, 1] = kundenList[i].Vorname;
                    sucharray[i, 2] = kundenList[i].Nachname;
                }
                for (int i = 0; i < sucharray.GetLength(0); i++)
                {
                    for (int k = 0; k < sucharray.GetLength(1); k++)
                    {
                        item = sucharray[i, k];
                        if (item.StartsWith(input))
                        {
                            listsuche.Add(kundenList[i]);
                        }
                    }
                }
                DgKunden.ItemsSource = "";
                DgKunden.ItemsSource = listsuche;
            }
            else
            {
                DgKunden.ItemsSource = "";
                DgKunden.ItemsSource = kundenList;
            }
        }
        public int ReadDGID
        {
            get { return (int)DgKunden.SelectedIndex; }
        }
        private void LesenKunden()
        {
            MainWindow mainWindow = new MainWindow();
            kundenList.Clear();
            string query = "SELECT * FROM Kunden";
            mainWindow.Data(out string[] output, query);
            for (int i = 0; i < output.Length - 1; i = i + 6)
            {
                kundenList.Add(new Kundenanzeige { KundenID = Convert.ToInt32(output[i]), Vorname = output[i + 1], Nachname = output[i + 2], Punkte = Convert.ToInt32(output[i + 3]), Email = output[i + 4], Telefone = output[i + 5] });
            }
            DgKunden.ItemsSource = "";
            DgKunden.ItemsSource = kundenList;
            
        }
    }
}
