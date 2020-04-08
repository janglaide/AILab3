using System;
using System.Collections.Generic;

namespace AILab3
{
    public class State
    {
        private static int _N;
        private readonly Ball[] _balls = new Ball[_N];   //balls array in current state
        private readonly int _ptr;   //empty place`s index
        private readonly int _cost;
        private bool _closed;
        private List<State> _neighbours = new List<State>();
        public List<State> Neighbours { get => _neighbours; }   //neighbour states
        public int Cost { get => _cost; }
        public bool Closed { get => _closed; set => _closed = value; }
        public Ball this[int i]
        {
            get => _balls[i];
            private set => _balls[i] = value;
        }
        public State(int ptr, Ball[] balls)
        {
            _ptr = ptr;
            _N = balls.Length;
            _balls = balls;
            _closed = false;
            _cost = GetCost();
        }
        private int GetCost()
        {
            var counter = 0;
            for(var i = 0; i < _N / 2; i++)
            {
                if (this[i].Color == "B")
                    counter++;
            }
            for(var i = _N / 2 + 1; i < _N; i++)
            {
                if (this[i].Color == "W")
                    counter++;
            }
            return counter;
        }
        public void FindNextStates() //finds next states (neighbours)
        {
            var states = new List<State>();

            if(_ptr == 0)   //x------
            {
                states.Add(NextState(1));
                states.Add(NextState(2));
            }
            else if (_ptr == _N - 1) //------x
            {
                states.Add(NextState(-1));
                states.Add(NextState(-2));
            }
            else if (_ptr == 1) //-x-----
            {
                states.Add(NextState(1));
                states.Add(NextState(2));
                states.Add(NextState(-1));
            }
            else if(_ptr == _N - 2)//-----x-
            {
                states.Add(NextState(-1));
                states.Add(NextState(-2));
                states.Add(NextState(1));
            }
            else//--x---- or ---x--- or ----x--
            {
                states.Add(NextState(1));
                states.Add(NextState(2));
                states.Add(NextState(-1));
                states.Add(NextState(-2));
            }
            _neighbours = states;
        }
        private State NextState(int op)     //subfunction for creating new states
        {
            var tmpBalls = CopyBalls(_balls);
            Swap(ref tmpBalls[_ptr], ref tmpBalls[_ptr + op]);
            var newState = new State(_ptr + op, tmpBalls);
            return newState;
        }
        private Ball[] CopyBalls(Ball[] balls)
        {
            var res = new Ball[_N];
            for (var i = 0; i < _N; i++)
                res[i] = balls[i];
            return res;
        }
        private void Swap(ref Ball a, ref Ball b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }
        public bool IfRecordFound() //returns true if we found a solution
        {
            return _cost == 6 ? true : false;
        }
        public bool IsInList(List<State> list)  //returns true if State is in list
        {
            foreach (var x in list)
                if (x.Equals(this))
                    return true;
            return false;
        }
        public List<State> GetNeighboursFromList(List<State> statesList)
        {                           // gets neighbours for state from done states list
            var list = new List<State>();
            foreach (var x in statesList)
            {
                if (x.Equals(this))
                {
                    foreach (var y in x.Neighbours)
                        list.Add(y);
                    break;
                }
            }
            return list;
        }
        private bool Equals(State state)
        {
            if (state == null)
                return false;
            for (var x = 0; x < _N; x++)
                if (this[x].Color != state._balls[x].Color)
                    return false;
            return true;
        }
        public void Output()
        {
            foreach(var x in _balls)
                Console.Write(x.Color + "   ");
            Console.WriteLine("\n"); 
        }
    }
}
