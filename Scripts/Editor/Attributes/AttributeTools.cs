using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using EditorTools.Attributes;
using UnityEditor;
using UnityEngine;

namespace EditorTools.InspectorTools
{
    /// <summary>
    /// Simple tools to check & get certain attributes from fields
    /// </summary>
    public class AttributeTools
    {

        #region Specific Property Attributes
        
        /// <summary>
        /// Gets Header Text from Attribute
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>the Header Text or null if no Header attribute is found</returns>
        public static string getHeadder(Editor editor, SerializedProperty property)
        {
            HeaderAttribute attribute = (HeaderAttribute) getFieldAttribute(editor, property, typeof(HeaderAttribute));

            if (attribute == null)
            {
                return null;
            }

            return attribute.header;
        }

        /// <summary>
        /// Gets Header Text from Attribute
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>the Header Text or null if no Header attribute is found</returns>
        public static string getHeadder(Type objectType, SerializedProperty property)
        {
            HeaderAttribute attribute = (HeaderAttribute) getFieldAttribute(objectType, property, typeof(HeaderAttribute));

            if (attribute == null)
            {
                return null;
            }

            return attribute.header;
        }

        
        /// <summary>
        /// Gets the Precission of the type
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>the precission of this type in bits, or -1 if not specified</returns>
        public static int getTypePrecission(Editor editor, SerializedProperty property)
        {
            TypePrecision attrib = (TypePrecision)getFieldAttribute(editor, property, typeof(TypePrecision));

            if (attrib != null)
            {
                return attrib.precissionBits;
            }

            return -1;
        }
        
        /// <summary>
        /// Gets the Precission of the type
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>the precission of this type in bits, or -1 if not specified</returns>
        public static int getTypePrecission(Type objectType, SerializedProperty property)
        {
            TypePrecision attrib = (TypePrecision)getFieldAttribute(objectType, property, typeof(TypePrecision));

            if (attrib != null)
            {
                return attrib.precissionBits;
            }

            return -1;
        }

        /// <summary>
        /// Gets the Unit of this Property
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>A String containing the Unit name, or the empty string if not specified</returns>
        public static string getUnit(UnityEditor.Editor editor, SerializedProperty property)
        {
            Unit unit = (Unit) getFieldAttribute(editor, property, typeof(Unit));

            if (unit != null)
            {
                return unit.unitName;
            }

            return null;
        }

        /// <summary>
        /// Gets the Unit of this Property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>A String containing the Unit name, or the empty string if not specified</returns>
        public static string getUnit(Type objectType, SerializedProperty property)
        {
            Unit unit = (Unit) getFieldAttribute(objectType, property, typeof(Unit));

            if (unit != null)
            {
                return unit.unitName;
            }

            return null;
        }
        
        /// <summary>
        /// Gets the valid range of the Property
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>a float[] containing [min, max], or null if not set</returns>
        public static float[] getRange(UnityEditor.Editor editor, SerializedProperty property)
        {
            Type behaviourClass = getEditorTargetClass(editor);
            return getRange(behaviourClass, property);
        }
        
        /// <summary>
        /// Gets the valid range of the Property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>a float[] containing [min, max], or null if not set</returns>
        public static float[] getRange(Type objectType, SerializedProperty property)
        {
            DynamicRange dynRange = (DynamicRange) getFieldAttribute(objectType, property, typeof(DynamicRange));

            if (dynRange != null)
            {
                string siblingBase = SerializedPropertyTools.getSiblingBasePath(property);
                
                float? min = SerializedPropertyTools.getValueAsFloat(property.serializedObject.FindProperty(siblingBase + dynRange.minFieldName));
                float? max = SerializedPropertyTools.getValueAsFloat(property.serializedObject.FindProperty(siblingBase + dynRange.maxFieldName));

                float minVal = 0;
                float maxVal = 10;
                
                if (min != null)
                {
                    minVal = (float)min;
                }
                else
                {
                    Debug.LogWarning("Cant resolve property (minFieldName): " + dynRange.minFieldName + " with base path " + siblingBase);
                }

                if (min != null)
                {
                    maxVal = (float)max;
                }
                else
                {
                    Debug.LogWarning("Cant resolve property (maxFieldName): " + dynRange.maxFieldName + " with base path " + siblingBase);
                }
                
                return new float[] {minVal, maxVal};
                
            }

            RangeAttribute range = (RangeAttribute) getFieldAttribute(objectType, property, typeof(RangeAttribute));

            if (range != null)
            {
                return new float[] {range.min, range.max};
            }
            
            return null;
        }

        /// <summary>
        /// Gets the number of lines allowed for this textfield
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>the number of lines for this string</returns>
        public static int getMultiLines(UnityEditor.Editor editor, SerializedProperty property)
        {
            MultilineAttribute mline = (MultilineAttribute) getFieldAttribute(editor, property, typeof(MultilineAttribute));

            if (mline != null)
            {
                return mline.lines;
            }
            
            return 1;
        }

        /// <summary>
        /// Gets the number of lines allowed for this textfield
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>the number of lines for this string</returns>
        public static int getMultiLines(Type objectType, SerializedProperty property)
        {
            MultilineAttribute mline = (MultilineAttribute) getFieldAttribute(objectType, property, typeof(MultilineAttribute));

            if (mline != null)
            {
                return mline.lines;
            }
            
            return 1;
        }
        
        /// <summary>
        ///  Checks if the NonResizeable Attribute is present on the property
        /// </summary>
        /// <param name="editor">Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the NonResizeable, False otherwise</returns>
        public static bool isNonResizeable(Editor editor, SerializedProperty property)
        {
            NonResizeable resizeable = (NonResizeable) getFieldAttribute(editor, property, typeof(NonResizeable));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the NonResizeable Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the NonResizeable, False otherwise</returns>
        public static bool isNonResizeable(Type objectType, SerializedProperty property)
        {
            NonResizeable resizeable = (NonResizeable) getFieldAttribute(objectType, property, typeof(NonResizeable));
            return (resizeable != null);
        }

        /// <summary>
        ///  Checks if the Tag Attribute is present on the property
        /// </summary>
        /// <param name="editor">Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the Tag is present, False otherwise</returns>
        public static bool isTag(Editor editor, SerializedProperty property)
        {
            TagAttribute resizeable = (TagAttribute) getFieldAttribute(editor, property, typeof(TagAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the Tag Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the Tag is present, False otherwise</returns>
        public static bool isTag(Type objectType, SerializedProperty property)
        {
            TagAttribute resizeable = (TagAttribute) getFieldAttribute(objectType, property, typeof(TagAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the Serializeable Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the Serializeable, False otherwise</returns>
        public static bool isSerializable(Editor editor, SerializedProperty property)
        {
            SerializableAttribute resizeable = (SerializableAttribute) getFieldAttribute(editor, property, typeof(SerializableAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the Serializeable Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the Serializeable, False otherwise</returns>
        public static bool isSerializable(Type objectType, SerializedProperty property)
        {
            SerializableAttribute resizeable = (SerializableAttribute) getFieldAttribute(objectType, property, typeof(SerializableAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the Serializeable Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the Serializeable, False otherwise</returns>
        public static bool isSerializable(FieldInfo info)
        {
            SerializableAttribute resizeable = (SerializableAttribute) getFieldAttribute(info, typeof(SerializableAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the HideInInspector Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the HideInInspector, False otherwise</returns>
        public static bool isHideInInspector(Editor editor, SerializedProperty property)
        {
            HideInInspector resizeable = (HideInInspector) getFieldAttribute(editor, property, typeof(HideInInspector));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the HideInInspector Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the HideInInspector, False otherwise</returns>
        public static bool isHideInInspector(Type objectType, SerializedProperty property)
        {
            HideInInspector resizeable = (HideInInspector) getFieldAttribute(objectType, property, typeof(HideInInspector));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the HideInInspector Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the HideInInspector, False otherwise</returns>
        public static bool isHideInInspector(FieldInfo info)
        {
            HideInInspector resizeable = (HideInInspector) getFieldAttribute(info, typeof(HideInInspector));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the HideInInspector Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the HideInInspector, False otherwise</returns>
        public static bool isScene(Editor editor, SerializedProperty property)
        {
            SceneAttribute resizeable = (SceneAttribute) getFieldAttribute(editor, property, typeof(SceneAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the HideInInspector Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the HideInInspector, False otherwise</returns>
        public static bool isScene(Type objectType, SerializedProperty property)
        {
            SceneAttribute resizeable = (SceneAttribute) getFieldAttribute(objectType, property, typeof(SceneAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        ///  Checks if the HideInInspector Attribute is present on the property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>True if the HideInInspector, False otherwise</returns>
        public static bool isScene(FieldInfo info)
        {
            SceneAttribute resizeable = (SceneAttribute) getFieldAttribute(info, typeof(SceneAttribute));
            return (resizeable != null);
        }
        
        /// <summary>
        /// Gets the valid choices for the Property
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <returns>a string[] containing all choices, or null if not set</returns>
        public static string[] getChoices(UnityEditor.Editor editor, SerializedProperty property)
        {
            Type behaviourClass = getEditorTargetClass(editor);
            return getChoices(behaviourClass, property);
        }
        
         /// <summary>
        /// Gets the valid choices for the Property
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
         /// <returns>a string[] containing all choices, or null if not set</returns>
        public static string[] getChoices(Type objectType, SerializedProperty property)
        {
            ChoiceRefAttribute choices = (ChoiceRefAttribute) getFieldAttribute(objectType, property, typeof(ChoiceRefAttribute));

            if (choices != null)
            {
                string siblingBase = SerializedPropertyTools.getSiblingBasePath(property);
                
                string[] values =
                    SerializedPropertyTools.getValuesAsStringArray(
                        property.serializedObject.FindProperty(siblingBase + choices.choiceRef));

                return values; //new float[] {minVal, maxVal};

            }

            ChoicesAttribute choice = (ChoicesAttribute) getFieldAttribute(objectType, property, typeof(ChoicesAttribute));

            if (choice != null)
            {
                return choice.choices; //new float[] {range.min, range.max};
            }
            
            return null;
        }
        
        #endregion

        #region Editor Tools

        /// <summary>
        /// Gets the type of the given property in the type of the editor.
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Type getFieldType(UnityEditor.Editor editor, SerializedProperty property)
        {
            return getFieldType(editor, property.name);
        }
        
        /// <summary>
        /// Gets the type of the given property in the given type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Type getFieldType(Type objectType, SerializedProperty property)
        {
            return getFieldType(objectType, property.name);
        }

        /// <summary>
        /// Gets the type of the property with the given name in the type of the editor.
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type getFieldType(Editor editor, string propertyName)
        {
            Type behaviourClass = getEditorTargetClass(editor);
            return getFieldType(behaviourClass, propertyName);
        }
        
        /// <summary>
        /// Gets the type of the property with the given name in the given type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Type getFieldType(Type objectType, string propertyName)
        {
            FieldInfo info = objectType.GetField(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            return info.FieldType;
        }
        
        /// <summary>
        /// Gets the attribute of the given type for the given property of the given Editor
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="property">SerialistedProperty whose attribute to get</param>
        /// <param name="attrbType">Type of Attribute to get</param>
        /// <returns>The Attribure of the given type, or null if not available</returns>
        public static Attribute getFieldAttribute(Editor editor, SerializedProperty property,
            Type attrbType)
        {
            return getFieldAttribute(editor, property.name, attrbType);
        }
        
        /// <summary>
        /// Gets the attribute of the given type for the given property of the given Editor
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="property">property whose value to get</param>
        /// <param name="attrbType">Type of Attribute to get</param>
        /// <returns></returns>
        public static Attribute getFieldAttribute(Type objectType, SerializedProperty property,
            Type attrbType)
        {
            return getFieldAttribute(objectType, property.name, attrbType);
        }
        
        /// <summary>
        /// Gets the attribute of the given type for the given property of the given Editor
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="propertyName">Name of the property whose value to get</param>
        /// <param name="attrbType">Type of Attribute to get</param>
        /// <returns></returns>
        public static Attribute getFieldAttribute(Editor editor, string propertyName, Type attrbType)
        {
            Type behaviourClass = getEditorTargetClass(editor);
            return getFieldAttribute(behaviourClass, propertyName, attrbType);
        }

        /// <summary>
        /// Gets the attribute of the given type for the given property of the given Editor
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="propertyName">Name of the property whose value to get</param>
        /// <param name="attrbType">Type of Attribute to get</param>
        /// <returns></returns>
        public static Attribute getFieldAttribute(Type objectType, string propertyName, Type attrbType)
        {
            FieldInfo info = objectType.GetField(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            
            if (info == null)
            {
                FieldInfo[] availableProperties =
                    objectType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Default);

                string names = "";
                foreach (FieldInfo pi in availableProperties)
                {
                    names += (pi.Name + ", ");
                }

                Debug.LogWarning("Field not Found: " + propertyName + ", Available: " + names);

                return null;
            }

            return getFieldAttribute(info, attrbType);
        }

        /// <summary>
        /// Gets the first available attribute of the given type for the given field, or null if no attribute is present
        /// </summary>
        /// <param name="info"></param>
        /// <param name="attrbType"></param>
        /// <returns></returns>
        public static Attribute getFieldAttribute(FieldInfo info, Type attrbType)
        {
            Attribute attrib = System.Attribute.GetCustomAttribute(info, attrbType);

            if (attrib == null)
            {
                Attribute[] availableAttributes = System.Attribute.GetCustomAttributes(info);

                string names = "";
                foreach (Attribute pi in availableAttributes)
                {
                    names += (pi.GetType() + ", ");
                }
            }
                
            return attrib;
        }
        
        /// <summary>
        /// Returns all Attributes for the Property of the given type
        /// </summary>
        /// <param name="editor">UnityEditor.Editor whose target type contains the property</param>
        /// <param name="propertyName">Name of the property whose value to get</param>
        /// <returns>An Array of all Attributes of the given property</returns>
        public static Attribute[] getFieldAttributes(UnityEditor.Editor editor, string propertyName)
        {
            Type behaviourClass = getEditorTargetClass(editor);
            return getFieldAttributes(behaviourClass, propertyName);
        }

        /// <summary>
        /// Returns all Attributes for the Property of the given type
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="propertyName">Name of the property whose value to get</param>
        /// <returns>An Array of all Attributes of the given property</returns>
        public static Attribute[] getFieldAttributes(Type objectType, string propertyName)
        {
            FieldInfo info = objectType.GetField(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            
            if (info == null)
            {
                return Array.Empty<Attribute>();
            }
            
            return System.Attribute.GetCustomAttributes(info);
        }

        /// <summary>
        /// Returns all fields visible to the Inspector
        /// </summary>
        /// <param name="editor">Editor whose visible fields to get</param>
        /// <returns>All fields of the Type visible to the inspector</returns>
        public static FieldInfo[] getVisibleFields(Editor editor)
        {
            Type behaviourClass = getEditorTargetClass(editor);
            return getVisibleFields(behaviourClass);
        }
        
        /// <summary>
        /// Returns all fields visible to the Inspector
        /// </summary>
        /// <param name="objectType">Type whose FieldInfos to get</param>
        /// <returns>All fields of the Type</returns>
        public static FieldInfo[] getVisibleFields(Type objectType)
        {
            FieldInfo[] allFields = objectType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Default);
            List<FieldInfo> fieldInfos = new List<FieldInfo>();

            foreach (FieldInfo field in allFields)
            {
                if (isVisible(field))
                {
                    fieldInfos.Add(field);
                }
            }
            
            return fieldInfos.ToArray();
        }
    
        /// <summary>
        /// Returns true if the given field should be visible within the inspector
        /// </summary>
        /// <param name="info">FieldInfo to check</param>
        /// <returns></returns>
        public static bool isVisible(FieldInfo info)
        {
            if (info.IsPublic)
            {
                return !isHideInInspector(info);
            }

            return isSerializable(info);
        }

        /// <summary>
        /// Returns all available fields
        /// </summary>
        /// <param name="objectType">Type whose FieldInfos to get</param>
        /// <returns>All fields of the Type</returns>
        public static FieldInfo[] getFields(Type objectType)
        {
            return objectType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Default);
        }
        
        /// <summary>
        /// Returns the Class an Editor targets
        /// </summary>
        /// <param name="editor">UnityEngine.Editor to get the target type of (Must have CustomEditorInfo attribute instead of UnityEngine.CustomEditor attribute)</param>
        /// <returns>the Editor's base type</returns>
        public static Type getEditorTargetClass(UnityEditor.Editor editor)
        {
            CustomEditorInfo customEditorAttrib = (CustomEditorInfo)getFirstAttributeOfType(editor.GetType(), typeof(CustomEditorInfo));
            return customEditorAttrib.instanceType;
        }
        
        #endregion
        
        #region General Tools

        /// <summary>
        /// Returns the first instance of the given attribute for the given type
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="attributeType">AttributeType to check for</param>
        /// <returns>The first attribute from type of AttributeType, or null if not found</returns>
        public static Attribute getFirstAttributeOfType(Type type, Type attributeType)
        {
            Attribute[] attributes = getTypeAttributes(type);

            List<Attribute> results = new List<Attribute>();

            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == attributeType)
                {
                    return attributes[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all attributes of the given attribute type for the given type
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="attributeType">AttributeType to check for</param>
        /// <returns>The first attribute from type of AttributeType, or null if not found</returns>
        public static Attribute[] getAttributesOfType(Type type, Type attributeType)
        {
            Attribute[] attributes = getTypeAttributes(type);

            List<Attribute> results = new List<Attribute>();

            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType().IsSubclassOf(attributeType))
                {
                    results.Add(attributes[i]);
                }
            }

            return results.ToArray();
        }
        
        /// <summary>
        /// Returns all Attributes for the given type
        /// </summary>
        /// <param name="type">Type to get attributes from</param>
        /// <returns>All attributes of type</returns>
        public static Attribute[] getTypeAttributes(Type type)
        {
            return System.Attribute.GetCustomAttributes(type);
        }
        
        #endregion
    }
}