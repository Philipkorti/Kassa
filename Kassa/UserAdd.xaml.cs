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
        public UserAdd()
        {
            InitializeComponent();
            RechteDB();
        }
        private void RechteDB()
        {
            List<string> rechte = new List<string>();
            string query = "SELECT Rechte FROM Rechte";
            MainWindow mainWindow = new MainWindow();
            mainWindow.Datenbank(out string[] output, query);
            for (int i = 0; i < output.Length-1; i++)
            {
                rechte.Add(output[i]);
            }
            mainWindow.Rechte(ref rechte);
            rechteaswahl.ItemsSource = rechte;
        }
    }
}
