using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Rook : Piece
    {
        private int id = 2;
        public override int Id { get { return id; } set { id = value; } }
        private Models.Color _color;
        public bool HasMoved { get; set; }

        public Rook(int x, int y, Models.Color color) : base(x, y, color)
        {
            HasMoved = false;
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

            for (int i = X + 1; i < 8; i++)
            {
                validMoves.Add(new Point(i, Y));
            }
            for (int i = X - 1; i >= 0; i--)
            {
                validMoves.Add(new Point(i, Y));
            }
            for (int j = Y + 1; j < 8; j++)
            {
                validMoves.Add(new Point(X, j));
            }
            for (int j = Y - 1; j >= 0; j--)
            {
                validMoves.Add(new Point(X, j));
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
                // Si la pieza está a la derecha
                if (p.X == X && p.Y > Y)
                {
                    for (int j = p.Y + 1; j < 8; j++)
                    {
                        validMoves.Remove(new Point(X, j));
                    }
                }
                // Si la pieza está a la izquierda
                else if (p.X == X && p.Y < Y)
                {
                    for (int j = p.Y - 1; j >= 0; j--)
                    {
                        validMoves.Remove(new Point(X, j));
                    }
                }
                // Si la pieza está por debajo
                else if (p.Y == Y && p.X > X)
                {
                    for (int i = p.X + 1; i < 8; i++)
                    {
                        validMoves.Remove(new Point(i, Y));
                    }
                }
                // Si la pieza está a por encima
                else if (p.Y == Y && p.X < X)
                {
                    for (int i = p.X - 1; i >= 0; i--)
                    {
                        validMoves.Remove(new Point(i, Y));
                    }
                }
            }
            return validMoves;
        }

        public override Piece DeepClone()
        {
            Rook r = (Rook) this.MemberwiseClone();
            r.HasMoved = this.HasMoved;
            return r;
        }
    }
}
