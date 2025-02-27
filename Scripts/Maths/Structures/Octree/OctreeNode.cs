using System;
using EditorTools.Display;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

using EditorTools.Maths.LinAlg;

namespace EditorTools.Maths.Structures.Octree
{
    /// <summary>
    /// Basic abstract implementation of a octree node
    /// </summary>
    /// <typeparam name="V">Parameter type of this node</typeparam>
    public abstract class OctreeNode<V> where V : OctreeNode<V>
    {
        protected V parent;
        protected V[] children = null;

        public Vector3 lowerCorner { get; private set; }

        public Vector3 dimensions { get; private set; }

        public uint depth { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        public OctreeNode(Vector3 lowerCorner, Vector3 upperCorner)
        {
            this.parent = null;
            this.lowerCorner = Vector3.Min(lowerCorner, upperCorner);
            upperCorner = Vector3.Max(lowerCorner, upperCorner);

            this.dimensions = upperCorner - this.lowerCorner;

            this.depth = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        /// <param name="parent">parent of this node</param>
        /// <param name="depth">depth of this node</param>
        protected OctreeNode(Vector3 lowerCorner, Vector3 dimensions, V parent, uint depth)
        {
            this.parent = parent;
            this.lowerCorner = lowerCorner;

            this.dimensions = dimensions;

            this.depth = depth;
        }

        /// <summary>
        /// Creates a new child node for this ndoe
        /// </summary>
        /// <param name="lowerCorner">minimal corner of the node</param>
        /// <param name="dimensions">size of the node</param>
        /// <param name="depth">depth of the node</param>
        /// <returns></returns>
        protected abstract V createChild(Vector3 lowerCorner, Vector3 dimensions, uint depth);
        
        /// <summary>
        /// returns a reference to this node (hack to get around limits of generics)
        /// </summary>
        /// <returns>a reference to this node</returns>
        protected abstract V self();
        
        /// <summary>
        /// checks if this node is a root node
        /// </summary>
        /// <returns>true if this is a root node</returns>
        public bool isRoot()
        {
            return parent == null;
        }

        /// <summary>
        /// checks if this node is a leaf node
        /// </summary>
        /// <returns>true if this node is a leaf</returns>
        public bool isLeaf()
        {
            return children == null;
        }

        /// <summary>
        /// Expands this octree node
        /// </summary>
        public virtual void expand()
        {
            this.children = new V[8];

            Vector3 dimensionsHalf = dimensions * 0.5f;

            this.children[0] = createChild(lowerCorner,
                dimensionsHalf, depth + 1);
            this.children[1] = createChild(new Vector3(lowerCorner.x + dimensionsHalf.x, lowerCorner.y, lowerCorner.z),
                dimensionsHalf, depth + 1);
            this.children[2] = createChild(new Vector3(lowerCorner.x, lowerCorner.y + dimensionsHalf.y, lowerCorner.z),
                dimensionsHalf, depth + 1);
            this.children[3] = createChild(
                new Vector3(lowerCorner.x + dimensionsHalf.x, lowerCorner.y + dimensionsHalf.y, lowerCorner.z),
                dimensionsHalf, depth + 1);

            this.children[4] = createChild(new Vector3(lowerCorner.x, lowerCorner.y, lowerCorner.z + dimensionsHalf.z),
                dimensionsHalf, depth + 1);
            this.children[5] = createChild(
                new Vector3(lowerCorner.x + dimensionsHalf.x, lowerCorner.y, lowerCorner.z + dimensionsHalf.z),
                dimensionsHalf, depth + 1);
            this.children[6] = createChild(
                new Vector3(lowerCorner.x, lowerCorner.y + dimensionsHalf.y, lowerCorner.z + dimensionsHalf.z),
                dimensionsHalf, depth + 1);
            this.children[7] = createChild(
                new Vector3(lowerCorner.x + dimensionsHalf.x, lowerCorner.y + dimensionsHalf.y,
                    lowerCorner.z + dimensionsHalf.z),
                dimensionsHalf, depth + 1);
        }

        /// <summary>
        /// recursively gets the descendant node at the given position and depth
        /// </summary>
        /// <param name="position">Position of the descendant to get</param>
        /// <param name="depth">depth of the descendant to get</param>
        /// <returns>the descendant at the given position and closest to the desired depth</returns>
        /// <exception cref="ArgumentException">in case position is outside of node</exception>
        public V getNodeAt(Vector3 position, uint depth = 0)
        {
            if (!VectorTools.isInAABB(position, lowerCorner, lowerCorner + dimensions))
            {
                throw new ArgumentException("Point not within bounds of this node ! " + position);
            }

            return _getNodeAt(position, depth);
        }

        /// <summary>
        /// recursively gets the descendant node at the given position and depth
        /// </summary>
        /// <param name="position">Position of the descendant to get</param>
        /// <param name="depth">depth of the descendant to get</param>
        /// <returns>the descendant at the given position and closest to the desired depth</returns>
        protected V _getNodeAt(Vector3 position, uint depth)
        {
            if (this.children == null)
            {
                expand();
            }

            V child = getChildAt(position);

            if (child.depth >= depth)
            {
                return child;
            }

            return child._getNodeAt(position, depth);
        }

        /// <summary>
        /// Gets the direct child at the given position
        /// </summary>
        /// <param name="position">Position to get the child (or self) for</param>
        /// <returns>the child containing position, or if no childs present: return sell</returns>
        public V getChildAt(Vector3 position)
        {
            if (children == null)
            {
                return self();
            }

            Vector3 relativePos = position - lowerCorner;
            Vector3 halfDim = dimensions * 0.5f;

            int idx = 0;

            if (relativePos.x > halfDim.x)
            {
                idx += 1;
            }

            if (relativePos.y > halfDim.y)
            {
                idx += 2;
            }

            if (relativePos.z > halfDim.z)
            {
                idx += 4;
            }
            
            return children[idx];
        }

        /// <summary>
        /// gets the child of this node by index
        /// </summary>
        /// <param name="idx">index of child to get</param>
        /// <returns>child with the given index</returns>
        public V getChild(int idx)
        {
            if (children == null)
            {
                return self();
            }

            return children[idx];
        }

        #region Debugging
        
        /// <summary>
        /// Draws this node with the given transform and color
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        /// <param name="color">Color to draw the node in</param>
        /// <param name="includeChildred">should children be drawn recursicely, defaults to true</param>
        public void drawDebug(Transform origin, Color color, bool includeChildred = true)
        {
            Geometry.drawBox(origin, dimensions, lowerCorner, color);

            if (children != null && includeChildred)
            {
                foreach (V node in children)
                {
                    node.drawDebug(origin, color);
                }
            }
        }

        /// <summary>
        /// Draws this node with the given transform and color
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        /// <param name="color">Color to draw the node in at depth index. wraps around</param>
        /// <param name="includeChildred">should children be drawn recursicely, defaults to true</param>
        public void drawDebug(Transform origin, Color[] color, bool includeChildred = true)
        {
            if (children != null && includeChildred)
            {
                foreach (V node in children)
                {
                    node.drawDebug(origin, color);
                }
            }

            Geometry.drawBox(origin, dimensions, lowerCorner, color[depth % color.Length]);

            if (isLeaf())
            {
                Geometry.drawSphere(origin, dimensions.magnitude * 0.25f, lowerCorner + (dimensions * 0.5f), 4, 1,
                    color[depth % color.Length]);
            }
        }

        #endregion
    }
}