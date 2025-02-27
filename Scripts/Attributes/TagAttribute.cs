using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Indicates that this field should be drawn as Tag drop down field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TagAttribute : Attribute
    {
        /// <summary>
        /// Indicates that this field should be drawn as Tag drop down field
        /// </summary>
        public TagAttribute()
        {
        }
    }
}