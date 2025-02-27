using System;
using UnityEngine;

// extra s in maths to allow disambiguation with built-in math module. Otherwise there would be confusion in some IDEs & Unity
namespace EditorTools.Maths.LinAlg
{
    /// <summary>
    /// Tools for handeling matrix operations
    /// </summary>
    public class MatrixTools
    {
        /// <summary>
        /// Takes a Transform-Rotate-Scale transformation matrix and removes the scaling components
        /// </summary>
        /// <param name="trs">Transformation matrix</param>
        /// <returns>Transformation matrix</returns>
        public static Matrix4x4 trsStripS(Matrix4x4 trs)
        {
            float mag1 = Mathf.Sqrt(trs.m00 * trs.m00 + trs.m10 * trs.m10 + trs.m20 * trs.m20); // Column 1
            float mag2 = Mathf.Sqrt(trs.m01 * trs.m01 + trs.m11 * trs.m11 + trs.m21 * trs.m21); // Column 1
            float mag3 = Mathf.Sqrt(trs.m02 * trs.m02 + trs.m12 * trs.m12 + trs.m22 * trs.m22); // Column 1

            Matrix4x4 result = new Matrix4x4();

            result.m00 = trs.m00 / mag1;
            result.m10 = trs.m10 / mag1;
            result.m20 = trs.m20 / mag1;
            result.m30 = trs.m30;
            
            result.m01 = trs.m01 / mag2;
            result.m11 = trs.m11 / mag2;
            result.m21 = trs.m21 / mag2;
            result.m31 = trs.m31;
            
            result.m02 = trs.m02 / mag3;
            result.m12 = trs.m12 / mag3;
            result.m22 = trs.m22 / mag3;
            result.m32 = trs.m32;
            
            result.m03 = trs.m03;
            result.m13 = trs.m13;
            result.m23 = trs.m23;
            result.m33 = trs.m33;
            
            return result;
        }
    }
}