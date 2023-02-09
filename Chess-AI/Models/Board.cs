using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Board
    {
        private Piece[,] board = new Piece[8, 8];
        private static string standartdFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public int turn { get; set; }
        public int jumpTurn { get; set; }

        public Board()
        {
            ReadFen(standartdFen);
        }

        public Board(String fen)
        {
            ReadFen(fen);
        }

        public Board(Piece[,] pieces, int turn, int jumpTurn)
        {
            this.turn = turn;
            this.jumpTurn = jumpTurn;

            foreach (Piece piece in pieces)
            {
                if (piece == null) continue;
                else if (piece is Pawn) 
                {
                    Pawn p = (Pawn) piece;
                    board[p.X, p.Y] = new Pawn(p.X, p.Y, p.Color, p.turnJumped);
                }
                else if (piece is Rook)
                {
                    Rook r = (Rook) piece;
                    board[r.X, r.Y] = new Rook(r.X, r.Y, r.Color, r.HasMoved);
                }
                else if (piece is Knight)
                {
                    board[piece.X, piece.Y] = new Knight(piece.X, piece.Y, piece.Color);
                }
                else if (piece is Bishop)
                {
                    board[piece.X, piece.Y] = new Bishop(piece.X, piece.Y, piece.Color);
                }
                else if (piece is Queen)
                {
                    board[piece.X, piece.Y] = new Queen(piece.X, piece.Y, piece.Color);
                }
                else if (piece is King)
                {
                    King k = (King) piece;
                    board[k.X, k.Y] = new King(k.X, k.Y, k.Color, k.CanCastle);
                }
            }
        }

        public void Move(int x1, int y1, int x2, int y2)
        {
            Piece p = board[x1, y1];
            Piece p2 = board[x2, y2];
            board[x1, y1].Move(x2, y2);

            // Necesario para las capturas al paso, ya que no corresponden a un cambio de casillas.
            if (p is Pawn && p2 is null && y1 != y2) GetEnPassantCapture(x1, y1, x2, y2);

            board[x2, y2] = board[x1, y1];
            board[x1, y1] = null;

            turn++;

            if (p is King)
            {
                King king = (King) p;
                king.CanCastle = false;
                // Comprobamos si el rey se ha enrocado
                if (y1 == 4 && y2 == 2 || y1 == 4 && y2 == 6) MakeCastle(x1, y2);
            }
            else if (p is Rook)
            {
                Rook rook = (Rook) p;
                rook.HasMoved = true;
            }

            if (p is Pawn && p.Color == Models.Color.White && x1 == 6 && x2 == 4 ||
                        p is Pawn && p.Color == Models.Color.Black && x1 == 1 && x2 == 3)
            {
                Pawn pawn = (Pawn)p;
                jumpTurn = turn - 1;
                board[x2, y2] = pawn;
                pawn.turnJumped = jumpTurn;
            }

            if (p is Pawn && p.X == 0 || p is Pawn && p.X == 7)
            {
                MakePromotion(p);
            }


        }

        private void GetEnPassantCapture(int x1, int y1, int x2, int y2)
        {
            board[x1, y2].IsAlive = false;
            board[x1, y2] = null;
        }

        private void MakeCastle(int x1, int y2)
        {
            if (x1 == 0 && y2 == 2) { board[0, 3] = board[0, 0]; board[0, 0].Move(0, 3); board[0, 0] = null; return; }
            else if (x1 == 0 && y2 == 6) { board[0, 5] = board[0, 7]; board[0, 7].Move(0, 5); board[0, 7] = null; return; }
            else if (x1 == 7 && y2 == 2) { board[7, 3] = board[7, 0]; board[7, 0].Move(7, 3); board[7, 0] = null; return; }
            else if (x1 == 7 && y2 == 6) { board[7, 5] = board[7, 7]; board[7, 7].Move(7, 5); board[7, 7] = null; return; }
        }

        private void MakePromotion(Piece piece)
        {
            Queen queen = new Queen(piece.X, piece.Y, piece.Color);
            board[piece.X, piece.Y] = queen;
        }

        private void ReadFen(string fen)
        {
            int numPieces = 0;
            for (int i = 0; i < fen.Length; i++)
            {
                if (fen[i] == 'w')
                {
                    break;
                }
                if (Char.IsLetter(fen[i]))
                {
                    numPieces++;
                }
            }

            string[] rows = fen.Split('/');
            string info = rows[7].Split('w')[1];
            int row = 0;
            int col = 0;

            string splitFen = fen.Split(' ')[0];

            for (int i = 0; i < splitFen.Length; i++)
            {
                switch (splitFen[i])
                {
                    case 'r':
                        board[row, col] = new Rook(row, col, Models.Color.Black); col++;
                        break;
                    case 'n':
                        board[row, col] = new Knight(row, col, Models.Color.Black); col++;
                        break;
                    case 'b':
                        board[row, col] = new Bishop(row, col, Models.Color.Black); col++;
                        break;
                    case 'q':
                        board[row, col] = new Queen(row, col, Models.Color.Black); col++;
                        break;
                    case 'k':
                        board[row, col] = new King(row, col, Models.Color.Black); col++;
                        break;
                    case 'p':
                        board[row, col] = new Pawn(row, col, Models.Color.Black); col++;
                        break;
                    case 'R':
                        board[row, col] = new Rook(row, col, Models.Color.White); col++;
                        break;
                    case 'N':
                        board[row, col] = new Knight(row, col, Models.Color.White); col++;
                        break;
                    case 'B':
                        board[row, col] = new Bishop(row, col, Models.Color.White); col++;
                        break;
                    case 'Q':
                        board[row, col] = new Queen(row, col, Models.Color.White); col++;
                        break;
                    case 'K':
                        board[row, col] = new King(row, col, Models.Color.White); col++;
                        break;
                    case 'P':
                        board[row, col] = new Pawn(row, col, Models.Color.White); col++;
                        break;
                    case '/':
                        row++; col = 0;
                        break;
                    default:
                        col += (int)Char.GetNumericValue(splitFen[i]);
                        break;
                }
            }

            if (!info.Contains('K') && !info.Contains('Q'))
            {
                foreach (Piece p in board)
                {
                    if (p is King && p.Color == Models.Color.White)
                    {
                        King k = (King)p;
                        k.CanCastle = false;
                    }
                }
            }
            if (!info.Contains('k') && !info.Contains('q'))
            {
                foreach (Piece p in board)
                {
                    if (p is King && p.Color == Models.Color.Black)
                    {
                        King k = (King)p;
                        k.CanCastle = false;
                    }
                }
            }
        }

        public Piece[,] GetBoard()
        {
            return board;
        }

        public void SetBoard(Piece[,] board) 
        {
            this.board = board;
        }

        public void SubstractTurn()
        {
            turn--;
        }

        public Piece GetPiece(int row, int col)
        {
            return board[row, col];
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < 8; i++)
            {
                sb.Append("[");
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == null) sb.Append(" ,");
                    else sb.Append(board[i, j].ToString() + ",");
                }
                sb.Append("]\n");
            }
            return sb.ToString();
        }
    }
}
