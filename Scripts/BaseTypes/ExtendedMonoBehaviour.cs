using UnityEngine;

namespace EditorTools.BaseTypes
{
    /// <summary>
    /// MonoBehaviour with more editor property changed callbacks
    /// </summary>
    public class ExtendedMonoBehaviour : MonoBehaviour
    {

        /// <summary>
        /// Called when a value in the inspector is about to be changed
        /// </summary>
        /// <param name="origin">String containing the name / path of the changed property</param>
        /// <param name="oldValue">Previous Value of the Control</param>
        /// <param name="newValue">Next Value of the Control</param>
        public virtual void OnInspectorValueChanged(string origin, object oldValue, object newValue) {
            
        }

        /// <summary>
        /// Called when a value in the inspector is about to be changed
        /// </summary>
        /// <param name="origin">String containing the name / path of the changed property</param>
        /// <param name="oldValue">Previous Value of the Control</param>
        /// <param name="newValue">Next Value of the Control</param>
        public virtual void OnInspectorValueChanged(string origin, float oldValue, float newValue)
        {
            OnInspectorValueChanged(origin, (object)oldValue, (object)newValue);
        }

        /// <summary>
        /// Called when a value in the inspector is about to be changed
        /// </summary>
        /// <param name="origin">String containing the name / path of the changed property</param>
        /// <param name="oldValue">Previous Value of the Control</param>
        /// <param name="newValue">Next Value of the Control</param>
        public virtual void OnInspectorValueChanged(string origin, int oldValue, int newValue)
        {
            OnInspectorValueChanged(origin, (object)oldValue, (object)newValue);
        }
        
    }
}