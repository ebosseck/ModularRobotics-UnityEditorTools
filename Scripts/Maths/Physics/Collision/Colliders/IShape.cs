using UnityEngine;

namespace EditorTools.Maths.Physics.Collision.Colliders
{
    public interface IShape
    {
        /// <summary>
        /// Computes the AABB of this collider in the provided reference frame
        /// </summary>
        /// <param name="octreeTransform">reference frame to compute the AABB in</param>
        /// <returns>Vector3[] containing minimal and maximal corners defining this AABB</returns>
        public Vector3[] getAABB(Transform octreeTransform);
        
        /// <summary>
        /// Support Function
        /// </summary>
        /// <param name="direction">Direction in World Space</param>
        /// <returns>the most extreme point in that direction</returns>
        public Vector3 support(Vector3 direction);

        #region Debugging

        /// <summary>
        /// Draw this collider in Unity Editor
        /// </summary>
        /// <param name="debugColor">color to draw the collider in</param>
        public void drawDebug(Color debugColor);

        #endregion

        /// <summary>
        /// gets the origin of this collider
        /// </summary>
        /// <returns>Origin of this collider</returns>
        public Vector3 getOrigin();
    }
}