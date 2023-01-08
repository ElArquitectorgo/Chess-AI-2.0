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
    public class BruteForce
    {
        private GameController gameController;
        private Models.Color Color = Models.Color.White;
        public BruteForce(GameController gameController) 
        {
            this.gameController = gameController;
        }
        public int Analyze(int depth)
        {
            if (depth == 0) return 1;

            int numPositions = 0;
            foreach (Piece piece in gameController.GetBoard())
            {
                if (piece.Color != this.Color || !piece.IsAlive) continue;

                int initPosX = piece.X; int initPosY = piece.Y;

                List<Point> moves = piece.GetValidMoves(gameController.GetBoard());
                foreach (Point p in moves)
                {
                    gameController.Move(initPosX, initPosY, p.X, p.Y);
                    ChangeColor();

                    numPositions += Analyze(depth - 1);

                    gameController.UnmakeMove();

                    ChangeColor();
                }
            }

            return numPositions;
        }

        private void ChangeColor()
        {
            if (Color == Models.Color.White) Color = Models.Color.Black;
            else Color = Models.Color.White;

            //color = (color == Models.Color.White) ? Models.Color.Black : Models.Color.White;
        }
    }
}
