using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
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
        //const int N = 100;
        List <double> listExp;
        FieldControl fieldView;
        int[,] matrix;
        double к = 1.3 * Math.Pow(10, -23);
        int N;
        int Nmkh;
        LineSeries lineEnergy;
        LineSeries lineC;

        public MainWindow()
        {
            InitializeComponent();
            fieldView = new FieldControl();
            gridTable.Children.Add(fieldView);

            #region initGraphEnergy

            var GraphModel = new PlotModel { Title = "Energy" };

            lineEnergy = new LineSeries { Title = "K", MarkerType = MarkerType.Circle };
            lineEnergy.Points.Add(new DataPoint(0, 0));

            GraphModel.Series.Add(lineEnergy);

            graphEnergy.Model = GraphModel;

            #endregion

            #region initKineticGraphic

            var GraphModel1 = new PlotModel { Title = "TempEmcost" };

            lineC = new LineSeries { Title = "T", MarkerType = MarkerType.Circle };
            lineC.Points.Add(new DataPoint(0, 0));

            GraphModel1.Series.Add(lineC);

            graphC.Model = GraphModel1;

            #endregion

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

        double T;
        int CountIter;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (double.Parse(textT.Text) > 2.5)
            {
                InitializeMatrix();
                fieldView.SetMatrix(matrix);
                VieweGraphics();
                return;
            } 
            StringBuilder sbDeltaEs = new StringBuilder(CountIter*10);
            Random rand = new Random();

            int[] valFiendCells1 = new int[4];

            int[] valFiendCells2 = new int[4];

            Cell selectCell;
            Cell friendCell;

            int e1=0;
            int e2=0;
            int deltae=0;
            for (int i = 0; i < 10; i++)
            {
                var t = T+1*i;
                listExp = new List<double>()
                {
                    Math.Exp(-4 / -(к * t)),
                    Math.Exp(-3 / -к * t),
                    Math.Exp(-2 / -к * t),
                    Math.Exp(-1 / -к * t),
                    Math.Exp(0 / -к * t),
                    Math.Exp(1 / -к * t),
                    Math.Exp(2 /- к * t),
                    Math.Exp(3 / -к * t),
                    Math.Exp(4 / -к * t)
                };

                for (int j = 0; j < CountIter; j++)
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
                        e1 = e1+item;
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
                        e2 = e2+item;
                    }

                    #endregion
                    deltae = e2 - e1;
                    var rand1 = rand.Next(0, 1);
                    if (e2 - e1 < 0 /*|| listExp.Any(p => rand1 < p)*/)
                    {
                        //перемещение элементов без доп. переменных
                        matrix[selectCell.I, selectCell.J] = matrix[selectCell.I, selectCell.J] + matrix[friendCell.I, friendCell.J];
                        matrix[friendCell.I, friendCell.J] = matrix[selectCell.I, selectCell.J] - matrix[friendCell.I, friendCell.J];
                        matrix[selectCell.I, selectCell.J] = matrix[selectCell.I, selectCell.J] - matrix[friendCell.I, friendCell.J];
                    }
                    if (e2 - e1 > 0 || listExp.Any(p => rand1 < p))
                    {
                        //перемещение элементов без доп. переменных
                        matrix[selectCell.I, selectCell.J] = matrix[selectCell.I, selectCell.J] + matrix[friendCell.I, friendCell.J];
                        matrix[friendCell.I, friendCell.J] = matrix[selectCell.I, selectCell.J] - matrix[friendCell.I, friendCell.J];
                        matrix[selectCell.I, selectCell.J] = matrix[selectCell.I, selectCell.J] - matrix[friendCell.I, friendCell.J];
                    }
                    sbDeltaEs.AppendLine((e2 - e1).ToString());
                }
                double summ = 0;
                for (int k = 0; k < matrix.GetLength(0)/2; k++)
                    for (int l = 0; l < matrix.GetLength(1)/2; l++)
                    {
                        summ += matrix[k, l];
                    }
                listpointEn.Add(new Point(t, (summ / N)));
                listpointC.Add(new Point(t, e2-e1));
            }
            fieldView.SetMatrix(matrix);
            VieweGraphics();
          //  File.AppendAllText(@"C:\Users\Artyo\Desktop\Test.txt", (deltaE));
        }

        private void btnIni_Click(object sender, RoutedEventArgs e)
        {
            lineEnergy.Points.Clear();
            lineC.Points.Clear();
            graphEnergy.Model.InvalidatePlot(true);
            graphC.Model.InvalidatePlot(true);
            N = int.Parse(textNch.Text);
            T = double.Parse(textT.Text);
            CountIter = int.Parse(textNp.Text);
            Nmkh = int.Parse(textNp.Text) / int.Parse(textNch.Text) * int.Parse(textNch.Text); 
            textNmkh.Text = (Nmkh/100).ToString();
            InitializeMatrix();
            fieldView.SetMatrix(matrix);
        }
        List<Point> listpointEn = new List<Point>();
        List<Point> listpointC = new List<Point>();


        private void VieweGraphics()
        { 
            lineEnergy.Points.Clear();
            lineC.Points.Clear();
            foreach (var item in listpointEn)
            {
                lineEnergy.Points.Add(new DataPoint(item.X,item.Y));
                
            }
            foreach (var item in listpointC)
            {
               
                lineC.Points.Add(new DataPoint(item.X, item.Y));
            }
            listpointEn.Clear();
            listpointC.Clear();

            graphEnergy.Model.InvalidatePlot(true);
            graphC.Model.InvalidatePlot(true);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
