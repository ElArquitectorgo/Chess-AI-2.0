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
        public bool HasJumped { get; set; }
        public Pawn(int x, int y, Models.Color color) : base(x, y, color)
        {
            HasJumped = false;
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

            if (this.Color == Models.Color.White)
            {
                if (X == 6) validMoves.Add(new Point(X - 2, Y));
                validMoves.Add(new Point(X - 1, Y));
            }
            else
            {
                if (X == 1) validMoves.Add(new Point(X + 2, Y));
                validMoves.Add(new Point(X + 1, Y));
            }

            foreach (Piece piece in board)
            {
                Point p = new Point(piece.X, piece.Y);
                // Elimina la piezas que tenga en frente
                if (validMoves.Contains(p) && piece.IsAlive)
                {
                    validMoves.Remove(p);
                }
                // Aprovecho el bucle para añadir las piezas que estén en diagonal
                if (piece.X == X - 1 && piece.Y == Y - 1 && this.Color == Models.Color.White && this.Color != piece.Color ||
                    piece.X == X - 1 && piece.Y == Y + 1 && this.Color == Models.Color.White && this.Color != piece.Color)
                {
                    validMoves.Add(p);
                }
                if (piece.X == X + 1 && piece.Y == Y - 1 && this.Color == Models.Color.Black && this.Color != piece.Color ||
                    piece.X == X + 1 && piece.Y == Y + 1 && this.Color == Models.Color.Black && this.Color != piece.Color)
                {
                    validMoves.Add(p);
                }
                if (this.Color == Models.Color.White && this.X == 6 && piece.X == 5 && this.Y == piece.Y) validMoves.Remove(new Point(4, this.Y));
                else if (this.Color == Models.Color.Black && this.X == 1 && piece.X == 2 && this.Y == piece.Y) validMoves.Remove(new Point(3, this.Y));
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

            if (this.Color == Models.Color.Black && this.X == 4 ||
                this.Color == Models.Color.White && this.X == 3)
                validMoves.AddRange(GetEnPassantMoves(board));

            return validMoves;
        }
        public List<Point> GetEnPassantMoves(Piece[] board)
        {
            List<Point> EnPassantMoves = new List<Point>();
            foreach (Piece piece in board)
            {
                if (piece is Pawn && piece.Color == Models.Color.White && this.Color == Models.Color.Black)
                {
                    Pawn p = (Pawn)piece;
                    if (p.HasJumped && p.X == this.X && p.Y == this.Y + 1) EnPassantMoves.Add(new Point(X + 1, Y + 1));
                    if (p.HasJumped && p.X == this.X && p.Y == this.Y - 1) EnPassantMoves.Add(new Point(X + 1, Y - 1));
                }
                else if (piece is Pawn && piece.Color == Models.Color.Black && this.Color == Models.Color.White)
                {
                    Pawn p = (Pawn)piece;
                    if (p.HasJumped && p.X == this.X && p.Y == this.Y + 1) EnPassantMoves.Add(new Point(X - 1, Y + 1));
                    if (p.HasJumped && p.X == this.X && p.Y == this.Y - 1) EnPassantMoves.Add(new Point(X - 1, Y - 1));
                }
            }

            return EnPassantMoves;
        }
        public override Piece DeepClone()
        {
            Pawn p = (Pawn)this.MemberwiseClone();
            p.HasJumped = this.HasJumped;
            return p;
        }
    }
}
