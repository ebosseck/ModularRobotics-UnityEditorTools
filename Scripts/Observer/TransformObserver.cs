using UnityEngine;

namespace EditorTools.Observer
{
    /// <summary>
    /// Observes a transform
    /// </summary>
    public class TransformObserver: AbstractObserver<Transform>
    {
        /// <summary>
        /// Unity Event, called after Update
        /// </summary>
        public void LateUpdate()
        {
            if (observed.hasChanged)
            {
                invokeChangeEvent();
                observed.hasChanged = false;
            }
        }
    }
}