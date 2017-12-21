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
    public struct Cell 
    {
        public int I { get; set; }
        public int J { get; set; }

        public Cell(int i, int j)
        {
            I = i;
            J = j;
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int N = 100;
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
            List<Cell> listPos = new List<Cell>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                listPos.Add(new Cell(i,j));
            }

            Random rand = new Random();

            for (int i = 0; i < Math.Pow(N,2); i++)
            {
                Cell p = listPos[rand.Next(0,listPos.Count-1)];
                listPos.Remove(p);
                if(rand.Next(0,2)==0)
                {
                    matrix[p.I, p.J] = -1;
                }
                else
                {
                    matrix[p.I, p.J] = 1;
                }
            }
        }    

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            Random rand = new Random();

            int[] valFiendCells1 = new int[4];

            int[] valFiendCells2 = new int[4];

            Cell selectCell;
            Cell friendCell;

            int e1;
            int e2;

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < 5000000; i++)
            {
                
                #region State1

                selectCell = new Cell(rand.Next(0, N - 1), rand.Next(0, N - 1));

                //Y
                valFiendCells1[0] = matrix[selectCell.I, (selectCell.J + 1) % N];
                valFiendCells1[1] = matrix[selectCell.I, (selectCell.J - 1) < 0 ? N - 1 : (selectCell.J - 1)];
                //X
                valFiendCells1[2] = matrix[(selectCell.I + 1) % N, selectCell.J];
                valFiendCells1[3] = matrix[(selectCell.I - 1) < 0 ? N - 1 : (selectCell.I - 1), selectCell.J];

                e1 = 0;

                foreach (var item in valFiendCells1)
                {
                    e1 = e1 + (matrix[selectCell.I, selectCell.J] + i);
                }

                #endregion

                #region State2

                friendCell = new Cell((selectCell.I + 1) % N, selectCell.J);

                //Y
                valFiendCells2[0] = matrix[friendCell.I, (friendCell.J + 1) % N];
                valFiendCells2[1] = matrix[friendCell.I, (friendCell.J - 1) < 0 ? N - 1 : (friendCell.J - 1)];
                //X
                valFiendCells2[2] = matrix[(friendCell.I + 1) % N, friendCell.J];
                valFiendCells2[3] = matrix[(friendCell.I - 1) < 0 ? N - 1 : (friendCell.I - 1), friendCell.J];

                e2 = 0;

                foreach (var item in valFiendCells2)
                {
                    e2 = e2 + (matrix[friendCell.I, friendCell.J] + i);
                }

                #endregion

                if (e1 - e2 < 0 /*|| rand.Next(0, 2) < Math.Exp((e1 - e2) / 0.026)*/)
                {
                    //перемещение элементов без доп. переменных
                    matrix[selectCell.I, selectCell.J] = matrix[selectCell.I, selectCell.J] + matrix[friendCell.I, friendCell.J];
                    matrix[friendCell.I, friendCell.J] = matrix[selectCell.I, selectCell.J] - matrix[friendCell.I, friendCell.J];
                    matrix[selectCell.I, selectCell.J] = matrix[selectCell.I, selectCell.J] - matrix[friendCell.I, friendCell.J];
                }
            }
            sw.Stop();
            fieldView.SetMatrix(matrix);
        }
    }
}
