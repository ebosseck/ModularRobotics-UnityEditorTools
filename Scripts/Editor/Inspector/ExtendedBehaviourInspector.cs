using System;
using System.Diagnostics;
using System.Reflection;
using EditorTools.Attributes;
using EditorTools.BaseTypes;
using EditorTools.InspectorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace EditorTools.Inspector
{
    /// <summary>
    /// Base Class for all custom property inspectors with the custom generator
    /// </summary>
    [CustomEditorInfo(typeof(ExtendedMonoBehaviour))]
    public class ExtendedBehaviourInspector : Editor
    {
        /// <summary>
        /// Called by Unity. Create the inspector gui
        /// </summary>
        /// <returns>the root element for the property inspector</returns>
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement myInspector = new VisualElement();

            createInspectorAuto(myInspector);
        
            return myInspector;
        }

        /// <summary>
        /// Function to generate the property inspector in the given root element
        /// </summary>
        /// <param name="root">Root Element to add the widgets of the inspector to</param>
        /// <returns>the root element</returns>
        public VisualElement createInspectorAuto(VisualElement root)
        {
            FieldInfo[] fields = AttributeTools.getVisibleFields(serializedObject.targetObject.GetType());

            foreach (FieldInfo field in fields)
            {
                SerializedProperty prop = serializedObject.FindProperty(field.Name);

                if (prop != null)
                {
                    GUIElementTools.addAllToElement(root, GUIElemetCreator.createFields(this, prop));
                }
                else
                {
                    Debug.LogWarning("Could not create SerializedProperty for field: " + field.Name);
                }
            }

            return root;
        }
    }
}