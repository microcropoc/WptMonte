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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfMonte
{
    /// <summary>
    /// Логика взаимодействия для FieldControl.xaml
    /// </summary>
    public partial class FieldControl : UserControl
    {
        public FieldControl(int N)
        {
            InitializeComponent();
            for (int i = 0; i < N; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }
            Random rand = new Random();
            for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
            {
                var rec = new Rectangle();
                grid.Children.Add(rec);
                Grid.SetRow(rec, i);
                Grid.SetColumn(rec, j);
                if (rand.Next(0, 3) == 0)
                    rec.Fill = Brushes.Red;
                else
                    rec.Fill = Brushes.Black;
            }
            
        }
    }
}
