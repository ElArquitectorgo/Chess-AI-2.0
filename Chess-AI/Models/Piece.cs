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
        public abstract List<Point> GetPseudoValidMoves(Piece[] board);
        /// <summary>
        /// La variable targetPiece solo es necesaria para que funcione la IA,
        /// para jugar yo manualmente no me haría falta.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<Point> GetValidMoves(Piece[] board)
        {
            List<Point> validMoves = GetPseudoValidMoves(board);
            Piece targetPiece = new Rook(-1, -1, Models.Color.Black); ;
            Point kingPos = new Point();
            foreach (Piece piece in board)
            {
                if (piece is King && piece.Color == this.Color)
                {
                    kingPos = new Point(piece.X, piece.Y);
                }
                if (piece.X == this.X && piece.Y == this.Y)
                {
                    targetPiece = piece;
                }
            }

            List<Point> validMovesCopy = new List<Point>(validMoves);
            Point initPosition = new Point(X, Y);
            // Creo una pieza arbitraria ya que sino después no me deja usar la variable
            Piece caught = new Rook(-1, -1, Models.Color.Black);
            foreach (Point p in validMovesCopy)
            {
                bool capture = false;              
                this.Move(p.X, p.Y);
                targetPiece.X = p.X;
                targetPiece.Y = p.Y;

                // Si el movimiento corresponde a una captura debemos eliminar la pieza capturada
                foreach (Piece piece in board)
                {
                    if (piece.Color != this.Color && piece.X == p.X && piece.Y == p.Y ||
                        this is Pawn && piece.Color != this.Color && p.X == piece.X - 1 && p.Y == piece.Y && p.X == 2 ||
                        this is Pawn && piece.Color != this.Color && p.X == piece.X - 1 && p.Y == piece.Y && p.X == 2 ||
                        this is Pawn && piece.Color != this.Color && p.X == piece.X + 1 && p.Y == piece.Y && p.X == 5 ||
                        this is Pawn && piece.Color != this.Color && p.X == piece.X + 1 && p.Y == piece.Y && p.X == 5)
                    {
                        capture = true;
                        caught = piece;
                        caught.IsAlive = false;
                        break;
                    }
                }
                // Si es el rey el que se mueve hay que actualizar su posición cada vez
                if (this is King)
                {
                    kingPos = new Point(p.X, p.Y);
                }
                foreach (Piece piece in board)
                {
                    if (piece.Color == this.Color || !piece.IsAlive) continue;
                    List<Point> enemyValidMoves = piece.GetPseudoValidMoves(board);
                    if (enemyValidMoves.Contains(kingPos))
                    {
                        validMoves.Remove(p);
                    }
                }
                // Si hemos capturado una pieza la volvemos a revivir (deshacer movimiento)
                if (capture)
                {
                    caught.IsAlive = true;
                }
            }
            this.Move(initPosition.X, initPosition.Y);
            targetPiece.X = initPosition.X;
            targetPiece.Y = initPosition.Y;

            return validMoves;
        }
        public abstract Piece DeepClone();
    }
}
