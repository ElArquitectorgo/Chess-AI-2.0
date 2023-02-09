using Chess_AI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.AI
{
    public class Evaluation
    {

        private static readonly int pawnValue = 100;
        private static readonly int knightValue = 300;
        private static readonly int bishopValue = 350;
        private static readonly int rookValue = 500;
        private static readonly int queenValue = 900;

        public static int Evaluate(Piece[,] board)
        {
            int evaluation = 0;

            foreach (Piece piece in board)
            {
                if (piece is null) continue;
                else if (piece is Pawn) evaluation += (piece.Color == Models.Color.White) ? pawnValue : -pawnValue;
                else if (piece is Knight) evaluation += (piece.Color == Models.Color.White) ? knightValue : -knightValue;
                else if (piece is Bishop) evaluation += (piece.Color == Models.Color.White) ? bishopValue : -bishopValue;
                else if (piece is Rook) evaluation += (piece.Color == Models.Color.White) ? rookValue : -rookValue;
                else if (piece is Queen) evaluation += (piece.Color == Models.Color.White) ? queenValue : -queenValue;
            }

            return evaluation;
        }
    }
}
