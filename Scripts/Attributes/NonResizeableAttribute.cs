using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Attribute marking that the arry field is fixed length
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NonResizeable: Attribute
    {
        public NonResizeable()
        {
        }
    }
}