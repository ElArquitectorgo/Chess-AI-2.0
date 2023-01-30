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
        public override List<Point> GetPseudoValidMoves(Piece[,] board)
        {
            List<Point> validMoves = new List<Point>();

            if (X - 1 >= 0 && Y - 2 >= 0 && board[X - 1, Y - 2] == null || X - 1 >= 0 && Y - 2 >= 0 && board[X - 1, Y - 2].Color != this.Color)
                validMoves.Add(new Point(X - 1, Y - 2));
            if (X - 2 >= 0 && Y - 1 >= 0 && board[X - 2, Y - 1] == null || X - 2 >= 0 && Y - 1 >= 0 && board[X - 2, Y - 1].Color != this.Color)
                validMoves.Add(new Point(X - 2, Y - 1));
            if (X + 1 < 8 && Y - 2 >= 0 && board[X + 1, Y - 2] == null || X + 1 < 8 && Y - 2 >= 0 && board[X + 1, Y - 2].Color != this.Color)
                validMoves.Add(new Point(X + 1, Y - 2));
            if (X + 2 < 8 && Y - 1 >= 0 && board[X + 2, Y - 1] == null || X + 2 < 8 && Y - 1 >= 0 && board[X + 2, Y - 1].Color != this.Color)
                validMoves.Add(new Point(X + 2, Y - 1));
            if (X - 1 >= 0 && Y + 2 < 8 && board[X - 1, Y + 2] == null || X - 1 >= 0 && Y + 2 < 8 && board[X - 1, Y + 2].Color != this.Color)
                validMoves.Add(new Point(X - 1, Y + 2));
            if (X - 2 >= 0 && Y + 1 < 8 && board[X - 2, Y + 1] == null || X - 2 >= 0 && Y + 1 < 8 && board[X - 2, Y + 1].Color != this.Color)
                validMoves.Add(new Point(X - 2, Y + 1));
            if (X + 1 < 8 && Y + 2 < 8 && board[X + 1, Y + 2] == null || X + 1 < 8 && Y + 2 < 8 && board[X + 1, Y + 2].Color != this.Color)
                validMoves.Add(new Point(X + 1, Y + 2));
            if (X + 2 < 8 && Y + 1 < 8 && board[X + 2, Y + 1] == null || X + 2 < 8 && Y + 1 < 8 && board[X + 2, Y + 1].Color != this.Color)
                validMoves.Add(new Point(X + 2, Y + 1));

            return validMoves;
        }
        public override Piece DeepClone()
        {
            Knight k = (Knight)this.MemberwiseClone();
            return k;
        }
    }
}
