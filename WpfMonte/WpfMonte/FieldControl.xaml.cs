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
        public FieldControl()
        {
            InitializeComponent(); 
        }

        public void SetMatrix(int[,] matrix)
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            //0-i-row
            for (int i = 0; i < matrix.GetLength(0); i++)
                grid.RowDefinitions.Add(new RowDefinition());
            //1-j-column
            for (int j = 0; j < matrix.GetLength(1); j++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                var rec = new Rectangle();
                grid.Children.Add(rec);

                Grid.SetRow(rec, i);
                Grid.SetColumn(rec, j);

                switch (matrix[i, j])
                {
                    case -1:
                        rec.Fill = Brushes.Black;
                        break;
                    case 1:
                        rec.Fill = Brushes.Red;
                        break;
                    default:
                        throw new Exception();
                }  
            }
        }
    }
}
