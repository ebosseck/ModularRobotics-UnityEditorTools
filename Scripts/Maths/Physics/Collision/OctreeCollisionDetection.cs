using System;
using System.Collections.Generic;
using EditorTools.Maths.Physics.Collision.Colliders;
using EditorTools.Maths.Structures.Octree;
using UnityEngine;

namespace EditorTools.Maths.Physics.Collision
{
    /// <summary>
    /// Physics solver for collision detection
    /// </summary>
    /// <typeparam name="V">Node type to check collisions with. Must be descendant of MarkingOctreeNode</typeparam>
    public class OctreeCollisionDetection<V> where V : MarkingOctreeNode<V>
    {
        public MarkingOctreeNode<V> root;
        private Transform voxelTransform;
        private IShape shape;
        private int maxDepth = 7;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="root">root node of octree</param>
        /// <param name="voxelTransform">octree transform</param>
        /// <param name="shape">Collision shape to check collisions with</param>
        /// <param name="maxDepth">maximum tree depth to check collision to</param>
        public OctreeCollisionDetection(MarkingOctreeNode<V> root, Transform voxelTransform, IShape shape, int maxDepth)
        {
            this.root = root;
            this.voxelTransform = voxelTransform;
            this.shape = shape;
            this.maxDepth = maxDepth;
        }

        /// <summary>
        /// Mark colliding nodes
        /// </summary>
        public void markNodes()
        {
            List<MarkingOctreeNode<V>> canidates = broadPhase();
            narrowPhase(ref canidates);
            
            //Debug.Log("Canidate Count: " + canidates.Count);
        }

        #region Narrow Phase

        /// <summary>
        /// Executes the narrow phase of collision detection on the canidates
        /// </summary>
        /// <param name="canidates">List of potential colliders, determined by broad phase</param>
        /// <returns>a list of all colliding canidates</returns>
        public List<MarkingOctreeNode<V>> narrowPhase(ref List<MarkingOctreeNode<V>> canidates)
        {
            foreach (var node in canidates)
            {
                checkNode(node);

            }
            
            return canidates;
        }

        /// <summary>
        /// Checks the given node for collisions, and then marks the node if collisions occured
        /// </summary>
        /// <param name="node">Node to check</param>
        private void checkNode(MarkingOctreeNode<V> node)
        {
            if (!node.isMarked() && gjk(node))
            {
                if (node.depth >= maxDepth)
                {
                    node.drawDebug(voxelTransform, Color.green, false);
                    node.mark();
                    return;
                }

                if (node.isLeaf()) {
                    node.expand();
                }

                for (int i = 0; i < 8; i++)
                {
                    checkNode(node.getChild(i));
                }
            }
            else
            {
                // node.drawDebug(voxelTransform, Color.blue, false);
            }
        }


        /// <summary>
        /// Checks if the given node collides with this shape. Used for narrow detection.
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>True if the node collides, false otherwise</returns>
        private bool gjk(MarkingOctreeNode<V> node)
        {
            Vector3[] simplex = new Vector3[4];
            int simplexCount = 0;

            Vector3 dir = Vector3.right;//shape.getOrigin() - (node.lowerCorner + node.dimensions / 2);

            Vector3 support = computeMinkowski(node, dir);
            pushVertex(support, ref simplex, ref simplexCount);

            dir = -support;

            while (true)
            {
                support = computeMinkowski(node, dir);

                if (Vector3.Dot(support, dir) < 0)
                {
                    //node.drawDebug(voxelTransform, Color.magenta, false);
                    return false;
                }
                
                pushVertex(support, ref simplex, ref simplexCount);

                if (nextSimplex(ref simplex, ref simplexCount, ref dir))
                {
                    //node.drawDebug(voxelTransform, Color.blue, false);
                    return true;
                }
            }
        }
        
        #region helpers

        #region next simplex
        /// <summary>
        /// Constructs the next simplex for the GJK algorithm
        /// </summary>
        /// <param name="simplex">vertices of the current simplex</param>
        /// <param name="simplexCount">count of vertices in the current simplex</param>
        /// <param name="dir">current search direction</param>
        /// <returns>true if the simplex contains the origin, false otherwise</returns>
        private bool nextSimplex(ref Vector3[] simplex, ref int simplexCount, ref Vector3 dir)
        {
            switch (simplexCount)
            {
                case 2: return nextSimplexLine(ref simplex, ref simplexCount, ref dir);
                case 3: return nextSimplexTriangle(ref simplex, ref simplexCount, ref dir);
                case 4: return nextSimplexTetrahedron(ref simplex, ref simplexCount, ref dir);
            }
            Debug.LogError("Simplex with invalid vertex count found ! Count = " + simplexCount);
            return false;
        }

        /// <summary>
        /// computes the next 1d simplex
        /// </summary>
        /// <param name="simplex">vertices of the current simplex</param>
        /// <param name="simplexCount">count of vertices in the current simplex</param>
        /// <param name="dir">current search direction</param>
        /// <returns>true if the simplex contains the origin, false otherwise</returns>
        private bool nextSimplexLine(ref Vector3[] simplex, ref int simplexCount, ref Vector3 dir)
        {
            Vector3 ab = simplex[1] - simplex[0];
            Vector3 ao = - simplex[0];

            if (sameDirection(ab, ao))
            {
                dir = Vector3.Cross(Vector3.Cross(ab, ao), ab);
                return false;
            }

            simplexCount = 1; // a is already first element, so assignment not neccessary
            dir = ao;
            return false;
        }
        
        
        /// <summary>
        /// Computes the next 2D simplex
        /// </summary>
        /// <param name="simplex">vertices of the current simplex</param>
        /// <param name="simplexCount">count of vertices in the current simplex</param>
        /// <param name="dir">current search direction</param>
        /// <returns>true if the simplex contains the origin, false otherwise</returns>
        private bool nextSimplexTriangle(ref Vector3[] simplex, ref int simplexCount, ref Vector3 dir)
        {
            Vector3 ab = simplex[1]-simplex[0];
            Vector3 ac = simplex[2]-simplex[0];
            Vector3 ao = -simplex[0];

            Vector3 abc = Vector3.Cross(ab, ac);

            if (sameDirection(Vector3.Cross(abc, ac), ao))
            {
                if (sameDirection(ac, ao))
                {
                    simplex[1] = simplex[2];
                    simplexCount = 2;
                    dir = Vector3.Cross(Vector3.Cross(ac, ao), ac);
                }
                else
                {
                    simplexCount = 2;
                    return nextSimplexLine(ref simplex, ref simplexCount, ref dir);
                }
            }
            else
            {
                if (sameDirection(Vector3.Cross(ab, abc), ao))
                {
                    simplexCount = 2;
                    return nextSimplexLine(ref simplex, ref simplexCount, ref dir);
                }
                else
                {
                    if (sameDirection(abc, ao))
                    {
                        dir = abc;
                    }
                    else
                    {
                        (simplex[2], simplex[3]) = (simplex[3], simplex[2]);

                        dir = -abc;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// computes the next 3D simplex
        /// </summary>
        /// <param name="simplex">vertices of the current simplex</param>
        /// <param name="simplexCount">count of vertices in the current simplex</param>
        /// <param name="dir">current search direction</param>
        /// <returns>true if the simplex contains the origin, false otherwise</returns>
        private bool nextSimplexTetrahedron(ref Vector3[] simplex, ref int simplexCount, ref Vector3 dir)
        {
            Vector3 ab = simplex[1] - simplex[0];
            Vector3 ac = simplex[2] - simplex[0];
            Vector3 ad = simplex[3] - simplex[0];
            Vector3 ao =  - simplex[0];

            Vector3 abc = Vector3.Cross(ab, ac);
            Vector3 acd = Vector3.Cross(ac, ad);
            Vector3 adb = Vector3.Cross(ad, ab);

            if (sameDirection(abc, ao))
            {
                simplexCount = 3;
                nextSimplexTriangle(ref simplex, ref simplexCount, ref dir);
            }

            if (sameDirection(acd, ao))
            {
                (simplex[1], simplex[2]) = (simplex[2], simplex[3]);
                simplexCount = 3;
                nextSimplexTriangle(ref simplex, ref simplexCount, ref dir);
            }
            
            if (sameDirection(adb, ao))
            {
                (simplex[1], simplex[2]) = (simplex[3], simplex[1]);
                simplexCount = 3;
                nextSimplexTriangle(ref simplex, ref simplexCount, ref dir);
            }
            
            return true;
        }
        
        #endregion

        /// <summary>
        /// Checks if two vectors are the same direction
        /// </summary>
        /// <param name="a">vector a</param>
        /// <param name="b">vetcor b</param>
        /// <returns>true if dot(a, b) > 0</returns>
        private bool sameDirection(Vector3 a, Vector3 b)
        {
            return Vector3.Dot(a, b) > 0;
        }

        /// <summary>
        /// Pushes the vertex onto the simplex vertex array
        /// </summary>
        /// <param name="position">vertex data to push</param>
        /// <param name="simplex">reference to the simplex array</param>
        /// <param name="simplexCount">number of simpleces currently valid in the simplex stack</param>
        private void pushVertex(Vector3 position, ref Vector3[] simplex, ref int simplexCount)
        {
            (simplex[0], simplex[1], simplex[2], simplex[3]) = (position, simplex[0], simplex[1], simplex[2]);
            simplexCount = Math.Min(simplexCount + 1, 4);
        }

        /// <summary>
        /// Computes a point on the Minkowski difference
        /// </summary>
        /// <param name="node">Node to check collision against</param>
        /// <param name="direction">Direction for computing the difference</param>
        /// <returns>a point on the Minkowski difference</returns>
        public Vector3 computeMinkowski(MarkingOctreeNode<V> node, Vector3 direction)
        {

            Vector3 s = shape.support(direction);
            Vector3 n = supportNode(-direction, node);
            
            return s - n;
        }

        
        
        /// <summary>
        /// Support Function for Octree Node
        /// </summary>
        /// <param name="direction">Direction in World Space</param>
        /// <returns>the most extreme point in that direction</returns>
        public Vector3 supportNode(Vector3 direction, MarkingOctreeNode<V> node)
        {
            // Transform Direction to Shape Space
            direction = voxelTransform.InverseTransformDirection(direction).normalized;

            Vector3 position = new Vector3(node.dimensions.x * sgn(direction.x), 
                node.dimensions.y * sgn(direction.y), 
                node.dimensions.z * sgn(direction.z));
            
            //Geometry.drawSphere(voxelTransform, 0.02f, position + node.lowerCorner, 4, 2, Color.yellow);
            
            return voxelTransform.TransformPoint(position + node.lowerCorner);
        }

        /// <summary>
        /// Modified sign function (as required for gjk)
        /// </summary>
        /// <param name="val">value to check</param>
        /// <returns>0 if val < 0, 1 otherwise</returns>
        private float sgn(float val)
        {
            if (val < 0)
            {
                return 0;
            }
            return 1f;
        }
        
        #endregion
        
        #endregion
        
        #region Broad Phase

        /// <summary>
        /// Broad phase of the Collsion detection
        /// </summary>
        /// <returns>A list of nodes for further checking</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<MarkingOctreeNode<V>> broadPhase()
        {
            Vector3[] bounds = shape.getAABB(voxelTransform);

            return broadPhase(bounds);
        }
        
        /// <summary>
        /// Broad phase of the Collsion detection
        /// </summary>
        /// <param name="bounds">Bounding Box to check against</param>
        /// <returns>A list of nodes for further checking</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<MarkingOctreeNode<V>> broadPhase(Vector3[] bounds)
        {
            List<MarkingOctreeNode<V>> list = new List<MarkingOctreeNode<V>>();
            return broadPhase(ref list, bounds, root);
        }

        /// <summary>
        /// Broad phase of the Collsion detection
        /// </summary>
        /// <param name="bounds">Bounding Box to check against</param>
        /// <returns>A list of nodes for further checking</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<MarkingOctreeNode<V>> broadPhase(ref List<MarkingOctreeNode<V>> list, Vector3[] bounds, MarkingOctreeNode<V> node)
        {
            Vector3[] nodeBounds = new[] {node.lowerCorner, node.lowerCorner + node.dimensions};
            
            if (checkEarlyTerm(bounds, nodeBounds)) {
                list.Add(node);
                return list;
            }
            
            if (checkAABB(bounds, nodeBounds))
            {
                //node.drawDebug(voxelTransform, Color.blue, false);
                
                if (node.depth >= maxDepth)
                {
                    list.AddRange(expandNodeFully(node));
                    return list;
                }

                if (node.isLeaf()) {
                    node.expand();
                }

                for (int i = 0; i < 8; i++)
                {
                    broadPhase(ref list, bounds, node.getChild(i));
                }

            }

            return list;
        }

        /// <summary>
        /// recursively expands leaf node to maximum depth and returns the new children
        /// </summary>
        /// <param name="node">Node to expand fully</param>
        /// <returns>list of descendants at deepest level</returns>
        private List<MarkingOctreeNode<V>> expandNodeFully(MarkingOctreeNode<V> node)
        {
            List<MarkingOctreeNode<V>> result = new List<MarkingOctreeNode<V>>();
            
            if (node.isLeaf())
            {
                if (node.depth < maxDepth)
                {
                    node.expand();

                    for (int i = 0; i < 8; i++)
                    {
                        result.AddRange(expandNodeFully(node.getChild(i)));
                    }
                }
                else
                {
                    result.Add(node);
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if AABBs collide
        /// </summary>
        /// <param name="boundsA">aabb 1</param>
        /// <param name="boundsB">aabb 2</param>
        /// <returns>True if the bounding boxes overlap</returns>
        private bool checkEarlyTerm(Vector3[] a, Vector3[] b)
        {
            return a[0].x <= b[0].x && b[1].x <= a[1].x &&
                   a[0].y <= b[0].y && b[1].y <= a[1].y &&
                   a[0].z <= b[0].z && b[1].z <= a[1].z;
        }

        /// <summary>
        /// Checks if AABBs collide
        /// </summary>
        /// <param name="boundsA">aabb 1</param>
        /// <param name="boundsB">aabb 2</param>
        /// <returns>True if the bounding boxes overlap</returns>
        private bool checkAABB(Vector3[] a, Vector3[] b)
        {
            return a[0].x <= b[1].x && a[1].x >= b[0].x &&
                   a[0].y <= b[1].y && a[1].y >= b[0].y &&
                   a[0].z <= b[1].z && a[1].z >= b[0].z;

        }
        
        #endregion
    }
}