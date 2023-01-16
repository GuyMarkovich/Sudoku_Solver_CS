using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
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
        public static String printMenu()
        {
            Console.WriteLine("1. Enter a string");
            Console.WriteLine("2. Enter a file");
            Console.WriteLine("3. Exit");
            String selection = Console.ReadLine();
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
        public static String printContinueMsg()
        {
            Console.WriteLine("To enter another board press C, to exit press E:");
            String userInput = Console.ReadLine();
            return userInput;
        }

        //exit message displayed before termination
        public static void printExitMsg()
        {
            Console.WriteLine("Goodbye");
        }




        public static void run()
        {
            UserInterface.printWelcomeMessage();
            String selection = ""; //string holds user selection from main menu
            Boolean exportToFile = false, solved = false; //flags indicate if solution needs to be exported and if solution is successful
            String inputString = ""; //string holds user input
            String continueCheck = ""; //holds user selection from the continue menu (appears after solution)
            while (selection != "3") // 3 indicates exit from the program
            {
                exportToFile = false; //reset the flags for each board input
                solved = false;
                selection = UserInterface.printMenu(); //get user selection
                switch (selection) 
                {
                    // enter board using string
                    case "1":
                        UserInterface.printEnterStringMessage();
                        inputString = Console.ReadLine(); //get board from console
                        solved = MainFuncs.processBoard(inputString, exportToFile);
                        break;
                    // enter board from file
                    case "2":
                        exportToFile = true; //set export flag
                        UserInterface.printEnterFileMessage(); 
                        string inputFilePath = Console.ReadLine(); //get path to source file
                        if (!File.Exists(inputFilePath)) //check that the file exists on the given path
                        {
                            throw new FileNotFoundException("File does not exist or path is incorrect");
                        }
                        // get board from file and process
                        inputString = File.ReadAllText(inputFilePath);
                        solved = MainFuncs.processBoard(inputString, exportToFile, inputFilePath);
                        break;

                    //exit
                    case "3":
                        break;
                    // if anything other than those 3 options is entered, print an error message, let user try again (and re print menu)
                    default:
                        Console.WriteLine("Invalid instruction, please enter a valid instruction from the provided menu");
                        break;
                }
                if (solved) // if solved display message, user can choose to exit or continue
                {
                    Console.WriteLine("Board solution successful");
                    continueCheck = UserInterface.printContinueMsg();
                }
                else // if not solved display message, user can choose to exit or continue
                {
                    Console.WriteLine("Board solution not successful");
                    continueCheck = UserInterface.printContinueMsg();
                }
                switch (continueCheck) //process user choice from continue menu
                {   //both upper and lower case input will be accepted
                    // if C/c entered, no need to exit, break the switch case and re run the function
                    case "C":
                    case "c":
                        break;
                    //for E/e set selection to "3" (exit condition) break the switch
                    case "E":
                    case "e":
                        selection = "3"; //pass exit code to function
                        break;
                    // for anything else, display message about unrecognized msg, exit the function
                    default:
                        Console.WriteLine("Unrecognized instruction received, exiting the program");
                        selection = "3";
                        break;
                }

            }
            UserInterface.printExitMsg();
        }



    }
}
