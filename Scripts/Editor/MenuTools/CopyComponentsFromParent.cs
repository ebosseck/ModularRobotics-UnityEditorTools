using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

/// <summary>
/// In Editor tool class to copy all components present in the parent to this object
/// </summary>
namespace EditorTools.MenuItems {
    public class CopyComponentsFromParent : MonoBehaviour
    {
        /// <summary>
        /// Adds an option to copy missing components from parent
        /// </summary>
        [MenuItem("Object Tools/Copy Missing Components From Parent")]
        public static void CopyComponentsFromParentContext ( ) {
            foreach (GameObject obj in Selection.gameObjects)
            {
                copyMissing(obj);
            }
        }
    
    
        /// <summary>
        /// copys missing components from parents
        /// </summary>
        /// <param name="dest">Object to copy components to</param>
        public static void copyMissing(GameObject dest)
        {
            Component[] srcComponents = dest.GetComponentsInParent<Component>();
            Component[] destComponents = dest.GetComponents<Component>();

            foreach (Component component in srcComponents)
            {
                if (component.GetType() == typeof(Transform) || component.GetType() == typeof(MeshFilter) || component.GetType() == typeof(MeshRenderer))
                {
                    continue;
                }

                if (destComponents.Contains(component))
                {
                    // Ignore
                    //EditorUtility.CopySerializedIfDifferent(component, dest.GetComponent(component.GetType()));
                }
                else
                {
                    // Component is missing
                    Component destcomponent = dest.AddComponent(component.GetType());
                    EditorUtility.CopySerializedIfDifferent(component, destcomponent);
                }
            }
        
        }
    }
}
