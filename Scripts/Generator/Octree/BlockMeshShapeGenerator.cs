using EditorTools.Display;
using EditorTools.Maths.Structures.Octree;
using UnityEngine;

namespace EditorTools.MeshGenerator.Octree
{
    /// <summary>
    /// Blocky mesh generator for Octree data
    /// </summary>
    public class BlockMeshShapeGenerator
    {

        /// <summary>
        /// Genreate a mesh form Octree data
        /// </summary>
        /// <param name="mesh">Mesh prototype to append data to</param>
        /// <param name="face">face of the node to create a mesh for</param>
        /// <param name="shape">shape of the face to generate (bitmask of subdivisions, see thesis for details)</param>
        /// <param name="node">node whose face to generate</param>
        /// <param name="flipped">should the face be flipped</param>
        /// <param name="flipNormal">should the normal be flipped</param>
        public void generate(MeshPrototype mesh, int face, int shape, MeshOctreeNode node, bool flipped, bool flipNormal)
        {
            int offset = generateVertices(mesh, face, shape, node, flipNormal);
            generateFaces(mesh, shape, offset, flipped);
        }

        /// <summary>
        /// Generates the vertices for the given node data
        /// </summary>
        /// <param name="mesh">Mesh prototype to append data to</param>
        /// <param name="face">face of the node to create a mesh for</param>
        /// <param name="shape">shape of the face to generate (bitmask of subdivisions, see thesis for details)</param>
        /// <param name="node">node whose face to generate</param>
        /// <param name="flipNormals">should the normal be flipped</param>
        /// <returns>offset of the first vertex</returns>
        public int generateVertices(MeshPrototype mesh, int face, int shape, MeshOctreeNode node, bool flipNormals)
        {
            int offset = 0;
            Vector3 normal;
            switch (face)
            {
                case MeshOctreeNode.X_POSITIVE:
                    normal = new Vector3(1, 0, 0);
                    if (flipNormals)
                    {
                        normal = normal * -1;
                    }

                    offset = mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, 0),
                        normal));

                    if ((shape & 0b0001) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y / 2f, 0), normal));
                    }

                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y, 0), normal));
                    
                    if ((shape & 0b0100) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, node.dimensions.z / 2f), normal));
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y, node.dimensions.z / 2f), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, node.dimensions.z), normal));
                    
                    if ((shape & 0b1000) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y / 2f, node.dimensions.z), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + node.dimensions, normal));
                    
                    break;
                case MeshOctreeNode.X_NEGATIVE:
                    normal = new Vector3(-1, 0, 0);
                    if (flipNormals)
                    {
                        normal = normal * -1;
                    }
                    
                    offset = mesh.addVertex(new VertexData(node.lowerCorner,
                        normal));

                    if ((shape & 0b0001) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y / 2f, 0), normal));
                    }

                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, 0), normal));
                    
                    if ((shape & 0b0100) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, 0, node.dimensions.z / 2f), normal));
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, node.dimensions.z / 2f), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, 0, node.dimensions.z), normal));
                    
                    if ((shape & 0b1000) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y / 2f, node.dimensions.z), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, node.dimensions.z), normal));
                    
                    break;
                case MeshOctreeNode.Y_POSITIVE:
                    normal = new Vector3(0, 1, 0);
                    if (flipNormals)
                    {
                        normal = normal * -1;
                    }
                    
                    offset = mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, 0),
                        normal));

                    if ((shape & 0b0001) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x/ 2f, node.dimensions.y, 0), normal));
                    }

                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y, 0), normal));
                    
                    if ((shape & 0b0100) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.x, node.dimensions.z / 2f), normal));
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y, node.dimensions.z / 2f), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, node.dimensions.z), normal));
                    
                    if ((shape & 0b1000) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f, node.dimensions.y, node.dimensions.z), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + node.dimensions, normal));
                    
                    break;
                case MeshOctreeNode.Y_NEGATIVE:
                    normal = new Vector3(0, -1, 0);
                    if (flipNormals)
                    {
                        normal = normal * -1;
                    }
                    
                    offset = mesh.addVertex(new VertexData(node.lowerCorner,
                        normal));

                    if ((shape & 0b0001) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0, 0), normal));
                    }

                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, 0), normal));
                    
                    if ((shape & 0b0100) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, 0, node.dimensions.z / 2f), normal));
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, node.dimensions.z / 2f), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, 0, node.dimensions.z), normal));
                    
                    if ((shape & 0b1000) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f,0, node.dimensions.z), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, node.dimensions.z), normal));
                    
                    break;
                case MeshOctreeNode.Z_POSITIVE:
                    normal = new Vector3(0, 0, 1);
                    if (flipNormals)
                    {
                        normal = normal * -1;
                    }
                    
                    offset = mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, 0, node.dimensions.z),
                        normal));

                    if ((shape & 0b0001) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0 , node.dimensions.z), normal));
                    }

                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, node.dimensions.z), normal));
                    
                    if ((shape & 0b0100) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y / 2f, node.dimensions.z), normal));
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y / 2f, node.dimensions.z), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, node.dimensions.z), normal));
                    
                    if ((shape & 0b1000) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f, node.dimensions.y, node.dimensions.z), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + node.dimensions, normal));
                    
                    break;
                case MeshOctreeNode.Z_NEGATIVE:
                    normal = new Vector3(0, 0, -1);
                    if (flipNormals)
                    {
                        normal = normal * -1;
                    }
                    
                    offset = mesh.addVertex(new VertexData(node.lowerCorner, normal));

                    if ((shape & 0b0001) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0, 0), normal));
                    }

                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, 0, 0), normal));
                    
                    if ((shape & 0b0100) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y/2f, 0), normal));
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y / 2f, 0), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(0, node.dimensions.y, 0), normal));
                    
                    if ((shape & 0b1000) != 0)
                    {
                        mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x / 2f, node.dimensions.y, 0), normal));
                    }
                    
                    mesh.addVertex(new VertexData(node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y, 0), normal));
                    
                    break;
            }

            return offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh">Mesh prototype to append data to</param>
        /// <param name="shape">shape of the face to generate (bitmask of subdivisions, see thesis for details)</param>
        /// <param name="offset">offset of first vertex in this face group</param>
        /// <param name="flipped">should the face be flipped</param>
        public void generateFaces(MeshPrototype mesh, int shape, int offset, bool flipped)
        {
            if (flipped)
            {
                switch (shape)
                {
                    case 0:
                        mesh.addFaces(new []{offset+ 0, offset+1, offset+3, offset+0, offset+3, offset+2});
                        break;
                    case 1:
                        mesh.addFaces(new []{offset+0, offset+1, offset+3, offset+1, offset+4, offset+3, offset+1, 
                            offset+2, offset+4});
                        break;
                    case 2:
                        mesh.addFaces(new []{offset+0, offset+2, offset+3, offset+3, offset+2, offset+4, offset+0, 
                            offset+1, offset+2});
                        break;
                    case 3:
                        mesh.addFaces(new []{offset+0, offset+1, offset+4, offset+1, offset+3, offset+4, offset+4, 
                            offset+3, offset+5, offset+1, offset+2, offset+3});
                        break;
                    case 4:
                        mesh.addFaces(new []{offset+0, offset+1, offset+2, offset+2, offset+4, offset+3, offset+1, 
                            offset+4, offset+2});
                        break;
                    case 5:
                        mesh.addFaces(new []{offset+0, offset+1, offset+3, offset+3, offset+5, offset+4, offset+1, 
                            offset+5, offset+3, offset+1, offset+2, offset+5});
                        break;
                    case 6:
                        mesh.addFaces(new []{offset+0, offset+1, offset+3, offset+0, offset+3, offset+2, offset+2, 
                            offset+3, offset+5, offset+2, offset+5, offset+4});
                        break;
                    case 7:
                        mesh.addFaces(new []{offset+0, offset+1, offset+3, offset+1, offset+4, offset+3, offset+1, 
                            offset+2, offset+4, offset+3, offset+6, offset+5, offset+3, offset+4, offset+6});
                        break;
                    case 8:
                        mesh.addFaces(new []{offset+0, offset+3, offset+2, offset+0, offset+1, offset+3, offset+1, 
                            offset+4, offset+3});
                        break;
                    case 9:
                        mesh.addFaces(new []{offset+0, offset+4, offset+3, offset+0, offset+1, offset+4, offset+1, 
                            offset+5, offset+4, offset+1, offset+2, offset+5});
                        break;
                    case 10:
                        mesh.addFaces(new []{offset+0, offset+4, offset+3, offset+0, offset+2, offset+4, offset+0, 
                            offset+1, offset+2, offset+2, offset+5, offset+4});
                        break;
                    case 11:
                        mesh.addFaces(new []{offset+0, offset+5, offset+4, offset+0, offset+1, offset+5, offset+1, 
                            offset+3, offset+5, offset+1, offset+2, offset+3, offset+5, offset+3, offset+6});
                        break;
                    case 12:
                        mesh.addFaces(new []{offset+0, offset+1, offset+2, offset+2, offset+4, offset+3, offset+1, 
                            offset+4, offset+2, offset+1, offset+5, offset+4});
                        break;
                    case 13:
                        mesh.addFaces(new []{offset+0, offset+1, offset+3, offset+3, offset+5, offset+4, offset+1, 
                            offset+5, offset+3, offset+1, offset+6, offset+5, offset+1, offset+2, offset+6});
                        break;
                    case 14:
                        mesh.addFaces(new []{offset+0, offset+3, offset+2, offset+0, offset+1, offset+3, offset+2, 
                            offset+5, offset+4, offset+2, offset+3, offset+5, offset+3, offset+6, offset+5});
                        break;
                    case 15:
                        mesh.addFaces(new []{offset+0, offset+1, offset+3, offset+1, offset+4, offset+3, offset+1, 
                            offset+2, offset+4, offset+3, offset+6, offset+5, offset+3, offset+4, offset+6, offset+4, 
                            offset+7, offset+6});
                        break;
                }
            }

            switch (shape)
            {
                case 0:
                    mesh.addFaces(new []{offset+ 0, offset+3, offset+1, offset+0, offset+2, offset+3});
                    break;
                case 1:
                    mesh.addFaces(new []{offset+0, offset+3, offset+1, offset+1, offset+3, offset+4, offset+1, 
                        offset+4, offset+2});
                    break;
                case 2:
                    mesh.addFaces(new []{offset+0, offset+3, offset+2, offset+3, offset+4, offset+2, offset+0, 
                        offset+2, offset+1});
                    break;
                case 3:
                    mesh.addFaces(new []{offset+0, offset+4, offset+1, offset+1, offset+4, offset+3, offset+4, 
                        offset+5, offset+3, offset+1, offset+3, offset+2});
                    break;
                case 4:
                    mesh.addFaces(new []{offset+0, offset+2, offset+1, offset+2, offset+3, offset+4, offset+1, 
                        offset+2, offset+4});
                    break;
                case 5:
                    mesh.addFaces(new []{offset+0, offset+3, offset+1, offset+3, offset+4, offset+5, offset+1, 
                        offset+3, offset+5, offset+1, offset+5, offset+2});
                    break;
                case 6:
                    mesh.addFaces(new []{offset+0, offset+3, offset+1, offset+0, offset+2, offset+3, offset+2, 
                        offset+5, offset+3, offset+2, offset+4, offset+5});
                    break;
                case 7:
                    mesh.addFaces(new []{offset+0, offset+3, offset+1, offset+1, offset+3, offset+4, offset+1, 
                        offset+4, offset+2, offset+3, offset+5, offset+6, offset+3, offset+6, offset+4});
                    break;
                case 8:
                    mesh.addFaces(new []{offset+0, offset+2, offset+3, offset+0, offset+3, offset+1, offset+1, 
                        offset+3, offset+4});
                    break;
                case 9:
                    mesh.addFaces(new []{offset+0, offset+3, offset+4, offset+0, offset+4, offset+1, offset+1, 
                        offset+4, offset+5, offset+1, offset+5, offset+2});
                    break;
                case 10:
                    mesh.addFaces(new []{offset+0, offset+3, offset+4, offset+0, offset+4, offset+2, offset+0, 
                        offset+2, offset+1, offset+2, offset+4, offset+5});
                    break;
                case 11:
                    mesh.addFaces(new []{offset+0, offset+4, offset+5, offset+0, offset+5, offset+1, offset+1, 
                        offset+5, offset+3, offset+1, offset+3, offset+2, offset+5, offset+6, offset+3});
                    break;
                case 12:
                    mesh.addFaces(new []{offset+0, offset+2, offset+1, offset+2, offset+3, offset+4, offset+1, 
                        offset+2, offset+4, offset+1, offset+4, offset+5});
                    break;
                case 13:
                    mesh.addFaces(new []{offset+0, offset+3, offset+1, offset+3, offset+4, offset+5, offset+1, 
                        offset+3, offset+5, offset+1, offset+5, offset+6, offset+1, offset+6, offset+2});
                    break;
                case 14:
                    mesh.addFaces(new []{offset+0, offset+2, offset+3, offset+0, offset+3, offset+1, offset+2, 
                        offset+4, offset+5, offset+2, offset+5, offset+3, offset+3, offset+5, offset+6});
                    break;
                case 15:
                    mesh.addFaces(new []{offset+0, offset+3, offset+1, offset+1, offset+3, offset+4, offset+1, 
                        offset+4, offset+2, offset+3, offset+5, offset+6, offset+3, offset+6, offset+4, offset+4, 
                        offset+6, offset+7});
                    break;
            }
        }
        
        /// <summary>
        /// Draws colored dots for each vertex generated. Useful for debugging vertex / face generation
        /// </summary>
        /// <param name="face">Face to generate dots for</param>
        /// <param name="shape">Shape of the face group</param>
        /// <param name="node">Node to draw dots on</param>
        /// <param name="transform">Transform of the octree</param>
        public void drawDebugVertcies(int face, int shape, MeshOctreeNode node, Transform transform)
        {
            float radius = node.dimensions.magnitude * 0.02f;
            
            int rx = 6, ry = 1;
            
            Color c0 = new Color(1, 0, 0);
            Color c1 = new Color(1, 1, 0);
            Color c2 = new Color(0, 1, 0);
            Color c3 = new Color(0.7f, 0, 0.7f);
            Color c4 = new Color(0, 0.7f, 0.7f);
            Color c5 = new Color(1, 0, 1);
            Color c6 = new Color(0, 0, 1);
            Color c7 = new Color(0, 1, 1);
            
            switch (face)
            {
                case MeshOctreeNode.X_POSITIVE:
                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(node.dimensions.x, 0.2f * node.dimensions.y, 0.2f * node.dimensions.z), 
                        rx, ry, c0);
                        
                    
                    if ((shape & 0b0001) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y / 2f, 0.2f * node.dimensions.z), 
                            rx, ry, c1);
                    }
                    
                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(node.dimensions.x, 0.8f * node.dimensions.y, 0.2f * node.dimensions.z),
                        rx, ry, c2);
                    
                    if ((shape & 0b0100) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x, 0.2f * node.dimensions.y, node.dimensions.z / 2f),
                            rx, ry, c3);
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x, 0.8f *node.dimensions.y, node.dimensions.z / 2f),
                            rx, ry, c4);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(node.dimensions.x, 0.2f * node.dimensions.y, 0.8f *node.dimensions.z),
                        rx, ry, c5);

                    if ((shape & 0b1000) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x, node.dimensions.y / 2f, 0.8f *node.dimensions.z),
                            rx, ry, c6);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(node.dimensions.x, 0.8f *node.dimensions.y, 0.8f *node.dimensions.z),
                        rx, ry, c7);
                    
                    break;
                case MeshOctreeNode.X_NEGATIVE:
                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0, 0.2f * node.dimensions.y, 0.2f * node.dimensions.z),
                        rx, ry, c0);

                    if ((shape & 0b0001) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0, node.dimensions.y / 2f, 0.2f * node.dimensions.z),
                            rx, ry, c1);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0, 0.8f *node.dimensions.y, 0.2f * node.dimensions.z),
                        rx, ry, c2);

                    if ((shape & 0b0100) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0, 0.2f * node.dimensions.y, node.dimensions.z / 2f),
                            rx, ry, c3);
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0, 0.8f *node.dimensions.y, node.dimensions.z / 2f),
                            rx, ry, c4);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0, 0.2f * node.dimensions.y, 0.8f *node.dimensions.z),
                        rx, ry, c5);
                    
                    if ((shape & 0b1000) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0, node.dimensions.y / 2f, 0.8f *node.dimensions.z),
                            rx, ry, c6);
                    }
                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0, 0.8f *node.dimensions.y, 0.8f *node.dimensions.z),
                        rx, ry, c7);
                    break;
                case MeshOctreeNode.Y_POSITIVE:

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, node.dimensions.y, 0.2f * node.dimensions.z),
                        rx, ry, c0);
                    
                    if ((shape & 0b0001) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x/ 2f, node.dimensions.y, 0.2f * node.dimensions.z),
                            rx, ry, c1);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f *node.dimensions.x, node.dimensions.y, 0.2f * node.dimensions.z),
                        rx, ry, c2);
                    
                    if ((shape & 0b0100) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.2f * node.dimensions.x, node.dimensions.y, node.dimensions.z / 2f),
                            rx, ry, c3);
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.8f *node.dimensions.x, node.dimensions.y, node.dimensions.z / 2f),
                            rx, ry, c4);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, node.dimensions.y, 0.8f *node.dimensions.z),
                        rx, ry, c5);
                    
                    if ((shape & 0b1000) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f, node.dimensions.y, 0.8f *node.dimensions.z),
                            rx, ry, c6);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f *node.dimensions.x, node.dimensions.y, 0.8f *node.dimensions.z),
                        rx, ry, c7);
                    
                    break;
                case MeshOctreeNode.Y_NEGATIVE:

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x,0,0.2f * node.dimensions.z),
                        rx, ry, c0);

                    if ((shape & 0b0001) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0, 0.2f * node.dimensions.z),
                            rx, ry, c1);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f * node.dimensions.x, 0, 0.2f * node.dimensions.z),
                        rx, ry, c2);

                    if ((shape & 0b0100) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.2f * node.dimensions.x, 0, node.dimensions.z / 2f),
                            rx, ry, c3);
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.8f * node.dimensions.x, 0, node.dimensions.z / 2f),
                            rx, ry, c4);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, 0, 0.8f * node.dimensions.z),
                        rx, ry, c5);

                    if ((shape & 0b1000) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f,0, 0.8f *node.dimensions.z),
                            rx, ry, c6);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f *node.dimensions.x, 0, 0.8f *node.dimensions.z),
                        rx, ry, c7);

                    break;
                case MeshOctreeNode.Z_POSITIVE:

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, 0.2f * node.dimensions.y, node.dimensions.z),
                        rx, ry, c0);

                    if ((shape & 0b0001) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0.2f * node.dimensions.y , node.dimensions.z),
                            rx, ry, c1);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f *node.dimensions.x, 0.2f * node.dimensions.y, node.dimensions.z),
                        rx, ry, c2);

                    if ((shape & 0b0100) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.2f * node.dimensions.x, node.dimensions.y / 2f, node.dimensions.z),
                            rx, ry, c3);
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.8f *node.dimensions.x, node.dimensions.y / 2f, node.dimensions.z),
                            rx, ry, c4);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, 0.8f *node.dimensions.y, node.dimensions.z),
                        rx, ry, c5);

                    if ((shape & 0b1000) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0.8f *node.dimensions.y, node.dimensions.z),
                            rx, ry, c6);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner +  new Vector3(0.8f *node.dimensions.x, 0.8f *node.dimensions.y, node.dimensions.z),
                        rx, ry, c7);

                    break;
                case MeshOctreeNode.Z_NEGATIVE:
                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, 0.2f * node.dimensions.y, 0),
                        rx, ry, c0);

                    if ((shape & 0b0001) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0.2f * node.dimensions.y, 0),
                            rx, ry, c1);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f *node.dimensions.x, 0.2f * node.dimensions.y, 0),
                        rx, ry, c2);
                    
                    if ((shape & 0b0100) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.2f * node.dimensions.x, node.dimensions.y/2f, 0),
                            rx, ry, c3);
                    }
                    
                    if ((shape & 0b0010) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(0.8f *node.dimensions.x, node.dimensions.y / 2f, 0),
                            rx, ry, c4);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.2f * node.dimensions.x, 0.8f *node.dimensions.y, 0),
                        rx, ry, c5);

                    if ((shape & 0b1000) != 0)
                    {
                        Geometry.drawSphere(transform, radius,
                            node.lowerCorner + new Vector3(node.dimensions.x / 2f, 0.8f *node.dimensions.y, 0),
                            rx, ry, c6);
                    }

                    Geometry.drawSphere(transform, radius,
                        node.lowerCorner + new Vector3(0.8f *node.dimensions.x, 0.8f *node.dimensions.y, 0),
                        rx, ry, c7);
                    
                    break;
            }
        }
    }
}