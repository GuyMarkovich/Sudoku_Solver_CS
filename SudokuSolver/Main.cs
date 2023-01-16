using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static SudokuSolver.User_Interface.UserInterface;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using static SudokuSolver.Exceptions.CustomExceptions;
using SudokuSolver.Main_Backend;

namespace SudokuSolver
{
    class MainProgram
    {
        //main function from where the program should be ran
        static void Main()
        {
            MainFuncs.run();
        }
    }
}
