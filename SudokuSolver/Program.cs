using System;

static class constants
{
    //length of the side of the grid
    public const double HEIGHT = 9;
    //width of the grid
    public const double WIDTH = 9;
}

class sudoku
{
    //height of a matrix which is part of the grid
    double mat_height = Math.Sqrt(constants.HEIGHT);
    //width of  a matrix which is part of the grid
    double mat_width = Math.Sqrt(constants.HEIGHT);
}
