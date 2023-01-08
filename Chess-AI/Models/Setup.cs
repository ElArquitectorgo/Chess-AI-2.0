using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_AI.Models
{
    public class Setup
    {
        /// <summary>
        /// Este método lee una cadena en notación FEN que nos indica una
        /// posición del tablero concreta. Si la pieza es negra se representa en 
        /// minúscula y si es blanca en mayúscula, cada letra corresponde a un
        /// tipo de pieza y los números representan los huecos entre piezas.
        /// El slash indica un salto de fila.
        /// 
        /// Una vez se representa el tablero se indica el color al que le toca
        /// jugar, w si es blanco b si es negro, los enroques disponibles
        /// K (white king side), Q (white queen side), k (black...), q (black...)
        /// y por último tres valores más, que corresponden respectivamente:
        /// la casilla donde un peón acaba de realizar un doble salto (si la hay),
        /// el número de movimientos que han hecho ambos jugadores desde el último
        /// avance de peón o captura (si llega a 100 el juego termina en tablas)
        /// y el número de turnos de la partida.
        /// De todo eso solo voy a usar lo de los enroques (y mal).
        /// </summary>
        /// <param name="fen"></param>
        /// <returns></returns>
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
            string info = rows[7].Split('w')[1];
            int row = 0;
            int col = 0;
            int cnt = 0;

            string splitFen = fen.Split(' ')[0];

            for (int i = 0; i < splitFen.Length; i++)
            {
                switch (splitFen[i])
                {
                    case 'r':
                        board[cnt] = new Rook(row, col, Models.Color.Black); cnt++; col++;
                        break;
                    case 'n':
                        board[cnt] = new Knight(row, col, Models.Color.Black); cnt++; col++;
                        break;
                    case 'b':
                        board[cnt] = new Bishop(row, col, Models.Color.Black); cnt++; col++;
                        break;
                    case 'q':
                        board[cnt] = new Queen(row, col, Models.Color.Black); cnt++; col++;
                        break;
                    case 'k':
                        board[cnt] = new King(row, col, Models.Color.Black); cnt++; col++;
                        break;
                    case 'p':
                        board[cnt] = new Pawn(row, col, Models.Color.Black); cnt++; col++;
                        break;
                    case 'R':
                        board[cnt] = new Rook(row, col, Models.Color.White); cnt++; col++;
                        break;
                    case 'N':
                        board[cnt] = new Knight(row, col, Models.Color.White); cnt++; col++;
                        break;
                    case 'B':
                        board[cnt] = new Bishop(row, col, Models.Color.White); cnt++; col++;
                        break;
                    case 'Q':
                        board[cnt] = new Queen(row, col, Models.Color.White); cnt++; col++;
                        break;
                    case 'K':
                        board[cnt] = new King(row, col, Models.Color.White); cnt++; col++;
                        break;
                    case 'P':
                        board[cnt] = new Pawn(row, col, Models.Color.White); cnt++; col++;
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
