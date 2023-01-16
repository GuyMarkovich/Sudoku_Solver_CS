using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SudokuSolver.Exceptions.CustomExceptions;



namespace SudokuSolver.SudokuBoard
{
    public class SudokuBitwiseBoard
    {
        private int _boardSize; //size of the board (height, width)
        private int _boxSize; //size of a subgrid
        private int[,] _grid; //the grid that holds the numbers of the board
        private long[,] _possibleValues; //grid that holds possible values for each cell

        private long[] _rowVals; //possible values for each row
        private long[] _colVals; //possible values for each column
        private long[] _boxVals; // possible values for each subgrid


        //constructor function for a board
        public SudokuBitwiseBoard(string nums)
        {
            Validation val = new Validation(); //used to validate the input string
            if (!val.validateInputLength(nums))
            {
                throw new InvalidInputLengthException("Invalid input string length");
            }
            int boardLength = (int)Math.Sqrt(nums.Length); // size of the board equals the square root of the length of the input string
            _boardSize = boardLength; //set the size of the board
            _boxSize = (int)Math.Sqrt(_boardSize); // set the size of a subgrid
            _grid = new int[_boardSize, _boardSize]; // initialize grid
            _possibleValues = new long[_boardSize, _boardSize]; // initialize possible values grid

            //initialize the possible values for each row column and box
            _rowVals = new long[_boardSize];
            _colVals = new long[_boardSize];
            _boxVals = new long[_boardSize];

            for (int i = 0; i < _boardSize; i++)
            {
                //for each row, column and box, turn on all bits from 0 to _boardSize -1
                _rowVals[i] = (1 << _boardSize) - 1;
                _colVals[i] = (1 << _boardSize) - 1;
                _boxVals[i] = (1 << _boardSize) - 1;
            }


            int iterator = 0; // iterator to go over input string
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    int temp = nums[iterator] - '0'; //convert char to int

                    if (!val.validNum(_boardSize, temp))
                    { //check if current number is valid, if not throw exception
                        throw new InvalidInputNumberException("Invalid number in input string, number is not within the range corresponding to the input length");
                    }
                    if (temp != 0) //if value in given cell is not 0, add it to the matrix, set _possibleValues to 0
                    {
                        _grid[row, col] = temp;
                        _possibleValues[row, col] = 1 << temp - 1;


                        // remove the value in the given cell from the row, column and box possibilities
                        _rowVals[row] &= ~(1 << temp - 1);
                        _colVals[col] &= ~(1 << temp - 1);
                        _boxVals[getBoxNumber(row, col)] &= ~(1 << temp - 1);
                    }
                    else //else, put a 0 on the board and fill the possible values with bits corresponding to the number of possible options
                    {
                        _grid[row, col] = 0;
                        _possibleValues[row, col] = (1 << _boardSize) - 1;
                    }
                    iterator++;
                }
            }

            //in the possibilities array, remove possibilities that are already present in the same row column or box
            for (int row = 0; row < _boardSize; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    if (_grid[row, col] != 0)
                    {
                        _possibleValues[row, col] = _possibleValues[row, col] & _rowVals[row]; //if the number is a possibility for the row and current cell keeep it on, else turn to 0
                        _possibleValues[row, col] = _possibleValues[row, col] & _colVals[col]; //if the number is a possibility for the column and current cell keeep it on, else turn to 0
                        _possibleValues[row, col] = _possibleValues[row, col] & _boxVals[getBoxNumber(row, col)]; //if the number is a possibility for the box and current cell keeep it on, else turn to 0
                    }
                }
            }




        }



        //creates a list of strings to print to a file and to the screen
        public List<string> createListforPrint()
        {
            //create a new list
            List<string> outputStr = new List<string> { };

            string line = "";
            for (int row = 0; row < _boardSize; row++)
            {
                line = ""; //reset line string
                //line borders
                if (row % _boxSize == 0)
                {
                    outputStr.Add("+----------+----------+----------+");
                }

                for (int col = 0; col < _boardSize; col++)
                {
                    if (col % _boxSize == 0)
                    {
                        line += "| ";
                    }
                    //convert the number to string for formatting and print
                    string numStr = Convert.ToString(_grid[row, col]);
                    if (_grid[row, col] < 10)
                    {
                        //pad the number with a 0 if its a single digit
                        numStr = numStr.PadLeft(2, '0');

                    }
                    line += numStr + " ";
                }
                line += "|";
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
            Boolean exitedInner = false; //flag that allows us to break the loop in case cell with 1 possibility found 
            for (int row = 0; row < _boardSize && !exitedInner; row++)
            {
                for (int col = 0; col < _boardSize; col++)
                {
                    if (_grid[row, col] == 0) //if the current cell is not a given one
                    {
                        int currPossibilities = CountPossibleValues(row, col);
                        if (currPossibilities < minPossibilities)
                        {
                            minRow = row;
                            minCol = col;
                            minPossibilities = currPossibilities;
                        }
                        if (minPossibilities == 1)
                        {
                            exitedInner = true;
                            break;
                        }
                    }
                }
            }

            if (minRow == -1) //condition to stop the recursion, if minRow stays -1 no empty cells are left and the board is solved
            {
                return true;
            }

            for (int val = 1; val <= _boardSize; val++)
            {
                long mask = 1 << val - 1; //set mask to value we want to check

                if (checkPlacement(minRow, minCol, val)) //if placement is legal, continue solving
                {
               
                    _grid[minRow, minCol] = val;
                    _possibleValues[minRow, minCol] = 0;
                    UpdatePossibleValues(minRow, minCol, val);
                    bool solvedNext = SolveBoard(); //try to continue solving
                    if (solvedNext) //if solution successful return true
                    {
                        return true;
                    }
                    //if no solution found reset the value in the current cell and restore the checked value for all other affected cells
                    _grid[minRow, minCol] = 0;
                    _possibleValues[minRow, minCol] = (1 << _boardSize) - 1;
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
            for (int val = 1; val <= _boardSize; val++)
            {
                long mask = 1 << val - 1;
                if ((_possibleValues[row, col] & mask) != 0) //if bit is set, the corresponding value is a possibility, increase count
                {
                    count++;
                }
            }
            return count;
        }

        //function that updates possible values for all cells in a row column and box,
        //the function receives the row, column and value of the cell currently being processed
        private void UpdatePossibleValues(int row, int col, int val)
        {
            // Update the possibilities for the cells in the same row
            for (int c = 0; c < _boardSize; c++)
            {
                if (c != col && _possibleValues[row, c] != 0) //for every column that is in the same row,
                                                                   //remove the current cells value from the possibilities
                {
                    _possibleValues[row, c] &= ~(1 << val - 1);
                }
            }

            // Update the possibilities for all cells in the same column
            for (int r = 0; r < _boardSize; r++)
            {
                if (r != row && _possibleValues[r, col] != 0)
                {
                    _possibleValues[r, col] &= ~(1 << val - 1);
                }
            }

            // Update the possibilities for the cells in the same subgrid
            int box = _boxSize;
            int subRow = row / box * box;
            int subCol = col / box * box;
            for (int r = 0; r < box; r++)
            {
                for (int c = 0; c < box; c++)
                {
                    int rr = subRow + r;
                    int cc = subCol + c;
                    if (rr != row && cc != col && _possibleValues[rr, cc] != 0)
                    {
                        _possibleValues[rr, cc] &= ~(1 << val - 1);
                    }
                }
            }




            //update the array of _rowVals
            _rowVals[row] &= ~(1 << val - 1);
            //update the array of _colVals
            _colVals[col] &= ~(1 << val - 1);
            //update the array of _boxVals
            _boxVals[getBoxNumber(row, col)] &= ~(1 << val - 1);

        }

        private void RestorePossibileValues(int row, int col, int val)
        {
            // Restore the possibilities for the cells in the same row
            for (int c = 0; c < _boardSize; c++)
            {
                if (c != col && _possibleValues[row, c] != 0)
                {
                    _possibleValues[row, c] |= 1 << val - 1; //update the corresponding bit in the val-1th place
                }
            }

            // Restore the possibilities for the cells in the same column
            for (int r = 0; r < _boardSize; r++)
            {
                if (r != row && _possibleValues[r, col] != 0)
                {
                    _possibleValues[r, col] |= 1 << val - 1;
                }
            }

            // Restore the possibilities for the cells in the same subgrid
            int box = _boxSize;
            int subRow = row / box * box; //find row of subgrid
            int subCol = col / box * box; //find column of subgrid
            for (int r = 0; r < box; r++)
            {
                for (int c = 0; c < box; c++)
                {
                    int rr = subRow + r;
                    int cc = subCol + c;
                    if (rr != row && cc != col && _possibleValues[rr, cc] != 0)
                    {
                        _possibleValues[rr, cc] |= 1 << val - 1;
                    }
                }
            }



            //update the array of _rowVals
            _rowVals[row] |= 1 << val - 1;
            //update the array of _colVals
            _colVals[col] |= 1 << val - 1;
            //update the array of _boxVals
            _boxVals[getBoxNumber(row, col)] |= 1 << val - 1;
        }

        //get subgrid number
        int getBoxNumber(int row, int col)
        {
            int box = row / _boxSize * _boxSize + col / _boxSize;
            return box;
        }


        private bool checkPlacement(int row, int col, int val)
        {
            long mask = 1 << val - 1;
            if ((_rowVals[row] & mask) == 0) //check if the value is already in the row
            {
                return false;
            }
            if ((_colVals[col] & mask) == 0) //check if the value is already in the column
            {
                return false;
            }
            if ((_boxVals[getBoxNumber(row, col)] & mask) == 0) //check if the value is already in the box
            {
                return false;
            }
            return true;
        }


        //function used to output the solved board to a string for easier testing
        public string outputSolutionAsStr()
        {
            string solution = "";
            for (int row = 0; row< this._boardSize; row++)
            {
                for (int col = 0; col<this._boardSize; col++)
                {
                    solution += this._grid[row,col];
                }
            }
            return solution;
        }




        // applies hidden singles on the current board, checks if only one cell in a given row, col, box has a candidate of a certain value if true it sets that cell to value 
        public bool hiddenSingles()
        {
            // flag to check if any changes have been made to the board
            bool changesMade = false;

            // for each row
            for (int row = 0; row < this._boardSize; row++)
            {
                // for each value
                for (int value = 1; value <= this._boardSize; value++)
                {
                    // count the number of cells in the row that have the value as a candidate
                    int count = 0;
                    int col = 0;

                    long mask = 1 << (value - 1); //bit mask

                    for (int i = 0; i < _boardSize; i++)
                    {
                        if ((_possibleValues[row,i] & mask) !=0 )
                        {
                            count++;
                            col = i;
                        }
                    }

                    // if there is only one cell in the row that has the value as a candidate
                    // then set the value of that cell to the value
                    if (count == 1)
                    {
                        _grid[row,col] = value;
                        UpdatePossibleValues(row, col, value);
                        changesMade = true;
                    }
                }
            }

            // for each column
            for (int col = 0; col < _boardSize; col++)
            {
                // for each value
                for (int value = 1; value <= _boardSize; value++)
                {
                    // count the number of cells in the column that have the value as a candidate
                    int count = 0;
                    int row = 0;

                    long mask = 1 << value - 1; //bit mask

                    for (int i = 0; i < _boardSize; i++)
                    {
                        if ((_possibleValues[i, col] & mask) != 0)
                        {
                            count++;
                            row = i;
                        }
                    }

                    // if there is only one cell in the column that has the value as a candidate
                    // then set the value of that cell to the value
                    if (count == 1)
                    {
                        _grid[row, col] = value;
                        UpdatePossibleValues(row, col, value);
                        changesMade = true;
                    }
                }
            }

            // for each block
            for (int blockRow = 0; blockRow < _boxSize; blockRow++)
            {
                for (int blockCol = 0; blockCol < _boxSize; blockCol++)
                {
                    // for each value
                    for (int value = 1; value <= _boardSize; value++)
                    {
                        // count the number of cells in the block that have the value as a candidate
                        int count = 0;
                        int row = 0;
                        int col = 0;

                        long mask = 1 << value - 1; //bit mask

                        for (int i = blockRow * _boxSize; i < blockRow * _boxSize + _boxSize; i++)
                        {
                            for (int j = blockCol * _boxSize; j < blockCol * _boxSize + _boxSize; j++)
                            {
                                if ((_possibleValues[i, col] & mask) != 0)
                                {
                                    count++;
                                    row = i;
                                    col = j;
                                }
                            }
                        }

                        // if there is only one cell in the block that has the value as a candidate
                        // then set the value of that cell to the value
                        if (count == 1)
                        {
                            _grid[row, col] = value;
                            UpdatePossibleValues(row, col, value);
                            changesMade = true;
                        }
                    }
                }
            }
            // return true if any changes were made, false otherwise
            return changesMade;
        }
























    }


}



