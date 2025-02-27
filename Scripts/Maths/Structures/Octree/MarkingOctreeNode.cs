using EditorTools.Display;
using UnityEngine;

namespace EditorTools.Maths.Structures.Octree
{
    /// <summary>
    /// Octree node implementing marking
    /// </summary>
    /// <typeparam name="V">Type of this node's parent</typeparam>
    public abstract class MarkingOctreeNode<V> : BalancedOctreeNode<V> where V : MarkingOctreeNode<V>
    {
        
        public volatile int marks;
        public volatile int markedDescendants;

        /// <summary>
        /// BitFlags if the face in this direction is an edge face (face is on an unmarked node, but the neighbour in that direction is marked)
        /// </summary>
        public int isedge { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        public MarkingOctreeNode(Vector3 lowerCorner, Vector3 upperCorner) : base(lowerCorner, upperCorner)
        {
            this.marks = 0;
            this.markedDescendants = 0;
            this.isedge = 0b111111;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        /// <param name="parent">parent of this node</param>
        /// <param name="depth">depth of this node</param>
        protected MarkingOctreeNode(Vector3 lowerCorner, Vector3 dimensions, V parent, uint depth): base(lowerCorner, dimensions, parent, depth)
        {
            this.marks = 0;
            this.markedDescendants = 0;
            this.isedge = 0;
        }

        #region abstract overrides

        //see BalancedOctreeNode
        protected override void onExpanded()
        {
            base.onExpanded();
            
            for (int i = 0; i < children.Length; i++)
            {
                children[i].checkNeighboursExist();
            }

            this.isedge = 0;
        }

        /// <summary>
        /// Updates the edge states of this node
        /// </summary>
        private void checkNeighboursExist()
        {
            bool hasChanged = false;
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i] == null)
                {
                    isedge |= (1 << i);
                    hasChanged = true;
                }
            }

            if (hasChanged)
            {
                edgeChanged();
            }
        }

        #endregion

        #region marking

        /// <summary>
        /// Called when a descendant of this node gets marked
        /// </summary>
        private void onDecendantMarked()
        {
            markedDescendants += 1;
            if (this.markedDescendants == 8)
            {
                mark();
            }
        }

        /// <summary>
        /// Marks this node
        /// </summary>
        public virtual void mark()
        {
            if (this.marks != 0)
            {
                return;
            }
        
            this.marks = 1;
            
            this.parent.onDecendantMarked();

            if (!this.isLeaf())
            {
                return; // No leaf, so no edge data
            }

            this.isedge = 0; // Reset Edge States (to get rid of edges between Void and unmarked surfaces)
            
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i] != null)
                {
                    if ((neighbours[i].marks & 0x01) == 0 && neighbours[i].isLeaf())
                    {
                        markEdge(i);
                    }
                    else
                    {
                        neighbours[i].unmarkEdge(i^0x1);
                    }
                    // i^0x1 switches the sign of the axis 
                }
            }
        }

        /// <summary>
        /// Checks if this node is marked
        /// </summary>
        /// <returns>true if this node is marked</returns>
        public bool isMarked()
        {
            return marks != 0;
        }

        /// <summary>
        /// sets the given side as edge
        /// </summary>
        /// <param name="side">Side to mark as edge</param>
        public virtual void markEdge(int side)
        {
            if (marks == 0)
            {
                return; // Node is unmarked, so there is no edge
            }

            isedge |= (1 << side); // Set the flag to mark the according edge
            
            edgeChanged();
        }
        
        /// <summary>
        /// unsets the given side as edge
        /// </summary>
        /// <param name="side">Side to unmark as edge</param>
        public virtual void unmarkEdge(int side)
        {
            if (marks == 0)
            {
                return; // Node is unmarked, so there is no edge
            }

            isedge &= ~(1 << side); // Set the flag to mark the according edge
            edgeChanged();
        }

        /// <summary>
        /// Called when the edge bitmask is changed
        /// </summary>
        public virtual void edgeChanged()
        {
            // Not Implemented
        }

        #endregion

        /// <summary>
        /// Draw this node in the given color, if the value of this node is not marked
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        /// <param name="marked">Color to draw the node with</param>
        public void drawDebugUnmarked(Transform origin, Color color)
        {
            

            if (children != null)
            {
                foreach (V node in children)
                {
                    node.drawDebugUnmarked(origin, color);
                }
            }

            if (marks == 0)
            {
                Geometry.drawBox(origin, dimensions, lowerCorner, color);
            }
        }
        
        /// <summary>
        /// Draw this node in the given color, if the value of this node is marked
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        /// <param name="marked">Color to draw the node with</param>
        public void drawDebugMarked(Transform origin, Color marked)
        {
            if (children != null)
            {
                foreach (V node in children)
                {
                    node.drawDebugMarked(origin, marked);
                }
            }
            
            if (marks!= 0)
            {
                Geometry.drawBox(origin, dimensions, lowerCorner, marked);
            }
        }
        
        /// <summary>
        /// Draw this nodes edge faces in the given color
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        /// <param name="marked">Color to draw the edge faces with</param>
        public void drawDebugEdges(Transform origin, Color marked)
        {
            if (children != null)
            {
                foreach (V node in children)
                {
                    node.drawDebugEdges(origin, marked);
                }
            }
            
            if (isedge != 0 && isLeaf())
            {
                Geometry.drawBox(origin, dimensions, lowerCorner, marked, isedge, true);
            }
        }
    }
}