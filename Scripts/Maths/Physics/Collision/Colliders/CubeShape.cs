using System;
using System.Numerics;
using EditorTools.Display;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

namespace EditorTools.Maths.Physics.Collision.Colliders
{
    /// <summary>
    /// Class implementing a Cube shaped collider
    /// </summary>
    public class CubeShape: IShape
    {
        public Transform transform;
        
        // Cube with local dimensions d = 1
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transform">Transform used on this collider</param>
        public CubeShape(Transform transform)
        {
            this.transform = transform;
        }

        /// <summary>
        /// gets the origin of this collider
        /// </summary>
        /// <returns>Origin of this collider</returns>
        public Vector3 getOrigin()
        {
            return transform.position;
        }

        /// <summary>
        /// Support Function
        /// </summary>
        /// <param name="direction">Direction in World Space</param>
        /// <returns>the most extreme point in that direction</returns>
        public Vector3 support(Vector3 direction)
        {
            // Transform Direction to Shape Space
            direction = transform.InverseTransformDirection(direction).normalized;

            Vector3 position = new Vector3(sgn(direction.x), sgn(direction.y), sgn(direction.z));
            //Geometry.drawSphere(transform, 0.1f, position, 4, 4, Color.white);

            return transform.TransformPoint(position);
        }

        /// <summary>
        /// sign functions, fitted for this use case
        /// </summary>
        /// <param name="val">value to check</param>
        /// <returns>-.5f if val < 0, .5f otherwise</returns>
        private float sgn(float val)
        {
            if (val < 0)
            {
                return -0.5f;
            }
            return .5f;
        }
        
        /// <summary>
        /// Computes the AABB of this collider in the provided reference frame
        /// </summary>
        /// <param name="octreeTransform">reference frame to compute the AABB in</param>
        /// <returns>Vector3[] containing minimal and maximal corners defining this AABB</returns>
        public Vector3[] getAABB(Transform octreeTransform)
        {
            Matrix4x4 t = octreeTransform.worldToLocalMatrix * transform.localToWorldMatrix; // 

            Vector4[] c = new Vector4[]
            {
                new Vector4(-.5f, -.5f, -.5f, 1),
                new Vector4(.5f, -.5f, -.5f, 1),
                new Vector4(-.5f, .5f, -.5f, 1),
                new Vector4(.5f, .5f, -.5f, 1),

                new Vector4(-.5f, -.5f, .5f, 1),
                new Vector4(.5f, -.5f, .5f, 1),
                new Vector4(-.5f, .5f, .5f, 1),
                new Vector4(.5f, .5f, .5f, 1),
            };

            Vector3[] bb = new Vector3[]
            {
                new Vector3(),
                new Vector3()
            };
            
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = t * c[i];

                if (i == 0)
                {
                    bb[0] = c[0];
                    bb[1] = c[0];
                }

                bb[0] = Vector3.Min(bb[0], c[i]);
                bb[1] = Vector3.Max(bb[1], c[i]);
            }

            return bb;
        }
        
        #region Debug
        /// <summary>
        /// Draw this collider in Unity Editor
        /// </summary>
        /// <param name="debugColor">color to draw the collider in</param>
        public void drawDebug(Color debugColor)
        {
            Geometry.drawBox(transform, Vector3.one,-Vector3.one*0.5f , debugColor);
        }
        
        #endregion
        
        
    }
}