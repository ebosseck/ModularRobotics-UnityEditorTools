using EditorTools.Ascii;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EditorTools.JSON {
    /// <summary>
    /// Basic JSON parser
    /// </summary>
    public class JsonParser: AsciiParser {

        public JsonParser() : base() {

        }

        /// <summary>Loads the content of the given Ascii File into the parser</summary>
        /// <param>path - Path to the file to load</param>
        public void loadFile (string path) {
            string content = System.IO.File.ReadAllText(path, Encoding.UTF8);
            loadText(content);
        }

        /// <summary>Reads a Json Object as Dictionary</summary>
        /// <returns>A Dictionary<string, object> contining all key-value pairs of the corresbonding json object</returns>
        public Dictionary<string, object> readObject() {
            skipWhitespace();
            char c = peek();
            if (c != '{') {
                throw new AsciiParserException(getLinePosition(), "Internal Error: '{' expected. Got " + c);
            }
            consume();

            skipWhitespace();
            c = peek();
            if (c == ',') {
                throw new AsciiParserException(getLinePosition(), "Internal Error: '}' or value expected. Got " + c);
            } else if (c == '}') {
                consume();
                return new Dictionary<string, object>();
            }

            Dictionary<string, object> values = new Dictionary<string, object>();

            
            while(true) {
                skipWhitespace();

                string key = readStringLiteral();
                skipWhitespace();
                check(':');
                consume();
                skipWhitespace();
                object value = readValue();
                values.Add(key, value);

                skipWhitespace();
                c = peek();

                if (c == '}') {
                    consume();
                    break;
                } else if (c == ',') {
                    consume();
                } else {
                    throw new AsciiParserException(getLinePosition(), "Internal Error: '}' or ',' expected. Got " + c);
                }

                
            }

            return values;

        }

        /// <summary>Reads a Json List as ArrayList</summary>
        /// <returns>A ArrayList contining all values of the corresbonding json list</returns>
        public ArrayList readArray() {
            skipWhitespace();
            char c = peek();
            if (c != '[') {
                throw new AsciiParserException(getLinePosition(), "Internal Error: '[' expected. Got " + c);
            }
            consume();
            skipWhitespace();
            c = peek();
            if (c == ',') {
                throw new AsciiParserException(getLinePosition(), "Internal Error: '}' or value expected. Got " + c);
            }  else if (c == ']') {
                consume();
                return new ArrayList();
            }

            ArrayList values = new ArrayList();

            while(true) {
                skipWhitespace();
                values.Add(readValue());
                skipWhitespace();
                c = peek();
                if (c == ']') {
                    consume();
                    break;
                } else if (c == ',') {
                    consume();
                } else {
                    throw new AsciiParserException(getLinePosition(), "Internal Error: ']' or ',' expected. Got " + c);
                }
            }

            return values;
        }

        ///<summary> Reads a Json Value</summary>
        ///<returns> the Json value read </returns>
        public object readValue() {
            skipWhitespace();
            switch(peek()) {
                case '{':
                    return readObject();
                case '[':
                    return readArray();
                case '"':
                    return readStringLiteral();
                case 't':
                    validate("true");
                    return true;
                case 'f':
                    validate("false");
                    return false;
                case 'n':
                    validate("null");
                    return null;
                default:
                    return readNumber();
            }
        }
    }
}