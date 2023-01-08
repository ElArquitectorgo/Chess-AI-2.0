using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Knight : Piece
    {
        private int id = 6;
        public override int Id { get { return id; } set { id = value; } }
        private Models.Color _color;

        public Knight(int x, int y, Models.Color color) : base(x, y, color)
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
            List<Point> validMoves = new List<Point>
            {
                new Point(X - 1, Y - 2),
                new Point(X - 2, Y - 1),
                new Point(X + 1, Y - 2),
                new Point(X + 2, Y - 1),
                new Point(X - 1, Y + 2),
                new Point(X - 2, Y + 1),
                new Point(X + 1, Y + 2),
                new Point(X + 2, Y + 1)
            };

            // Elimina las piezas aliadas
            foreach (Piece piece in board)
            {
                Point p = new Point(piece.X, piece.Y);
                if (validMoves.Contains(p) && piece.Color == this.Color && piece.IsAlive)
                {
                    validMoves.Remove(p);
                }
            }

            // Elimina las casillas fuera del tablero
            List<Point> validMovesCopy = new List<Point>(validMoves);
            foreach (Point p in validMovesCopy)
            {
                if (p.X < 0 || p.X > 7 || p.Y < 0 || p.Y > 7)
                {
                    validMoves.Remove(p);
                }
            }
            return validMoves;
        }
        public override Piece DeepClone()
        {
            Knight k = (Knight)this.MemberwiseClone();
            return k;
        }
    }
}
