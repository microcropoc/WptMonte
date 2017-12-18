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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int N = 20;
        FieldControl fieldView;
        int[,] matrix;
        public MainWindow()
        {
            InitializeComponent();
            fieldView = new FieldControl();
            gridTable.Children.Add(fieldView);

            InitializeMatrix();
            fieldView.SetMatrix(matrix);

        }

        public void InitializeMatrix()
        {
            matrix = new int[N,N];
            List<Point> listPos = new List<Point>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                listPos.Add(new Point(i,j));
            }

            Random rand = new Random();

            for (int i = 0; i < Math.Pow(N,2); i++)
            {
                Point p = listPos[rand.Next(0,listPos.Count-1)];
                listPos.Remove(p);
                if(rand.Next(0,2)==0)
                {
                    matrix[(int)p.X, (int)p.Y] = -1;
                }
                else
                {
                    matrix[(int)p.X, (int)p.Y] = 1;
                }
            }
        }

        

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5000000; i++)
            {

                Random rand = new Random();

                #region State1

                Point selectCell = new Point(rand.Next(0, N - 1), rand.Next(0, N - 1));

                List<int> valFiendCells1 = new List<int>();

                //Y
                valFiendCells1.Add(matrix[(int)(selectCell.X), (int)((selectCell.Y + 1) % N)]);
                valFiendCells1.Add(matrix[(int)(selectCell.X), (int)((selectCell.Y - 1) < 0 ? N - 1 : (selectCell.Y - 1))]);
                //X
                valFiendCells1.Add(matrix[(int)((selectCell.X + 1) % N), (int)(selectCell.Y)]);
                valFiendCells1.Add(matrix[(int)((selectCell.X - 1) < 0 ? N - 1 : (selectCell.X - 1)), (int)(selectCell.Y)]);

                int e1 = 0;

                foreach (var item in valFiendCells1)
                {
                    e1 = e1 + (matrix[(int)selectCell.X, (int)selectCell.Y] + i);
                }

                #endregion

                #region State2

                Point friendCell = new Point((int)((selectCell.X + 1) % N), (int)(selectCell.Y));

                List<int> valFiendCells2 = new List<int>();

                //Y
                valFiendCells2.Add(matrix[(int)(friendCell.X), (int)((friendCell.Y + 1) % N)]);
                valFiendCells2.Add(matrix[(int)(friendCell.X), (int)((friendCell.Y - 1) < 0 ? N - 1 : (friendCell.Y - 1))]);
                //X
                valFiendCells2.Add(matrix[(int)((friendCell.X + 1) % N), (int)(friendCell.Y)]);
                valFiendCells2.Add(matrix[(int)((friendCell.X - 1) < 0 ? N - 1 : (friendCell.X - 1)), (int)(friendCell.Y)]);

                int e2 = 0;

                foreach (var item in valFiendCells2)
                {
                    e2 = e2 + (matrix[(int)selectCell.X, (int)selectCell.Y] + i);
                }

                #endregion


                if (e1 - e2 < 0 || rand.Next(0, 2) < Math.Exp((e1 - e2) / 0.026))
                {
                    int tmp = matrix[(int)selectCell.X, (int)selectCell.Y];
                    matrix[(int)selectCell.X, (int)selectCell.Y] = matrix[(int)friendCell.X, (int)friendCell.Y];
                    matrix[(int)friendCell.X, (int)friendCell.Y] = tmp;
                }
            }
            fieldView.SetMatrix(matrix);
        }
    }
}
