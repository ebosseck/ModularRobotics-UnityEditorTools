using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EditorTools.Display;

namespace EditorTools.Tools
{
    /// <summary>
    /// Draws a cylinder in the editor. Useful for debugging empties
    /// </summary>
    [ExecuteInEditMode]
    public class DrawDebugCylinder : MonoBehaviour
    {

        public Vector3 offset = Vector3.zero;
        public float radius = 0.5f;
        public float height = 1f;
        public int resolution = 16;
        
        public bool drawCaps = true;
        public Color color = Color.white;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            Geometry.drawCylinder(gameObject.transform, radius, height, offset, drawCaps, resolution, color);
        }
    }
}

