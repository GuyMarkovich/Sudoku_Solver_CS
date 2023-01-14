using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    internal class UserInterface
    {
        public void printWelcomeMessage()
        {
            Console.WriteLine("Welcome to Sudoku Solver");
            Console.WriteLine("You can enter a sudoku board using either a string or a file");
        }

        public void printMenu()
        {
            Console.WriteLine("1. Enter a string");
            Console.WriteLine("2. Enter a file");
            Console.WriteLine("3. Exit");
        }

        public void printEnterStringMessage()
        {
            Console.WriteLine("Enter a string: ");
        }
        public void printEnterFileMessage()
        {
            Console.WriteLine("Enter a file path including the name of the file: ");
        }


    }
}
