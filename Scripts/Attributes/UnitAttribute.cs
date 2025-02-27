using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Marks the (SI-)Unit of a value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class Unit: Attribute
    {
        /// <summary>
        /// Unit Name
        /// </summary>
        public string unitName { get; private set; } = "";

        /// <summary>
        /// Marks the unit of a value
        /// </summary>
        /// <param name="unitName">Unit Name</param>
        public Unit(string unitName)
        {
            this.unitName = unitName;
        }
    }
}