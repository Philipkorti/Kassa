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
    /// Interaktionslogik für LieferDate.xaml
    /// </summary>
    public partial class LieferDate : Window
    {
        public LieferDate()
        {
            InitializeComponent();
        }

        private void set_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            if (LieferDatum() > dateTime)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Das Datum darf nicht in der vergangenheit sein!");
            }
            
        }
        public DateTime LieferDatum()
        {
            DateTime date;
            date = lieferdate.SelectedDate.Value.Date;
            return date;
        }
    }
}