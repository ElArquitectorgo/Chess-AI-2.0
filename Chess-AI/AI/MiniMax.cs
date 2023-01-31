using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Chess_AI.Models;

namespace Chess_AI.AI
{
    public class MiniMax : IAlgorithm
    {
        private Board board;
        private Color Color = Color.White;
        public MiniMax(Board board)
        {
            this.board = board;
        }
        public int Analyze(int depth)
        {
            return 0;
        }
    }
}
