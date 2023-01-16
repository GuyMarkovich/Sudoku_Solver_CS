﻿using SudokuSolver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SudokuSolver.Exceptions.CustomExceptions;



namespace SudokuSolver
{
    public class SudokuBitwiseBoard
    {
        private int boardSize;
        private int boxSize;
        private int[,] grid;
        private long[,] possibleValues;

        private long[] rowVals;
        private long[] colVals;
        private long[] boxVals;


        //constructor function for a board
        public SudokuBitwiseBoard(string nums)
        {
            Validation val = new Validation(); //used to validate the input string
            if (!val.validateInputLength(nums))
            {
                throw new InvalidInputLengthException("Invalid input string length");
            }
            int boardLength = (int)Math.Sqrt(nums.Length);
            this.boardSize = boardLength;
            this.boxSize = (int)Math.Sqrt(this.boardSize);
            this.grid = new int[boardSize, boardSize];
            this.possibleValues = new long[boardSize, boardSize];

            //initialize the possible values for each row column and box
            this.rowVals = new long[boardSize];
            this.colVals = new long[boardSize];
            this.boxVals = new long[boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                //for each row, column and box, turn on all bits from 0 to boardSize -1
                rowVals[i] = (1 << boardSize) - 1; 
                colVals[i] = (1 << boardSize) - 1;
                boxVals[i] = (1 << boardSize) - 1;
            }


            int iterator = 0;
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    int temp = nums[iterator] - '0'; //convert char to int
                    
                    if (!val.validNum(boardSize, temp)){ //check if current number is valid, if not throw exception
                        throw new InvalidInputNumberException("Invalid number in input string, number is not within the range corresponding to the input length");
                    }
                    if (temp != 0) //if value in given cell is not 0, add it to the matrix, set possibleValues to 0
                    {
                        this.grid[row, col] = temp;
                        this.possibleValues[row, col] = 1 << (temp - 1);

                        
                        // remove the value in the given cell from the row, column and box
                        this.rowVals[row] &= ~(1 << (temp - 1));
                        this.colVals[col] &= ~(1 << (temp - 1));
                        this.boxVals[getBoxNumber(row, col)] &= ~(1 << (temp - 1));
                    }
                    else //else, put a 0 on the board and fill the possible values with bits corresponding to the number of possible options
                    {
                        this.grid[row, col] = 0;
                        this.possibleValues[row, col] = (1 << boardSize) - 1;
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
                    Console.WriteLine("+----------+----------+----------+");
                }

                for (int col = 0; col < this.boardSize; col++)
                {
                    if (col % this.boxSize == 0)
                    {
                        Console.Write("| ");
                    }
                    //convert the number to string for formatting and print
                    String numStr = Convert.ToString(this.grid[row, col]);
                    if (this.grid[row, col] < 10)
                    {
                        //pad the number with a 0 if its a single digit
                        numStr = numStr.PadLeft(2, '0');

                    }
                    Console.Write( numStr+ " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("+----------+----------+----------+");

        }

        //creates a list of strings to print to a file and to the screen
        public List<String> createListforPrint()
        {
            //create a new list
            List<String> outputStr = new List<String> { };

            String line = "";
            for (int row = 0; row < this.boardSize; row++)
            {
                line = ""; //reset line string
                //line borders
                if (row % this.boxSize == 0)
                {
                    outputStr.Add("+----------+----------+----------+");
                }

                for (int col = 0; col < this.boardSize; col++)
                {
                    if (col % this.boxSize == 0)
                    {
                        line += ("| ");
                    }
                    //convert the number to string for formatting and print
                    String numStr = Convert.ToString(this.grid[row, col]);
                    if (this.grid[row, col] < 10)
                    {
                        //pad the number with a 0 if its a single digit
                        numStr = numStr.PadLeft(2, '0');

                    }
                    line += (numStr + " ");
                }
                line +=("|");
                outputStr.Add(line);
            }
            outputStr.Add("+----------+----------+----------+");

            return outputStr;
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
                

                    if (checkPlacementNew(minRow, minCol, val)) //if placement is legal, continue solving
                    {
                        this.grid[minRow, minCol] = val;
                        this.possibleValues[minRow, minCol] = 0;
                        UpdatePossibleValues(minRow, minCol, val);
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




            //update the array of rowVals
            this.rowVals[row] &= ~(1 << (val - 1));
            //update the array of colVals
            this.colVals[col] &= ~(1 << (val - 1));
            //update the array of boxVals
            this.boxVals[getBoxNumber(row,col)] &= ~(1 << (val - 1));

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


            
            //update the array of rowVals
            this.rowVals[row] |= (1 << (val - 1));
            //update the array of colVals
            this.colVals[col] |= (1 << (val - 1));
            //update the array of boxVals
            this.boxVals[getBoxNumber(row, col)] |= (1 << (val - 1));
        }

        int getBoxNumber(int row, int col)
        {
            int box = (row / this.boxSize) * this.boxSize + (col / this.boxSize);
            //box -= 1; //array of boxes uses zero based index, as such we need to subtract 1 from the box number
            return box;
        }


        private bool checkPlacementNew(int row, int col, int val) {
            long mask = 1 << (val - 1);
            if ((this.rowVals[row] & mask) == 0) //check if the value is already in the row
            {
                return false;
            }
            if ((this.colVals[col] & mask) == 0) //check if the value is already in the column
            {
                return false;
            }
            if ((this.boxVals[getBoxNumber(row, col)] & mask) == 0) //check if the value is already in the box
            {
                return false;
            }
            return true;
        }
        

    }



}



