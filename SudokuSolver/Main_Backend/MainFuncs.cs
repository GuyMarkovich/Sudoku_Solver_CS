using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SudokuSolver.Exceptions.CustomExceptions;
using SudokuSolver.SudokuBoard;
using SudokuSolver.User_Interface;

namespace SudokuSolver.Main_Backend
{
    public static class MainFuncs
    {

        // function to create and process a board, it receives a string and a flag that indicates if the user wants to export the solution to a file, if the flag is true, it receives the path of the file, default path is empty
        public static bool processBoard(string nums, bool exportToFile, string path = "")
        {
            bool solved = false;
            Stopwatch s = new Stopwatch();
            //create a new board
            SudokuBitwiseBoard s1;
            try //try to generate a board from the string given by the user
            {
                s1 = new SudokuBitwiseBoard(nums);
            }
            // if an exception is raised catch and display the error msg, set the solved flag to false
            catch (InvalidInputLengthException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (InvalidInputNumberException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (InvalidInputFileException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            // if no exception is raised, start the timer and solve the board
            s1.hiddenSingles(); //apply hidden singles to the board
            s.Start();
            if (s1.SolveBoard())
            {
                solved = true;
            }
            // stop the timer and display the time elapsed
            s.Stop();
            // if solution solved display the solution
            if (solved)
            {
                //print the board
                UserInterface.printBoard(s1);
                Console.WriteLine(s1.outputSolutionAsStr());
            }
            // else display message that the board could not be solved
            else
            {
                Console.WriteLine("The board could not be solved");
            }

            //print the time it took to solve the board
            Console.WriteLine("Time elapsed to solve: {0}", s.Elapsed);

            //if the user wants to export the board to a file, do so (only if solution  is succesful)
            if (exportToFile && solved)
            {
                UserInterface.exportBoardToFile(s1, path);
            }
            return solved; //return success or failure of solution to main function
        }

        //functions that runs the program
        public static void run()
        {
            UserInterface.printWelcomeMessage();
            string selection = ""; //string holds user selection from main menu
            bool exportToFile = false, solved = false; //flags indicate if solution needs to be exported and if solution is successful
            string inputString = ""; //string holds user input
            string continueCheck = ""; //holds user selection from the continue menu (appears after solution)
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
                        solved = processBoard(inputString, exportToFile);
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
                        solved = processBoard(inputString, exportToFile, inputFilePath);
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
