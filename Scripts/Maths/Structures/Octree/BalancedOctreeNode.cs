using UnityEngine;

namespace EditorTools.Maths.Structures.Octree
{
    /// <summary>
    /// abstract Octree node implementation maintaining balance at all times
    /// </summary>
    /// <typeparam name="V">Type of this node</typeparam>
    public abstract class BalancedOctreeNode<V> : OctreeNode<V> where V : BalancedOctreeNode<V>
    {
        public const int X_POSITIVE = 0;
        public const int X_NEGATIVE = 1;
        public const int Y_POSITIVE = 2;
        public const int Y_NEGATIVE = 3;
        public const int Z_POSITIVE = 4;
        public const int Z_NEGATIVE = 5;
        
        public const int X_POSITIVE_BITMASK = 0b00_00_01;
        public const int X_NEGATIVE_BITMASK = 0b00_00_10;
        public const int Y_POSITIVE_BITMASK = 0b00_01_00;
        public const int Y_NEGATIVE_BITMASK = 0b00_10_00;
        public const int Z_POSITIVE_BITMASK = 0b01_00_00;
        public const int Z_NEGATIVE_BITMASK = 0b10_00_00;
        
        protected V[] neighbours;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        protected BalancedOctreeNode(Vector3 lowerCorner, Vector3 upperCorner) : base(lowerCorner, upperCorner)
        {
            neighbours = new V[6];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        /// <param name="parent">parent of this node</param>
        /// <param name="depth">depth of this node</param>
        protected BalancedOctreeNode(Vector3 lowerCorner, Vector3 dimensions, V parent, uint depth) : base(lowerCorner, dimensions, parent, depth)
        {
            neighbours = new V[6];
        }

        /// <summary>
        /// Sets the neighbour of this node in the given direction
        /// </summary>
        /// <param name="neighbour">Position of the neighbour</param>
        /// <param name="node">Node to set as neighbour</param>
        protected void setNeighbour(int neighbour, V node)
        {
            this.neighbours[neighbour] = node;
        }

        /// <summary>
        /// Neighbour children to each other
        /// </summary>
        /// <param name="dir">Axis of children to neighbour to each other. USE ONLY POSITIVE DIRECTIONS FOR THIS</param>
        /// <param name="neg">child in negative direction along axis</param>
        /// <param name="pos">child in positive direction along axis</param>
        private void neighbourChildren(int dir, int neg, int pos)
        {
            // TODO: Ensure direction is positive variant ( dir & 0xfffffffe ?)
            this.children[neg].setNeighbour(dir, this.children[pos]);
            this.children[pos].setNeighbour(dir+1, this.children[neg]);
        }

        /// <summary>
        /// Neighbour children to each other
        /// </summary>
        /// <param name="dir">Axis of children to neighbour to each other. USE ONLY POSITIVE DIRECTIONS FOR THIS</param>
        /// <param name="ownidx">own child index</param>
        /// <param name="othidx">other nodes child index</param>
        /// <param name="other">Other node, whose child to neighbour</param>
        private void neighbour(int dir, int ownidx, int othidx, BalancedOctreeNode<V> other)
        {
            this.children[ownidx].setNeighbour(dir, other.children[othidx]);
            other.children[othidx].setNeighbour(dir^0x01, this.children[ownidx]);
        }
        
        /// <summary>
        /// expand this octree node
        /// </summary>
        public override void expand()
        {
            base.expand(); // Expand the Octree
            
            // Update Neighbours of this node
            updateNeighbours();
            
            //TODO: Handle also Indirect neighbours ! 
            
            // Update Neighbour information for Children
            neighbourChildren(X_POSITIVE, 0, 1);
            neighbourChildren(X_POSITIVE, 2, 3);
            neighbourChildren(X_POSITIVE, 4, 5);
            neighbourChildren(X_POSITIVE, 6, 7);
            
            neighbourChildren(Y_POSITIVE, 0, 2);
            neighbourChildren(Y_POSITIVE, 1, 3);
            neighbourChildren(Y_POSITIVE, 4, 6);
            neighbourChildren(Y_POSITIVE, 5, 7);
            
            neighbourChildren(Z_POSITIVE, 0, 4);
            neighbourChildren(Z_POSITIVE, 1, 5);
            neighbourChildren(Z_POSITIVE, 2, 6);
            neighbourChildren(Z_POSITIVE, 3, 7);

            V n = neighbours[X_NEGATIVE];

            if (n != null)
            {
                if (n.isLeaf())
                {
                    children[0].setNeighbour(X_NEGATIVE, n);
                    children[2].setNeighbour(X_NEGATIVE, n);
                    children[4].setNeighbour(X_NEGATIVE, n);
                    children[6].setNeighbour(X_NEGATIVE, n);
                }
                else
                {
                    neighbour(X_NEGATIVE, 0, 1, n);
                    neighbour(X_NEGATIVE, 2, 3, n);
                    neighbour(X_NEGATIVE, 4, 5, n);
                    neighbour(X_NEGATIVE, 6, 7, n);
                }
            }
            
            n = neighbours[X_POSITIVE];
            
            if (n != null)
            {
                if (n.isLeaf())
                {
                    children[1].setNeighbour(X_POSITIVE, n);
                    children[3].setNeighbour(X_POSITIVE, n);
                    children[5].setNeighbour(X_POSITIVE, n);
                    children[7].setNeighbour(X_POSITIVE, n);
                }
                else
                {
                    neighbour(X_POSITIVE, 1, 0, n);
                    neighbour(X_POSITIVE, 3, 2, n);
                    neighbour(X_POSITIVE, 5, 4, n);
                    neighbour(X_POSITIVE, 7, 6, n);
                }
            }

            n = neighbours[Y_NEGATIVE];

            if (n != null)
            {
                if (n.isLeaf())
                {
                    children[0].setNeighbour(Y_NEGATIVE, n);
                    children[1].setNeighbour(Y_NEGATIVE, n);
                    children[4].setNeighbour(Y_NEGATIVE, n);
                    children[5].setNeighbour(Y_NEGATIVE, n);
                }
                else
                {
                    neighbour(Y_NEGATIVE, 0, 2, n);
                    neighbour(Y_NEGATIVE, 1, 3, n);
                    neighbour(Y_NEGATIVE, 4, 6, n);
                    neighbour(Y_NEGATIVE, 5, 7, n);
                }
            }

            n = neighbours[Y_POSITIVE];

            if (n != null)
            {
                if (n.isLeaf())
                {
                    children[2].setNeighbour(Y_POSITIVE, n);
                    children[3].setNeighbour(Y_POSITIVE, n);
                    children[6].setNeighbour(Y_POSITIVE, n);
                    children[7].setNeighbour(Y_POSITIVE, n);
                }
                else
                {
                    neighbour(Y_POSITIVE, 2, 0, n);
                    neighbour(Y_POSITIVE, 3, 1, n);
                    neighbour(Y_POSITIVE, 6, 4, n);
                    neighbour(Y_POSITIVE, 7, 5, n);
                }
            }

            n = neighbours[Z_NEGATIVE];

            if (n != null)
            {
                if (n.isLeaf())
                {
                    children[0].setNeighbour(Z_NEGATIVE, n);
                    children[1].setNeighbour(Z_NEGATIVE, n);
                    children[2].setNeighbour(Z_NEGATIVE, n);
                    children[3].setNeighbour(Z_NEGATIVE, n);
                }
                else
                {
                    neighbour(Z_NEGATIVE, 0, 4, n);
                    neighbour(Z_NEGATIVE, 1, 5, n);
                    neighbour(Z_NEGATIVE, 2, 6, n);
                    neighbour(Z_NEGATIVE, 3, 7, n);
                }
            }

            n = neighbours[Z_POSITIVE];

            if (n != null)
            {
                if (n.isLeaf())
                {
                    children[4].setNeighbour(Z_POSITIVE, n);
                    children[5].setNeighbour(Z_POSITIVE, n);
                    children[6].setNeighbour(Z_POSITIVE, n);
                    children[7].setNeighbour(Z_POSITIVE, n);
                }
                else
                {
                    neighbour(Z_POSITIVE, 4, 0, n);
                    neighbour(Z_POSITIVE, 5, 1, n);
                    neighbour(Z_POSITIVE, 6, 2, n);
                    neighbour(Z_POSITIVE, 7, 3, n);
                }
            }
            
            onExpanded();
        }

        /// <summary>
        /// Called once this octree node was expanded
        /// </summary>
        protected virtual void onExpanded()
        {
            // Not Implemented
        }

        /// <summary>
        /// Finds the neighbour of this node in the given direction
        /// </summary>
        /// <param name="neighbourPosition">direction of the neighbour to look for</param>
        /// <returns>the neighbour in the given direction, or null if the neighbour does not exist</returns>
        private V findNeighbour(int neighbourPosition)
        {
            V neighbour = neighbours[neighbourPosition];
            
            if (neighbour == null)
            {
                return null;
            }

            if (neighbour.depth == this.depth)
            {
                return neighbour;
            }
            
            Vector3 pos;
            
            switch (neighbourPosition)
            {
                case X_POSITIVE:
                    pos = new Vector3(lowerCorner.x + (dimensions.x * 1.5f), 
                        lowerCorner.y + (dimensions.y * 0.5f), 
                        lowerCorner.z + (dimensions.z * 0.5f));
                    return neighbour.getChildAt(pos);
                case X_NEGATIVE:
                    pos = new Vector3(lowerCorner.x - (dimensions.x * 0.5f), 
                        lowerCorner.y + (dimensions.y * 0.5f), 
                        lowerCorner.z + (dimensions.z * 0.5f));
                    return neighbour.getChildAt(pos);
                case Y_POSITIVE:
                    pos = new Vector3(lowerCorner.x + (dimensions.x * 0.5f), 
                        lowerCorner.y + (dimensions.y * 1.5f), 
                        lowerCorner.z + (dimensions.z * 0.5f));
                    return neighbour.getChildAt(pos);
                case Y_NEGATIVE:
                    pos = new Vector3(lowerCorner.x + (dimensions.x * 0.5f), 
                        lowerCorner.y - (dimensions.y * 0.5f), 
                        lowerCorner.z + (dimensions.z * 0.5f));
                    return neighbour.getChildAt(pos);
                case Z_POSITIVE:
                    pos = new Vector3(lowerCorner.x + (dimensions.x * 0.5f), 
                        lowerCorner.y + (dimensions.y * 0.5f), 
                        lowerCorner.z + (dimensions.z * 1.5f));
                    return neighbour.getChildAt(pos);
                case Z_NEGATIVE:
                    pos = new Vector3(lowerCorner.x + (dimensions.x * 0.5f), 
                        lowerCorner.y + (dimensions.y * 0.5f), 
                        lowerCorner.z - (dimensions.z * 0.5f));
                    return neighbour.getChildAt(pos);
            }

            return null;
        }

        /// <summary>
        /// Updates all neighbours of this node if this node gets expanded (used for balancing)
        /// </summary>
        private void updateNeighbours()
        {
            if (neighbours == null)
            {
                return;
            }

            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i] != null && neighbours[i].depth < this.depth)
                {
                    if (neighbours[i].isLeaf())
                    {
                        neighbours[i].expand(); // Expand the neighbour if the depth of the neighbour is lower than the depth of this node
                    }

                    neighbours[i] = findNeighbour(i);
                }
            }
            
            
        }
        
        //TODO: Marked Octree: Implement Node Collapse (Node Collapses when all children are marked and no neighbourhood constraints go against collapse)
    }
}