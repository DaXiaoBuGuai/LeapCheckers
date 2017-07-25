using System;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace CheckersBoard
{
    class Button
    {
        public string Name;
        public int Row;
        public int Column;
    }

    /// <summary>
    /// CheckersManager, descendent of MainWindow.xaml
    /// </summary>
    public class CheckersManager : MonoBehaviour
    {
        private Move currentMove;
        private String winner;
        private String turn;
        private String Title;
        private Button[,] CheckersGrid;
        private double timeLeft = 0.5;
        private const float boardPosY = 0.0f;
        private const float checkerHeight = 0.01f;
        private const float grid_scale = (0.5f / 8);
        private const float halfGrid = grid_scale / 2f;

        public CheckersManager()
        {
            this.Title = "Checkers! Blacks turn!";
            currentMove = null;
            winner = null;
            turn = "Black";
            CheckersGrid = new Button[9,8];
            MakeBoard();
        }

        private void ClearBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);
                    button.Name = "none";
                }
            }
        }

        private void MakeBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    CheckersGrid[r, c] = new Button();
                    CheckersGrid[r,c].Row = r;
                    CheckersGrid[r,c].Column = c;
                    CheckersGrid[r,c].Name = "none";
                }
            }
            MakeButtons();
        }

        private void MakeButtons()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r-1, c);
                    switch (r)
                    {
                        case 1:
                            if (c % 2 == 1)
                            {
                                button.Name = "buttonRed" + r + c;
                            }
                            break;
                        case 2:
                            if (c % 2 == 0)
                            {
                                button.Name = "buttonRed" + r + c;
                            }
                            break;
                        case 3:
                            if (c % 2 == 1)
                            {
                                button.Name = "buttonRed" + r + c;
                            }
                            break;
                        case 4:
                            if (c % 2 == 0)
                            {
                                button.Name = "none" + r + c;
                            }
                            break;
                        case 5:
                            if (c % 2 == 1)
                            {
                                button.Name = "none" + r + c;
                            }
                            break;
                        case 6:
                            if (c % 2 == 0)
                            {
                                button.Name = "buttonBlack" + r + c;
                            }
                            break;
                        case 7:
                            if (c % 2 == 1)
                            {
                                button.Name = "buttonBlack" + r + c;
                            }
                            break;
                        case 8:
                            if (c % 2 == 0)
                            {
                                button.Name = "buttonBlack" + r + c;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            CheckerBoard currentBoard = GetCurrentBoard();
        }

        Button GetGridElement(Button[,] g, int r, int c)
        {
            if (r == 8) r = 7;
            return g[r+1,c];
        }

        public void SetMove(int start_row, int start_column, int end_row, int end_column)
        {
            currentMove = new CheckersBoard.Move();
            currentMove.piece1 = new CheckersBoard.Piece(start_row, start_column);
            currentMove.piece2 = new CheckersBoard.Piece(end_row, end_column);
        }

        public Boolean CheckMove()
        {
            Button button1 = GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
            Button button2 = GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);

            if ((turn == "Black") && (button1.Name.Contains("Red")))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Debug.Log("It is blacks turn.");
                return false;
            }
            if ((turn == "Red") && (button1.Name.Contains("Black")))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Debug.Log("It is reds turn.");
                return false;
            }
            if (button1.Equals(button2))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                return false;
            }
            if(button1.Name.Contains("Black"))
            {
                return CheckMoveBlack(button1, button2);
            }
            else if (button1.Name.Contains("Red"))
            {
                return CheckMoveRed(button1, button2);
            }
            else
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Debug.Log("button1.name = " + button1.Name + " button2.name = " + button2.Name);
                Debug.Log("False");
                return false;
            }
        }

        private bool CheckMoveRed(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.checkJumps("Red");

            if (jumpMoves.Count > 0)
            {
                bool invalid = true;
                foreach (Move move in jumpMoves)
                {
                    if (currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    Debug.Log("Jump must be taken");
                    currentMove.piece1 = null;
                    currentMove.piece2 = null;
                    Debug.Log("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Red"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Black"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((currentMove.isAdjacent("Red")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("Red");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Black"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            currentMove = null;
            Debug.Log("Invalid Move. Try Again.");
            return false;
        }

        private bool CheckMoveBlack(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.checkJumps("Black");

            if (jumpMoves.Count > 0)
            {
                bool invalid = true;
                foreach (Move move in jumpMoves)
                {
                    if (currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    Debug.Log("Jump must be taken");
                    currentMove.piece1 = null;
                    currentMove.piece2 = null;
                    Debug.Log("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Black"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Red"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((currentMove.isAdjacent("Black")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("Black");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        Button middleButton = GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        if (middleButton.Name.Contains("Red"))
                        {
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            currentMove = null;
            Debug.Log("Invalid Move. Try Again.");
            return false;
        }

        public GameObject FindCylinder(int row, int column)
        {
            GameBoard gameBoard = FindObjectOfType<GameBoard>();

            Vector3 source_pos = gameBoard.transform.position +
                new Vector3(column * grid_scale + halfGrid,
                            boardPosY + checkerHeight * 0.7f,
                            (7 - row) * grid_scale + halfGrid);
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            float dist = 0.0f;
            GameObject checker = null;
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;
                if (g.name.Length < 9)
                    continue;
                string substr = g.name.Substring(0, 9);
                if (substr == "Checker_R" || substr == "Checker_B")
                {
                    float test_dist = Vector3.Distance(source_pos, g.transform.position);
                    if (checker == null || test_dist < dist)
                    {
                        checker = g;
                        dist = test_dist;
                    }
                }
            }
            return checker;
        }

        public void PhysicalMove(int row, int column, int target_row, int target_column) {
            GameObject checker = FindCylinder(row, column);
            GameBoard gameBoard = FindObjectOfType<GameBoard>();

            if (checker != null) {
                Debug.Log("moving the checker piece for the AI");
                checker.transform.position = gameBoard.transform.position +
                  new Vector3(target_column * grid_scale + halfGrid, boardPosY + checkerHeight * 0.7f,
                      (7 - target_row) * grid_scale + halfGrid);
                SnapPiecetoBoard sptb_script = checker.GetComponent<SnapPiecetoBoard>();
                sptb_script.SaveSnappedPosition();
            }
        }

        public void MakeMove(bool moveCylinder = false)
        {
            if ((currentMove.piece1 != null) && (currentMove.piece2 != null))
            {
                Debug.Log("Piece1 " + currentMove.piece1.Row + ", " + currentMove.piece1.Column);
                Debug.Log("Piece2 " + currentMove.piece2.Row + ", " + currentMove.piece2.Column);
                Button button1 = GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
                Button button2 = GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);
                string temp1 = button1.Name;
                //string temp2 = button2.Name;
                button1.Name = "none";
                button2.Name = temp1;
                if (moveCylinder) {
                    PhysicalMove(currentMove.piece1.Row, currentMove.piece1.Column, currentMove.piece2.Row, currentMove.piece2.Column);
                }
                checkKing(currentMove.piece2);
                currentMove = null;
                if (turn == "Black")
                {
                    this.Title = "Checkers! Reds turn!";
                    turn = "Red";
                }
                else if (turn == "Red")
                {
                    this.Title = "Checkers! Blacks turn!";
                    turn = "Black";
                }
                checkWin();
            }
        }

        void Update()
        {
        }

        public void aiMakeMove()
        {
            currentMove = CheckersAI.GetMove(GetCurrentBoard(), "Red");
            if (currentMove != null)
            {
                if (CheckMove())
                {
                    MakeMove(true);
                }
            }
        }

        private void playerMakeMove()
        {
            currentMove = CheckersAI.GetMove(GetCurrentBoard(), "Black");
            if (currentMove != null)
            {
                if (CheckMove())
                {
                    MakeMove();
                }
            }
        }

        private CheckerBoard GetCurrentBoard()
        {
            CheckerBoard board = new CheckerBoard();
            Debug.Log("GetCurrentBoard()");
            for (int r = 1; r < 9; r++)
            {
                string str = "";
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);
                    {
                        if (button.Name.Contains("Red"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 3);
                            else
                                board.SetState(r - 1, c, 1);
                        }
                        else if (button.Name.Contains("Black"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 4);
                            else
                                board.SetState(r - 1, c, 2);
                        }
                        else
                            board.SetState(r - 1, c, 0);
                    }
                    str += board.GetState(r - 1, c);
                }
                Debug.Log(str);
            }
            return board;
        }

        private void checkKing(Piece tmpPiece)
        {
            Button button = GetGridElement(CheckersGrid, tmpPiece.Row, tmpPiece.Column);

            {
                if ((button.Name.Contains("Black")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 1)
                    {
                        button.Name = "button" + "Black" + "King" + tmpPiece.Row + tmpPiece.Column;
                    }
                }
                else if ((button.Name.Contains("Red")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 8)
                    {
                        button.Name = "button" + "Red" + "King" + tmpPiece.Row + tmpPiece.Column;
                    }
                }
            }
        }
        
        private void addBlackButton(Piece middleMove)
        {
            // This means block out this section as the piece is removed
            Button button = GetGridElement(CheckersGrid, middleMove.Row, middleMove.Column);
            button.Name = "none" + middleMove.Row + middleMove.Column;
            GameObject jumped = FindCylinder(middleMove.Row, middleMove.Column);
            if (jumped)
            {
                Rigidbody rb = jumped.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddForce(UnityEngine.Random.Range(0.0f, 0.2f), 0.3f, 0.0f);
                InteractionBehaviour ib = jumped.GetComponent<InteractionBehaviour>();
                ib.enabled = false;
            }
        }

        private void checkWin()
        {
            int totalBlack = 0, totalRed = 0;
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Button button = GetGridElement(CheckersGrid, r, c);

                    {
                        if (button.Name.Contains("Red"))
                            totalRed++;
                        if (button.Name.Contains("Black"))
                            totalBlack++;
                    }
                }
            }
            if (totalBlack == 0)
                winner = "Red";
            if (totalRed == 0)
                winner = "Black";
            if (winner != null)
            {
                for (int r = 1; r < 9; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        //Button button = GetGridElement(CheckersGrid, r, c);
                        // N/A
                    }
                }
                string str = winner + " is the winner! Would you like to play another? Call newGame() if so";
                Debug.Log(str);
            }
        }

        private void newGame()
        {
            currentMove = null;
            winner = null;
            this.Title = "Checkers! Blacks turn!";
            turn = "Black";
            ClearBoard();
            MakeBoard();
        }

        private void newGame_Click()
        {
            newGame();
        }

        private void exit_Click()
        {
            Application.Quit();
        }
    }
}
