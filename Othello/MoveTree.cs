using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class MoveTree
    {
        // class for nodes in tree
        public class TreeNode
        {
            // stores reference to parent node
            public TreeNode Parent { get; private set; }

            // stores references to child nodes
            public List<TreeNode> Children { get; private set; }

            // stores ply that tree node is at
            public int Ply { get; set; }

            // default constructor
            public TreeNode(TreeNode parent)
            {
                Parent = parent;
                Children = new List<TreeNode>();
                Ply = parent.Ply + 1;
            }

            // add a child to the node
            public void AddChild(TreeNode newNode)
            {
                Children.Add(newNode);
            }
        }

        // store reference to root node of tree
        public TreeNode Root { get; private set; }

        // default constructor
        public MoveTree()
        {
            Root = new TreeNode(null);
        }

        // TODO: Finish this class
    }
}
