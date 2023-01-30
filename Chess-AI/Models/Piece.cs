using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public abstract class Piece
    {
        public abstract int Id { get; set; }
        public abstract Models.Color Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }

        public Piece(int x, int y, Models.Color color)
        {
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.IsAlive = true;
        }
        public void Move(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public abstract List<Point> GetPseudoValidMoves(Piece[,] board);
        /// <summary>
        /// La variable targetPiece solo es necesaria para que funcione la IA,
        /// para jugar yo manualmente no me haría falta.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<Point> GetValidMoves(Piece[,] board)
        {
            List<Point> validMoves = GetPseudoValidMoves(board);
            Point initPos = new Point(X, Y);

            List<Point> validMovesCopy = new List<Point>(validMoves);
            foreach (Point p in validMovesCopy)
            {
                Piece p2 = null;
                Piece EnPassantPawn = null;
                if (board[p.X, p.Y] != null)
                    p2 = board[p.X, p.Y].DeepClone();

                if (this is Pawn & initPos.Y != p.Y && board[p.X, p.Y] == null)
                {
                    if (p.X == 2 && board[p.X + 1, p.Y] is Pawn && board[p.X + 1, p.Y].Color != this.Color)
                    {
                        EnPassantPawn = board[p.X + 1, p.Y].DeepClone();
                        board[p.X + 1, p.Y] = null;
                    }
                    else if (p.X == 5 && board[p.X - 1, p.Y] is Pawn && board[p.X - 1, p.Y].Color != this.Color)
                    {
                        EnPassantPawn = board[p.X - 1, p.Y].DeepClone();
                        board[p.X - 1, p.Y] = null;
                    }
                }

                board[initPos.X, initPos.Y] = null;
                board[p.X, p.Y] = this;
                this.Move(p.X, p.Y);

                foreach (Piece piece in board)
                {
                    if (piece == null || piece.Color == this.Color) continue;
                    List<Point> enemyValidMoves = piece.GetPseudoValidMoves(board);
                    for (int i = 0; i < enemyValidMoves.Count; i++)
                    {
                        if (board[enemyValidMoves[i].X, enemyValidMoves[i].Y] is King)
                        {
                            validMoves.Remove(p);
                            break;
                        }
                    }
                }

                board[initPos.X, initPos.Y] = this;
                this.Move(initPos.X, initPos.Y);

                board[p.X, p.Y] = p2;
                if (EnPassantPawn != null) board[EnPassantPawn.X, EnPassantPawn.Y] = EnPassantPawn;

                // En principio no debería tener en cuenta el enroque del rey ya que en su propia clase
                // se comprueba si estaría siendo atacado o no
            }

            return validMoves;
        }
        public abstract Piece DeepClone();
    }
}
