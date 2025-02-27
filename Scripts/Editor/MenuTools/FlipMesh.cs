using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

/// <summary>
/// Adds an option to flip all faces in a mesh to Unity's UI
/// </summary>
namespace EditorTools.MenuItems
{
    public class FlipMesh : MonoBehaviour
    {
        /// <summary>
        /// Adds an option to flip all faces in a mesh
        /// </summary>
        [MenuItem("Object Tools/Flip Mesh")]
        public static void FlipMeshContext()
        {
            Mesh mesh = Selection.activeGameObject.GetComponent<MeshFilter>().sharedMesh;
            if (mesh == null)
            {
                return;
            }

            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }
}