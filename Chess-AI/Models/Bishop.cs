using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Chess_AI.Models
{
    public class Bishop : Piece
    {
        private int id = 4;
        public override int Id { get { return id; } set { id = value; } }
        private Models.Color _color;

        public Bishop(int x, int y, Models.Color color) : base(x, y, color)
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

            // Abajo derecha
            int i = X + 1;
            int j = Y + 1;
            while (i < 8 && j < 8)
            {
                validMoves.Add(new Point(i, j));
                i++; j++;
            }
            // Arriba derecha
            i = X - 1;
            j = Y + 1;
            while (i >= 0 && j < 8)
            {
                validMoves.Add(new Point(i, j));
                i--; j++;
            }
            // Abajo izquierda
            i = X + 1;
            j = Y - 1;
            while (i < 8 && j >= 0)
            {
                validMoves.Add(new Point(i, j));
                i++; j--;
            }
            // Arriba izquierda
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
            }
            return validMoves;
        }
        public override Piece DeepClone()
        {
            Bishop b = (Bishop)this.MemberwiseClone();
            return b;
        }
    }
}
