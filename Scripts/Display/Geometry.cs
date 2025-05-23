using System;
using System.Collections;
using System.Collections.Generic;
using EditorTools.MeshGenerator;
using JetBrains.Annotations;
using UnityEngine;

namespace EditorTools.Display
{
    /// <summary>
    /// Class for drawing geometric shapes in the UnityEditor. This is mainly useful for debugging
    /// </summary>
    public class Geometry : MonoBehaviour
    {

        #region AABB
        /// <summary>
        /// Draws a axis aligned wireframe bounding box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box in world coordinates</param>
        /// <param name="dimensions">Dimensions of the box in world coordinates</param>
        /// <param name="color">Color of the lines</param>
        public static void drawAABB(Vector3 origin, Vector3 dimensions, Color? color = null)
        {
            drawAABB(origin, dimensions, Vector3.zero, color);
        }
        
        /// <summary>
        /// Draws a axis aligned wireframe bounding box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box in world coordinates</param>
        /// <param name="dimensions">Dimensions of the box in world coordinates</param>
        /// <param name="offset">Offset in world coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        public static void drawAABB(Vector3 origin, Vector3 dimensions, Vector3? offset = null, Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = origin + doffset;
            Vector3 p01 = new Vector3(origin.x + dimensions.x, origin.y, origin.z) + doffset;
            Vector3 p02 = new Vector3(origin.x, origin.y+ dimensions.y, origin.z) + doffset;
            Vector3 p03 = new Vector3(origin.x + dimensions.x, origin.y + dimensions.y, origin.z) + doffset;
            
            Vector3 p10 = new Vector3(origin.x, origin.y, origin.z + dimensions.z) + doffset;
            Vector3 p11 = new Vector3(origin.x + dimensions.x, origin.y, origin.z + dimensions.z) + doffset;
            Vector3 p12 = new Vector3(origin.x, origin.y + dimensions.y, origin.z + dimensions.z) + doffset;
            Vector3 p13 = origin + dimensions + doffset;
            
            Debug.DrawLine(p00, p01, dcolor);
            Debug.DrawLine(p00, p02, dcolor);
            Debug.DrawLine(p01, p03, dcolor);
            Debug.DrawLine(p02, p03, dcolor);
            
            Debug.DrawLine(p00, p10, dcolor);
            Debug.DrawLine(p01, p11, dcolor);
            Debug.DrawLine(p02, p12, dcolor);
            Debug.DrawLine(p03, p13, dcolor);
            
            Debug.DrawLine(p10, p11, dcolor);
            Debug.DrawLine(p10, p12, dcolor);
            Debug.DrawLine(p11, p13, dcolor);
            Debug.DrawLine(p12, p13, dcolor);
        }

        #endregion
        
        #region AABB Between
        /// <summary>
        /// Draws a axis aligned wireframe bounding box in UnityEditor
        /// </summary>
        /// <param name="lower">lower corner of the AABB</param>
        /// <param name="upper">upper corner of the AABB</param>
        /// <param name="color">Color of the lines</param>
        public static void drawAABBBetween(Vector3 lower, Vector3 upper, Color? color = null)
        {
            drawAABBBetween(lower, upper, Vector3.zero, color);
        }
        
        /// <summary>
        /// Draws a axis aligned wireframe bounding box in UnityEditor
        /// </summary>
        /// <param name="lower">lower corner of the AABB</param>
        /// <param name="upper">upper corner of the AABB</param>
        /// <param name="offset">Offset in world coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        public static void drawAABBBetween(Vector3 lower, Vector3 upper, Vector3? offset = null, Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = lower + doffset;
            Vector3 p01 = new Vector3(upper.x, lower.y, lower.z) + doffset;
            Vector3 p02 = new Vector3(lower.x, upper.y, lower.z) + doffset;
            Vector3 p03 = new Vector3(upper.x, upper.y, lower.z) + doffset;
            
            Vector3 p10 = new Vector3(lower.x, lower.y, upper.z) + doffset;
            Vector3 p11 = new Vector3(upper.x, lower.y, upper.z) + doffset;
            Vector3 p12 = new Vector3(lower.x, upper.y, upper.z) + doffset;
            Vector3 p13 = upper + doffset;
            
            Debug.DrawLine(p00, p01, dcolor);
            Debug.DrawLine(p00, p02, dcolor);
            Debug.DrawLine(p01, p03, dcolor);
            Debug.DrawLine(p02, p03, dcolor);
            
            Debug.DrawLine(p00, p10, dcolor);
            Debug.DrawLine(p01, p11, dcolor);
            Debug.DrawLine(p02, p12, dcolor);
            Debug.DrawLine(p03, p13, dcolor);
            
            Debug.DrawLine(p10, p11, dcolor);
            Debug.DrawLine(p10, p12, dcolor);
            Debug.DrawLine(p11, p13, dcolor);
            Debug.DrawLine(p12, p13, dcolor);
        }

        #endregion

        #region Line

        /// <summary>
        /// Draws a wireframe box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box</param>
        /// <param name="dimensions">Dimensions of the box</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        public static void drawLine(Transform origin, Vector3 start, Vector3 end, Color? color = null)
        {
            
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = origin.TransformPoint(start);
            Vector3 p01 = origin.TransformPoint(end);


            Debug.DrawLine(p00, p01, dcolor);
        }

        #endregion
        
        #region Box
        /// <summary>
        /// Draws a wireframe box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box</param>
        /// <param name="dimensions">Dimensions of the box</param>
        /// <param name="color">Color of the lines</param>
        public static void drawBox(Transform origin, Vector3 dimensions, Color? color = null)
        {
            drawBox(origin, dimensions, Vector3.zero, color);
        }

        /// <summary>
        /// Draws a wireframe box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box</param>
        /// <param name="dimensions">Dimensions of the box</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        public static void drawBox(Transform origin, Vector3 dimensions, Vector3? offset = null, Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = origin.TransformPoint(doffset);
            Vector3 p01 = origin.TransformPoint(new Vector3(dimensions.x, 0, 0) + doffset);
            Vector3 p02 = origin.TransformPoint(new Vector3(0, dimensions.y, 0) + doffset);
            Vector3 p03 = origin.TransformPoint(new Vector3(dimensions.x, dimensions.y, 0) + doffset);
            
            Vector3 p10 = origin.TransformPoint(new Vector3(0, 0, dimensions.z) + doffset);
            Vector3 p11 = origin.TransformPoint(new Vector3(dimensions.x, 0, dimensions.z) + doffset);
            Vector3 p12 = origin.TransformPoint(new Vector3(0, dimensions.y, dimensions.z) + doffset);
            Vector3 p13 = origin.TransformPoint(dimensions + doffset);
            
            
            Debug.DrawLine(p00, p01, dcolor);
            Debug.DrawLine(p00, p02, dcolor);
            Debug.DrawLine(p01, p03, dcolor);
            Debug.DrawLine(p02, p03, dcolor);
            
            Debug.DrawLine(p00, p10, dcolor);
            Debug.DrawLine(p01, p11, dcolor);
            Debug.DrawLine(p02, p12, dcolor);
            Debug.DrawLine(p03, p13, dcolor);
            
            Debug.DrawLine(p10, p11, dcolor);
            Debug.DrawLine(p10, p12, dcolor);
            Debug.DrawLine(p11, p13, dcolor);
            Debug.DrawLine(p12, p13, dcolor);
        }

        /// <summary>
        /// Draws a wireframe box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box</param>
        /// <param name="dimensions">Dimensions of the box</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        public static void drawBoxInverse(Transform origin, Vector3 dimensions, Vector3? offset = null, Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = origin.InverseTransformPoint(doffset);
            Vector3 p01 = origin.InverseTransformPoint(new Vector3(dimensions.x, 0, 0) + doffset);
            Vector3 p02 = origin.InverseTransformPoint(new Vector3(0, dimensions.y, 0) + doffset);
            Vector3 p03 = origin.InverseTransformPoint(new Vector3(dimensions.x, dimensions.y, 0) + doffset);
            
            Vector3 p10 = origin.InverseTransformPoint(new Vector3(0, 0, dimensions.z) + doffset);
            Vector3 p11 = origin.InverseTransformPoint(new Vector3(dimensions.x, 0, dimensions.z) + doffset);
            Vector3 p12 = origin.InverseTransformPoint(new Vector3(0, dimensions.y, dimensions.z) + doffset);
            Vector3 p13 = origin.InverseTransformPoint(dimensions + doffset);
            
            
            Debug.DrawLine(p00, p01, dcolor);
            Debug.DrawLine(p00, p02, dcolor);
            Debug.DrawLine(p01, p03, dcolor);
            Debug.DrawLine(p02, p03, dcolor);
            
            Debug.DrawLine(p00, p10, dcolor);
            Debug.DrawLine(p01, p11, dcolor);
            Debug.DrawLine(p02, p12, dcolor);
            Debug.DrawLine(p03, p13, dcolor);
            
            Debug.DrawLine(p10, p11, dcolor);
            Debug.DrawLine(p10, p12, dcolor);
            Debug.DrawLine(p11, p13, dcolor);
            Debug.DrawLine(p12, p13, dcolor);
        }
        
        /// <summary>
        /// Draws a wireframe box in UnityEditor
        /// </summary>
        /// <param name="dimensions">Dimensions of the box</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        public static void drawBox(Vector3 dimensions, Vector3? offset = null, Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = (doffset);
            Vector3 p01 = (new Vector3(dimensions.x, 0, 0) + doffset);
            Vector3 p02 = (new Vector3(0, dimensions.y, 0) + doffset);
            Vector3 p03 = (new Vector3(dimensions.x, dimensions.y, 0) + doffset);
            
            Vector3 p10 = (new Vector3(0, 0, dimensions.z) + doffset);
            Vector3 p11 = (new Vector3(dimensions.x, 0, dimensions.z) + doffset);
            Vector3 p12 = (new Vector3(0, dimensions.y, dimensions.z) + doffset);
            Vector3 p13 = (dimensions + doffset);
            
            
            Debug.DrawLine(p00, p01, dcolor);
            Debug.DrawLine(p00, p02, dcolor);
            Debug.DrawLine(p01, p03, dcolor);
            Debug.DrawLine(p02, p03, dcolor);
            
            Debug.DrawLine(p00, p10, dcolor);
            Debug.DrawLine(p01, p11, dcolor);
            Debug.DrawLine(p02, p12, dcolor);
            Debug.DrawLine(p03, p13, dcolor);
            
            Debug.DrawLine(p10, p11, dcolor);
            Debug.DrawLine(p10, p12, dcolor);
            Debug.DrawLine(p11, p13, dcolor);
            Debug.DrawLine(p12, p13, dcolor);
        }
        
        #endregion

        #region Partial Box
        // <summary>
        // Draws a wireframe box in UnityEditor
        // </summary>
        // <param name="origin">Origin point of the box</param>
        // <param name="dimensions">Dimensions of the box</param>
        // <param name="color">Color of the lines</param>
        // <param name="faces">Bitflags which faces should be drawn in format [z-|z+|y-|y+|x-|x+] </param>
        //public static void drawBox(Transform origin, Vector3 dimensions, Color? color = null, int faces = 0b111111)
        //{
        //    drawBox(origin, dimensions, Vector3.zero, color, faces);
        //}
        
        /// <summary>
        /// Draws a partial wireframe box in UnityEditor
        /// </summary>
        /// <param name="origin">Origin point of the box</param>
        /// <param name="dimensions">Dimensions of the box</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="color">Color of the lines</param>
        /// <param name="faces">Bitflags which faces should be drawn in format [z-|z+|y-|y+|x-|x+] </param>
        public static void drawBox(Transform origin, Vector3 dimensions, Vector3? offset = null, Color? color = null, 
            int faces = 0b111111, bool triangulate = false)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            Vector3 p00 = origin.TransformPoint(doffset);
            Vector3 p01 = origin.TransformPoint(new Vector3(dimensions.x, 0, 0) + doffset);
            Vector3 p02 = origin.TransformPoint(new Vector3(0, dimensions.y, 0) + doffset);
            Vector3 p03 = origin.TransformPoint(new Vector3(dimensions.x, dimensions.y, 0) + doffset);
            
            Vector3 p10 = origin.TransformPoint(new Vector3(0, 0, dimensions.z) + doffset);
            Vector3 p11 = origin.TransformPoint(new Vector3(dimensions.x, 0, dimensions.z) + doffset);
            Vector3 p12 = origin.TransformPoint(new Vector3(0, dimensions.y, dimensions.z) + doffset);
            Vector3 p13 = origin.TransformPoint(dimensions + doffset);


            if ((faces & 0b101_000) != 0) // z-, y-
            {
                Debug.DrawLine(p00, p01, dcolor);
            }

            if ((faces & 0b100_010) != 0) // z-, x-
            {
                Debug.DrawLine(p00, p02, dcolor);
            }

            if ((faces & 0b100_001) != 0) // z-, x+
            {
                Debug.DrawLine(p01, p03, dcolor);
            }

            if ((faces & 0b100_100) != 0) // z-, y+
            {
                Debug.DrawLine(p02, p03, dcolor);
            }

            if ((faces & 0b001_010) != 0) // y-, x-
            {
                Debug.DrawLine(p00, p10, dcolor);
            }

            if ((faces & 0b001_001) != 0) // y-, x+
            {
                Debug.DrawLine(p01, p11, dcolor);
            }

            if ((faces & 0b000_110) != 0) // y+, x-
            {
                Debug.DrawLine(p02, p12, dcolor);
            }

            if ((faces & 0b000_101) != 0) // y+, x+
            {
                Debug.DrawLine(p03, p13, dcolor);
            }

            if ((faces & 0b011_000) != 0) // z+, y-
            {
                Debug.DrawLine(p10, p11, dcolor);
            }

            if ((faces & 0b010_010) != 0) // z+, x-
            {
                Debug.DrawLine(p10, p12, dcolor);
            }

            if ((faces & 0b010_001) != 0) // z+, x+
            {
                Debug.DrawLine(p11, p13, dcolor);
            }

            if ((faces & 0b010_100) != 0) // z+, y+
            {
                Debug.DrawLine(p12, p13, dcolor);
            }

            if (triangulate)
            {
                if ((faces & 0b00_00_01) != 0) // x+
                {
                    Debug.DrawLine(p01, p13, dcolor);
                }
                
                if ((faces & 0b00_00_10) != 0) // x-
                {
                    Debug.DrawLine(p00, p12, dcolor);
                }
                
                if ((faces & 0b00_01_00) != 0) // y+
                {
                    Debug.DrawLine(p02, p13, dcolor);
                }
                
                if ((faces & 0b00_10_00) != 0) // y-
                {
                    Debug.DrawLine(p00, p11, dcolor);
                }
                
                if ((faces & 0b01_00_00) != 0) // z+
                {
                    Debug.DrawLine(p10, p13, dcolor);
                }
                
                if ((faces & 0b10_00_00) != 0) // z-
                {
                    Debug.DrawLine(p00, p03, dcolor);
                }
            }
        }

        #endregion
        
        #region Cylinder

        /// <summary>
        /// Draws a Cylinder in UnityEditor
        /// </summary>
        /// <param name="origin">Origin transform of the cylinder</param>
        /// <param name="radius">Radius of the cylinder</param>
        /// <param name="height">Height of the cylinder</param>
        /// <param name="drawCaps">If True, draws a Triangle Fan to the cylinder caps, defaults to true</param>
        /// <param name="resolution">Sampling resolution for drawing the cylinder, defaults to 16</param>
        /// <param name="color">Color of the cylinder, defaults to white</param>
        public static void drawCylinder(Transform origin, float radius, float height,
            bool drawCaps = true,
            int resolution = 16, Color? color = null)
        {
            drawCylinder(origin, radius, height, Vector3.zero, drawCaps, resolution, color);
        }

        /// <summary>
        /// Draws a Cylinder in UnityEditor
        /// </summary>
        /// <param name="origin">Origin transform of the cylinder</param>
        /// <param name="radius">Radius of the cylinder</param>
        /// <param name="height">Height of the cylinder</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0)</param>
        /// <param name="drawCaps">If True, draws a Triangle Fan to the cylinder caps, defaults to true</param>
        /// <param name="resolution">Sampling resolution for drawing the cylinder, defaults to 16</param>
        /// <param name="color">Color of the cylinder, defaults to white</param>
        public static void drawCylinder(Transform origin, float radius, float height, Vector3? offset = null, bool drawCaps = true, 
            int resolution = 16, Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }
            
            float stepSize = 2.0f / resolution;
            
            Vector3 centerPoint = origin.TransformPoint(doffset);
            Vector3[] baseVertices = new Vector3[resolution];
            
            Vector3 topPoint = origin.TransformPoint((Vector3.up*height) + doffset);
            Vector3[] topVertices = new Vector3[resolution];
            
            for (int i = 0; i < resolution; i++)
            {
                
                baseVertices[i] = origin.TransformPoint(new Vector3(radius*(float)Math.Sin(Math.PI*stepSize*i), 0, radius*(float)Math.Cos(Math.PI*stepSize*i))+ doffset);
                topVertices[i] = origin.TransformPoint(new Vector3(radius*(float)Math.Sin(Math.PI*stepSize*i), height, radius*(float)Math.Cos(Math.PI*stepSize*i))+ doffset);
            }

            for (int i = 0; i < resolution; i++)
            {
                Debug.DrawLine(baseVertices[i], baseVertices[(i+1)%resolution], dcolor);
                Debug.DrawLine(topVertices[i], topVertices[(i+1)%resolution], dcolor);
                Debug.DrawLine(baseVertices[i], topVertices[i], dcolor);
                if (drawCaps)
                {
                    Debug.DrawLine(baseVertices[i], centerPoint, dcolor);
                    Debug.DrawLine(topVertices[i], topPoint, dcolor);
                }
            }
        }

        #endregion

        #region UVSphere

        /// <summary>
        /// Draws a UV Sphere in UnityEditor
        /// </summary>
        /// <param name="origin">Origin transform of the sphere</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0))</param>
        /// <param name="resolutionXZ">Resolution of the Sphere's rings, defaults to 16</param>
        /// <param name="resolutionY">Resolution of the Sphere's Y-axis, defaults to 16</param>
        /// <param name="color">Color of the sphere, defaults to white</param>
        public static void drawSphere(Transform origin, float radius, int resolutionXZ = 16,
            int resolutionY = 16,
            Color? color = null)
        {
            drawSphere(origin, radius, Vector3.zero, resolutionXZ, resolutionY, color);
        }

        /// <summary>
        /// Draws a UV Sphere in UnityEditor
        /// </summary>
        /// <param name="origin">Origin transform of the sphere</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="offset">Offset in local coordinates from the object's origin, defaults to (0,0,0))</param>
        /// <param name="resolutionXZ">Resolution of the Sphere's rings, defaults to 16</param>
        /// <param name="resolutionY">Resolution of the Sphere's Y-axis, defaults to 16</param>
        /// <param name="color">Color of the sphere, defaults to white</param>
        public static void drawSphere(Transform origin, float radius, Vector3? offset = null, int resolutionXZ = 16, int resolutionY = 16, 
            Color? color = null)
        {
            Vector3 doffset = Vector3.zero;
            if (offset != null)
            {
                doffset = (Vector3)offset;
            }
            
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }
            
            float stepSizeXY = 2.0f / resolutionXZ;
            float stepSizeY = 1.0f / (resolutionY + 2);
            
            
            Vector3 bottom = origin.TransformPoint(-(Vector3.up*radius) + doffset);
            Vector3 top = origin.TransformPoint((Vector3.up*radius) + doffset);
            Vector3[][] vertices = new Vector3[resolutionY+1][];
            
            
            for (int i = 1; i < resolutionY+2; i++)
            {
                vertices[i-1] = new Vector3[resolutionXZ];
                
                float ringrad =  radius * (float) Math.Sin(Math.PI * stepSizeY * (i));
                float height = -radius  * (float) Math.Cos(Math.PI * stepSizeY * (i));
                
                for (int j = 0; j < resolutionXZ; j++)
                {
                    vertices[i-1][j] = origin.TransformPoint(new Vector3(ringrad*(float)Math.Sin(Math.PI*stepSizeXY*j), height, ringrad*(float)Math.Cos(Math.PI*stepSizeXY*j)) + doffset);
                }
            }

            for (int i = 0; i < resolutionY+1; i++)
            {
                for (int j = 0; j < resolutionXZ; j++)
                {
                    Debug.DrawLine(vertices[i][j], vertices[i][(j + 1) % resolutionXZ], dcolor);
                    if (i > 0)
                    {
                        Debug.DrawLine(vertices[i-1][j], vertices[i][j], dcolor);
                    }
                }
            }

            if (resolutionY > 0)
            {
                for (int j = 0; j < resolutionXZ; j++)
                {
                    Debug.DrawLine(bottom, vertices[0][(j + 1) % resolutionXZ], dcolor);
                    Debug.DrawLine(vertices[resolutionY][(j + 1) % resolutionXZ], top, dcolor);
                }
            }
        }

        #endregion

        #region Simplex

        /// <summary>
        /// Draws a (up to) threedimensional simplex 
        /// </summary>
        /// <param name="transform">Origin transform of the simplex</param>
        /// <param name="vertices">Vertices of the simplex</param>
        /// <param name="vertexCount">number of vertices of the simplex</param>
        /// <param name="color">Color of the simplex</param>
        public static void drawSimplex([CanBeNull] Transform transform, Vector3[] vertices, int vertexCount, Color? color)
        {
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            if (transform != null)
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    vertices[i] = transform.TransformPoint(vertices[i]);
                }
            }

            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = i + 1; j < vertexCount; j++)
                {
                    Debug.DrawLine(vertices[i], vertices[j], dcolor);
                }
            }
        }

        #endregion

        #region Mesh Prototype

        /// <summary>
        /// Draws a mesh prototype object in Editor
        /// </summary>
        /// <param name="transform">Transform of the mesh to draw</param>
        /// <param name="mesh">Mesh to draw</param>
        /// <param name="color">Color of the mesh to draw</param>
        public static void drawProtoMesh(Transform transform, MeshPrototype mesh,
            Color? color)
        {
            Color dcolor = Color.white;
            if (color != null)
            {
                dcolor = (Color) color;
            }

            for (int i = 0; i < mesh.faces.Count; i+=3)
            {
                Debug.DrawLine(transform.TransformPoint(mesh.vertices[mesh.faces[i]].pos), transform.TransformPoint(mesh.vertices[mesh.faces[i+1]].pos), dcolor);
                Debug.DrawLine(transform.TransformPoint(mesh.vertices[mesh.faces[i+1]].pos), transform.TransformPoint(mesh.vertices[mesh.faces[i+2]].pos), dcolor);
                Debug.DrawLine(transform.TransformPoint(mesh.vertices[mesh.faces[i+2]].pos), transform.TransformPoint(mesh.vertices[mesh.faces[i]].pos), dcolor);
            }
        }

        #endregion
    }
}
