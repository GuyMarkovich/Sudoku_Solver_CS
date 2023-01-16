using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.SudokuBoard;

namespace SudokuSolver.User_Interface
{
    public static class UserInterface
    {
        //welcome message displayed on entry to the program
        public static void printWelcomeMessage()
        {
            Console.WriteLine("Welcome to Sudoku Solver");
            Console.WriteLine("You can enter a sudoku board using either a string or a file");
        }


        // prints menu of options, returns user input of menu options
        public static string printMenu()
        {
            Console.WriteLine("1. Enter a string");
            Console.WriteLine("2. Enter a file");
            Console.WriteLine("3. Exit");
            string selection = Console.ReadLine();
            return selection;
        }


        // message with instructions of entering a board via string to console
        public static void printEnterStringMessage()
        {
            Console.WriteLine("Enter a string: ");
        }

        // message with instructions of entering a board via external file 
        public static void printEnterFileMessage()
        {
            Console.WriteLine("Enter a file path including the name of the file: ");
        }

        // function to export solved board to file, file is created in the same directory as the source file, it shares the same name and is marked by _SOLVED
        public static void exportBoardToFile(SudokuBitwiseBoard board, string filePath)
        {
            string fileDirectory = Path.GetDirectoryName(filePath); //get directory of source file
            string fileName = Path.GetFileName(filePath); //get source file name
            string solvedFileName = fileName.Replace(".txt", "_SOLVED.txt"); //replace original name with name of solution file
            string solvedFilePath = Path.Combine(fileDirectory, solvedFileName); //reatach to directory
            File.AppendAllLines(solvedFilePath, board.createListforPrint()); //export solution to file

        }

        // print board to console
        public static void printBoard(SudokuBitwiseBoard board)
        {
            foreach (string line in board.createListforPrint())
            {
                Console.WriteLine(line);
            }
        }



        // prints message asking if user wants to continue, and returns answer
        public static string printContinueMsg()
        {
            Console.WriteLine("To enter another board press C, to exit press E:");
            string userInput = Console.ReadLine();
            return userInput;
        }

        //exit message displayed before termination
        public static void printExitMsg()
        {
            Console.WriteLine("Goodbye");
        }








    }
}
