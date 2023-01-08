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
        // Falta implementar activar los turnos, terminar lo del fen.
        // Falta la clase Status.
        // Con fen3, si hago Pe5,Pg5 me deja comer al paso las dos con el negro, no debería
        // aunque sea una situación irreal. Parece que si muevo una blanca no afecta al HasJumped.
        // El mayor problema es no tener acceso a las posiciones del tablero.

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
        public void Move(int r1, int c1, int r2, int c2)
        {
            foreach (Piece piece in board)
            {
                // Solo puede comerse al paso justo después de haber saltado, no en otro turno
                if (piece is Pawn && turn != jumpTurn + 1)
                {
                    Pawn pawn= (Pawn)piece;
                    if (pawn.HasJumped)
                    {
                        pawn.HasJumped = false;
                    }
                }
                if (piece.X == r1 && piece.Y == c1)
                {
                    Point finalMove = new Point(r2, c2);

                    //if (piece.GetValidMoves(board).Contains(finalMove))
                    //{
                    piece.Move(r2, c2);
                    // Comprobamos si la pieza ha realizado una captura
                    GetCapture(piece, r1, c1, r2, c2);
                    turn++;

                    // Si el rey o la torre se mueven, el rey pierde el derecho a enrocarse
                    if (piece is King)
                    {
                        King p = (King) piece;
                        p.CanCastle = false;
                        // Comprobamos si el rey se ha enrocado
                        if (c1 == 4 && c2 == 2 || c1 == 4 && c2 == 6) MakeCastle(r1, c2);
                    }
                    else if (piece is Rook)
                    {
                        Rook p = (Rook) piece;
                        p.HasMoved = true;
                    }
                    // Si el peón realiza un salto inicial lo registramos para poder comer al paso
                    if (piece is Pawn && piece.Color == Models.Color.White && r1 == 6 && r2 == 4 ||
                        piece is Pawn && piece.Color == Models.Color.Black && r1 == 1 && r2 == 3)
                    {
                        Pawn p = (Pawn) piece;
                        p.HasJumped = true;
                        jumpTurn = turn - 1;
                    }
                    // Comprobamos si un peón ha coronado
                    if (piece is Pawn && piece.X == 0 || piece is Pawn && piece.X == 7) MakePromotion(piece);
                                               
                    history.Add(turn, MakeBoardCopy(board));
                    //}
                }
            }
        }

        public Piece[] GetBoard()
        {
            return board;
        }

        public void SetBoard(String fen)
        {
            board = Models.Setup.ReadFenNotation(fen);
            history = new Dictionary<int, Piece[]>
            {
                { turn, MakeBoardCopy(board) }
            };
            turn = 0;
            jumpTurn = 0;
        }

        public String[] GetFenList()
        {
            return fen;
        }
        private void MakeCastle(int r1, int c2)
        {
            foreach (Piece piece in board) 
            {
                if (piece is not Rook) continue;
                if (r1 == 0 && c2 == 2 && piece.X == 0 && piece.Y == 0) { piece.Move(0, 3); return; }
                else if (r1 == 0 && c2 == 6 && piece.X == 0 && piece.Y == 7) { piece.Move(0, 5); return; }
                else if (r1 == 7 && c2 == 2 && piece.X == 7 && piece.Y == 0) { piece.Move(7, 3); return; }
                else if (r1 == 7 && c2 == 6 && piece.X == 7 && piece.Y == 7) { piece.Move(7, 5); return; }
            }
        }

        private void MakePromotion(Piece piece)
        {
            Queen queen = new Queen(piece.X, piece.Y, piece.Color);

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == piece)
                {
                    board[i] = queen;
                    piece.IsAlive = false;
                    return;
                }
            }
        }

        private void GetCapture(Piece piece, int r1, int c1, int r, int c)
        {
            foreach (Piece p in board)
            {
                if (p != piece && p.X == r && p.Y == c ||
                    piece is Pawn && piece.Color == Models.Color.Black && p.Color != piece.Color && piece.X == p.X + 1 && piece.Y == p.Y && c1 != c && piece.X == 5 ||
                    piece is Pawn && piece.Color == Models.Color.White && p.Color != piece.Color && piece.X == p.X - 1 && piece.Y == p.Y && c1 != c && piece.X == 2)
                {
                    p.IsAlive = false;
                    p.X = -1;
                    p.Y = -1;
                    return;
                }
            }
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
