using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    //this class holds functions that validate the input of the user
    internal class Validation
    {
        //check that the input string is of valid length
        //this solver supports boards of sizes 1x1, 4x4, 9x9, 16x16, 25x25
        public Boolean validateInputLength(string input)
        {
            int totalLength = input.Length;

            //the size of the board should be the square root of the total length of the input string
            double boardSize = Math.Sqrt(totalLength);
            //the size of a box/subgrid in the board should be the square root of the board size
            double boxSize = Math.Sqrt(boardSize);


            // if the board size or box size is not an integer, the input string is invalid
            if (boardSize % 1 != 0 || boxSize % 1 != 0)
            {
                return false;
            }
            else
                return true;     
        }

        //funtion that checks if the current number is valid for a given size of board
        public Boolean validNum(int boardLength, int num)
        {
            if (num < 0 || num > boardLength) //a cell in the sudoku board can hold numbers from 0 (empty cell) up to the size of the board,
                                              //so if the number is outside this range, it is invalid
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }



    }
}
