using System;
using UnityEngine;

// extra s in maths to allow disambiguation with built-in math module. Otherwise there would be confusion in some IDEs & Unity
namespace EditorTools.Maths.LinAlg
{
    /// <summary>
    /// Tools for handeling vector operations
    /// </summary>
    public class VectorTools
    {
        /// <summary>
        /// Multiplies two Vector3's Component Wise
        /// </summary>
        /// <param name="left">Left Vector3</param>
        /// <param name="right">Right Vector3</param>
        /// <returns>the Resulting Vector3</returns>
        public static Vector3 multiplyComponents(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
        }

        /// <summary>
        /// Divides two Vector3's Component Wise
        /// </summary>
        /// <param name="left">Left Vector3</param>
        /// <param name="right">Right Vector3</param>
        /// <returns>the Resulting Vector3</returns>
        public static Vector3 divideComponents(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x / right.x, left.y / right.y, left.z / right.z);
        }
        
        /// <summary>
        /// Checks if the given position is within the AABB
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <param name="lower">Lower corner of the AABB</param>
        /// <param name="upper">Upper corner of the AABB</param>
        /// <returns>True if position in AABB</returns>
        public static bool isInAABB(Vector3 position, Vector3 lower, Vector3 upper)
        {
            return lower.x < position.x && position.x < upper.x &&
                   lower.y < position.y && position.y < upper.y &&
                   lower.z < position.z && position.z < upper.z;
        }

        /// <summary>
        /// Computes the component wise absolute of the given value, and writes the result in result
        /// </summary>
        /// <param name="value">Value to compute absolute of</param>
        /// <param name="result">reference of the result</param>
        public static void componentAbs(Vector3 value, out Vector3 result)
        {
            result.x = Math.Abs(value.x);
            result.y = Math.Abs(value.y);
            result.z = Math.Abs(value.z);
        }
        
        /// <summary>
        /// Computes the component wise absolute of the given value
        /// </summary>
        /// <param name="value">Value to compute the absolute of</param>
        /// <returns>The component wise absolute of value</returns>
        public static Vector3 componentAbs(Vector3 value)
        {
            Vector3 res = new Vector3();
            componentAbs(value, out res);
            return res;
        }
        
        /// <summary>
        /// Gets the index of the maximal absolute component in value
        /// </summary>
        /// <param name="value">vector to check</param>
        /// <returns>index of the maximum in [0, 2] (x=0, y=1, z=2) or -1 on error</returns>
        public static int getMaxComponentIdx(Vector3 value)
        {
            value = componentAbs(value);

            if (value.x >= value.y && value.x >= value.z)
            {
                return 0;
            }
            
            if (value.y >= value.x && value.y >= value.z)
            {
                return 1;
            }
            
            if (value.z >= value.y && value.z >= value.x)
            {
                return 2;
            }

            return -1;
        }
        
        /// <summary>
        /// Gets the index of the minimal absolute component in value
        /// </summary>
        /// <param name="value">vector to check</param>
        /// <returns>index of the minimal in [0, 2] (x=0, y=1, z=2) or -1 on error</returns>
        public static int getMinComponentIdx(Vector3 value)
        {
            value = componentAbs(value);

            if (value.x <= value.y && value.x <= value.z)
            {
                return 0;
            }
            
            if (value.y <= value.x && value.y <= value.z)
            {
                return 1;
            }
            
            if (value.z <= value.y && value.z <= value.x)
            {
                return 2;
            }

            return -1;
        }

        /// <summary>
        /// Clamps the given vector in range [0, 1]
        /// </summary>
        /// <param name="vec">Vector to clamp component wise</param>
        /// <returns>the vector clamped in range [0, 1]</returns>
        public static Vector3 clamp01Components(Vector3 vec)
        {
            return new Vector3(Mathf.Clamp01(vec.x), Mathf.Clamp01(vec.y), Mathf.Clamp01(vec.z));
        }

        /// <summary>
        /// Clamp value then multiply with dimensions
        /// </summary>
        /// <param name="dimensions">dimensions</param>
        /// <param name="value">Value to clamp</param>
        /// <returns>the interpolated value</returns>
        public static Vector3 lerpComponentWise(Vector3 dimensions, Vector3 value)
        {
            value = clamp01Components(value);
            return multiplyComponents(dimensions, value);
        }
    }
}