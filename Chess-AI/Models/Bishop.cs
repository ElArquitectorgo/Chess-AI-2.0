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
        public override List<Point> GetPseudoValidMoves(Piece[,] board)
        {
            List<Point> validMoves = new List<Point>();

            // Abajo derecha
            int i = X + 1;
            int j = Y + 1;
            while (i < 8 && j < 8 && board[i, j] == null)
            {
                validMoves.Add(new Point(i, j));
                i++; j++;
            }
            if (i < 8 && j < 8 && board[i, j] != null && board[i, j].Color != this.Color)
            {
                validMoves.Add(new Point(i, j));
            }
            // Arriba derecha
            i = X - 1;
            j = Y + 1;
            while (i >= 0 && j < 8 && board[i, j] == null)
            {
                validMoves.Add(new Point(i, j));
                i--; j++;
            }
            if (i >= 0 && j < 8 && board[i, j] != null && board[i, j].Color != this.Color)
            {
                validMoves.Add(new Point(i, j));
            }
            // Abajo izquierda
            i = X + 1;
            j = Y - 1;
            while (i < 8 && j >= 0 && board[i, j] == null)
            {
                validMoves.Add(new Point(i, j));
                i++; j--;
            }
            if (i < 8 && j >= 0 && board[i, j] != null && board[i, j].Color != this.Color)
            {
                validMoves.Add(new Point(i, j));
            }
            // Arriba izquierda
            i = X - 1;
            j = Y - 1;
            while (i >= 0 && j >= 0 && board[i, j] == null)
            {
                validMoves.Add(new Point(i, j));
                i--; j--;
            }
            if (i >= 0 && j >= 0 && board[i, j] != null && board[i, j].Color != this.Color)
            {
                validMoves.Add(new Point(i, j));
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
