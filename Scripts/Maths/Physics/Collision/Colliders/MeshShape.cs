using System;
using UnityEngine;

namespace EditorTools.Maths.Physics.Collision.Colliders
{
    /// <summary>
    /// Class implementing a convex mesh collider
    /// </summary>
    public class MeshShape : IShape
    {
        private Mesh collisionObject;
        private Transform transform;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collisionObject">convex mesh used for collision detection</param>
        /// <param name="transform">Transform used on this collider</param>
        public MeshShape(Mesh collisionObject, Transform transform)
        {
            this.collisionObject = collisionObject;
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
        /// Computes the AABB of this collider in the provided reference frame
        /// </summary>
        /// <param name="octreeTransform">reference frame to compute the AABB in</param>
        /// <returns>Vector3[] containing minimal and maximal corners defining this AABB</returns>
        public Vector3[] getAABB(Transform octreeTransform)
        {
            Matrix4x4 t = octreeTransform.worldToLocalMatrix * transform.localToWorldMatrix; // 

            Vector3[] bb = new Vector3[]
            {
                new Vector3(),
                new Vector3()
            };

            Vector3[] c = collisionObject.vertices;
            
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
        
        /// <summary>
        /// Support Function
        /// </summary>
        /// <param name="direction">Direction in World Space</param>
        /// <returns>the most extreme point in that direction</returns>
        public Vector3 support(Vector3 direction)
        {
            direction = transform.InverseTransformDirection(direction).normalized;

            Vector3 position = collisionObject.vertices[0];
            float dist = Single.NegativeInfinity;
            float distance;
            
            foreach (Vector3 v in collisionObject.vertices)
            {
                distance = Vector3.Dot(v, direction);
                if (distance > dist)
                {
                    dist = distance;
                    position = v;
                }
            }

            return transform.TransformPoint(position);
        }

        #region Debugging
        /// <summary>
        /// Draw this collider in Unity Editor
        /// </summary>
        /// <param name="debugColor">color to draw the collider in</param>
        public void drawDebug(Color debugColor) {
            // TODO: Implement Mesh debug
            throw new NotImplementedException();
        }

        #endregion
    }
}