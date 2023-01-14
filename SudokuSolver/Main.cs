using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static SudokuSolver.UserInterface;

namespace SudokuSolver
{
    class MainProgram
    {
        static void Main()
        {

            UserInterface ui = new UserInterface();
            ui.printWelcomeMessage();
            ui.printMenu();
            string input = "0";
            while (input != "3")
            {
                input = Console.ReadLine();
                string inputString = "";
                switch (input)
                {
                    // enter board using string
                    case "1":
                        ui.printEnterStringMessage();
                        inputString = Console.ReadLine();
                        break;
                    // enter board from file
                    case "2":
                        ui.printEnterFileMessage();
                        string inputFilePath = Console.ReadLine();
                        inputString = File.ReadAllText(inputFilePath);
                        break;

                    //exit
                    case "3":
                        break;
                    // if anything other than those 3 options is entered, print an error message, try again
                    default:
                        Console.WriteLine("Invalid instruction, please enter a valid instruction from the provided menu");
                        ui.printMenu();
                        break;
                }
                
            }
            

            Stopwatch s = new Stopwatch();
            s.Start();
            SudokuBitwiseBoard s1 = new SudokuBitwiseBoard("800000070006010053040600000000080400003000700020005038000000800004050061900002000");
            s1.printBoardFancy();
            //s1.printPossibilities();
            if (s1.SolveBoard())
            {
                s.Stop();
                s1.printBoardFancy();

            }
            else
            {
                Console.WriteLine("Not solved");
            }

            //s1.printPossibilities();
            Console.WriteLine(s.Elapsed);
        }
    }
}
