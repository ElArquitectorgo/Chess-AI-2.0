using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Pawn : Piece
    {
        private int id = 10;
        public override int Id { get { return id; } set { id = value; } }
        private Models.Color _color;
        public int turnJumped = 0;
        public Pawn(int x, int y, Models.Color color, int turnJumped = 0) : base(x, y, color)
        {
            this.turnJumped = turnJumped;
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

            if (this.Color == Models.Color.White)
            {
                if (X - 1 >= 0 && board[X - 1, Y] == null)
                {
                    validMoves.Add(new Point(X - 1, Y));
                    if (X == 6 && board[4, Y] == null) validMoves.Add(new Point(X - 2, Y));
                }
                // Diagonales
                if (X - 1 >= 0 && Y - 1 >= 0 && board[X - 1, Y - 1] != null && board[X - 1, Y - 1].Color != this.Color)
                    validMoves.Add(new Point(X - 1, Y - 1));
                if (X - 1 >= 0 && Y + 1 < 8 && board[X - 1, Y + 1] != null && board[X - 1, Y + 1].Color != this.Color)
                    validMoves.Add(new Point(X - 1, Y + 1));
            }
            else
            {
                if (X + 1 < 8 && board[X + 1, Y] == null)
                {
                    validMoves.Add(new Point(X + 1, Y));
                    if (X == 1 && board[3, Y] == null) validMoves.Add(new Point(X + 2, Y));
                }
                // Diagonales
                if (X + 1 < 8 && Y - 1 >= 0 && board[X + 1, Y - 1] != null && board[X + 1, Y - 1].Color != this.Color)
                    validMoves.Add(new Point(X + 1, Y - 1));
                if (X + 1 < 8 && Y + 1 < 8 && board[X + 1, Y + 1] != null && board[X + 1, Y + 1].Color != this.Color)
                    validMoves.Add(new Point(X + 1, Y + 1));
            }

            return validMoves;
        }
        public List<Point> GetEnPassantMoves(Piece[,] board, int turn, int jumpTurn)
        {
            List<Point> EnPassantMoves = new List<Point>();

            if (X + 1 < 8 && Y - 1 >= 0 && board[X, Y - 1] is Pawn && this.Color == Models.Color.Black && board[X, Y - 1].Color != this.Color)
            {
                Pawn p = (Pawn) board[X, Y - 1];
                if (p.turnJumped == jumpTurn) EnPassantMoves.Add(new Point(X + 1, Y - 1));
            }
            if (X + 1 < 8 && Y + 1 < 8 && board[X, Y + 1] is Pawn && this.Color == Models.Color.Black && board[X, Y + 1].Color != this.Color)
            {
                Pawn p = (Pawn)board[X, Y + 1];
                if (p.turnJumped == turn - 1) EnPassantMoves.Add(new Point(X + 1, Y + 1));
            }
            if (X - 1 >= 0 && Y - 1 >= 0 && board[X, Y - 1] is Pawn && this.Color == Models.Color.White && board[X, Y - 1].Color != this.Color)
            {
                Pawn p = (Pawn)board[X, Y - 1];
                if (p.turnJumped == turn - 1) EnPassantMoves.Add(new Point(X - 1, Y - 1));
            }
            if (X - 1 >= 0 && Y + 1 < 8 && board[X, Y + 1] is Pawn && this.Color == Models.Color.White && board[X, Y + 1].Color != this.Color)
            {
                Pawn p = (Pawn)board[X, Y + 1];
                if (p.turnJumped == turn - 1) EnPassantMoves.Add(new Point(X - 1, Y + 1));
            }

            return EnPassantMoves;
        }
        public override Piece DeepClone()
        {
            Pawn p = (Pawn)this.MemberwiseClone();
            return p;
        }
    }
}
