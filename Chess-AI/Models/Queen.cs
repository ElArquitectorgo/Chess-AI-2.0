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
        public override List<Point> GetPseudoValidMoves(Piece[,] board)
        {
            List<Point> validMoves = new List<Point>();

            for (int i = X + 1; i < 8; i++)
            {
                if (board[i, Y] != null)
                {
                    if (board[i, Y].Color != this.Color) validMoves.Add(new Point(i, Y));
                    break;
                }
                validMoves.Add(new Point(i, Y));
            }
            for (int i = X - 1; i >= 0; i--)
            {
                if (board[i, Y] != null)
                {
                    if (board[i, Y].Color != this.Color) validMoves.Add(new Point(i, Y));
                    break;
                }
                validMoves.Add(new Point(i, Y));
            }
            for (int j = Y + 1; j < 8; j++)
            {
                if (board[X, j] != null)
                {
                    if (board[X, j].Color != this.Color) validMoves.Add(new Point(X, j));
                    break;
                }
                validMoves.Add(new Point(X, j));
            }
            for (int j = Y - 1; j >= 0; j--)
            {
                if (board[X, j] != null)
                {
                    if (board[X, j].Color != this.Color) validMoves.Add(new Point(X, j));
                    break;
                }
                validMoves.Add(new Point(X, j));
            }

            // Abajo derecha
            int x = X + 1;
            int y = Y + 1;
            while (x < 8 && y < 8 && board[x, y] == null)
            {
                validMoves.Add(new Point(x, y));
                x++; y++;
            }
            if (x < 8 && y < 8 && board[x, y] != null && board[x, y].Color != this.Color)
            {
                validMoves.Add(new Point(x, y));
            }
            // Arriba derecha
            x = X - 1;
            y = Y + 1;
            while (x >= 0 && y < 8 && board[x, y] == null)
            {
                validMoves.Add(new Point(x, y));
                x--; y++;
            }
            if (x >= 0 && y < 8 && board[x, y] != null && board[x, y].Color != this.Color)
            {
                validMoves.Add(new Point(x, y));
            }
            // Abajo izquierda
            x = X + 1;
            y = Y - 1;
            while (x < 8 && y >= 0 && board[x, y] == null)
            {
                validMoves.Add(new Point(x, y));
                x++; y--;
            }
            if (x < 8 && y >= 0 && board[x, y] != null && board[x, y].Color != this.Color)
            {
                validMoves.Add(new Point(x, y));
            }
            // Arriba izquierda
            x = X - 1;
            y = Y - 1;
            while (x >= 0 && y >= 0 && board[x, y] == null)
            {
                validMoves.Add(new Point(x, y));
                x--; y--;
            }
            if (x >= 0 && y >= 0 && board[x, y] != null && board[x, y].Color != this.Color)
            {
                validMoves.Add(new Point(x, y));
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
