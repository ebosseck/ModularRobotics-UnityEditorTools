using System;
using UnityEditor;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Custom Editor Info Attribute in order to access the type of this Attribute
    /// </summary>
    public class CustomEditorInfo : CustomEditor
    {
        /// <summary>
        /// Type this is an Editor for
        /// </summary>
        public Type instanceType { get; private set; } = null;

        /// <summary>
        ///   <para>Defines which object type the custom editor class can edit.</para>
        /// </summary>
        /// <param name="inspectedType">Type that this editor can edit.</param>
        public CustomEditorInfo(System.Type inspectedType) : base(inspectedType)
        {
            this.instanceType = inspectedType;
        }

        /// <summary>
        ///   <para>Defines which object type the custom editor class can edit.</para>
        /// </summary>
        /// <param name="inspectedType">Type that this editor can edit.</param>
        /// <param name="editorForChildClasses">If true, child classes of inspectedType will also show this editor. Defaults to false.</param>
        public CustomEditorInfo(System.Type inspectedType, bool editorForChildClasses) : base(inspectedType, editorForChildClasses)
        {
            this.instanceType = inspectedType;
        }
    }
}