using System;
using EditorTools.Display;
using UnityEngine;

namespace EditorTools.Maths.Physics.Collision.Colliders
{
    /// <summary>
    /// Class implementing a Cylinder shaped collider
    /// </summary>
    public class CylinderShape: IShape
    {
        public Transform transform;
        
        // Cylinder with local dimensions r = 1, h = 1
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transform">Transform used on this collider</param>
        public CylinderShape(Transform transform)
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
            direction = transform.InverseTransformDirection(direction);

            float s = Mathf.Sqrt((direction.x * direction.x) + (direction.z * direction.z));
            
            Vector3 position = new Vector3(direction.x/s, sgn(direction.y), direction.z/s);
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

            float r =1, h = 0.5f;
            
            Vector4[] c = new Vector4[]
            {
                new Vector4(-r, -h, -r, 1),
                new Vector4(r, -h, -r, 1),
                new Vector4(-r, h, -r, 1),
                new Vector4(r, h, -r, 1),

                new Vector4(-r, -h, r, 1),
                new Vector4(r, -h, r, 1),
                new Vector4(-r, h, r, 1),
                new Vector4(r, h, r, 1),
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

        
        /*
         * The version below should produce a tighter fit, but is currently buggy. Therefore the approximation of a cube above is done
         */
        /*public Vector3[] getAABB(Transform octreeTransform)
        {
            //TODO: Fix scale factors
            Matrix4x4 t = octreeTransform.worldToLocalMatrix * transform.localToWorldMatrix; // 

            Vector4 pb = t * new Vector4(0, 1, 0, 1);
            Vector4 pa = t * new Vector4(0, 0, 0, 1);
            
            //Geometry.drawSphere(octreeTransform, 0.1f, pa, color: Color.red);
            //Geometry.drawSphere(octreeTransform, 0.1f, pb, color: Color.yellow);

            Vector3 ra = t.GetScale(); //*
            
            Vector4 a = t * new Vector4(0, 1, 0, 0); // transform.localToWorldMatrix;

            float da = Vector4.Dot(a, a);
            
            Vector4 e = new Vector4(
                ra.x * Mathf.Sqrt(1.0f - ((a.x * a.x) / da)),
                ra.y * Mathf.Sqrt(1.0f - ((a.y * a.y) / da)),
                ra.z * Mathf.Sqrt(1.0f - ((a.z * a.z) / da)),
                0);
            
            //Geometry.drawSphere(octreeTransform, 0.1f, Vector3.Min((pa - e), (pb - e)), color: Color.blue);
            //Geometry.drawSphere(octreeTransform, 0.1f, Vector3.Max((pa + e), (pb + e)), color: Color.green);
            
            return new[] {
                Vector3.Min((pa - e), (pb - e)), 
                Vector3.Max((pa + e), (pb + e))};
        } */

        /// <summary>
        /// Draw this collider in Unity Editor
        /// </summary>
        /// <param name="debugColor">color to draw the collider in</param>
        public void drawDebug(Color debugColor)
        {
            Geometry.drawCylinder(transform, 1, 1, new Vector3(0, -.5f, 0), true, 16, debugColor);
        }

    }
}