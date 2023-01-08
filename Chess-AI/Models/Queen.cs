using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Queen : Piece
    {
        private int id = 8;
        public override int Id { get { return id; } set { id = value; } }
        private Models.Color _color;

        public Queen(int x, int y, Models.Color color) : base(x, y, color)
        {
        }

        public override Color Color
        {
            get { return _color; }
            set
            {
                if (value == Models.Color.White) Id++;
                _color = value;
            }
        }

        /// <summary>
        /// Obtiene la lista de movimientos pseudo válidos, es decir, sin tener en cuenta si la pieza está clavada
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public override List<Point> GetPseudoValidMoves(Piece[] board)
        {
            List<Point> validMoves = new List<Point>();

            for (int x = X + 1; x < 8; x++)
            {
                validMoves.Add(new Point(x, Y));
            }
            for (int x = X - 1; x >= 0; x--)
            {
                validMoves.Add(new Point(x, Y));
            }
            for (int y = Y + 1; y < 8; y++)
            {
                validMoves.Add(new Point(X, y));
            }
            for (int y = Y - 1; y >= 0; y--)
            {
                validMoves.Add(new Point(X, y));
            }

            int i = X + 1;
            int j = Y + 1;
            while (i < 8 && j < 8)
            {
                validMoves.Add(new Point(i, j));
                i++; j++;
            }

            i = X - 1;
            j = Y + 1;
            while (i >= 0 && j < 8)
            {
                validMoves.Add(new Point(i, j));
                i--; j++;
            }

            i = X + 1;
            j = Y - 1;
            while (i < 8 && j >= 0)
            {
                validMoves.Add(new Point(i, j));
                i++; j--;
            }

            i = X - 1;
            j = Y - 1;
            while (i >= 0 && j >= 0)
            {
                validMoves.Add(new Point(i, j));
                i--; j--;
            }

            return RemoveMovesBeyondPieces(validMoves, board);
        }

        /// <summary>
        /// Elimina de los movimientos válidos aquellas casillas que impliquen saltar piezas
        /// </summary>
        /// <param name="validMoves"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<Point> RemoveMovesBeyondPieces(List<Point> validMoves, Piece[] board)
        {
            foreach (Piece piece in board)
            {
                Point p = new Point(piece.X, piece.Y);
                if (!validMoves.Contains(p) || !piece.IsAlive) continue;

                // Si la pieza es del mismo color
                if (piece.Color == this.Color)
                {
                    validMoves.Remove(p);
                }

                // Si la pieza está abajo derecha
                int i = piece.X + 1;
                int j = piece.Y + 1;
                if (piece.X > this.X && piece.Y > this.Y)
                {
                    while (i < 8 && j < 8)
                    {
                        validMoves.Remove(new Point(i, j));
                        i++; j++;
                    }
                }
                // Si la pieza está arriba derecha
                i = piece.X - 1;
                j = piece.Y + 1;
                if (piece.X < this.X && piece.Y > this.Y)
                {
                    while (i >= 0 && j < 8)
                    {
                        validMoves.Remove(new Point(i, j));
                        i--; j++;
                    }
                }
                // Si la pieza está abajo izquierda
                i = piece.X + 1;
                j = piece.Y - 1;
                if (piece.X > this.X && piece.Y < this.Y)
                {
                    while (i < 8 && j >= 0)
                    {
                        validMoves.Remove(new Point(i, j));
                        i++; j--;
                    }
                }
                // Si la pieza está arriba izquierda
                i = piece.X - 1;
                j = piece.Y - 1;
                if (piece.X < this.X && piece.Y < this.Y)
                {
                    while (i >= 0 && j >= 0)
                    {
                        validMoves.Remove(new Point(i, j));
                        i--; j--;
                    }
                }

                // Si la pieza está a la derecha
                if (p.X == X && p.Y > Y)
                {
                    for (int y = p.Y + 1; y < 8; y++)
                    {
                        validMoves.Remove(new Point(X, y));
                    }
                }
                // Si la pieza está a la izquierda
                else if (p.X == X && p.Y < Y)
                {
                    for (int y = p.Y - 1; y >= 0; y--)
                    {
                        validMoves.Remove(new Point(X, y));
                    }
                }
                // Si la pieza está por debajo
                else if (p.Y == Y && p.X > X)
                {
                    for (int x = p.X + 1; x < 8; x++)
                    {
                        validMoves.Remove(new Point(x, Y));
                    }
                }
                // Si la pieza está por encima
                else if (p.Y == Y && p.X < X)
                {
                    for (int x = p.X - 1; x >= 0; x--)
                    {
                        validMoves.Remove(new Point(x, Y));
                    }
                }
            }
            return validMoves;
        }
        public override Piece DeepClone()
        {
            Queen q = (Queen)this.MemberwiseClone();
            return q;
        }
    }
}
