using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using SearchAlgorithmsLib;

namespace Server
{
    /// <summary>
    /// class that creates a solid searchable object
    /// </summary>
    class ObjectAdapter : ISearchable<Position>
    {
        /// <summary>
        /// the members
        /// </summary>
        /// <remarks> specificly to this adapter we use a maze,
        ///           the maze start and goal state, rows and colomns
        /// </remarks>
        Maze myMaze;
        State<Position> start;
        State<Position> goal;
        int rowsNumber;
        int columnsNumber;

        /// <summary>
        /// the constructor
        /// </summary>
        /// <param name="maze"> maze object</param>
        public ObjectAdapter(Maze maze)
        {
            myMaze = maze;
            rowsNumber = maze.Rows;
            columnsNumber = maze.Cols;
            start = new State<Position>(myMaze.InitialPos);
            goal = new State<Position>(myMaze.GoalPos);
        }

        /// <summary>
        /// finds and return all possible states of the given state "s"
        /// </summary>
        /// <param name="s"> state as position, has x and y axises </param>
        /// <returns> a list of all neighbouring states </returns>
        public List<State<Position>> GetAllPossibleStates(State<Position> s)
        {
            List<State<Position>> neighbouringStates = new List<State<Position>>();
            int row = s.GetState().Row;
            int col = s.GetState().Col;
            //checks if s has a neighbour above
            if ((row - 1 >= 0) && myMaze[row - 1, col] == CellType.Free)
            {
                Position tmpPos = new Position(row - 1, col);
                State<Position> tmpState = new State<Position>(tmpPos);
                tmpState.SetFatherState(s);
                tmpState.SetCost(1);
                tmpState.SetCost(s);
                neighbouringStates.Add(tmpState);
            }
            //checks if s has a left neighbour 
            if ((col - 1 >= 0) && myMaze[row, col - 1] == CellType.Free)
            {
                Position tmpPos = new Position(row, col - 1);
                State<Position> tmpState = new State<Position>(tmpPos);
                tmpState.SetFatherState(s);
                tmpState.SetCost(1);
                tmpState.SetCost(s);
                neighbouringStates.Add(tmpState);
            }
            //checks if s has a right neighbour 
            if ((col + 1 < columnsNumber) && myMaze[row, col + 1] == CellType.Free)
            {
                Position tmpPos = new Position(row, col + 1);
                State<Position> tmpState = new State<Position>(tmpPos);
                tmpState.SetFatherState(s);
                tmpState.SetCost(1);
                tmpState.SetCost(s);
                neighbouringStates.Add(tmpState);
            }
            //checks if s has a neighbour below
            if ((row + 1 < rowsNumber) && myMaze[row + 1, col] == CellType.Free)
            {
                Position tmpPos = new Position(row + 1, col);
                State<Position> tmpState = new State<Position>(tmpPos);
                tmpState.SetFatherState(s);
                tmpState.SetCost(1);
                tmpState.SetCost(s);
                neighbouringStates.Add(tmpState);
            }
            return neighbouringStates;

        }

        /// <summary>
        /// returns the goal state of the puzzle
        /// </summary>
        /// <returns> state position </returns>
        public State<Position> GetGoalState()
        {
            return this.goal;
        }

        /// <summary>
        /// returns the initial state of the puzzle
        /// </summary>
        /// <returns> state position </returns>
        public State<Position> GetInitialState()
        {
            return this.start;
        }
    }
}
