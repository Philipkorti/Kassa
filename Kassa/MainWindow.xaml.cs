using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kassa;
using Kassa.Classes;
using System.Diagnostics;
using System.Windows.Threading;

namespace Kassa
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Products> produkte = new List<Products>();
        List<Products> kaufen = new List<Products>();
        List<Products> suche = new List<Products>();

        public MainWindow()
        {
            InitializeComponent();
            tbuid.Text = "User-ID: ";
        }
        private void anmelden_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            DateTime stdate = DateTime.Now;
            Anmeldung anmelden = new Anmeldung();
            string query = "SELECT p.ID, p.Name, p.Preis, l.Lager, l.Lieferung FROM Produkte p JOIN Lager l ON p.ID = l.ID";

            if (anmelden.ShowDialog() == true)
            {
                tbuid.Text += anmelden.Anmelden;

                Data(out string[] output, query);

                Input(output);
                CheckLieferDatum();
                dgProdukteliste.ItemsSource = produkte;
                produkteverwaltung.ItemsSource = produkte;
                entfernprodukte.IsEnabled = true;
                addProdukte.IsEnabled = true;
                lieferdatum.IsEnabled = true;

            }
        }

        private void abmelden_Click(object sender, RoutedEventArgs e)
        {

            if (tbuid.Text.Length >= 12)
            {
                tbuid.Text = "User-ID: ";
                dgProdukteliste.ItemsSource = null;
                Rechnung.ItemsSource = null;
                produkte.Clear();
                kaufen.Clear();
                MessageBox.Show("Du wurdest erfolgfreich abgemeldet!");
            }
            else
            {
                MessageBox.Show("Du bist nicht angemeldet!");
            }

        }

        public void Datenbank(out string[] output, string query)
        {
            Data(out output, query);


        }
        private void Data(out string[] output, string query)
        {

            List<string> input = new List<string>();
            string dbName = "Kassa";
            string connectionString = @"Data Source=MSI\SQLEXPRESS;" + "Trusted_Connection=yes;" + $"database={dbName};" + "connection timeout=10";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        command.Connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            WriteSQLData(reader, out input);
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder errorMessage = new StringBuilder();

                        for (int i = 0; i < ex.Errors.Count; i++)
                        {
                            errorMessage.AppendLine("Index #" + i);
                            errorMessage.AppendLine("Message: " + ex.Errors[i].Message);
                            errorMessage.AppendLine("LineNumber: " + ex.Errors[i].LineNumber);
                            errorMessage.AppendLine("Source: " + ex.Errors[i].Source);
                            errorMessage.AppendLine("Procedure: " + ex.Errors[i].Procedure);
                        }

                        MessageBox.Show(errorMessage.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception: " + ex.Message);
                    }
                }
            }
            output = new string[input.Count + 1];
            for (int i = 0; i < input.Count; i++)
            {
                output[i] = input[i];
            }
            output[output.Length - 1] = "";
        }
        private void WriteSQLData(SqlDataReader reader, out List<string> input)
        {
            input = new List<string>();
            try
            {

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        input.Add(reader.GetValue(i).ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        private void tbsuche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string test;
            string suchetest = tbsuche.Text;
            dgProdukteliste.ItemsSource = null;
            suche.Clear();
            if (suchetest == null || suchetest == "" || suchetest == " ")
            {
                dgProdukteliste.ItemsSource = produkte;
            }
            else
            {
                for (int i = 0; i < produkte.Count; i++)
                {
                    test = Convert.ToString(produkte[i].ID);
                    if (test.StartsWith(suchetest))
                    {
                        suche.Add(produkte[i]);
                    }
                }
                dgProdukteliste.ItemsSource = suche;
            }

        }

        private void Rechnung_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Mengen mengen = new Mengen();
            int pos = Rechnung.SelectedIndex;
            betrag.Text = Convert.ToString(Convert.ToDouble(betrag.Text) - kaufen[pos].Gesamtpreis);
            produkte[kaufen[pos].ID - 1].InStock += kaufen[pos].InStock;
            mengen.ShowDialog();
            produkte[kaufen[pos].ID - 1].InStock = produkte[kaufen[pos].ID - 1].InStock - Convert.ToInt32(mengen.anzahl);
            reloadgprodukte();
            kaufen[pos].InStock = Convert.ToInt32(mengen.anzahl);
            reloadrechnung();
            betrag.Text = Convert.ToString(Convert.ToDouble(betrag.Text) + kaufen[pos].Gesamtpreis);
        }

        private void dgProdukteliste_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int anzahl;
            int zahl;
            double gesamt = 0;
            double test = Convert.ToDouble(Rowrechnung.ActualHeight);
            double test1 = Convert.ToDouble(Rechnung.Height);

            try
            {
                gesamt = Convert.ToDouble(betrag.Text);
            }
            catch (Exception ex)
            {

            }
            Mengen mengen = new Mengen();
            mengen.ShowDialog();
            try
            {
                entfernen.Visibility = Visibility.Visible;
                abschliessen.Visibility = Visibility.Visible;
                gesamt += produkte[dgProdukteliste.SelectedIndex].Preis * Convert.ToDouble(mengen.anzahl);
                anzahl = produkte[dgProdukteliste.SelectedIndex].InStock;
                zahl = Convert.ToInt32(mengen.anzahl);
                anzahl = anzahl - zahl;
                produkte[dgProdukteliste.SelectedIndex].InStock = anzahl;
                kaufen.Add((Products)produkte[dgProdukteliste.SelectedIndex].Clone());
                reloadgprodukte();
                kaufen[kaufen.Count - 1].InStock = Convert.ToInt32(mengen.anzahl);
                betrag.Text = Convert.ToString(gesamt);

                if (Convert.ToInt32(Rowrechnung.ActualHeight) / 1.3 > Convert.ToInt32(Rechnung.Height))
                {
                    Rechnung.Height = Rechnung.Height + 30;
                }
                else
                {
                    kaufen.RemoveAt(0);
                }
                reloadrechnung();
            }
            catch
            {

            }
        }

        private void abschliessen_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Der Gesamt Betrag beträgt: " + betrag.Text + "€");
            kaufen.Clear();
            Rechnung.ItemsSource = "";
            betrag.Text = "";
            Rechnung.Height = 30;
        }


        private void entfernen_Click(object sender, RoutedEventArgs e)
        {
            int pos = Rechnung.SelectedIndex;
            betrag.Text = Convert.ToString(Convert.ToDouble(betrag.Text) - (kaufen[pos].Preis * kaufen[pos].InStock));
            produkte[kaufen[pos].ID - 1].InStock += kaufen[pos].InStock;
            reloadgprodukte();
            kaufen.RemoveAt(pos);
            reloadrechnung();
            Rechnung.Height = Rechnung.Height - 30;

            if (kaufen.Count == 0)
            {
                entfernen.Visibility = Visibility.Hidden;
            }
        }
        private void reloadgprodukte()
        {
            dgProdukteliste.ItemsSource = "";
            dgProdukteliste.ItemsSource = produkte;
        }
        private void reloadrechnung()
        {
            Rechnung.ItemsSource = "";
            Rechnung.ItemsSource = kaufen;
        }


        private void reloadprodukteverwaltung()
        {
            produkteverwaltung.ItemsSource = "";
            produkteverwaltung.ItemsSource = produkte;
        }

        private void addProdukte_Click(object sender, RoutedEventArgs e)
        {
            ProdukteAdd produkteAdd = new ProdukteAdd();
            produkteAdd.ShowDialog();
            string query = "SELECT p.ID, p.Name, p.Preis, l.Lager, l.Lieferung FROM Produkte p JOIN Lager l ON p.ID = l.ID";
            Data(out string[] output, query);
            Input(output);
            reloadprodukteverwaltung();
            

        }

        private void entfernprodukte_Click(object sender, RoutedEventArgs e)
        {
            int id = produkteverwaltung.SelectedIndex;
            string query = $"DELETE Lager WHERE ID =  {produkte[id].ID}";
            try
            {
                Data(out string[] output, query);
                query = $"DELETE Produkte WHERE ID = {produkte[id].ID}";
                Data(out output, query);
                produkte.RemoveAt(id);
                reloadprodukteverwaltung();
                reloadgprodukte();
                
            }
            catch
            {
                MessageBox.Show("Es war kein Produkt selektiert!");
            }

        }

        private void produkteverwaltung_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!e.Cancel)
            {
                int id = produkteverwaltung.SelectedIndex;
                string lager = (e.EditingElement as TextBox).Text;
                string query = $"UPDATE Lager SET Lager = {lager} WHERE ID = {produkte[id].ID}";
                Data(out string[] output, query);
            }
        }

        private void lieferdatum_Click(object sender, RoutedEventArgs e)
        {
            int id = produkteverwaltung.SelectedIndex;
            string query;
            LieferDate lieferDate = new LieferDate();
            if (id != -1)
            {
                lieferDate.ShowDialog();
                query = $"UPDATE Lager SET Lieferung = '{lieferDate.LieferDatum()}' WHERE ID = {produkte[id].ID}";
                Data(out string[] output, query);
                query = "SELECT p.ID, p.Name, p.Preis, l.Lager, l.Lieferung FROM Produkte p JOIN Lager l ON p.ID = l.ID";
                Data(out output, query);
                Input(output);
                reloadprodukteverwaltung();
            }
            else
            {
                MessageBox.Show("Sie müssen ein Element auswählen!");
            }
        }
        private void Input(string[] output)
        {
            DateTime date;
            string stringdate;
            produkte.Clear();
            for (int i = 0; i < output.Length - 1; i = i + 5)
            {
                produkte.Add(new Products { ID = Convert.ToInt32(output[i]), Name = output[i + 1], Preis = Convert.ToDouble(output[i + 2]), InStock = Convert.ToInt32(output[i + 3])});
                if (DateTime.TryParse(output[i + 4], out date))
                {
                    stringdate = date.ToString("dd MMM yyyy");
                    produkte[produkte.Count - 1].Lieferung = stringdate;

                }
                else
                {
                    produkte[produkte.Count - 1].Lieferung = "";

                }
            }
        }
        private void CheckLieferDatum()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-1);
            DateTime date;
            string query;
            for (int i = 0; i < produkte.Count; i++)
            {
                if (DateTime.TryParse(produkte[i].Lieferung, out date))
                {
                    if (date < dateTime)
                    {
                        produkte[i].Lieferung = "";
                        query = $"UPDATE Lager SET Lieferung = null WHERE ID = {produkte[i].ID}";
                        Data(out string[] output, query);
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string test;
            string suchetest = produkteverwaltungsuche.Text;
            produkteverwaltung.ItemsSource = null;
            suche.Clear();
            if (suchetest == null || suchetest == "" || suchetest == " ")
            {
                produkteverwaltung.ItemsSource = produkte;
            }
            else
            {
                for (int i = 0; i < produkte.Count; i++)
                {
                    test = Convert.ToString(produkte[i].ID);
                    if (test.StartsWith(suchetest))
                    {
                        suche.Add(produkte[i]);
                    }
                }
                produkteverwaltung.ItemsSource = suche;
            }
        }
    }
}
