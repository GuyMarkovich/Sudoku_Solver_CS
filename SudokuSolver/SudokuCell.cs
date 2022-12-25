using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuCell
    {
        public int Value { get; set; } //value of the cell
        public bool IsGiven { get; set; } // is the cell given when the puzzle is loaded
        public bool IsSolved { get; set; } // is the cell solved
        public int Row { get; set; } // row of the cell
        public int Column { get; set; } // column of the cell
        public List<int> PossibleValues { get; set; } //possible values that can be assigned to the cell

        public SudokuCell(int value, bool isGiven, int row, int column, int matrix) // constructor function
        {
            Value = value;
            IsGiven = isGiven;
            IsSolved = false;
            Row = row;
            Column = column;
            PossibleValues = new List<int>();
        }
        public void addPossibleValue(int value) //add possible values to list
        {
            PossibleValues.Add(value);
        }
    }
}
