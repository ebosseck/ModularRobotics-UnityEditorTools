using System;
using EditorTools.Ascii;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Audio;

namespace EditorTools.InspectorTools
{
    /// <summary>
    /// Tools for preparing serialized properties
    /// </summary>
    public class SerializedPropertyTools
    {
        #region Array Tools
        /// <summary>
        /// Checks if the given property is part of an array type
        /// </summary>
        /// <param name="property">Property to check</param>
        /// <returns>True, if the property is part of an array type</returns>
        public static bool isPartOfArray(SerializedProperty property)
        {
            BottomUpAsciiParser parser = new BottomUpAsciiParser();
            parser.loadText(property.propertyPath);

            return parser.readToChar(']');

        }
        
        /// <summary>
        /// Returns the index of the property within an array, or -1 if not part of an array
        /// </summary>
        /// <param name="property">The property to check</param>
        /// <returns>the Index of this property within the array, or -1 if not found</returns>
        public static int getArrayIndex(SerializedProperty property)
        {
            BottomUpAsciiParser parser = new BottomUpAsciiParser();
            parser.loadText(property.propertyPath);

            if (parser.readToChar(']'))
            {
                parser.consume();
                int value = parser.readInteger();
                parser.check('[');
                return value;
            }

            return -1;

        }

        #region Path Tools

        /// <summary>
        /// Returns the base path to siblings in the property tree
        /// </summary>
        /// <param name="property">Property to check</param>
        /// <returns>the base path to all siblings of this property</returns>
        public static string getSiblingBasePath(SerializedProperty property)
        {
            BottomUpAsciiParser parser = new BottomUpAsciiParser();
            parser.loadText(property.propertyPath);

            parser.readToChar('.');

            return parser.readAllRemaining();
        }

        #endregion
        
        #endregion
        
        #region Value Tools

        /// <summary>
        /// Returns the properties value as float
        /// </summary>
        /// <param name="property">property whose value to get</param>
        /// <returns>the properties value as float</returns>
        public static float? getValueAsFloat(SerializedProperty property)
        {
            if (property == null)
            {
                return null;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return (float) property.intValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                default:
                    Debug.Log("PropertyType " + property.propertyType);
                    return null;
            }
        }

        /// <summary>
        /// Returns the properties value as string[]
        /// </summary>
        /// <param name="property">property whose value to get</param>
        /// <returns>the properties value as string[]</returns>
        public static string[] getValuesAsStringArray(SerializedProperty property)
        {
            if (property == null)
            {
                return null;
            }

            if (property.isArray)
            {
                string[] values = new string[property.arraySize];
                
                for (int i = 0; i < property.arraySize; i++)
                {
                    values[i] = property.GetArrayElementAtIndex(i).stringValue;
                }

                return values;
            }

            return new String []{};
        }

        /// <summary>
        /// Returns the properties value as int[]
        /// </summary>
        /// <param name="property">property whose value to get</param>
        /// <returns>the properties value as int[]</returns>
        public static int[] getValuesAsIntArray(SerializedProperty property)
        {
            if (property == null)
            {
                return null;
            }

            if (property.isArray)
            {
                int[] values = new int[property.arraySize];

                for (int i = 0; i < property.arraySize; i++)
                {
                    values[i] = property.GetArrayElementAtIndex(i).intValue;
                }

                return values;
            }

            return new int []{};
        }
        
        /// <summary>
        /// Returns the properties value as float[]
        /// </summary>
        /// <param name="property">property whose value to get</param>
        /// <returns>the properties value as float[]</returns>
        public static float[] getValuesAsFloatArray(SerializedProperty property)
        {
            if (property == null)
            {
                return null;
            }

            if (property.isArray)
            {
                float[] values = new float[property.arraySize];

                for (int i = 0; i < property.arraySize; i++)
                {
                    values[i] = property.GetArrayElementAtIndex(i).floatValue;
                }

                return values;
            }

            return new float []{};
        }
        
        #endregion
    }
}