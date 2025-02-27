using System;
using EditorTools.Attributes;
using EditorTools.BaseTypes;
using UnityEngine;

namespace EditorTools.Tools
{
    /// <summary>
    /// Delete this object on collision with a game object with the given tag
    /// </summary>
    public class DeleteOnHit : ExtendedMonoBehaviour
    {
        [Tag]
        [Tooltip("Tag for all objects, that should be able to collide with this object, without deleting it")]
        public String deleteTag;
        
        [Unit("s")]
        public float delay = 0.05f;

        [Unit("s")]
        [Tooltip("Maximum Lifetime for this object. If < 0, no maximum lifetime is enforced")]
        public float maxLifeTime = -1;

        /// <summary>
        /// Unity event called when object is instantiated
        /// </summary>
        private void Start()
        {
            if (maxLifeTime > 0)
            {
                Destroy(this.gameObject, maxLifeTime);
            }
        }

        /// <summary>
        /// Called once a collision is detected
        /// </summary>
        /// <param name="other">Collision partner</param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(deleteTag))
            {
                Destroy(this.gameObject, delay);
            }
        }
    }
}

