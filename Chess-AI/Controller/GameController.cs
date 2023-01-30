using Chess_AI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.Controller
{
    public class GameController
    {
        // No activo los turnos por comodidad de testeo.
        // Falta la clase Status.
        // No puedo explorar más profundo por lo que crece la búsqueda.

        // Position 1, depth 6 = 119,060,324 -> 119,055,076 / 4e-5 % pérdidas
        // Position 2, depth 3 = 97,862 -> (con depth 4 no me sirve tampoco) 97,862

        // Position 3, depth 6 = 11,030,083 -> 11,024,419 / 5e-4 % pérdidas
        // Position 3, depth 7 = 178,633,661 -> 178,447,267 / 1e-4 % pérdidas (las de la coronación seguro)

        // Position 4, 5 no me sirven porque se prueban las promociones con caballo, torre y alfil.
        // Position 6, depth 5 = 164,075,551 -> 164,075,551

        private static readonly string[] fen =
        {
            "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
            "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -",
            "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -",
            "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1",
            "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8",
            "r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10",
        };

        private Piece[] board = Models.Setup.ReadFenNotation(fen[0]);
        private int turn = 0;
        private int jumpTurn = 0;
        private Dictionary<int, Piece[]> history = new Dictionary<int, Piece[]>();

        public GameController()
        {
            history.Add(turn, MakeBoardCopy(board));
        }

        public void SetBoardFromFen(String fen)
        {
            board = Models.Setup.ReadFenNotation(fen);
            history.Clear();
            turn = 0;
            jumpTurn = 0;
            history.Add(turn, MakeBoardCopy(board));       
        }

        public String[] GetFenList()
        {
            return fen;
        }

        public void UnmakeMove()
        {
            if (turn == 0) return;
            board = MakeBoardCopy(history[turn - 1]);
            history.Remove(turn);
            turn--;
        }
        
        private Piece[] MakeBoardCopy(Piece[] source)
        {
            Piece[] boardCopy = new Piece[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                boardCopy[i] = source[i].DeepClone();
            }
            return boardCopy;
        }
    }
}
