using System;
using System.Collections.Generic;
using System.Linq;

namespace AILab3
{
    class Program
    {
        static void Main()
        {
            const int N = 7;
            Ball[] balls = new Ball[N];
            for (var i = 0; i < N / 2; i++)
                balls[i] = new Ball("W");
            for (var i = N / 2 + 1; i < N; i++)
                balls[i] = new Ball("B");
            balls[N / 2] = new Ball("--");

            var start = new State(3, balls);

            var statesList = new List<State>();

            CreateTree(start, ref statesList); //creating a states tree // 140 states
            var iterations = 0;
            var result = SHCAlgorythm(140, start, statesList, ref iterations); //stochastic hill climb

            foreach (var x in result)
                x.Output();
            Console.WriteLine("Path count: " + result.Count.ToString() + "\tOverall iterations: " + 
                iterations.ToString());
            Console.ReadKey();
        }

        static void CreateTree(State current, ref List<State> stateList)    //creating a states tree
        {
            if (current.IsInList(stateList))
                return;
            stateList.Add(current);
            current.FindNextStates();
            foreach (var x in current.Neighbours)
                CreateTree(x, ref stateList);
        }

        static List<State> SHCAlgorythm(int maxI, State start, List<State> statesList, ref int iterations)
        {                                                           //stochastic hill climb
            var current = start;
            var path = new List<State>();
            while(iterations < maxI) //infinite cycle
            {
                path.Add(current);
                if (current.IfRecordFound())    //end computing if the solution is found
                    return path.Distinct().ToList();

                var neighbours = current.GetNeighboursFromList(statesList); //get neighbours

                var unclosedNeighbours = GetUnclosed(neighbours, path, current);     //filter neighbours from 
                                                                            //states in the path and dead ends

                if(unclosedNeighbours.Count == 0)   // no candidates => returning to previous State
                {
                    current.Closed = true;      //current state is a dead end, we shall not come here
                                                //                                          once more
                    path.RemoveAt(path.Count - 1);
                    path.RemoveAt(path.Count - 1);
                    current = path[path.Count - 1]; //returning to previous step in the path
                    continue;
                }
                State candidate;

                var random = new Random();
                var number = random.Next(0, unclosedNeighbours.Count);  //choosing a random next state
                                                                        //from opened neighbours
                candidate = unclosedNeighbours[number];
                current = candidate;   
                iterations++;
            }
            return path.Distinct().ToList();
        }
        static List<State> GetUnclosed(List<State> neighbours, List<State> path, State current) 
        {       //gets only opened neighbours (which are not in the path and are not dead end)
                //checks hill climb condition - we do not go down to lower cost
            var res = new List<State>();
            foreach(var x in neighbours)
            {
                if (!x.IsInList(path) && !x.Closed && x.Cost >= current.Cost) 
                    res.Add(x);
            }
            return res;
        }
    }
}
