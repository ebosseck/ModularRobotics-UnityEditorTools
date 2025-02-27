using EditorTools.Display;
using UnityEngine;

namespace EditorTools.Maths.Physics.Collision.Colliders
{
    /// <summary>
    /// Class implementing a Sphere shaped collider
    /// </summary>
    public class SphereShape: IShape
    {
        public Transform transform;
        
        /// <summary>
        /// Sphere with d=1
        /// </summary>
        /// <param name="transform">Local transform of this shape</param>
        public SphereShape(Transform transform)
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
            //Geometry.drawSphere(transform, 0.1f, direction, 4, 4, Color.white);
            return transform.TransformPoint(direction);
        }
        
        /// <summary>
        /// Computes the AABB of this collider in the provided reference frame
        /// Based on https://tavianator.com/2014/ellipsoid_bounding_boxes.html
        /// </summary>
        /// <param name="octreeTransform">reference frame to compute the AABB in</param>
        /// <returns>Vector3[] containing minimal and maximal corners defining this AABB</returns>
        public Vector3[] getAABB(Transform octreeTransform)
        {
            Matrix4x4 t =  octreeTransform.worldToLocalMatrix * transform.localToWorldMatrix;

            float sqr1 = Mathf.Sqrt((t.m00 * t.m00) + (t.m01 * t.m01) + (t.m02 * t.m02));
            float sqr2 = Mathf.Sqrt((t.m10 * t.m10) + (t.m11 * t.m11) + (t.m12 * t.m12));
            float sqr3 = Mathf.Sqrt((t.m20 * t.m20) + (t.m21 * t.m21) + (t.m22 * t.m22));
            
            Vector3[] result = new Vector3[]{new Vector3(), new Vector3()};

            result[0].x = (t.m03 - sqr1);
            result[0].y = (t.m13 - sqr2);
            result[0].z = (t.m23 - sqr3);
            
            result[1].x = (t.m03 + sqr1);
            result[1].y = (t.m13 + sqr2);
            result[1].z = (t.m23 + sqr3);
            
            return result;
        }

        #region Debug
        
        /// <summary>
        /// Draw this collider in Unity Editor
        /// </summary>
        /// <param name="debugColor">color to draw the collider in</param>
        public void drawDebug(Color debugColor)
        {
            Geometry.drawSphere(transform, 1f, 16, 16, debugColor);
        }
        
        #endregion
        
        
    }
}