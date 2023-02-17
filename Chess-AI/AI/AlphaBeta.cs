using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Documents;
using Chess_AI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Chess_AI.AI
{
    public class AlphaBeta : IAlgorithm
    {
        private Models.Color Color = Models.Color.White;
        private (Point, string) bestMove = (new Point(-9, -9), "");
        private int bestEvaluation = int.MinValue;
        private static readonly ParallelOptions ParallelOptions = new()
        {
            MaxDegreeOfParallelism = 8
        };
        public AlphaBeta()
        {
        }

        public int Analyze(Board board, int depth)
        {
            //int bestEvaluation = int.MinValue;
            Piece[] pieces = new Piece[64];
            int cnt = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.GetPiece(i, j) == null) continue;
                    pieces[cnt] = board.GetPiece(i, j);
                    cnt++;
                }
            }

            Parallel.ForEach(pieces, ParallelOptions, piece =>
            {
                if (piece != null && piece.Color == this.Color && piece.IsAlive)
                {
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
                        if (bestMove.Item1.X == -9) bestMove = (p, piece.GetType().Name);

                        Board next = new Board(board.GetBoard(), board.turn, board.jumpTurn);

                        next.Move(initPosX, initPosY, p.X, p.Y);

                        int evaluation = Search(next, depth - 1, int.MinValue, int.MaxValue, false);

                        if (evaluation >= bestEvaluation)
                        {
                            bestEvaluation = evaluation;
                            bestMove = (p, piece.GetType().Name);
                        }

                        piece.Move(initPosX, initPosY);
                        next.SubstractTurn();

                    }
                }
            });
            return bestEvaluation;
        }

        public int Search(Board board, int depth, int alpha, int beta, bool isMaximizing)
        {
            if (depth == 0) return Evaluation.Evaluate(board.GetBoard());

            if (isMaximizing)
            {
                int bestEvaluation = int.MinValue;
                foreach (Piece piece in board.GetBoard())
                {
                    if (piece == null || piece.Color != Models.Color.White || !piece.IsAlive) continue;
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

                        int evaluation = Search(next, depth - 1, alpha, beta, false);
                        piece.Move(initPosX, initPosY);
                        next.SubstractTurn();
                        bestEvaluation = Math.Max(evaluation, bestEvaluation);
                        alpha = Math.Max(alpha, bestEvaluation);

                        if (beta <= alpha) break;
                    }
                }
                return bestEvaluation;
            }
            else
            {
                int bestEvaluation = int.MaxValue;
                foreach (Piece piece in board.GetBoard())
                {
                    if (piece == null || piece.Color != Models.Color.Black || !piece.IsAlive) continue;
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

                        int evaluation = Search(next, depth - 1, alpha, beta, true);
                        piece.Move(initPosX, initPosY);
                        next.SubstractTurn();

                        bestEvaluation = Math.Min(evaluation, bestEvaluation);
                        beta = Math.Min(beta, bestEvaluation);

                        if (beta <= alpha) break;
                    }
                }
                return bestEvaluation;
            }

        }

        public (Point, string) BestMove()
        {
            return bestMove;
        }

        private void ChangeColor()
        {
            Color = (Color == Models.Color.White) ? Models.Color.Black : Models.Color.White;
        }
    }
}
