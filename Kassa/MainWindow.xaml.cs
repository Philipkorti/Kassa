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
        List<Products> kaufen = new List<Products>();
        List<User> userVerwaltung = new List<User>();
        List<Products> produkte = new List<Products>();
        public MainWindow()
        {
            InitializeComponent();
            tbuid.Text = "User-ID: ";
        }
        private void anmelden_Click(object sender, RoutedEventArgs e)
        {
            List<string> rechte = new List<string>();
            DateTime date;
            DateTime stdate = DateTime.Now;
            Anmeldung anmelden = new Anmeldung();
            string user;

            if (anmelden.ShowDialog() == true)
            {
                tbuid.Text += anmelden.Anmelden;
                Produktelesen();
                CheckLieferDatum();
                dgProdukteliste.ItemsSource = produkte;
                produkteverwaltung.ItemsSource = produkte;
                entfernprodukte.IsEnabled = true;
                addProdukte.IsEnabled = true;
                lieferdatum.IsEnabled = true;
                user = tbuid.Text;
                Rechte(ref rechte, user);
                daten.Visibility = Visibility.Visible;

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
                kassa.Visibility = Visibility.Visible;
                kassa.IsSelected = true;
                useranzeige.Visibility = Visibility.Collapsed;
                produkteanzeige.Visibility = Visibility.Collapsed;
                MessageBox.Show("Du wurdest erfolgfreich abgemeldet!");
                daten.Visibility = Visibility.Collapsed;
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
            string suchetb = tbsuche.Text;
            List<Products> suche = new List<Products>();
            dgProdukteliste.ItemsSource = null;
            produktesuche(ref suche, suchetb);
            if (suche.Count != 0)
            {
                dgProdukteliste.ItemsSource = suche;
            }
            


        }

        private void Rechnung_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Mengen mengen = new Mengen();
            int pos = Rechnung.SelectedIndex;
            betrag.Text = Convert.ToString(Convert.ToDouble(betrag.Text) - kaufen[pos].Gesamtpreis);
           
            mengen.ShowDialog();
            for (int i = 0; i < produkte.Count; i++)
            {
                if (kaufen[pos].ID == produkte[i].ID)
                {
                    produkte[i].InStock += kaufen[pos].InStock;
                    produkte[i].InStock = produkte[i].InStock - Convert.ToInt32(mengen.anzahl);
                }
               
            }
            
            kaufen[pos].InStock = Convert.ToInt32(mengen.anzahl);
            betrag.Text = Convert.ToString(Convert.ToDouble(betrag.Text) + kaufen[pos].Gesamtpreis);
            reloadrechnung();
            reloadgprodukte();
        }

        private void dgProdukteliste_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = dgProdukteliste.SelectedIndex;
            int anzahl;
            int zahl;
            double gesamt = 0;
            double test = Convert.ToDouble(Rowrechnung.ActualHeight);
            double test1 = Convert.ToDouble(Rechnung.Height);
            List<Products> suche = new List<Products>();
            string suchetb = tbsuche.Text;

            try
            {
                gesamt = Convert.ToDouble(betrag.Text);
            }
            catch
            {

            }
           
            Mengen mengen = new Mengen();
            mengen.ShowDialog();
            try
            {
                entfernen.Visibility = Visibility.Visible;
                abschliessen.Visibility = Visibility.Visible;
                gesamt += produkte[id].Preis * Convert.ToDouble(mengen.anzahl);
                anzahl = produkte[id].InStock;
                zahl = Convert.ToInt32(mengen.anzahl);
                anzahl = anzahl - zahl;
                produkte[id].InStock = anzahl;
                dgProdukteliste.ItemsSource = "";
                dgProdukteliste.ItemsSource = produkte;
                if (tbsuche.Text != "")
                {
                    produktesuche(ref suche, suchetb);
                    kaufen.Add((Products)suche[id].Clone());
                    tbsuche.Text = "";
                }
                else
                {
                    kaufen.Add((Products)produkte[id].Clone());
                   
                }
                
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void abschliessen_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Der Gesamt Betrag beträgt: " + betrag.Text + "€");
            kaufen.Clear();
            Rechnung.ItemsSource = "";
            betrag.Text = "";
            Rechnung.Height = 30;
            entfernen.Visibility = Visibility.Hidden;
            abschliessen.Visibility = Visibility.Hidden;
        }


        private void entfernen_Click(object sender, RoutedEventArgs e)
        {
            int pos = Rechnung.SelectedIndex;
            betrag.Text = Convert.ToString(Convert.ToDouble(betrag.Text) - (kaufen[pos].Preis * kaufen[pos].InStock));
            for (int i = 0; i < produkte.Count; i++)
            {
                if (kaufen[pos].ID == produkte[i].ID)
                {
                    produkte[i].InStock += kaufen[pos].InStock; 
                }
            }
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
            reloadprodukteverwaltung();
            reloadgprodukte();
            
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
                Produktelesen();
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
                Produktelesen();
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
                Produktelesen();
                reloadprodukteverwaltung();
            }
            else
            {
                MessageBox.Show("Sie müssen ein Element auswählen!");
            }
        }
        private void CheckLieferDatum()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-2);
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
            string suchetb = produkteverwaltungsuche.Text;
            List<Products> suche = new List<Products>();
            Produktelesen();
            produkteverwaltung.ItemsSource = null;
            produktesuche(ref suche, suchetb);
            if (suche.Count() != 0)
            {
                produkteverwaltung.ItemsSource = suche;
            }
            
        }

        private void Useradd_Click(object sender, RoutedEventArgs e)
        {
            string user = tbuid.Text;
            UserAdd userAdd = new UserAdd(user);
            userAdd.ShowDialog();
        }
        public void Rechte(ref List<string> rechte, string user)
        {
            Rechtelesen(user, out string[] output);
            List<User> userVerwaltung = new List<User>();
            switch (output[0])
            {
                case "Admin":
                    {
                        produkteanzeige.Visibility = Visibility.Visible;
                        useranzeige.Visibility = Visibility.Visible;
                        UserAnzeige(ref userVerwaltung);
                        
                        break;
                    }
                case "Produkte":
                    {
                        produkteanzeige.Visibility = Visibility.Visible;
                        produkteanzeige.IsSelected = true;
                        kassa.Visibility = Visibility.Collapsed;
                        break;
                    }
                case "User":
                    {
                        useranzeige.Visibility = Visibility.Visible;
                        useranzeige.IsSelected = true;
                        kassa.Visibility = Visibility.Collapsed;
                        UserAnzeige(ref userVerwaltung);
                        rechte.Remove("Admin");
                        break;
                    }
                case "Verwaltung":
                    {
                        
                        produkteanzeige.Visibility = Visibility.Visible;
                        useranzeige.Visibility = Visibility.Visible;
                        produkteanzeige.IsSelected = true;
                        kassa.Visibility = Visibility.Collapsed;
                        UserAnzeige(ref userVerwaltung);
                        rechte.Remove("Admin");
                        break;
                    }
                case "Kassa":
                    {
                        break;
                    }
            }
        }
        public void UserA()
        {
            List<User> userVerwaltung = new List<User>();
            UserAnzeige(ref userVerwaltung);
        }
        private void UserAnzeige(ref List<User> userVerwaltung)
        {
            string query = "SELECT ku.M_ID, ku.Vorname, ku.Nachname, ku.DatumEinstelung, r.Rechte  FROM KUser ku JOIN Rechte r ON r.RechteID = ku.IDRechte";
            Data(out string[] output, query);
            userVerwaltung = new List<User>();
            for (int i = 0; i < output.Length - 1; i= i+5)
            {
                userVerwaltung.Add(new User { ID = Convert.ToInt32(output[i]), Vorname = output[i+1], Nachname = output[i+2], Einstellungsdatum = Convert.ToDateTime(output[i+3]),  Rechte = output[i+4] });
            }

            userverwaltung.ItemsSource = "";
            userverwaltung.ItemsSource = userVerwaltung;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            string user = tbuid.Text;
            int id = userverwaltung.SelectedIndex;
            Rechtelesen(user, out string[] output);
            List<User> userVerwaltung = new List<User>();
            UserAnzeige(ref userVerwaltung);
            if (output[0] == "Admin")
            {
                string query = $"DELETE KUser WHERE M_ID = {userVerwaltung[id].ID}";
                Data(out output, query);
            }
            else
            {
                if (userVerwaltung[id].Rechte != "Admin")
                {
                    string query = $"DELETE KUser WHERE M_ID = {userVerwaltung[id].ID}";
                    Data(out output, query);
                }
                else
                {
                    MessageBox.Show("Du darfst kein Admin löschen!");
                }
            }
           
            UserAnzeige(ref userVerwaltung);
        }
        private void Rechtelesen(string user, out string[] output)
        {
            string[] userid = user.Split(' ');
            string query = $"SELECT r.Rechte FROM KUser ku JOIN Rechte r ON r.RechteID = ku.IDRechte WHERE ku.M_ID = {userid[1]}";
            Data(out output, query);
        }
        private void Produktelesen()
        {
            produkte = new List<Products>();
            DateTime date;
            string stringdate;
            string query = "SELECT p.ID, p.Name, p.Preis, l.Lager, l.Lieferung FROM Produkte p JOIN Lager l ON p.ID = l.ID";
            Data(out string[] output, query);

            produkte.Clear();
            for (int i = 0; i < output.Length - 1; i = i + 5)
            {
                produkte.Add(new Products { ID = Convert.ToInt32(output[i]), Name = output[i + 1], Preis = Convert.ToDouble(output[i + 2]), InStock = Convert.ToInt32(output[i + 3]) });
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
        private void produktesuche(ref List<Products> suche, string suchetb)
        {
            string test;
            List<Products> produkte = new List<Products>();
            suche = new List<Products>();
            suche.Clear();
            if (suchetb == null || suchetb == "" || suchetb == " ")
            {
                dgProdukteliste.ItemsSource = "";
                dgProdukteliste.ItemsSource = produkte;
                produkteverwaltung.ItemsSource = "";
                produkteverwaltung.ItemsSource = produkte;
            }
            else
            {
                for (int i = 0; i < produkte.Count; i++)
                {
                    test = Convert.ToString(produkte[i].ID);
                    if (test.StartsWith(suchetb))
                    {
                        suche.Add(produkte[i]);
                    }
                }
                
            }
        }
        private void sucheuser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string suchetb = sucheuser.Text;
            string id;
            List<User> usersuche = new List<User>();
            List<User> userVerwaltung = new List<User>();
            UserAnzeige(ref userVerwaltung);
            if (suchetb == null || suchetb == "" || suchetb == " ")
            {
                userverwaltung.ItemsSource = "";
                userverwaltung.ItemsSource = userVerwaltung;
            }
            else
            {
                for (int i = 0; i < userVerwaltung.Count; i++)
                {
                    id = Convert.ToString(userVerwaltung[i].ID);
                    if (id.StartsWith(suchetb))
                    {
                        usersuche.Add(userVerwaltung[i]);
                    }
                }
                userverwaltung.ItemsSource = "";
                userverwaltung.ItemsSource = usersuche;
            }
        }

        private void psr_Click(object sender, RoutedEventArgs e)
        {
            int id = userverwaltung.SelectedIndex;
            string query;
            string user = tbuid.Text;
            List<User> userVerwaltung = new List<User>();
            UserAnzeige(ref userVerwaltung);
            Rechtelesen(user, out string[] output);
            if (id != -1)
            {
                if (output[0] == "Admin")
                {
                    query = $"UPDATE KUSER SET M_Pass = 'Firma123' WHERE M_ID = {userVerwaltung[id].ID}";
                    Data(out output, query);
                    MessageBox.Show($"Das Passwort wurde für {userVerwaltung[id].FullName} zurückgesetzt!");
                }
                else
                {
                    if (userVerwaltung[id].Rechte != "Admin")
                    {
                        query = $"UPDATE KUSER SET M_Pass = 'Firma123' WHERE M_ID = {userVerwaltung[id].ID}";
                        Data(out output, query);
                        MessageBox.Show($"Das Passwort wurde für {userVerwaltung[id].FullName} zurückgesetzt!");
                    }
                    else
                    {
                        MessageBox.Show("Du darfst kein Admin das Paswort reseten!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Es wurde kein User ausgewählt!");
            }
        }
        private void daten_Click(object sender, RoutedEventArgs e)
        {
            string stringid = tbuid.Text;
            string[] id = stringid.Split(new char[] { ' ' });
            int intid = Convert.ToInt32(id[1]);
            string query;
            Datenändern datenändern = new Datenändern(intid);
            datenändern.ShowDialog();
            datenändern.datenaendernreturn(out string[] edit);
            query = $"UPDATE KUser SET Vorname = '{edit[0]}' WHERE M_ID = {intid}";
            Data(out string[] output, query);
            query = $"UPDATE KUser SET Nachname = '{edit[1]}' WHERE M_ID = {intid}";
            Data(out output, query);
            query = $"UPDATE KUser SET M_Pass = '{edit[2]}' WHERE M_ID = {intid}";
            Data(out output, query);

        }
    }
}
