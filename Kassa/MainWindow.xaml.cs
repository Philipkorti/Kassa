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
        List<Products> produkte = new List<Products>();
        List<Products> produkteverwaltungl = new List<Products>();
        List<Products> dbkaufen = new List<Products>();
        List<User> userVerwaltung = new List<User>();
        List<Rechnung> rechnung = new List<Rechnung>();
        List<VerkaufProdukte> produkteVerkauf = new List<VerkaufProdukte>();
        List<Kundenanzeige> kundenanzeigel = new List<Kundenanzeige>();
        int userID;
        int betragpreis;
        public MainWindow()
        {
            InitializeComponent();
            tbuid.Text = "User-ID: ";
        }
        private void anmelden_Click(object sender, RoutedEventArgs e)
        {
            List<string> rechte = new List<string>();
            Anmeldung anmelden = new Anmeldung();
            string user;
            string query;

            if (anmelden.ShowDialog() == true)
            {
                tbuid.Text += anmelden.Anmelden;
                userID = Convert.ToInt32(anmelden.Anmelden);
                Produktelesen();
                Produkteverwaltunglesen();
                CheckLieferDatum();
                dgProdukteliste.ItemsSource = produkte;
                entfernprodukte.IsEnabled = true;
                addProdukte.IsEnabled = true;
                lieferdatum.IsEnabled = true;
                user = tbuid.Text;
                Rechte(ref rechte, user);
                query = "SELECT Rechte FROM Rechte";
                Data(out string[] output, query);
                foreach (string item in output)
                {
                    rechte.Add(item);
                }
                rechteanzeige.ItemsSource = rechte;
                daten.Visibility = Visibility.Visible;
                LesenRechnung();
                LesenKunden();
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
                produkteverwaltungl.Clear();
                kassa.Visibility = Visibility.Visible;
                kassa.IsSelected = true;
                useranzeige.Visibility = Visibility.Collapsed;
                produkteanzeige.Visibility = Visibility.Collapsed;
                tabkunde.Visibility = Visibility.Collapsed;
                tabrechnung.Visibility = Visibility.Collapsed;
                MessageBox.Show("Du wurdest erfolgfreich abgemeldet!");
                daten.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Du bist nicht angemeldet!");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <param name="query"></param>

        public void Data(out string[] output, string query)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="input"></param>
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
            catch
            {

            }

        }
        private void tbsuche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string suchetb = tbsuche.Text;
            List<Products> suche = new List<Products>();
            dgProdukteliste.ItemsSource = null;
            suche.Clear();

            if (suchetb != "" && suchetb != " " && suchetb != null && suchetb != "Suche")
            {
                Produktesuche(ref suche, suchetb, produkte);
                dgProdukteliste.ItemsSource = suche;
            }
            else
            {
                dgProdukteliste.ItemsSource = produkte;
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
                    Produktesuche(ref suche, suchetb, produkte);
                    kaufen.Add((Products)suche[id].Clone());
                    dbkaufen.Add((Products)suche[id].Clone());
                    tbsuche.Text = "";
                }
                else
                {
                    kaufen.Add((Products)produkte[id].Clone());
                    dbkaufen.Add((Products)produkte[id].Clone());
                }
                
                kaufen[kaufen.Count - 1].InStock = Convert.ToInt32(mengen.anzahl);
                dbkaufen[kaufen.Count - 1].InStock = Convert.ToInt32(mengen.anzahl);
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
            string query;
            int kundenid;
            int gesamtpreis;
            if (rechnung.Count != 0)
            {
                gesamtpreis = Convert.ToInt32(betrag.Text);
                betragpreis = gesamtpreis;
                Kunden kunden = new Kunden(gesamtpreis);
                kunden.ShowDialog();
                kaufen.Clear();
                kundenid = kunden.ReadDGID;
                query = $"EXEC PVerkauf {userID}, {kundenanzeigel[kundenid].KundenID},{dbkaufen.Count}, {gesamtpreis}";
                Data(out string[] output, query);
                for (int i = 0; i < dbkaufen.Count; i++)
                {
                    query = $"EXEC AddProdukteVerkauf @VerkaufID = 0, @ProduktID = {dbkaufen[i].ID}, @Anzahl = {dbkaufen[i].InStock}, @Geamtpreis = {dbkaufen[i].Preis}";
                    Data(out output, query);
                }
                for (int i = 0; i < dbkaufen.Count; i++)
                {
                    for (int k = 0; k < produkte.Count; k++)
                    {
                        if (dbkaufen[i].ID == produkte[k].ID)
                        {
                            query = $"UPDATE Lager SET Lager = {produkte[k].InStock} WHERE ID = {dbkaufen[i].ID}";
                            Data(out output, query);
                        }
                    }
                }
                dbkaufen.Clear();
                Produktelesen();
                Produkteverwaltunglesen();

                Rechnung.ItemsSource = "";
                betrag.Text = "";
                Rechnung.Height = 30;
                entfernen.Visibility = Visibility.Hidden;
                abschliessen.Visibility = Visibility.Hidden;
                LesenRechnung();
            }
           
        }


        private void entfernen_Click(object sender, RoutedEventArgs e)
        {
            int pos = Rechnung.SelectedIndex;
            if (pos != -1)
            {
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
                    abschliessen.Visibility = Visibility.Hidden;
                }
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
            produkteverwaltung.ItemsSource = produkteverwaltungl;
        }

        private void addProdukte_Click(object sender, RoutedEventArgs e)
        {
            ProdukteAdd produkteAdd = new ProdukteAdd();
            produkteAdd.ShowDialog();
            produkteverwaltungl.Clear();
            Produkteverwaltunglesen();
            Produktelesen();
        }

        private void entfernprodukte_Click(object sender, RoutedEventArgs e)
        {
            int id = produkteverwaltung.SelectedIndex;
            if (id != -1)
            {
                string query = $"EXEC DELETEPRODUKTE {produkteverwaltungl[id].ID}";
                Data(out string[] output, query);
                produkteverwaltungl.RemoveAt(id);
                Produktelesen();
                reloadprodukteverwaltung();
                reloadgprodukte();
            }
            else
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
            string date;
            if (id != -1)
            {
                lieferDate.ShowDialog();
                date = lieferDate.LieferDatum().ToString("dd MMM yyyy");
                if (date != "01 Jan 0001")
                {
                    query = $"UPDATE Lager SET Lieferung = '{lieferDate.LieferDatum()}' WHERE ID = {produkteverwaltungl[id].ID}";
                    Data(out string[] output, query);
                    Produkteverwaltunglesen();
                    reloadprodukteverwaltung();
                }
                
            }
            else
            {
                MessageBox.Show("Sie müssen ein Element auswählen!");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckLieferDatum()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-2);
            DateTime date;
            string query;
            for (int i = 0; i < produkteverwaltungl.Count; i++)
            {
                if (DateTime.TryParse(produkteverwaltungl[i].Lieferung, out date))
                {
                    if (date < dateTime)
                    {
                        query = $"INSERT INTO Lieferungen VALUES ({produkteverwaltungl[i].ID},'{produkteverwaltungl[i].Lieferung}')";
                        Data(out string[] output, query);
                        produkteverwaltungl[i].Lieferung = "";
                        query = $"UPDATE Lager SET Lieferung = null WHERE ID = {produkteverwaltungl[i].ID}";
                        Data(out output, query);
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string suchetb = produkteverwaltungsuche.Text; 
            List<Products> suche = new List<Products>();
            produkteverwaltung.ItemsSource = null;
            suche.Clear();
            if (suchetb != "" && suchetb != " " && suchetb != null)
            {
                Produktesuche(ref suche, suchetb, produkteverwaltungl);
                produkteverwaltung.ItemsSource = suche;
            }
            else
            {
                produkteverwaltung.ItemsSource = produkteverwaltungl;
            }
            


        }

        private void Useradd_Click(object sender, RoutedEventArgs e)
        {
            string user = tbuid.Text;
            UserAdd userAdd = new UserAdd(user);
            userAdd.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rechte"></param>
        /// <param name="user"></param>
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
                        tabkunde.Visibility = Visibility.Visible;
                        tabrechnung.Visibility = Visibility.Visible;
                        UserAnzeige();
                        
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
                        tabkunde.Visibility = Visibility.Visible;
                        UserAnzeige();
                        rechte.Remove("Admin");
                        break;
                    }
                case "Verwaltung":
                    {
                        
                        produkteanzeige.Visibility = Visibility.Visible;
                        useranzeige.Visibility = Visibility.Visible;
                        produkteanzeige.IsSelected = true;
                        kassa.Visibility = Visibility.Collapsed;
                        tabkunde.Visibility = Visibility.Visible;
                        UserAnzeige();
                        rechte.Remove("Admin");
                        break;
                    }
                case "Kassa":
                    {
                        tabrechnung.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void UserA()
        {
            UserAnzeige();
        }
        /// <summary>
        /// 
        /// </summary>
        private void UserAnzeige()
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
            UserAnzeige();
            if (id != -1)
            {
                if (output[0] == "Admin")
                {
                    string query = $"EXEC DELETEUSER {userVerwaltung[id].ID}, 0";
                    Data(out output, query);
                }
                else
                {
                    if (userVerwaltung[id].Rechte != "Admin")
                    {
                        string query = $"EXEC DELETEUSER {userVerwaltung[id].ID}, 0";
                        Data(out output, query);
                    }
                    else
                    {
                        MessageBox.Show("Du darfst kein Admin löschen!");
                    }
                }
            }
           
            UserAnzeige();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="output"></param>
        private void Rechtelesen(string user, out string[] output)
        {
            string[] userid = user.Split(' ');
            string query = $"SELECT r.Rechte FROM KUser ku JOIN Rechte r ON r.RechteID = ku.IDRechte WHERE ku.M_ID = {userid[1]}";
            Data(out output, query);
        }
        /// <summary>
        /// Read the products from the data base
        /// </summary>
        private void Produktelesen()
        {
            DateTime date;
            string stringdate;
            string query = "SELECT p.ID, p.Name, p.Preis, l.Lager, l.Lieferung FROM Produkte p JOIN Lager l ON p.ID = l.ID WHERE NOT l.Lager = 0";
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
            reloadgprodukte();

        }
        /// <summary>
        /// This is for the surch of Products
        /// </summary>
        /// <param name="suche"></param>
        /// <param name="suchetb"></param>
        /// <param name="input"></param>
        private void Produktesuche(ref List<Products> suche, string suchetb, List<Products> input)
        {
            string test;
            suche = new List<Products>();
            suche.Clear();
            string[,] suchearray = new string[produkte.Count, 4]; 
            
            for (int i = 0; i < produkte.Count; i++)
            {
                suchearray[i, 0] = Convert.ToString(input[i].ID);
                suchearray[i, 1] = Convert.ToString(input[i].Name);
                suchearray[i, 2] = Convert.ToString(input[i].InStock);
                suchearray[i, 3] = Convert.ToString(input[i].Preis);
            }
            for (int i = 0; i < suchearray.GetLength(0); i++)
            {
                for (int k = 0; k < suchearray.GetLength(1); k++)
                {
                    test = suchearray[i, k];
                    if (test.StartsWith(suchetb))
                    {
                        suche.Add(input[i]);
                    }
                }
            }
        }
        private void sucheuser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string suchetb = sucheuser.Text;
            string test;
            List<User> usersuche = new List<User>();
            string[,] suchearray = new string[userVerwaltung.Count, 4];
            if (suchetb == null || suchetb == "" || suchetb == " ")
            {
                userverwaltung.ItemsSource = "";
                userverwaltung.ItemsSource = userVerwaltung;
            }
            else
            {
                for (int i = 0; i < userVerwaltung.Count; i++)
                {
                    suchearray[i, 0] = Convert.ToString(userVerwaltung[i].ID);
                    suchearray[i, 1] = Convert.ToString(userVerwaltung[i].Vorname);
                    suchearray[i, 2] = Convert.ToString(userVerwaltung[i].Nachname);
                    suchearray[i, 3] = Convert.ToString(userVerwaltung[i].Rechte);
                }
                for (int i = 0; i < suchearray.GetLength(0); i++)
                {
                    for (int k = 0; k < suchearray.GetLength(1); k++)
                    {
                        test = suchearray[i, k];
                        if (test.StartsWith(suchetb))
                        {
                            usersuche.Add(userVerwaltung[i]);
                        }
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
            UserAnzeige();
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
        /// <summary>
        /// Read the Produkts from the Data Base
        /// </summary>
        private void Produkteverwaltunglesen()
        {
            DateTime date;
            string stringdate;
            produkteverwaltungl.Clear();
            string query = "SELECT l.ID, p.Name, p.Preis, l.Lager, l.Lieferung FROM Produkte p JOIN Lager l ON p.ID = l.ID";
            Data(out string[] output, query);

            for (int i = 0; i < output.Length - 1; i = i+5)
            {
                produkteverwaltungl.Add(new Products { ID = Convert.ToInt32(output[i]), Name = output[i+1], Preis = Convert.ToDouble(output[i+2]), InStock = Convert.ToInt32(output[i+3])});
                if (DateTime.TryParse(output[i + 4], out date))
                {
                    stringdate = date.ToString("dd MMM yyyy");
                    produkteverwaltungl[produkteverwaltungl.Count - 1].Lieferung = stringdate;

                }
                else
                {
                    produkteverwaltungl[produkteverwaltungl.Count - 1].Lieferung = "";

                }
            }
            produkteverwaltung.ItemsSource = "";
            produkteverwaltung.ItemsSource = produkteverwaltungl;
        }

        private void userverwaltung_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string query;
            int id = userverwaltung.SelectedIndex;
            string recht = (e.EditingElement as ComboBox).Text;
            int userid = userVerwaltung[id].ID;
            query = $"SELECT RechteID FROM Rechte WHERE Rechte = '{recht}'";
            Data(out string[] output, query);
            query = $"UPDATE KUser SET IDRechte = {output[0]} WHERE M_ID = {userid}";
            Data(out output, query);

        }

        private void anzeigeRechnung_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = anzeigeRechnung.SelectedIndex;
            string query = $"SELECT pv.VerkaufID, p.ID, p.Name, pv.Anzahl, pv.Geamtpreis FROM Produkte_Verkauf pv JOIN " +
                $"Produkte p ON p.ID = pv.ProduktID WHERE pv.VerkaufID = {rechnung[id].ID}";
            produkteVerkauf.Clear();
            Data(out string[] output, query);
            for (int i = 0; i < output.Length - 1; i = i + 5)
            {
                produkteVerkauf.Add(new VerkaufProdukte {VerkaufID = Convert.ToInt32(output[i]) ,ProduktID = Convert.ToInt32(output[i+1]), ProduktName = output[i + 2], Anzahl = Convert.ToInt32(output[i + 3]), Gesamtpreis = Convert.ToDouble(output[i + 4]) });
            }
            rechnungprodukte.ItemsSource = "";
            rechnungprodukte.ItemsSource = produkteVerkauf;

        }

        private void rechnungsuche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string suchetb = rechnungsuche.Text;
            string test;
            bool check = true;
            List<Rechnung> suche = new List<Rechnung>();
            string[,] suchearray = new string[rechnung.Count, 6];
            if (suchetb == null || suchetb == "" || suchetb == " ")
            {
                anzeigeRechnung.ItemsSource = "";
                anzeigeRechnung.ItemsSource = rechnung;
            }
            else
            {
                for (int i = 0; i < rechnung.Count; i++)
                {
                    suchearray[i, 0] = Convert.ToString(rechnung[i].ID);
                    suchearray[i, 1] = Convert.ToString(rechnung[i].User);
                    suchearray[i, 2] = rechnung[i].KundenVorname;
                    suchearray[i, 3] = rechnung[i].KundenNachname;
                    suchearray[i, 4] = Convert.ToString(rechnung[i].Anzahl);
                    suchearray[i, 5] = Convert.ToString(rechnung[i].Preis);
                }
                for (int i = 0; i < suchearray.GetLength(0); i++)
                {
                    for (int k = 0; k < suchearray.GetLength(1); k++)
                    {
                        test = suchearray[i, k];
                        if (test.StartsWith(suchetb))
                        {
                            for (int j = 0; j < suche.Count; j++)
                            {
                                if (suche[j].ID == rechnung[i].ID)
                                {
                                    check = false;
                                }
                            }
                            if (check)
                            {
                                suche.Add(rechnung[i]);
                                break;
                            }
                            
                        }
                    }
                }
                anzeigeRechnung.ItemsSource = "";
                anzeigeRechnung.ItemsSource = suche;
            }
        }

        private void buchen_Click(object sender, RoutedEventArgs e)
        {
            int idrechnung = anzeigeRechnung.SelectedIndex;
            int idprodukte = rechnungprodukte.SelectedIndex;
            string query;
            int lager;
            int menge;
            int anzahlprodukte = 0;
            List<VerkaufProdukte> produktebuchung = new List<VerkaufProdukte>();
            if (idrechnung != -1)
            {
                if (idprodukte != -1)
                {
                    if (produkteVerkauf[idprodukte].Anzahl > 0)
                    {
                        Mengen mengen = new Mengen();
                        mengen.ShowDialog();
                        menge = Convert.ToInt32(mengen.anzahl);

                        if (menge <= 0 || menge > produkteVerkauf[idprodukte].Anzahl)
                        {
                            MessageBox.Show("Die menge ist zu klein oder zu groß!");

                        }
                        else
                        {
                            anzahlprodukte = menge;
                            Buchung(idprodukte, idrechnung, anzahlprodukte);
                        }
                    }
                    else
                    {
                        anzahlprodukte = produkteVerkauf[idprodukte].Anzahl;
                        Buchung( idprodukte, idrechnung, anzahlprodukte);
                    }
                    
                }
                else
                {
                    query = $"SELECT ProduktID, Anzahl FROM Produkte_Verkauf WHERE VerkaufID = {rechnung[idrechnung].ID}";
                    Data(out string[] output, query);
                    for (int i = 0; i < output.Length - 1; i = i + 2)
                    {
                        produktebuchung.Add(new VerkaufProdukte { ProduktID = Convert.ToInt32(output[i]), Anzahl = Convert.ToInt32(output[i + 1]) });
                    }
                    for (int i = 0; i < produktebuchung.Count; i++)
                    {
                        query = $"SELECT Lager FROM Lager WHERE ID = {produktebuchung[i].ProduktID}";
                        Data(out output, query);
                        lager = Convert.ToInt32(output[0]);
                        lager += produktebuchung[i].Anzahl;
                        query = $"UPDATE Lager SET Lager = {lager} WHERE ID = {produktebuchung[i].ProduktID}";
                        Data(out output, query);

                    }
                    query = $"DELETE Produkte_Verkauf WHERE VerkaufID = {rechnung[idrechnung].ID}";
                    Data(out output, query);
                    query = $"DELETE Verkauf WHERE VerkaufID = {rechnung[idrechnung].ID}";
                    Data(out output, query);
                    LesenRechnung();
                    Produktelesen();
                }
                
            }
            else
            {
              MessageBox.Show("Sie müssen ein Element asuswählen!");   
            }
        }
        /// <summary>
        /// read the invoice from the Data Base
        /// </summary>
        private void LesenRechnung()
        {
            string query;
            query = "SelECT v.VerkaufID, v.UserID, k.Vorname, k.Nachname, v.AnzahlProdukte, v.GesamtPreis, v.Datum FROM Verkauf v JOIN Kunden k ON k.KundenID = v.KundenID";
            Data(out string[] output, query);
            rechnung.Clear();
            for (int i = 0; i < output.Length - 1; i = i + 7)
            {
                rechnung.Add(new Rechnung { ID = Convert.ToInt32(output[i]),User = Convert.ToInt32(output[i+1]), KundenVorname = output[i+2], KundenNachname = output[i+3], Anzahl = Convert.ToInt32(output[i+4]), Preis = Convert.ToInt32(output[i+5]), RechnungsDatum = Convert.ToDateTime(output[i+6]) });
            }
            anzeigeRechnung.ItemsSource = "";
            anzeigeRechnung.ItemsSource = rechnung;
        }
        /// <summary>
        /// Delete Produkts from the Invoce and Insert the prodack back do the istock. 
        /// When the Invoice empty is then it is deleted 
        /// </summary>
        /// <param name="idprodukte"></param>
        /// <param name="idrechnung"></param>
        /// <param name="anzahlprodukte"></param>
        private void Buchung(int idprodukte, int idrechnung, int anzahlprodukte)
        {
            double preis;
            string query;
            preis = rechnung[idrechnung].Preis - produkteVerkauf[idprodukte].Gesamtpreis * anzahlprodukte;
            query = $"EXEC produkteverkaufbuchnung {anzahlprodukte}, {produkteVerkauf[idprodukte].ProduktID}, {produkteVerkauf[idprodukte].VerkaufID}, 0, 0";
            Data(out string[] output, query);
            query = $"UPDATE Verkauf SET GesamtPreis = {preis} WHERE VerkaufID = {produkteVerkauf[idprodukte].VerkaufID}";
            Data(out output, query);
            query = $"SELECT Anzahlprodukte FROM Verkauf WHERE VerkaufID = {produkteVerkauf[idprodukte].VerkaufID}";
            Data(out output, query);
            if (Convert.ToInt32(output[0]) == 0)
            {
                query = $"DELETE Verkauf WHERE VerkaufID = {produkteVerkauf[idprodukte].VerkaufID}";
                Data(out output, query);
            }
            produkteVerkauf.RemoveAt(idprodukte);
            rechnungprodukte.ItemsSource = "";
            rechnungprodukte.ItemsSource = produkteVerkauf;
            LesenRechnung();
            Produktelesen();
        }

        private void Kundeadd_Click(object sender, RoutedEventArgs e)
        {
            KundenHizufuegen kundenHizufuegen = new KundenHizufuegen();
            kundenHizufuegen.ShowDialog();
            LesenKunden();
        }

        private void Kundedelete_Click(object sender, RoutedEventArgs e)
        {
            int id = kundenverwaltung.SelectedIndex;
            string query;
            if (id != -1)
            {
                query = $"EXEC DeleteKunde {kundenanzeigel[id].KundenID}, 0";
                Data(out string[] output, query);
            }
            else
            {
                MessageBox.Show("Sie müssen einen Kunden auswählen!");
            }
        }
        /// <summary>
        /// Reading out Coustomer from the Data Base
        /// </summary>
        public void LesenKunden()
        {
            string query = "SELECT * FROM Kunden";
            Data(out string[] output, query);
            kundenanzeigel.Clear();
            for (int i = 0; i < output.Length - 1; i = i+6)
            {
                kundenanzeigel.Add(new Kundenanzeige { KundenID = Convert.ToInt32(output[i]), Vorname = output[i+1], Nachname = output[i+2], Punkte = Convert.ToInt32(output[i+3]), Email = output[i+4], Telefone = output[i+5] });
            }
            kundenverwaltung.ItemsSource = "";
            kundenverwaltung.ItemsSource = kundenanzeigel;
        }

        private void suchekunden_TextChanged(object sender, TextChangedEventArgs e)
        {
            string suchetb = suchekunden.Text;
            string test;
            bool check = true;
            List<Kundenanzeige> suche = new List<Kundenanzeige>();
            string[,] suchearray = new string[rechnung.Count, 3];
            if (suchetb == null || suchetb == "" || suchetb == " ")
            {
                kundenverwaltung.ItemsSource = "";
                kundenverwaltung.ItemsSource = kundenanzeigel;
            }
            else
            {
                for (int i = 0; i < rechnung.Count; i++)
                {
                    suchearray[i, 0] = Convert.ToString(kundenanzeigel[i].KundenID);
                    suchearray[i, 1] = kundenanzeigel[i].Vorname;
                    suchearray[i, 2] = kundenanzeigel[i].Nachname;
                }
                for (int i = 0; i < suchearray.GetLength(0); i++)
                {
                    for (int k = 0; k < suchearray.GetLength(1); k++)
                    {
                        test = suchearray[i, k];
                        if (test.StartsWith(suchetb))
                        {
                            for (int j = 0; j < suche.Count; j++)
                            {
                                if (suche[j].KundenID == kundenanzeigel[i].KundenID)
                                {
                                    check = false;
                                }
                            }
                            if (check)
                            {
                                suche.Add(kundenanzeigel[i]);
                                break;
                            }

                        }
                    }
                }
                kundenverwaltung.ItemsSource = "";
                kundenverwaltung.ItemsSource = suche;
            }
        }

        
    }
}
