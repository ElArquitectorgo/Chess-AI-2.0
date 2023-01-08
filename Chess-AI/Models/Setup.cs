using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Setup
    {
        public static Piece[] ReadFenNotation(string fen)
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
            Piece[] board = new Piece[numPieces];
            string[] rows = fen.Split('/');
            string last = rows[7].Split(' ')[0];
            string info = rows[7].Split('w')[1];
            rows[7] = last;
            int row = 0;
            int col = 0;
            int cnt = 0;

            foreach (string cad in rows)
            {
                for (int i = 0; i < cad.Length; i++)
                {
                    switch (cad[i])
                    {
                        case 'r':
                            board[cnt] = new Rook(row, col, Models.Color.Black); cnt++;
                            break;
                        case 'n':
                            board[cnt] = new Knight(row, col, Models.Color.Black); cnt++;
                            break;
                        case 'b':
                            board[cnt] = new Bishop(row, col, Models.Color.Black); cnt++;
                            break;
                        case 'q':
                            board[cnt] = new Queen(row, col, Models.Color.Black); cnt++;
                            break;
                        case 'k':
                            board[cnt] = new King(row, col, Models.Color.Black); cnt++;
                            break;
                        case 'p':
                            board[cnt] = new Pawn(row, col, Models.Color.Black); cnt++;
                            break;
                        case 'R':
                            board[cnt] = new Rook(row, col, Models.Color.White); cnt++;
                            break;
                        case 'N':
                            board[cnt] = new Knight(row, col, Models.Color.White); cnt++;
                            break;
                        case 'B':
                            board[cnt] = new Bishop(row, col, Models.Color.White); cnt++;
                            break;
                        case 'Q':
                            board[cnt] = new Queen(row, col, Models.Color.White); cnt++;
                            break;
                        case 'K':
                            board[cnt] = new King(row, col, Models.Color.White); cnt++;
                            break;
                        case 'P':
                            board[cnt] = new Pawn(row, col, Models.Color.White); cnt++;
                            break;
                        default:
                            col += (int) Char.GetNumericValue(cad[i]) - 1;
                            break;
                    }
                    col++;
                }
                row++;
                col = 0;
            }

            if (!info.Contains('K') && !info.Contains('Q'))
            {
                foreach (Piece p in board)
                {
                    if (p is King && p.Color == Models.Color.White)
                    {
                        King k = (King) p;
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

            return board;
        }
    }
}
