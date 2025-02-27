using EditorTools.MeshGenerator;
using EditorTools.MeshGenerator.Octree;
using UnityEngine;

namespace EditorTools.Maths.Structures.Octree
{
    /// <summary>
    /// Octree node containing mesh related data
    /// </summary>
    public class MeshOctreeNode : MarkingOctreeNode<MeshOctreeNode>
    {
        //private VertexData[] vertices = null;
        private byte[] faceShapes = null;

        //private bool isModified = false;
        /// <summary>
        /// Faces to flip
        /// </summary>
        private const int filpFaces = X_POSITIVE_BITMASK | Y_NEGATIVE_BITMASK | Z_POSITIVE_BITMASK;
        
        /// <summary>
        /// Mesh generator
        /// </summary>
        private BlockMeshShapeGenerator generator = new BlockMeshShapeGenerator();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        public MeshOctreeNode(Vector3 lowerCorner, Vector3 upperCorner) : base(lowerCorner, upperCorner)
        {
            // Not Implemented
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lowerCorner">minimal corner of this node</param>
        /// <param name="upperCorner">maximal corner of this node</param>
        /// <param name="parent">parent of this node</param>
        /// <param name="depth">depth of this node</param>
        protected MeshOctreeNode(Vector3 lowerCorner, Vector3 dimensions, MeshOctreeNode parent, uint depth) : base(lowerCorner, dimensions, parent, depth)
        {
            // Not Implemented
        }

        //see Octree Node
        protected override MeshOctreeNode createChild(Vector3 lowerCorner, Vector3 dimensions, uint depth)
        {
            return new MeshOctreeNode(lowerCorner, dimensions, this, depth);
        }

        //see Octree Node
        protected override MeshOctreeNode self()
        {
            return this;
        }
        
        /// <summary>
        /// Generate the mesh for this node and all descendants
        /// </summary>
        /// <param name="mesh">MeshPrototype to append the data to</param>
        public void generateMesh(MeshPrototype mesh)
        {
            if (!isLeaf())
            {
                foreach (MeshOctreeNode child in children)
                {
                    child.generateMesh(mesh);
                }
                return;
            }
            
            faceShapes = new byte[6];
            
            for (int i = 0; i < 6; i++)
            {
                if ((isedge & (0x01 << i)) != 0)
                {
                    faceShapes[i] = (byte)getShape(i);
                    if (neighbours[i] != null)
                    {
                        generator.generate(mesh, i, faceShapes[i], this, ((filpFaces & (0x01 << i)) == 0), true);
                        continue;
                    }

                    generator.generate(mesh, i, faceShapes[i], this, ((filpFaces & (0x01 << i)) != 0), false);
                }
            }
        }

        /// <summary>
        /// Check if the edge in the given direction is subdivided
        /// </summary>
        /// <param name="neighbour">direction of the relevant neighbour</param>
        /// <param name="face_dir">direction of the face to check for</param>
        /// <returns>true if the edge has to be subdivided</returns>
        private int isSubdivided(int neighbour, int face_dir)
        {
            if ((isedge & (0x01 << neighbour)) != 0)
            {
                return 0; // Face on the same cell, so there cant be subdivisions
            }

            MeshOctreeNode nd = neighbours[neighbour];
            
            if (nd != null && nd.depth <= depth && (nd.isedge & (0x01 << face_dir)) != 0)
            {
                return 0; // Neighbour has a coplanar face 
            }

            MeshOctreeNode nfd = neighbours[face_dir];
            
            if (nd == null && nfd != null && nfd.depth <= depth && (nfd.isedge & (0x01 << neighbour)) != 0)
            {
                return 0; // Neighbour has no coplanar face, but indirect neighbour is null, and neighbour has face to the outside of that neighbour
            }

            if (nfd == null && nd != null && nd.depth <= depth && (nd.isedge & (0x01 << (neighbour ^ 0x01))) != 0)
            {
                return 0; // This is a edge node with a neighbour marked in the direction of this node
            }

            MeshOctreeNode nid = null;
            
            if (nd != null)
            {
                nid = neighbours[neighbour].neighbours[face_dir];
            }

            if (nid != null && nid.depth <= depth && (nid.isedge & (0x01 << (neighbour ^ 0x01))) != 0)
            {
                return 0; // There exists an indirect neighbour with a face neighbouring this node's face of the same or lower subsurf level
            }

            return 1;
            
            /*
            if (nd != null && nd.isMarked() && nd.isLeaf()) // Direct Same or higher level neighbour with planar face // && (isedge & (0x01 << face_dir)) != 0
            {
                return 0;
            } 
            
            if (nid != null && nid.isMarked() && nid.isLeaf() && (nid.isedge & (0x01<<(neighbour^0x01))) != 0) // Indirect Same or higher level neighbour with orthogonal face
            {
                return 0;
            }
            
            return 1;*/
        }

        /// <summary>
        /// Computes the shape of the face in the given direction
        /// </summary>
        /// <param name="edge">Face direction</param>
        /// <returns>the shape of the face in the given direction (4bit bitmask, see thesis for details)</returns>
        public int getShape(int edge)
        {
            int sub1 = 0, sub2 = 0, sub3 = 0, sub4 = 0;
            switch (edge)
            {
                case X_POSITIVE: // x+ 
                    sub1 = isSubdivided(Z_POSITIVE, X_POSITIVE) << 3;
                    sub2 = isSubdivided(Y_NEGATIVE, X_POSITIVE) << 2;
                    sub3 = isSubdivided(Y_POSITIVE, X_POSITIVE) << 1;
                    sub4 = isSubdivided(Z_NEGATIVE, X_POSITIVE) << 0;
                    // Neighbour Bit order: z+|y-|y+|z-
                    break; 
                case X_NEGATIVE: // x-
                    sub1 = isSubdivided(Z_POSITIVE, X_NEGATIVE) << 3;
                    sub2 = isSubdivided(Y_POSITIVE, X_NEGATIVE) << 1;
                    sub3 = isSubdivided(Y_NEGATIVE, X_NEGATIVE) << 2;
                    sub4 = isSubdivided(Z_NEGATIVE, X_NEGATIVE) << 0;
                    // Neighbour Bit order: z+|y+|y-|z-
                    break;
                case Y_POSITIVE: // y+
                    sub1 = isSubdivided(Z_POSITIVE, Y_POSITIVE) << 3;
                    sub2 = isSubdivided(X_POSITIVE, Y_POSITIVE) << 1;
                    sub3 = isSubdivided(X_NEGATIVE, Y_POSITIVE) << 2;
                    sub4 = isSubdivided(Z_NEGATIVE, Y_POSITIVE) << 0;
                    // Neighbour Bit order: z+|x+|x-|z-
                    break;
                case Y_NEGATIVE: // y-
                    sub1 = isSubdivided(Z_POSITIVE, Y_NEGATIVE) << 3;
                    sub2 = isSubdivided(X_NEGATIVE, Y_NEGATIVE) << 2;
                    sub3 = isSubdivided(X_POSITIVE, Y_NEGATIVE) << 1;
                    sub4 = isSubdivided(Z_NEGATIVE, Y_NEGATIVE) << 0;
                    // Neighbour Bit order: z+|x-|x+|z-
                    break;
                case Z_POSITIVE: // z+ 
                    sub1 = isSubdivided(X_POSITIVE, Z_POSITIVE) << 1;
                    sub2 = isSubdivided(Y_POSITIVE, Z_POSITIVE) << 3;
                    sub3 = isSubdivided(Y_NEGATIVE, Z_POSITIVE) << 0;
                    sub4 = isSubdivided(X_NEGATIVE, Z_POSITIVE) << 2;
                    // Neighbour Bit order: x+|y+|y-|x-
                    break;
                case Z_NEGATIVE: // z-
                    sub1 = isSubdivided(X_NEGATIVE, Z_NEGATIVE) << 2;
                    sub2 = isSubdivided(Y_POSITIVE, Z_NEGATIVE) << 3;
                    sub3 = isSubdivided(Y_NEGATIVE, Z_NEGATIVE) << 0;
                    sub4 = isSubdivided(X_POSITIVE, Z_NEGATIVE) << 1;
                    // Neighbour Bit order: x-|y+|y-|x+
                    break;
                default:
                    break;
            }

            return sub1 | sub2 | sub3 | sub4 ;
        }

        //see MeshOctreeNode
        public override void edgeChanged()
        {
            //isModified = true;
        }
        
        /// <summary>
        /// Draws debug vertices on all edge faces
        /// </summary>
        /// <param name="origin">Transform of the octree</param>
        public void drawDebugVertices(Transform origin)
        {
            if (!isLeaf())
            {
                foreach (MeshOctreeNode child in children)
                {
                    child.drawDebugVertices(origin);
                }
                return;
            }

            if (generator == null || faceShapes == null)
            {
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                if ((isedge & (0x01 << i)) != 0)
                {
                    generator.drawDebugVertcies(i, faceShapes[i], this, origin);
                }
            }
        }
    }
}