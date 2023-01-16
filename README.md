This is a sudoku solving program that utilizes bitwise backtracking to solve a given board
A board can be submitted using a string of ASCII characters beggining with 0 to mark empty cells, and corresponding characters for the numbers after
Example:
characters '1'-'9' represent digits 1-9, character ':' represnts 10, etc

A board can be entered via a string directly from console, or an external file using a full path to file
If a board is entered via a file, a solution file will be created in the same directory.

In all cases a solution if one is found will be displayed to console.




* Due to utilizing a backtracking algorithm, this solver will struglle with harder boards despite my best efforts (in the time given) to optimize the solution function
