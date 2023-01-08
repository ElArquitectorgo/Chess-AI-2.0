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

        public King(int x, int y, Models.Color color) : base(x, y, color)
        {
            CanCastle = true;
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
                new Point(X, Y + 1),
                new Point(X, Y - 1),
                new Point(X + 1, Y),
                new Point(X - 1, Y),
                new Point(X + 1, Y + 1),
                new Point(X + 1, Y - 1),
                new Point(X - 1, Y + 1),
                new Point(X - 1, Y - 1)
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

            if (CanCastle)
            {
                validMoves.AddRange(GetCastleMoves(board));
            }

            return validMoves;
        }
        private List<Point> GetCastleMoves(Piece[] board)
        {
            List<Point> CastleMoves = new List<Point>();

            bool wLeftCastling = false;
            bool wRightCastling = false;
            bool bLeftCastling = false;
            bool bRightCastling = false;

            bool wLeftBlocked = false;
            bool wRightBlocked = false;
            bool bLeftBlocked = false;
            bool bRightBlocked = false;

            foreach (Piece piece in board)
            {
                // Enroque blancas
                if (piece.X == 7 && piece is not King && this.Color == Models.Color.White)
                {
                    // Enroque largo blanco
                    if (piece.Y == 1 || piece.Y == 2 || piece.Y == 3) wLeftBlocked = true;
                    else if (piece.Y == 0 && piece is Rook && piece.Color == this.Color)
                    {
                        Rook p = (Rook)piece;
                        if (!p.HasMoved) wLeftCastling = true;
                    }
                    // Enroque corto
                    if (piece.Y == 5 || piece.Y == 6) wRightBlocked = true;
                    else if (piece.Y == 7 && piece is Rook && piece.Color == this.Color)
                    {
                        Rook p = (Rook)piece;
                        if (!p.HasMoved) wRightCastling = true;
                    }
                }
                // Enroque negras
                else if (piece.X == 0 && piece is not King && this.Color == Models.Color.Black)
                {
                    // Enroque largo blanco
                    if (piece.Y == 1 || piece.Y == 2 || piece.Y == 3) bLeftBlocked = true;
                    else if (piece.Y == 0 && piece is Rook && piece.Color == this.Color)
                    {
                        Rook p = (Rook)piece;
                        if (!p.HasMoved) bLeftCastling = true;
                    }
                    // Enroque corto
                    if (piece.Y == 5 || piece.Y == 6) bRightBlocked = true;
                    else if (piece.Y == 7 && piece is Rook && piece.Color == this.Color)
                    {
                        Rook p = (Rook)piece;
                        if (!p.HasMoved) bRightCastling = true;
                    }
                }
            }

            // Comprobamos que no se amenaza ninguna casilla del enroque
            // Importante que la pieza no sea un rey pues si no saltaría un StackOverflow
            // Técnicamente el rey podría parar un enroque pero creo que eso no se ha dado en la vida
            // Tengo que hacerlo en un bucle distinto.

            foreach (Piece piece in board)
            {

                if (this.Color == Models.Color.White && piece.Color == Models.Color.Black && piece is not King)
                {
                    List<Point> enemyMoves = piece.GetPseudoValidMoves(board);
                    if (enemyMoves.Contains(new Point(7, 2)) || enemyMoves.Contains(new Point(7, 3)))
                    {
                        wLeftCastling = false;
                    }
                    if (enemyMoves.Contains(new Point(7, 5)) || enemyMoves.Contains(new Point(7, 6))) wRightCastling = false;
                }
                else if (this.Color == Models.Color.Black && piece.Color == Models.Color.White && piece is not King)
                {
                    List<Point> enemyMoves = piece.GetPseudoValidMoves(board);
                    if (enemyMoves.Contains(new Point(0, 2)) || enemyMoves.Contains(new Point(0, 3))) bLeftCastling = false;
                    if (enemyMoves.Contains(new Point(0, 5)) || enemyMoves.Contains(new Point(0, 6))) bRightCastling = false;
                }

            }

            if (wLeftCastling && !wLeftBlocked) CastleMoves.Add(new Point(7, 2));
            if (wRightCastling && !wRightBlocked) CastleMoves.Add(new Point(7, 6));
            if (bLeftCastling && !bLeftBlocked) CastleMoves.Add(new Point(0, 2));
            if (bRightCastling && !bRightBlocked) CastleMoves.Add(new Point(0, 6));

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
