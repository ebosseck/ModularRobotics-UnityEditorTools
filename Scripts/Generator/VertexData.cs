using System;
using UnityEngine;
using System.Runtime.InteropServices;
using EditorTools.Maths.LinAlg;
using UnityEditor;

namespace EditorTools.MeshGenerator
{
    /// <summary>
    /// Structure containing vertex data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexData
    {
        public Vector3 pos;
        public Vector3 normal;
        public float u, v;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">Position component of the vertex</param>
        /// <param name="normal">Normal of the vertex</param>
        public VertexData(Vector3 pos, Vector3 normal)
        {
            this.pos = pos;
            this.normal = normal;

            int dir = VectorTools.getMaxComponentIdx(normal);

            switch (dir)
            {
                case 0:
                    u = pos.y; 
                    v = pos.z;
                    break;
                case 1:
                    u = pos.x; 
                    v = pos.z;
                    break;
                case 2:
                    u = pos.x; 
                    v = pos.y;
                    break;
                default:
                    Debug.Log("Could not determine Face orientation from normal. ");
                    u = pos.y; 
                    v = pos.z;
                    break;
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pos">Position component of the vertex</param>
        /// <param name="normal">Normal of the vertex</param>
        /// <param name="u">first component of texture coordinates</param>
        /// <param name="v">second component of texture coordinates</param>
        public VertexData(Vector3 pos, Vector3 normal, float u, float v)
        {
            this.pos = pos;
            this.normal = normal;
            this.u = u;
            this.v = v;
        }
    }
}