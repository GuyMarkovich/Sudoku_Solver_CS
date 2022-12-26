using SudokuSolver;
using System;

namespace SudokuSolver
{
    public class SudokuBoard
    {
        private int boardSize;
        private int boxSize;
        private int[,] grid;

        //constructor function for the board, default size if not specified otherwise is 9x9
        public SudokuBoard(string nums, int boardSize = 9)
        {
            this.boardSize = boardSize;
            //boxSize refers to the smaller boxes that are part of the sudoku board, ie in a 9x9 board a box will be a 3x3 segment
            this.boxSize = (int)Math.Sqrt(boardSize);
            int iterator = 0;
            this.grid = new int[this.boardSize, this.boardSize];
            for (int i = 0; i< this.boardSize; i++)
            {
                for (int j = 0; j< this.boardSize; j++)
                {
                    grid[i, j] = nums[iterator] - '0'; //input the number into the correct place in the grid (converting from char to int)
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
                    Console.Write(this.grid[row, col] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+-------+-------+-------+");

        }

    }

    
}

class Progs
{
    static void Main()
    {
        SudokuBoard s1 = new SudokuBoard("800000070006010053040600000000080400003000700020005038000000800004050061900002000");
        s1.printBoardFancy();


    }
}