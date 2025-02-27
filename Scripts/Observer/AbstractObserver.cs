using System;
using UnityEngine;

namespace EditorTools.Observer
{
    /// <summary>
    /// Value Change observer
    /// </summary>
    /// <typeparam name="T">Type of observed Value</typeparam>
    public class AbstractObserver<T> : MonoBehaviour
    {
        protected T observed;
        public EventHandler changeEventHandler;
    
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AbstractObserver() {
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="observed">Value to observe</param>
        public AbstractObserver(T observed) {
            setObservedReference(observed);
        }

        /// <summary>
        /// Sets the reference to observe
        /// </summary>
        /// <param name="observed">object to observe</param>
        public virtual void setObservedReference(T observed)
        {
            this.observed = observed;
        }

        /// <summary>
        /// Invokes the value changed callback
        /// </summary>
        public virtual void invokeChangeEvent()
        {
            changeEventHandler.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Get the ValueChanged event handler
        /// </summary>
        /// <returns>the ValueChanged event</returns>
        public virtual EventHandler getEventHandler()
        {
            return changeEventHandler;
        }
    }
}