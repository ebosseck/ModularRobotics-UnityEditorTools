using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Dynamic Range Attribute for extended property inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DynamicRange: Attribute
    {
        /// <summary>
        /// Variable name of the field containing the minimal value
        /// </summary>
        public string minFieldName { get; private set; } = null;
        /// <summary>
        /// Variable name of the field containing the maximal value
        /// </summary>
        public string maxFieldName { get; private set; } = null;

        /// <summary>
        /// Dynamic range attribute allows to draw per instance specific ranged sliders in PropertyInspector
        /// </summary>
        /// <param name="minFieldName">Variable name of the field containing the minimal value</param>
        /// <param name="maxFieldName">Variable name of the field containing the maximal value</param>
        public DynamicRange(string minFieldName, string maxFieldName)
        {
            this.minFieldName = minFieldName;
            this.maxFieldName = maxFieldName;
        }
    }
}