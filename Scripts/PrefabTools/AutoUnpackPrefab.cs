using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace EditorTools.PrefabTools
{
    /// <summary>
    /// Unpacks prefabs recursive once they are added to the UnityEditor from the asset browser
    /// </summary>
    public class AutoUnpackPrefab : MonoBehaviour
    {
#if (UNITY_EDITOR)
        private void OnValidate()
        {
            if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                unpackRecursive(gameObject);
            }
        }

        /// <summary>
        /// Recursive unpacking the given game object and all children
        /// </summary>
        /// <param name="go">GameObject to unpack</param>
        private void unpackRecursive(GameObject go)
        {
            if (go == null)
            {
                return;
            }

            try
            {
                PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            }
            catch (ArgumentException)
            {
                // GameObject is not ROOT, try parent
                if (go.transform.parent != null)
                {
                    unpackRecursive(go.transform.parent.gameObject);
                }
            }
        }
#endif
    }
}




