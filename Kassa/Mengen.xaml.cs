﻿using System;
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
            tbMenge.Focus();
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
            if (tbMenge.Text == null || tbMenge.Text == "" || tbMenge.Text == " ")
            {
                MessageBox.Show("Sie müssen eine Menge eingeben!");
            }
            else
            {
                try
                {
                    kontrolle = Convert.ToInt32(tbMenge.Text);
                    this.Close();

                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
