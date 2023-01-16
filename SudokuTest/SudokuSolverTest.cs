namespace SudokuTest
{
    [TestClass]
    public class SudokuSolverTest
    {
        [TestMethod]
        public void TestSudokuBasic9x9Board()
        {
            string boardString = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";

            SudokuSolver.SudokuBoard.SudokuBitwiseBoard board = new SudokuSolver.SudokuBoard.SudokuBitwiseBoard(boardString);
            board.SolveBoard();
            string solution = board.outputSolutionAsStr();

            Assert.AreEqual("831529674796814253542637189159783426483296715627145938365471892274958361918362547", solution); //check if the solution is correct, throws exception if not

        }

        [TestMethod]
        public void TestSudokuHarder9x9Board()
        {
            string boardString = "100000027000304015500170683430962001900007256006810000040600030012043500058001000";

            SudokuSolver.SudokuBoard.SudokuBitwiseBoard board = new SudokuSolver.SudokuBoard.SudokuBitwiseBoard(boardString);
            board.SolveBoard();
            string solution = board.outputSolutionAsStr();

            Assert.AreEqual("193586427867324915524179683435962871981437256276815349749658132612743598358291764", solution);


        }

        [TestMethod]
        public void TestSudoku16x16Board()
        {
            string boardString = "10023400<06000700080007003009:6;0<00:0010=0;00>0300?200>000900<0=000800:0<201?000;76000@000?005=000:05?0040800;0@0059<00100000800200000=00<580030=00?0300>80@000580010002000=9?000<406@0=00700050300<0006004;00@0700@050>0010020;1?900=002000>000>000;0200=3500<";

            SudokuSolver.SudokuBoard.SudokuBitwiseBoard board = new SudokuSolver.SudokuBoard.SudokuBitwiseBoard(boardString);
            board.SolveBoard();
            string solutionFromProgram = board.outputSolutionAsStr();
            string correctSolution = "15:2349;<@6>?=78>@8=5?7<43129:6;9<47:@618=?;35>236;?2=8>75:94@<1=4>387;:5<261?@98;76412@9:>?<35=<91:=5?634@8>2;7@?259<>31;7=:68462@>;94=?1<587:37=91?235;>8:@<46583;1:<7264@=9?>?:<4>6@8=9372;152358<>:?6794;1=@:7=<@359>8;1642?;1?968=4@25<7>3:4>6@7;12:?=3589<";


            Assert.AreEqual(solutionFromProgram, correctSolution);


        }
    }
}