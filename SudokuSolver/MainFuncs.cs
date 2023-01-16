using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver;
using static SudokuSolver.Exceptions.CustomExceptions;

namespace SudokuSolver
{
    public static class MainFuncs
    {

        // function to create and process a board, it receives a string and a flag that indicates if the user wants to export the solution to a file, if the flag is true, it receives the path of the file, default path is empty
        public static Boolean processBoard(string nums, Boolean exportToFile, string path = "")
        {
            Boolean solved = false;
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



       



    }
}
