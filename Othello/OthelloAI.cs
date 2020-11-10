using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class OthelloAI
    {
        // tree for storing the potential variations of a move
        MoveTree moveTree;

        // default constructor
        public OthelloAI()
        {
            // initialize variables
            moveTree = new MoveTree();
        }
    }
}
