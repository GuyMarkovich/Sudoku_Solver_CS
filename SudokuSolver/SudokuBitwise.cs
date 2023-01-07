using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuBitwiseBoard
    {
        private int boardSize;
        private int boxSize;
        private int[,] grid;
        private long[,] possibleValues;


        //constructor function for a board
        public SudokuBitwiseBoard(int boardSize, string nums)
        {
            this.boardSize = boardSize;
            this.boxSize = (int)Math.Sqrt(boardSize);
            this.grid = new int[boardSize, boardSize];
            this.possibleValues = new long[boardSize, boardSize];
            int iterator = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int temp = nums[iterator] - '0'; //convert char to int
                    if (temp != 0) //if value in given cell is not 0, add it to the matrix, set possibleValues to 0
                    {
                        this.grid[i, j] = temp;
                        this.possibleValues[i, j] = 1 << (temp - 1);
                    }
                    else //else, put a 0 on the board and fill the possible values with bits corresponding to the number of possible options
                    {
                        this.grid[i, j] = 0;
                        this.possibleValues[i, j] = (1 << boardSize) - 1;
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
                //line borders
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



        public void printPossibilities() // function for testing purposes only
        {
            for (int row = 0; row < this.boardSize; row++)
            {
                //line borders
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
                    Console.Write(this.possibleValues[row, col] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+-------+-------+-------+");
        }


        public bool SolveBoard()
        {
            int minRow = -1; // row of cell with least possibilities
            int minCol = -1; //col of cell with least possibilities
            int minPossibilities = int.MaxValue; //counter holds least number of possibilities, intialized to max int value to ensure it will work for all sizes of boards
            //find cell with least number of possibilities
            for (int row = 0; row < this.boardSize; row++)
            {
                for (int col = 0; col < this.boardSize; col++)
                {
                    if (this.grid[row, col] == 0) //if the current cell is not a given one
                    {
                        int currPossibilities = CountPossibleValues(row, col);
                        if (currPossibilities < minPossibilities)
                        {
                            minRow = row;
                            minCol = col;
                            minPossibilities = currPossibilities;
                        }
                    }
                }
            }

            if (minRow == -1) //condition to stop the recursion, if minRow stays -1 no empty cells are left and the board is solved
            {
                return true;
            }

            for (int val = 1; val <= this.boardSize; val++)
            {
                long mask = 1 << (val - 1); //create a bit mask with the bit in the val-1th place set to 1
                if ((this.possibleValues[minRow, minCol] & mask) != 0) //if corresponding bit in possible values is set to 1, it is a potential candidate
                {
                    //set the value of the place in the grid
                    this.grid[minRow, minCol] = val;
                    this.possibleValues[minRow, minCol] = 0;
                    UpdatePossibleValues( minRow, minCol, val);
                    if (!checkPlacement(minRow, minCol)) //if placement is illegal skip solving the next step, restore any changes and move on to the next candidate
                    {
                        this.grid[minRow, minCol] = 0;
                        this.possibleValues[minRow, minCol] = (1 << this.boardSize) - 1;
                        RestorePossibileValues(minRow, minCol, val);
                        continue;
                    }
                    
                    bool solvedNext = SolveBoard(); //try to continue solving
                    if (solvedNext) //if solution successful return true
                    {
                        return true;
                    }
                    
                    
                    //if no solution found reset the value in the current cell and restore the checked value for all other affected cells
                    this.grid[minRow, minCol] = 0;
                    this.possibleValues[minRow, minCol] = (1 << this.boardSize) - 1;
                    RestorePossibileValues(minRow, minCol, val);
                  
                }
            }
            //if no value for the current cell worked, backtrack to the previous cell
            return false;
        }

        //returns number of possible options (candidates) for a given cell
        private int CountPossibleValues(int row, int col)
        {
            int count = 0; //number of possible values a cell can have
            for (int val = 1; val <= this.boardSize; val++)
            {
                long mask = 1 << (val - 1);
                if ((this.possibleValues[row,col] & mask) != 0) //if bit is set, the corresponding value is a possibility, increase count
                {
                    count++;
                }
            }
            return count;
        }

        //check if value in given cell is valid and follows the rules of sudoku
        private bool checkPlacement(int row, int col)
        {
            int num = this.grid[row, col]; //num to check

            // Check if the number is already used in the row, go over all columns except the one where the given cell is
            for (int currCol = 0; currCol < this.boardSize; currCol++)
            {
                if ((this.grid[row, currCol] == num)&&(currCol!= col))
                {
                    return false;
                }
            }

            // Check if the number is already used in the column, go over all rows except the one where the given cell is
            for (int currRow = 0; currRow < this.boardSize; currRow++)
            {
                if ((this.grid[currRow, col] == num)&&(currRow!= row))
                {
                    return false;
                }
            }

            // Check if the number is already used in the subgrid
            int startRow = row - row % this.boxSize; //find start row and column of subgrid
            int startCol = col - col % this.boxSize;
            for (int currRow = startRow; currRow < startRow + this.boxSize; currRow++)
            {
                for (int currCol = startCol; currCol < startCol + this.boxSize; currCol++)
                {
                    if ((this.grid[currRow, currCol] == num)&&(currCol!= col)&&(currRow != row)) //check every cell other than one given
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //function that updates possible values for all cells in a row column and box,
        //the function receives the row, column and value of the cell currently being processed
        private void UpdatePossibleValues(int row, int col, int val) 
        {
            // Update the possibilities for the cells in the same row
            for (int c = 0; c < this.boardSize; c++)
            {
                if (c != col && this.possibleValues[row, c] != 0) //for every column that is in the same row,
                                                                  //remove the current cells value from the possibilities
                {
                    this.possibleValues[row, c] &= ~(1 << (val - 1));
                }
            }
            // Update the possibilities for all cells in the same column
            for (int r = 0; r < this.boardSize; r++)
            {
                if (r != row && this.possibleValues[r, col] != 0)
                {
                    this.possibleValues[r, col] &= ~(1 << (val - 1));
                }
            }

            // Update the possibilities for the cells in the same subgrid
            int box = this.boxSize;
            int subRow = row / box * box;
            int subCol = col / box * box;
            for (int r = 0; r < box; r++)
            {
                for (int c = 0; c < box; c++)
                {
                    int rr = subRow + r;
                    int cc = subCol + c;
                    if (rr != row && cc != col && this.possibleValues[rr, cc] != 0)
                    {
                        this.possibleValues[rr, cc] &= ~(1 << (val - 1));
                    }
                }
            }
        }

        private void RestorePossibileValues(int row, int col, int val)
        {
            // Restore the possibilities for the cells in the same row
            for (int c = 0; c < this.boardSize; c++)
            {
                if (c != col && this.possibleValues[row, c] != 0)
                {
                    this.possibleValues[row, c] |= (1 << (val - 1)); //update the corresponding bit in the val-1th place
                }
            }

            // Restore the possibilities for the cells in the same column
            for (int r = 0; r < this.boardSize; r++)
            {
                if (r != row && this.possibleValues[r, col] != 0)
                {
                    this.possibleValues[r, col] |= (1 << (val - 1));
                }
            }

            // Restore the possibilities for the cells in the same subgrid
            int box = this.boxSize;
            int subRow = row / box * box; //find row of subgrid
            int subCol = col / box * box; //find column of subgrid
            for (int r = 0; r < box; r++)
            {
                for (int c = 0; c < box; c++)
                {
                    int rr = subRow + r;
                    int cc = subCol + c;
                    if (rr != row && cc != col && this.possibleValues[rr, cc] != 0)
                    {
                        this.possibleValues[rr, cc] |= (1 << (val - 1));
                    }
                }
            }
        }


    }



}

class Progs
{
    static void Main()
    {
        Stopwatch s = new Stopwatch();
        s.Start();
        SudokuBitwiseBoard s1 = new SudokuBitwiseBoard(9,"800000070006010053040600000000080400003000700020005038000000800004050061900002000");
        s1.printBoardFancy();
        //s1.printPossibilities();
        if (s1.SolveBoard())
        {
            s1.printBoardFancy();
        }
        else
        {
            Console.WriteLine("Not solved");
        }
        s.Stop();
        //s1.printPossibilities();
        Console.WriteLine(s.Elapsed);

    }
}

