using EditorTools.Display;
using UnityEngine;

namespace EditorTools.Maths.Structures.Octree
{
    /// <summary>
    /// Octree node containing a single bool value for each field
    /// </summary>
    public class BoolOctreeNode: BalancedOctreeNode<BoolOctreeNode>
    {
        public volatile bool value;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        public BoolOctreeNode(Vector3 lowerCorner, Vector3 upperCorner) : base(lowerCorner, upperCorner)
        {
            this.value = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        /// <param name="parent">parent of this node</param>
        /// <param name="depth">depth of this node</param>
        protected BoolOctreeNode(Vector3 lowerCorner, Vector3 dimensions, BoolOctreeNode parent, uint depth): base(lowerCorner, dimensions, parent, depth)
        {
            this.value = false;
        }

        // see Octree node
        protected override BoolOctreeNode createChild(Vector3 lowerCorner, Vector3 dimensions, uint depth)
        {
            return new BoolOctreeNode(lowerCorner, dimensions, this, depth);
        }

        // see Octree node
        protected override BoolOctreeNode self()
        {
            return this;
        }

        /// <summary>
        /// Draw this node in the given color, if the value of this node is true
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        /// <param name="marked">Color to draw the node with</param>
        public void drawDebugMarked(Transform origin, Color marked)
        {
            if (children != null)
            {
                foreach (BoolOctreeNode node in children)
                {
                    node.drawDebugMarked(origin, marked);
                }
            }
            
            if (value)
            {
                Geometry.drawBox(origin, dimensions, lowerCorner, marked);
            }
        }
    }
}