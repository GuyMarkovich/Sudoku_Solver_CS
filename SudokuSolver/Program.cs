using SudokuSolver;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SudokuSolver
{
    public class SudokuBoard
    {
        private int boardSize;
        private int boxSize;
        private SudokuCell[,] grid;

        //constructor function for the board, default size if not specified otherwise is 9x9
        public SudokuBoard(string nums, int boardSize = 9)
        {
            this.boardSize = boardSize;
            //boxSize refers to the smaller boxes that are part of the sudoku board, ie in a 9x9 board a box will be a 3x3 segment
            this.boxSize = (int)Math.Sqrt(boardSize);
            int iterator = 0;
            this.grid = new SudokuCell[this.boardSize, this.boardSize];
            for (int i = 0; i< this.boardSize; i++)
            {
                for (int j = 0; j< this.boardSize; j++)
                {
                    int temp = nums[iterator] - '0'; //input the number into the correct place in the grid (converting from char to int)
                    if (temp != 0)
                    {
                        List<int> possibleValues = new List<int>();
                        possibleValues.Add(temp);
                        this.grid[i, j] = new SudokuCell(temp, true, i, j, possibleValues);
                    }
                    else
                    {
                        List<int> possibleValues = new List<int>();
                        for (int k = 1; k <= this.boardSize; k++)
                        {
                            possibleValues.Add(k);
                        }
                        this.grid[i, j] = new SudokuCell(temp, false, i, j, possibleValues);
                    }


                    iterator++;

                }
            }
        }

        public void printBoardFancy()
        {
            //print the sudoku board for the user
            for (int row = 0; row < this.boardSize; row++)
            {
                if (row % this.boxSize == 0)
                {
                    Console.WriteLine("+-------+-------+-------+");
                }

                for (int col = 0; col < this.boardSize; col++)
                {
                    if (col % this.boxSize == 0)
                    {
                        Console.Write("| ");
                    }
                    Console.Write(this.grid[row, col].Value + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+-------+-------+-------+");

        }





        public bool solveBoard()
        {
            for (int i = 0; i < this.boardSize; i++)
            {
                for (int j = 0; j < this.boardSize; j++)
                {
                    if (this.grid[i, j].Value == 0)
                    {
                        // Try numbers from 1 to 9
                        for (int k = 1; k <= this.boardSize; k++)
                        {
                            // Check if the number is allowed in the current cell
                            if (checkPlacement(i, j, k))
                            {
                                // Assign the number and move to the next cell
                                this.grid[i, j].Value = k;
                                if (solveBoard())
                                {
                                    return true;
                                }
                                // Backtrack and try the next number
                                this.grid[i, j].Value = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private bool checkPlacement(int row, int col, int num)
        {
            // Check if the number is already used in the row
            for (int i = 0; i < 9; i++)
            {
                if (this.grid[row, i].Value == num)
                {
                    return false;
                }
            }

            // Check if the number is already used in the column
            for (int i = 0; i < 9; i++)
            {
                if (this.grid[i, col].Value == num)
                {
                    return false;
                }
            }

            // Check if the number is already used in the 3x3 block
            int startRow = row - row % this.boxSize;
            int startCol = col - col % this.boxSize;
            for (int i = startRow; i < startRow + this.boxSize; i++)
            {
                for (int j = startCol; j < startCol + this.boxSize; j++)
                {
                    if (this.grid[i, j].Value == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }

    
}

class Progs
{
    static void Main()
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        SudokuBoard s1 = new SudokuBoard("000700000100000000000430200000000006000509000000000418000081000002000050040000300");
        s1.printBoardFancy();
        s1.solveBoard();
        s.Stop();
        s1.printBoardFancy();
        Console.WriteLine(s.Elapsed);

    }
}