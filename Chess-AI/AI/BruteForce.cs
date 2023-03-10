using Chess_AI.Controller;
using Chess_AI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.AI
{
    public class BruteForce: IAlgorithm
    {
        private Models.Color Color = Models.Color.White;
        public BruteForce() 
        {
        }

        // 17 fen1 depth 5
        // 16 fen6 depth 4
        public int Analyze(Board board, int depth)
        {
            //return Analyze2(board, depth);
            if (depth == 0) return 1;

            int numPositions = 0;
            foreach (Piece piece in board.GetBoard())
            {
                if (piece == null || piece.Color != this.Color || !piece.IsAlive) continue;
                int initPosX = piece.X; int initPosY = piece.Y;

                List<Point> moves = piece.GetValidMoves(board.GetBoard());
                if (piece is Pawn && piece.Color == Models.Color.Black && initPosX == 4 ||
                    piece is Pawn && piece.Color == Models.Color.White && initPosX == 3)
                {
                    Pawn p = (Pawn)piece;
                    if (board.turn == board.jumpTurn + 1)
                    {
                        moves.AddRange(p.GetEnPassantMoves(board.GetBoard(), board.turn, board.jumpTurn));
                        moves = piece.FilterMoves(board.GetBoard(), moves);
                    }  
                }
                foreach (Point p in moves)
                {
                    Piece p2 = null;
                    if (board.GetPiece(p.X, p.Y) != null)
                        p2 = board.GetPiece(p.X, p.Y).DeepClone();
                    
                    Piece EnPassantPawn = null;
                    if (piece is Pawn & initPosY != p.Y && board.GetPiece(p.X, p.Y) == null)
                    {
                        if (p.X == 2 && board.GetPiece(p.X + 1, p.Y) is Pawn && board.GetPiece(p.X + 1, p.Y).Color != this.Color)
                        {
                            EnPassantPawn = board.GetPiece(p.X + 1, p.Y).DeepClone();
                        }
                        else if (p.X == 5 && board.GetPiece(p.X - 1, p.Y) is Pawn && board.GetPiece(p.X - 1, p.Y).Color != this.Color)
                        {
                            EnPassantPawn = board.GetPiece(p.X - 1, p.Y).DeepClone();
                        }
                    }

                    bool hadMoved = true;
                    Rook rook = null;
                    if (piece is Rook)
                    {
                        rook = (Rook) piece;
                        if (!rook.HasMoved) hadMoved = false;
                    }

                    Pawn pawn = null;
                    int jumpTurn;
                    if (piece is Pawn)
                    {
                        pawn = (Pawn) piece.DeepClone();
                        jumpTurn = pawn.turnJumped;
                    }

                    bool canCastle = false;
                    King king = null;
                    if (piece is King)
                    {
                        king = (King) piece.DeepClone();
                        canCastle = king.CanCastle;
                    }

                    int BoardJumpTurn = board.jumpTurn;
                    board.Move(initPosX, initPosY, p.X, p.Y);
                    ChangeColor();

                    numPositions += Analyze(board, depth - 1);

                    piece.Move(initPosX, initPosY);
                    board.jumpTurn = BoardJumpTurn;

                    if (!hadMoved) rook.HasMoved = false;
                    if (pawn != null)
                    {
                        Pawn pa = (Pawn)piece;
                        pa.turnJumped = pawn.turnJumped;
                    }

                    Piece[,] tempBoard = board.GetBoard();
                    tempBoard[initPosX, initPosY] = piece;
                    
                    tempBoard[p.X, p.Y] = p2;
                    if (EnPassantPawn != null) 
                        tempBoard[EnPassantPawn.X, EnPassantPawn.Y] = EnPassantPawn;

                    if (king != null)
                    {
                        King k = (King)piece;
                        k.CanCastle = canCastle;
                    }

                    if (piece is King && initPosY == 4 && Math.Abs(p.Y - initPosY) == 2)
                    {
                        if (p.Y == 2 && p.X == 0)
                        {
                            tempBoard[0, 0] = tempBoard[0, 3];
                            tempBoard[0, 3].Move(0, 0);
                            tempBoard[0, 3] = null;
                        }
                        else if (p.Y == 2 && p.X == 7)
                        {
                            tempBoard[7, 0] = tempBoard[7, 3];
                            tempBoard[7, 3].Move(7, 0);
                            tempBoard[7, 3] = null;
                        }
                        else if (p.Y == 6 && p.X == 0)
                        {
                            tempBoard[0, 7] = tempBoard[0, 5];
                            tempBoard[0, 5].Move(0, 7);
                            tempBoard[0, 5] = null;
                        }
                        else if (p.Y == 6 && p.X == 7)
                        {
                            tempBoard[7, 7] = tempBoard[7, 5];
                            tempBoard[7, 5].Move(7, 7);
                            tempBoard[7, 5] = null;
                        }
                    }

                    board.SubstractTurn();

                    ChangeColor();
                }
            }

            return numPositions;
        }

        // 25 fen 6 depth 4
        private int Analyze2(Board board, int depth)
        {
            if (depth == 0) return 1;

            int numPositions = 0;
            foreach (Piece piece in board.GetBoard())
            {
                if (piece == null || piece.Color != this.Color || !piece.IsAlive) continue;
                int initPosX = piece.X; int initPosY = piece.Y;

                List<Point> moves = piece.GetValidMoves(board.GetBoard());
                if (piece is Pawn && piece.Color == Models.Color.Black && initPosX == 4 ||
                    piece is Pawn && piece.Color == Models.Color.White && initPosX == 3)
                {
                    Pawn p = (Pawn)piece;
                    if (board.turn == board.jumpTurn + 1)
                    {
                        moves.AddRange(p.GetEnPassantMoves(board.GetBoard(), board.turn, board.jumpTurn));
                        moves = piece.FilterMoves(board.GetBoard(), moves);
                    }
                }
                foreach (Point p in moves)
                {
                    Board next = new Board(board.GetBoard(), board.turn, board.jumpTurn);
                    next.Move(initPosX, initPosY, p.X, p.Y);
                    ChangeColor();

                    numPositions += Analyze2(next, depth - 1);

                    piece.Move(initPosX, initPosY);

                    next.SubstractTurn();

                    ChangeColor();
                }
            }

            return numPositions;
        }

        public (Point, string) BestMove()
        {
            throw new NotImplementedException();
        }

        private void ChangeColor()
        {
            if (Color == Models.Color.White) Color = Models.Color.Black;
            else Color = Models.Color.White;

            //color = (color == Models.Color.White) ? Models.Color.Black : Models.Color.White;
        }
    }
}
