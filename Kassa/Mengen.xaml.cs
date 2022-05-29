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
    /// Interaktionslogik für Mengen.xaml
    /// </summary>
    public partial class Mengen : Window
    {
        public Mengen()
        {
            InitializeComponent();
        }

        private void btbest_Click(object sender, RoutedEventArgs e)
        {
            Input();
            
        }

        public string anzahl
        {
            get { return tbMenge.Text; }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Input();
            }
        }
        private void Input()
        {
            int kontrolle;
            
                try
                {
                    kontrolle = Convert.ToInt32(tbMenge.Text);
                    this.Close();

                }
                catch
                {
                    errorms.Text = "Sie müssen eine Menge eingeben!";
                }
        }

        private void tbMenge_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMenge.Text == "Menge")
            {
                tbMenge.Text = "";
                tbMenge.Foreground = Brushes.Black;
                textmenge.Text = "Menge";
            }
        }

        private void tbMenge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMenge.Text))
            {
                tbMenge.Text = "Menge";
                tbMenge.Foreground = Brushes.LightGray;
                textmenge.Text = "";
            }
        }

        private void btabbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
