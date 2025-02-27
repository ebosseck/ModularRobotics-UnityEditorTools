using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Attribute describing that this field's value should point to a valid scene 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SceneAttribute: Attribute
    {
        
    }
}