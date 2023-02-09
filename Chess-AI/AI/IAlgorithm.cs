using Chess_AI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.AI
{
    public interface IAlgorithm
    {
        int Analyze(Board board, int depth);
        (Point, string) BestMove();
    }
}
