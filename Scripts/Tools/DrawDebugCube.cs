using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EditorTools.Display;

namespace EditorTools.Tools
{
    /// <summary>
    /// Draws a cube in the editor. Useful for debugging empties
    /// </summary>
    [ExecuteInEditMode]
    public class DrawDebugCube : MonoBehaviour
    {
 
        public Vector3 offset = Vector3.zero;
        public Vector3 dimensions = Vector3.one;
        public Color color = Color.white;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Geometry.drawBox(transform, dimensions, offset, color);
        }
    }
}
