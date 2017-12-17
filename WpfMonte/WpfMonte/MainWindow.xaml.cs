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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfMonte
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int N = 200;
        FieldControl fieldView;
        public MainWindow()
        {
            InitializeComponent();
            fieldView = new FieldControl(N);
            gridTable.Children.Add(fieldView);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
