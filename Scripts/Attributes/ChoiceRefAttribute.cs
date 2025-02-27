using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Attribute for Extended Property inspector, indicating that the field with this attribute should be drawn in a drop down box.
    /// Variant: Gets content of the dropdown box from the referenced field
    /// </summary>
    public class ChoiceRefAttribute : Attribute
    {
        public string choiceRef;
        
        /// <summary>
        /// Attribute for Extended Property inspector, indicating that the field with this attribute should be drawn in a drop down box.
        /// </summary>
        /// <param name="choiceRef">Name of field containing choices as string[]</param>
        public ChoiceRefAttribute(string choiceRef)
        {
            this.choiceRef = choiceRef;
        }
    }
}