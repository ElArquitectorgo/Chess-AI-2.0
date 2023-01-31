using Chess_AI.AI;
using Chess_AI.Controller;
using Chess_AI.Models;
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

namespace Chess_AI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] pieceImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/bKing.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/wKing.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/bRook.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/wRook.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/bBishop.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/wBishop.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/bKnight.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/wKnight.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/bQueen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/wQueen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/bPawn.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/wPawn.png", UriKind.Relative)),
        };

        private readonly Image[,] imageControls = new Image[8, 8];
        private readonly UIElement[,] movementIndicators = new UIElement[8, 8];

        private readonly GameController gameController = new GameController();
        private List<System.Drawing.Point> moves = new List<System.Drawing.Point>();
        private Point pointA = new Point(-1, -1);
        private Board board = new Board();
        private IAlgorithm algorithm;
        public MainWindow()
        {
            InitializeComponent();
            SetupGameGrid();
            SetupListView();
            Draw();
        }

        /// <summary>
        /// Esto lo necesito para tener el control de las imágenes y las elipses en arrays
        /// </summary>
        private void SetupGameGrid()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Image imageControl = new Image();
                    PieceGrid.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;

                    Ellipse ellipse = new Ellipse
                    {
                        Width = 25,
                        Height = 25,
                        Fill = Brushes.LightGray,
                        Opacity = 0.6,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 10,
                    };
                    UIElement ellipseUiElement = ellipse;
                    ellipseUiElement.Visibility = Visibility.Hidden;
                    movementIndicators[r, c] = ellipseUiElement;
                    PointGrid.Children.Add(ellipseUiElement);
                }
            }
        }

        private void SetupListView()
        {
            FenList.ItemsSource = gameController.GetFenList();
            FenList.SelectedIndex = 0;
            AlgorithmList.ItemsSource = gameController.GetAlgorithmsList();
            AlgorithmList.SelectedIndex = 0;
        }

        private void ResetImages()
        {
            foreach (Image img in imageControls)
            {
                img.Source = null;
            }

            foreach (UIElement circle in movementIndicators)
            {
                circle.Visibility = Visibility.Hidden;

                Ellipse elipse = (Ellipse)circle;
                elipse.Width = 25;
                elipse.Height = 25;
                elipse.Fill = Brushes.LightGray;
            }
        }
        private void ShowMovementIndicators(Piece piece)
        {
            moves = piece.GetValidMoves(board.GetBoard());
            foreach (System.Drawing.Point p in moves)
            {
                movementIndicators[p.X, p.Y].Visibility = Visibility.Visible;
                if (imageControls[p.X, p.Y].Source != null)
                {
                    Ellipse elipse = (Ellipse)movementIndicators[p.X, p.Y];
                    elipse.Width = 100;
                    elipse.Height = 100;
                    elipse.Fill = Brushes.Transparent;
                }
            }
        }

        private void Draw()
        {
            foreach (Piece piece in board.GetBoard())
            {
                if (piece == null || !piece.IsAlive) continue;
                imageControls[piece.X, piece.Y].Opacity = 1;
                imageControls[piece.X, piece.Y].Source = pieceImages[piece.Id];         
            }
        }

        private void PieceGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = PieceGrid.Width / 8;
            Point clickPosition = e.GetPosition(PieceGrid);
            int row = (int)(clickPosition.Y / squareSize);
            int col = (int)(clickPosition.X / squareSize);

            // Nos aseguramos de que hemos pulsado una pieza viendo si la casilla tiene una imagen asociada.
            if (pointA.X == -1 && imageControls[row, col].Source != null)
            {
                pointA = new Point(row, col);
                Piece piece = board.GetPiece(row, col);
                if (piece != null) ShowMovementIndicators(piece);
                else pointA = new Point(-1, -1);
            }
            else
            {
                if (moves.Contains(new System.Drawing.Point(row, col)))
                {
                    board.Move((int)pointA.X, (int)pointA.Y, row, col);
                }

                ResetImages();
                Draw();
                pointA = new Point(-1, -1);
            }
        }

        private void Windows_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
            {
                gameController.UnmakeMove();
                ResetImages();
                Draw();
            }
        }

        private void FenList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            board = new Board(FenList.SelectedItem.ToString());
            ResetImages();
            Draw();
        }

        private void AlgorithmList_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            
            
        }

        private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AlgorithmList.SelectedItem.ToString() == "Brute Force")
                {
                    algorithm = new BruteForce(new Board(FenList.SelectedItem.ToString()));
                }
                else if (AlgorithmList.SelectedItem.ToString() == "MiniMax")
                {
                    algorithm = new MiniMax(new Board(FenList.SelectedItem.ToString()));
                }

                int c = algorithm.Analyze(Convert.ToInt32(DepthTextBox.Text));
                MessageBox.Show(c.ToString() + " moves found with depth = " + DepthTextBox.Text);

                // A modo de reset para que el diccionario se vacíe
                board = new Board(FenList.SelectedItem.ToString());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
