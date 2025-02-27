using System;
using System.Text;

namespace EditorTools.Ascii
{
    /// <summary>
    /// Bottom-Up Ascii parser (Parser starts parsing at the end of file, and parses to the end)
    /// </summary>
    public class BottomUpAsciiParser
    {
        protected int nextPos;
        protected char[] data;
	
        public char decimalPoint = '.';
        
        /// <summary>
        /// Bottom-Up Ascii parser (Parser starts parsing at the end of file, and parses to the end)
        /// </summary>
        public BottomUpAsciiParser()
        {
            
        }
        
        /// <summary> Load the given String into the ASCII Parser. It is faster to load a char[] directly </summary>
        /// <param> text - String to load</param>
        public void loadText (string text) {
            data = text.ToCharArray();
            nextPos = data.Length -1;
            
        }
        
        /// <summary> Load the given char[] into the ASCII Parser. This is the fastest way to load data. </summary>
        /// <param> data - the char[] to load </param>
        public void loadData (char[] data) {
            this.data = data;
            nextPos = data.Length -1;
            
        }
        
        /// <summary> Load the given byte[] into the ASCII Parser. It is faster to load a char[] directly </summary>
        /// <param> data - the byte[] to load </param>
        public void loadData (byte[] data) {
            this.data = new char[data.Length];
            for (int i = 0; i < data.Length; i++) {
                this.data[i] = (char)(data[i] &0xff);
            }
            nextPos = data.Length;
            
        }
        
        /// <returns>an int[] with {lineNo, CharNo, nextPos}</returns>
        public int[] getLinePosition() {
            return getLinePosition(nextPos);
        }

        /// <param>nextPos - Position in text to get character for</param>
        /// <returns>an int[] with {lineNo, CharNo, nextPos}</returns>
        public int[] getLinePosition(int nextPos) {
            int line = 1;
            int chars = 1;
            
            for (int i = 0; i < nextPos; i++) {
                if (data[i] == '\n') {
                    line++;
                    chars = 1;
                } else {
                    chars++;
                }
            }
            
            return new int[] {line, chars, nextPos};
        }
        
        /// <returns>true if there is any data left to parse</returns>
        public bool available () {
            return 0 <= nextPos;
        }
        
        /// <summary>fetches the next char from data, without counting up the position</summary>
        /// <returns>the next char in data.</returns>
        /// <exception>AsciiParserException - if no more data is available</exception>
        public char peek () {
            if (!available()) {
                throw new AsciiParserException (getLinePosition(), "Unexpected EOF");
            }
            return data[nextPos];
        }
        
        /// <summary>Fetches the next char from data, counting the position up by one.</summary>
        /// <returns>the next char in data</returns>
        /// <exception>AsciiParserException  - if no more data is available</exception>
        public char read () {
            if (!available()) {
                throw new AsciiParserException (getLinePosition(), "Unexpected EOF");
            }
            return data[nextPos--];
        }
        
        /// <summary>consume the current character (goto the next character)</summary>
        /// <exception>AsciiParserException  - if no more data is available</exception>
        public void consume () {
            if (!available()) {
                throw new AsciiParserException (getLinePosition(), "Unexpected EOF");
            }
            nextPos--;
        }
        
        /// <summary>Checks if the next char equals c</summary>
        /// <param>c - the expected char</param>
        /// <exception>AsciiParserException - if the next char != c</exception>
        public void check (char c) {
            char t = peek ();
            if (t != c) {
                throw new AsciiParserException (getLinePosition(), "Char '" + c + "' Expected. Found '" + t + "'.");
            }
        }
        
        ///<summary>Skips all characters identified as Whitespace by Char.IsWhiteSpace()</summary>
        ///<returns>the number of whitespaces skipped</returns>
        ///<exception>AsciiParserException</exception>
        public int skipWhitespace () {
            int whitespaces = 0;
            while (available()) {
                char c = peek ();
                if (Char.IsWhiteSpace(c)) {
                    consume();
                    whitespaces ++;
                } else {
                    return whitespaces;
                }
            }
            
            return whitespaces;
        }
        
        ///<summary>read an Integer from data[]</summary>
        ///<returns>the Integer read</returns>
        ///<exception>AsciiParserException</exception>
        public int readInteger () {
            int value = 0;
            skipWhitespace ();
            
            char c = peek ();
            if (c == '-') {
                consume ();
                skipWhitespace ();
                c = read ();
                value = -getNumberFromChar(c);
            } else {
                value = getNumberFromChar (c);
                consume ();
            }

            int exp = 1;
            
            while (true) {
                if (!available()) {
                    return value;
                }
                
                c = peek ();
                if (!Char.IsNumber(c)) {
                    return value;
                }
                exp *= 10;
                value += (getNumberFromChar(c) * exp);
                consume ();
            }
        }
        
        /// <summary>
        /// Reads to the next occurence of given char exclusive
        /// </summary>
        /// <param name="e">The char to read to</param>
        /// <returns>True if the char was found, false otherwise</returns>
        public bool readToChar(char e)
        {
            char c = peek();

            while (true)
            {
                if (c == e)
                {
                    return true;
                }
                consume();
                if (!available())
                {
                    return false;
                }
                c = peek();
            }
        }
        
        /// <summary>
        /// Returns all remaining text
        /// </summary>
        /// <returns>all Text remaining</returns>
        public string readAllRemaining()
        {
            StringBuilder sb = new StringBuilder();
            
            while (true)
            {
                if (!available())
                {
                    return sb.ToString();
                }

                sb.Insert(0, read());
            }
        }

        ///<summary>Resolves Characters to digits</summary>
        ///<param>c - c the character to solve</param>
        ///<returns>the digit corresponding to c</returns>
        ///<exception>AsciiParserException - if c is not representing a digit</exception>
        protected int getNumberFromChar (char c) {
            if (!Char.IsNumber(c)) {
                throw new AsciiParserException (getLinePosition(), "Internal Error: Digit expected, got " + c);
            }
            return c - '0';
        }

        ///<param>c - the char to check</param>
        ///<returns>true if c is a hex digit, false otherwise</returns>
        protected bool isHexDigit(char c) {
            if (c >= '0' && c <= '9') {
                return true;
            }

            if (c >= 'a' && c <= 'f') {
                return true;
            }

            if (c >= 'A' && c <= 'F') {
                return true;
            }

            return false;
        }

        ///<summary>Converts the given hex digit to its numeric value</summary>
        ///<param>c - char to convert</param>
        ///<returns>the value of c</returns>
        ///<exception>AsciiParserException - if c is not a hex digit</exception>
        protected int getHexNumberFromChar(char c) {
            if (c >= '0' && c <= '9') {
                return c - '0';
            }

            if (c >= 'a' && c <= 'f') {
                return 10 + (c - 'a');
            }

            if (c >= 'A' && c <= 'F') {
                return 10 + (c - 'A');
            }

            throw new AsciiParserException (getLinePosition(), "Internal Error: Hex Digit expected, got " + c);
        }
    }
}