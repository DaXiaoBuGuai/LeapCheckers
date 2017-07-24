using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersBoard
{
    class CheckersAI
    {
        public static Move GetMove(CheckerBoard currentBoard, string color)
        {
            List<Move> avaliableMoves = GetAvaliableMoves(currentBoard, color);
            avaliableMoves.Shuffle();
            if (avaliableMoves.Count < 1)
                return null;
            return avaliableMoves[0];
        }

        private static List<Move> GetAvaliableMoves(CheckerBoard currentBoard, string color)
        {
            List<Piece> currentPieces = new List<Piece>();
            List<Move> avaliableMoves = new List<Move>();
            List<Move> jumpMoves = currentBoard.checkJumps(color);
			int myColor, myKing;
			if (color == "Red")
			{
				myColor = 1;
				myKing = 3;
			}
			else
			{
				myColor = 2;
				myKing = 4;
			}


            if (jumpMoves.Count > 0)
            {
                return jumpMoves;
            }
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if ((currentBoard.GetState(r, c) == myColor) || (currentBoard.GetState(r, c) == myKing))
                    {
                        currentPieces.Add(new Piece(r, c));
                    }
                }
            }
            foreach (Piece p in currentPieces)
            {
                avaliableMoves.AddRange(CheckForMoves(p, currentBoard, color));
            }
            return avaliableMoves;
        }

        private static List<Move> CheckForMoves(Piece piece, CheckerBoard currentBoard, string color)
        {
			int rowUp, theirColor, theirKing, myKing;
			if (color == "Red")
			{
				rowUp = -1;
				theirColor = 2;
				theirKing = 4;
				myKing = 3;
			}
			else
			{
				rowUp = 1;
				theirColor = 1;
				theirKing = 3;
				myKing = 4;
			}

            List<Move> moves = new List<Move>();
            if (currentBoard.GetState(piece.Row, piece.Column) == myKing)
            {
                if ((currentBoard.GetState(piece.Row + rowUp, piece.Column - 1) == theirColor) || (currentBoard.GetState(piece.Row + rowUp, piece.Column - 1) == theirKing))
                {
                    if(currentBoard.GetState(piece.Row + 2 * rowUp, piece.Column - 2) == 0)
                        moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row + rowUp, piece.Column - 2)));
                }
                if ((currentBoard.GetState(piece.Row + rowUp, piece.Column + 1) == theirColor) || (currentBoard.GetState(piece.Row + rowUp, piece.Column + 1) == theirKing))
                {
                    if (currentBoard.GetState(piece.Row + 2 * rowUp, piece.Column + 2) == 0)
                        moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row + rowUp, piece.Column + 2)));
                }
                if ((currentBoard.GetState(piece.Row - rowUp, piece.Column - 1) == theirColor) || (currentBoard.GetState(piece.Row - rowUp, piece.Column - 1) == theirKing))
                {
                    if (currentBoard.GetState(piece.Row - 2 * rowUp, piece.Column - 2) == 0)
                        moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 3 * rowUp, piece.Column - 2)));
                }
                if ((currentBoard.GetState(piece.Row - rowUp, piece.Column + 1) == theirColor) || (currentBoard.GetState(piece.Row - rowUp, piece.Column + 1) == theirKing))
                {
                    if (currentBoard.GetState(piece.Row - 2 * rowUp, piece.Column + 2) == 0)
                        moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 3 * rowUp, piece.Column + 2)));
                }
                if (currentBoard.GetState(piece.Row + rowUp, piece.Column - 1) == 0)
                    moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row, piece.Column - 1)));
                if (currentBoard.GetState(piece.Row + rowUp, piece.Column + 1) == 0)
                    moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row, piece.Column + 1)));
                if (currentBoard.GetState(piece.Row - rowUp, piece.Column - 1) == 0)
                    moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 2 * rowUp, piece.Column - 1)));
                if (currentBoard.GetState(piece.Row - rowUp, piece.Column + 1) == 0)
                    moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 2 * rowUp, piece.Column + 1)));
            }
            else if (currentBoard.GetState(piece.Row, piece.Column) == 1)
            {
                if ((currentBoard.GetState(piece.Row - rowUp, piece.Column - 1) == theirColor) || (currentBoard.GetState(piece.Row - rowUp, piece.Column - 1) == theirKing))
                {
                    if (currentBoard.GetState(piece.Row - 2 * rowUp, piece.Column - 2) == 0)
                        moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 3 * rowUp, piece.Column - 2)));
                }
                if ((currentBoard.GetState(piece.Row - rowUp, piece.Column + 1) == 2) || (currentBoard.GetState(piece.Row - rowUp, piece.Column + 1) == theirKing))
                {
                    if (currentBoard.GetState(piece.Row - 2 * rowUp, piece.Column + 2) == 0)
                        moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row + 3, piece.Column + 2)));
                }
                if (currentBoard.GetState(piece.Row - rowUp, piece.Column - 1) == 0)
                    moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 2 * rowUp, piece.Column - 1)));
                if (currentBoard.GetState(piece.Row - rowUp, piece.Column + 1) == 0)
                    moves.Add(new Move(new Piece(piece.Row - rowUp, piece.Column), new Piece(piece.Row - 2 * rowUp, piece.Column + 1)));
            }
            return moves;
        }

    }
}
