using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions
{
   
    public class CustomExceptions
    {

        //this class holds custom exceptions that are used in the program

        //exception raised when the input string is not of valid length supported by the program
        public class InvalidInputLengthException : Exception
        {
            public InvalidInputLengthException()
            {
            }

            public InvalidInputLengthException(string message)
                : base(message)
            {
            }

            public InvalidInputLengthException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        // exception raised when the input string contains a number that is not valid for the size of the board
        public class InvalidInputNumberException : Exception
        {
            public InvalidInputNumberException()
            {
            }

            public InvalidInputNumberException(string message)
                : base(message)
            {
            }

            public InvalidInputNumberException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        // exception raised when the input file is not valid (not found)
        public class InvalidInputFileException : Exception
        {
            public InvalidInputFileException()
            {
            }

            public InvalidInputFileException(string message)
                : base(message)
            {
            }

            public InvalidInputFileException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }


    }
}
