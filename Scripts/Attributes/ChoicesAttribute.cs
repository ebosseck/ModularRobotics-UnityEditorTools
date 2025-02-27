using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Attribute for Extended Property inspector, indicating that the field with this attribute should be drawn in a drop down box.
    /// </summary>
    public class ChoicesAttribute : Attribute
    {
        public string[] choices;
        
        /// <summary>
        /// Attribute for Extended Property inspector, indicating that the field with this attribute should be drawn in a drop down box.
        /// </summary>
        /// <param name="choices">Valid choices of the drop down box</param>
        public ChoicesAttribute(string[] choices)
        {
            this.choices = choices;
        }
    }
}