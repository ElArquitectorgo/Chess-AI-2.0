using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class King : Piece
    {
        private int id = 0;
        public override int Id { get { return id; } set { id = value; } }
        private Models.Color _color;
        public bool CanCastle { get; set; }

        public King(int x, int y, Models.Color color, bool canCastle = true) : base(x, y, color)
        {
            this.CanCastle = canCastle;
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

            if (Y + 1 < 8 && board[X, Y + 1] == null || Y + 1 < 8 && board[X, Y + 1].Color != this.Color)
                validMoves.Add(new Point(X, Y + 1));
            if (Y - 1 >= 0 && board[X, Y - 1] == null || Y - 1 >= 0 && board[X, Y - 1].Color != this.Color)
                validMoves.Add(new Point(X, Y - 1));
            if (X + 1 < 8 && board[X + 1, Y] == null || X + 1 < 8 && board[X + 1, Y].Color != this.Color)
                validMoves.Add(new Point(X + 1, Y));
            if (X - 1 >= 0 && board[X - 1, Y] == null || X - 1 >= 0 && board[X - 1, Y].Color != this.Color)
                validMoves.Add(new Point(X - 1, Y));
            if (X + 1 < 8 && Y + 1 < 8 && board[X + 1, Y + 1] == null || X + 1 < 8 && Y + 1 < 8 && board[X + 1, Y + 1].Color != this.Color)
                validMoves.Add(new Point(X + 1, Y + 1));
            if (X + 1 < 8 && Y - 1 >= 0 && board[X + 1, Y - 1] == null || X + 1 < 8 && Y - 1 >= 0 && board[X + 1, Y - 1].Color != this.Color)
                validMoves.Add(new Point(X + 1, Y - 1));
            if (X - 1 >= 0 && Y + 1 < 8 && board[X - 1, Y + 1] == null || X - 1 >= 0 && Y + 1 < 8 && board[X - 1, Y + 1].Color != this.Color)
                validMoves.Add(new Point(X - 1, Y + 1));
            if (X - 1 >= 0 && Y - 1 >= 0 && board[X - 1, Y - 1] == null || X - 1 >= 0 && Y - 1 >= 0 && board[X - 1, Y - 1].Color != this.Color)
                validMoves.Add(new Point(X - 1, Y - 1));

            if (CanCastle)
            {
                validMoves.AddRange(GetCastleMoves(board));
            }

            return validMoves;
        }
        private List<Point> GetCastleMoves(Piece[,] board)
        {
            List<Point> CastleMoves = new List<Point>();

            bool wLeftCastling = true;
            bool wRightCastling = true;
            bool bLeftCastling = true;
            bool bRightCastling = true;

            bool wLeftBlocked = true;
            bool wRightBlocked = true;
            bool bLeftBlocked = true;
            bool bRightBlocked = true;

            if (board[7, 0] is Rook && board[7, 0].Color == Models.Color.White && board[7, 1] == null && board[7, 2] == null && board[7, 3] == null)
                wLeftBlocked = false;
            if (board[7, 7] is Rook && board[7, 7].Color == Models.Color.White && board[7, 5] == null && board[7, 6] == null)
                wRightBlocked = false;

            if (board[0, 0] is Rook && board[0, 0].Color == Models.Color.Black && board[0, 1] == null && board[0, 2] == null && board[0, 3] == null)
                bLeftBlocked = false;
            if (board[0, 7] is Rook && board[0, 7].Color == Models.Color.Black && board[0, 5] == null && board[0, 6] == null)
                bRightBlocked = false;

            // Comprobamos que no se amenaza ninguna casilla del enroque
            // Importante que la pieza no sea un rey pues si no saltaría un StackOverflow
            // Técnicamente el rey podría parar un enroque pero creo que eso no se ha dado en la vida
            // Tengo que hacerlo en un bucle distinto.

            foreach (Piece piece in board)
            {
                if (piece == null || piece.Color == this.Color) continue;

                if (this.Color == Models.Color.White && piece.Color == Models.Color.Black && piece is not King)
                {
                    List<Point> enemyMoves = piece.GetPseudoValidMoves(board);
                    if (enemyMoves.Contains(new Point(7, 2)) || enemyMoves.Contains(new Point(7, 3))) wLeftCastling = false;
                    if (enemyMoves.Contains(new Point(7, 5)) || enemyMoves.Contains(new Point(7, 6))) wRightCastling = false;
                }
                else if (this.Color == Models.Color.Black && piece.Color == Models.Color.White && piece is not King)
                {
                    List<Point> enemyMoves = piece.GetPseudoValidMoves(board);
                    if (enemyMoves.Contains(new Point(0, 2)) || enemyMoves.Contains(new Point(0, 3))) bLeftCastling = false;
                    if (enemyMoves.Contains(new Point(0, 5)) || enemyMoves.Contains(new Point(0, 6))) bRightCastling = false;
                }

            }

            if (wLeftCastling && !wLeftBlocked)
            {
                Rook r = (Rook)board[7, 0];
                if (!r.HasMoved)
                    CastleMoves.Add(new Point(7, 2));
            }
            if (wRightCastling && !wRightBlocked)
            {
                Rook r = (Rook)board[7, 7];
                if (!r.HasMoved)
                    CastleMoves.Add(new Point(7, 6));
            }
            if (bLeftCastling && !bLeftBlocked)
            {
                Rook r = (Rook)board[0, 0];
                if (!r.HasMoved)
                    CastleMoves.Add(new Point(0, 2));
            }
            if (bRightCastling && !bRightBlocked)
            {
                Rook r = (Rook)board[0, 7];
                if (!r.HasMoved)
                    CastleMoves.Add(new Point(0, 6));
            }

            return CastleMoves;
        }
        public override Piece DeepClone()
        {
            King k = (King)this.MemberwiseClone();
            k.CanCastle = this.CanCastle;
            return k;
        }
    }
}
