using SudokuSolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SudokuSolver
{
    //struct that can store the row, column of a cell
    struct Coordinates
    {
        public int row;
        public int col;
    }
    public class SudokuBoard
    {
        private int boardSize;
        private int boxSize;
        private SudokuCell[,] grid;
        private List<Coordinates> emptyCells;

        //constructor function for the board, default size if not specified otherwise is 9x9
        public SudokuBoard(string nums, int boardSize)
        {
            this.boardSize = boardSize;
            //boxSize refers to the smaller boxes that are part of the sudoku board, ie in a 9x9 board a box will be a 3x3 segment
            this.boxSize = (int)Math.Sqrt(boardSize);
            int iterator = 0;
            this.grid = new SudokuCell[this.boardSize, this.boardSize];
            this.emptyCells = new List<Coordinates>();

            //initialize every cell in the array with an empty sudoku cell
            List<int> possibleValues = new List<int>();
            for (int k = 1; k <= this.boardSize; k++)
            {
                possibleValues.Add(k);
            }
            for (int i = 0; i < this.boardSize; i++)
            {
                for (int j = 0; j < this.boardSize; j++)
                {
                    this.grid[i, j] = new SudokuCell(0, false, i,j, new List<int> (possibleValues));
                }
            }


            for (int i = 0; i< this.boardSize; i++)
            {
                for (int j = 0; j< this.boardSize; j++)
                {
                    //input the number into the correct place in the grid (converting from char to int)
                    int temp = nums[iterator] - '0'; 
                    if (temp != 0)
                    {
                        this.grid[i, j].Value = temp;
                        this.grid[i, j].IsGiven = true;
                        this.grid[i, j].PossibleValues.Clear();
                        this.grid[i, j].PossibleValues.Add(temp); //only possible value for a given space is its given value

                    }
                    //if the cell is empty, add it to the list of empty cells
                    else
                    {
                        Coordinates coords = new Coordinates();
                        coords.row = i;
                        coords.col = j;
                        this.emptyCells.Add(coords); //add the coordinates of the empty cells to the emptyCells array
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
                    Console.Write(this.grid[row, col].Value + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+-------+-------+-------+");

        }




        // function that solves the board
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


        //solution utilizing an array of empty cells
        public bool solveBoardNew(int iterator = 0) //unless given a different value then function will start from the first cell in the array
        {
            if(iterator == this.emptyCells.Count())
            {
                return true;
            }
            //Console.WriteLine(iterator);
            foreach (var num in this.grid[this.emptyCells[iterator].row, this.emptyCells[iterator].col].PossibleValues)
            {
                if (checkPlacement(this.emptyCells[iterator].row, this.emptyCells[iterator].col, num))//check placement for every empty cell
                {
                    this.grid[this.emptyCells[iterator].row, this.emptyCells[iterator].col].Value = num;
                    if (solveBoardNew(++iterator)) //move recursively to the next empty cell
                    {
                        return true;
                    }
                    //reset value of current cell
                    this.grid[this.emptyCells[iterator].row, this.emptyCells[iterator].col].Value = 0;
                    iterator--; //return iterator to current cell}
                }
            }  
            return false;
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


        //function to eliminate candidates from row/col/box when they are present in given cells
        private void simpleElimination(int x, int y, int val)
        {
            //eliminate from same row
            //eliminate from same col
            //eliminate from same box
        }

    }

    
}

//class Progs
//{
//    static void Main()
//    {
//        Stopwatch s = new Stopwatch();
//        s.Start();
//        SudokuBoard s1 = new SudokuBoard("800000070006010053040600000000080400003000700020005038000000800004050061900002000", 9);
//        s1.printBoardFancy();
//        s1.solveBoardNew();
//        s.Stop();
//        s1.printBoardFancy();
//        Console.WriteLine(s.Elapsed);

//    }
//}

