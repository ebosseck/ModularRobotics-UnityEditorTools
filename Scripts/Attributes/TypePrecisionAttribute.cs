using System;

namespace EditorTools.Attributes
{
    /// <summary>
    /// Attribute describing the number of bits used for this value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TypePrecision : Attribute
    {
        /// <summary>
        /// Bitcount of the Value
        /// </summary>
        public int precissionBits { get; private set; } = 32;

        /// <summary>
        /// Attribute describing the number of bits used for this value.
        /// </summary>
        /// <param name="bits">Bitcount of the Value</param>
        public TypePrecision(int bits)
        {
            precissionBits = bits;
        }
    }
}