using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EditorTools.Display;

namespace EditorTools.Tools
{
    /// <summary>
    /// Draws a sphere in the editor. Useful for debugging empties
    /// </summary>
    [ExecuteInEditMode]
    public class DrawDebugSphere : MonoBehaviour
    {
 
        public Vector3 offset = Vector3.zero;
        public float radius = 1f;
        public int resolutionXZ = 16;
        public int resolutionY = 16;
        public Color color = Color.white;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Geometry.drawSphere(gameObject.transform, radius, offset, resolutionXZ, resolutionY, color);
        }
    }
}
