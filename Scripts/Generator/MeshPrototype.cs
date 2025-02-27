using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.UI;

namespace EditorTools.MeshGenerator
{
    /// <summary>
    /// Class for collecting data to later apply to a mesh object. Optimized for quick adding of data
    /// </summary>
    public class MeshPrototype
    {
        public List<VertexData> vertices = new List<VertexData>();
        public List<int> faces = new List<int>();

        /// <summary>
        /// Add a face with the vertices at the given indices in the vertex list
        /// </summary>
        /// <param name="a">First vertex of face</param>
        /// <param name="b">Second vertex of face</param>
        /// <param name="c">Third vertex of face</param>
        public void addFace(int a, int b, int c)
        {
            faces.Add(a);
            faces.Add(b);
            faces.Add(c);
        }

        /// <summary>
        /// Bulk add faces
        /// </summary>
        /// <param name="indices">List of vertex indices to add to the face list</param>
        public void addFaces(int[] indices)
        {
            faces.AddRange(indices);
        }

        /// <summary>
        /// Adds the given VertexData to the vertex list
        /// </summary>
        /// <param name="vertex">VertexData to Add</param>
        /// <returns>index of the newly added vertex data</returns>
        public int addVertex(VertexData vertex)
        {
            vertices.Add(vertex);

            return vertices.Count - 1;
        }

        /// <summary>
        /// Bulk adds the given VertexData[] to the vertex list
        /// </summary>
        /// <param name="vertex">VertexData array to Add</param>
        /// <returns>index of the last added vertex</returns>
        public int addVertex(VertexData[] vertex)
        {
            vertices.AddRange(vertex);
            return vertices.Count - 1;
        }
        
        /// <summary>
        /// Get the current vertex count
        /// </summary>
        /// <returns>the current length of the vertex list</returns>
        public int getCurrentVertexOffset()
        {
            return vertices.Count;
        }

        /// <summary>
        /// Apply the current data to the given mesh
        /// </summary>
        /// <param name="mesh">Mesh to apply the current data to</param>
        public void applyToMesh(Mesh mesh)
        {
            var layout = new[]
            {
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2),
            };
            
            mesh.SetVertexBufferParams(vertices.Count, layout);
            mesh.SetVertexBufferData(vertices, 0, 0, vertices.Count);
            
            mesh.SetIndexBufferParams(faces.Count, IndexFormat.UInt32);
            mesh.SetIndexBufferData(faces, 0, 0, faces.Count);

            mesh.subMeshCount = 1;
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, faces.Count));
        }
    }
    
}