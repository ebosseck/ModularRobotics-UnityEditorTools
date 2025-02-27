using System;
using System.Collections.Generic;
using EditorTools.BaseTypes;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorTools.InspectorTools
{
    /// <summary>
    /// Create custom UI Elements
    /// </summary>
    public class GUIElemetCreator
    {
        #region Configuration
        
        /// <summary>
        /// Font Size for Header Texts
        /// </summary>
        public static readonly float HEADER_TEXT_SIZE = 12f;

        #endregion

        #region Generic Functions

        /// <summary>
        /// Creates all required fields for the given property with the given Editor
        /// </summary>
        /// <param name="editor">Editor whose target class contains the property to create the field for</param>
        /// <param name="property">Property to create fields for</param>
        /// <returns>An array of visual elements to add to the GUI </returns>
        public static VisualElement[] createFields(Editor editor, SerializedProperty property)
        {
            return createFields(AttributeTools.getEditorTargetClass(editor), property);
        }

        /// <summary>
        /// Creates all required fields for the given property with the given Editor
        /// </summary>
        /// <param name="objectType">Objec type containing the property to create the field for</param>
        /// <param name="property">Property to create fields for</param>
        /// <returns>An array of visual elements to add to the GUI </returns>
        public static VisualElement[] createFields(Type objectType, SerializedProperty property)
        {
            
            List<VisualElement> elements = new List<VisualElement>();


            string header = AttributeTools.getHeadder(objectType, property);

            if (header != null)
            {
                elements.Add(createHeaderLabel(header));
            }

            if (property.isArray)
            {
                if (AttributeTools.isNonResizeable(objectType, property))
                {
                    int precission = AttributeTools.getTypePrecission(objectType, property);
                    string unit = AttributeTools.getUnit(objectType, property);
                    float[] range = AttributeTools.getRange(objectType, property);
                    string[] choices = AttributeTools.getChoices(objectType, property);
                    bool isTag = AttributeTools.isTag(objectType, property);
                    
                    Foldout root = createFoldout(property.displayName);

                    for (int i = 0; i < property.arraySize; i++)
                    {
                        root.Add(createField(objectType, property.GetArrayElementAtIndex(i), precission, unit, range, choices, isTag));
                    }

                    elements.Add(root);
                    return elements.ToArray();
                }
            }

            elements.Add(createField(objectType, property));
            return elements.ToArray();
        }

        /// <summary>
        /// Creates the field for the given property
        /// </summary>
        /// <param name="editor">Editor whose target class contains the property to create the field for</param>
        /// <param name="property">Property to create the field for</param>
        /// <returns>The visual element to add to the GUI</returns>
        public static VisualElement createField(Editor editor, SerializedProperty property)
        {
            return createField(AttributeTools.getEditorTargetClass(editor), property);
        }

        /// <summary>
        /// Creates the field for the given property
        /// </summary>
        /// <param name="objectType">Object type containing the property to create the field for</param>
        /// <param name="property">Property to create the field for</param>
        /// <returns>The visual element to add to the GUI</returns>
        public static VisualElement createField(Type objectType, SerializedProperty property)
        {
            int precission = AttributeTools.getTypePrecission(objectType, property);
            string unit = AttributeTools.getUnit(objectType, property);
            float[] range = AttributeTools.getRange(objectType, property);
            string[] choices = AttributeTools.getChoices(objectType, property);
            bool isTag = AttributeTools.isTag(objectType, property);
            
            return createField(objectType, property, precission, unit, range, choices, isTag);
        }

        /// <summary>
        /// Creates the field for the given property
        /// </summary>
        /// <param name="objectType">Object type containing the property to create the field for</param>
        /// <param name="property">Property to create the field for</param>
        /// <param name="precission">Precission parameter (Overrides Attribute)</param>
        /// <param name="unit">Unit parameter (Overrides Attribute)</param>
        /// <param name="range">Range parameter (Overrides Attribute)</param>
        /// <returns>The visual element to add to the GUI</returns>
        public static VisualElement createField(Type objectType, SerializedProperty property, int precission,
            string unit, float[] range, string[] choices, bool isTagField)
        {
            VisualElement element;
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    element = createPropertyField(property);
                    addCallbacks<UnityEngine.Object>(element, property);
                    return element;
                case SerializedPropertyType.Integer:
                    /*if (choices != null) //TODO: Figure out how to bind index of DropdownField
                    {
                        element = createDropdownField(property, choices, unit);
                        addCallbacks<int>(element, property);
                        return element;
                    }*/

                    if (precission <= 32)
                    {
                        if (range != null)
                        {
                            element = createIntSlider(property, (int) range[0], (int) range[1], unit: unit);
                            addCallbacks<int>(element, property);
                            return element;
                        }

                        element = createIntField(property, unit);
                        addCallbacks<int>(element, property);
                        return element;
                    }
                    else
                    {
                        element = createLongField(property, unit);
                        addCallbacks<long>(element, property);
                        return element;
                    }
                case SerializedPropertyType.Boolean:
                    element = createToggle(property, unit);
                    addCallbacks<bool>(element, property);
                    return element;
                case SerializedPropertyType.Float:
                    if (precission <= 32)
                    {
                        if (range != null)
                        {
                            element = createSlider(property, range[0], range[1], unit: unit);
                            addCallbacks<float>(element, property);
                            return element;
                        }

                        element = createFloatField(property, unit);
                        addCallbacks<float>(element, property);
                        return element;
                    }
                    else
                    {
                        element = createDoubleField(property, unit);
                        addCallbacks<double>(element, property);
                        return element;
                    }
                case SerializedPropertyType.String:
                    
                    if (choices != null)
                    {
                        element = createDropdownField(property, choices, unit);
                        addCallbacks<string>(element, property);
                        return element;
                    }

                    if (isTagField)
                    {
                        element = createTagField(property, unit);
                        addCallbacks<string>(element, property);
                        return element;
                    }

                    if (AttributeTools.isScene(objectType, property))
                    {
                        string[] scenes = GUIElementTools.getSceneList().ToArray();
                        element = createDropdownField(property, scenes, unit);
                        addCallbacks<string>(element, property);
                        return element;
                    }
                    
                    element = createPropertyField(property);
                    addCallbacks<string>(element, property);
                    return element;
                case SerializedPropertyType.Color:
                    element = createColorField(property, unit);
                    addCallbacks<Color>(element, property);
                    return element;
                case SerializedPropertyType.ObjectReference:
                    Type fieldType = AttributeTools.getFieldType(objectType, property);
                    element = createObjectField(property, fieldType, unit);
                    addCallbacks<UnityEngine.Object>(element, property);
                    return element;
                case SerializedPropertyType.LayerMask:
                    element = createPropertyField(property);
                    addCallbacks<LayerMask>(element, property);
                    return element;
                case SerializedPropertyType.Enum:
                    element = createPropertyField(property);
                    addCallbacks<Enum>(element, property);
                    return element;
                case SerializedPropertyType.Vector2:
                    element = createVector2Field(property, unit);
                    addCallbacks<Vector2>(element, property);
                    return element;
                case SerializedPropertyType.Vector3:
                    element = createVector3Field(property, unit);
                    addCallbacks<Vector3>(element, property);
                    return element;
                case SerializedPropertyType.Vector4:
                    element = createVector4Field(property, unit);
                    addCallbacks<Vector4>(element, property);
                    return element;
                case SerializedPropertyType.Rect:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<Rect>(element, property);
                    return element;
                case SerializedPropertyType.ArraySize:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<int>(element, property);
                    return element;
                case SerializedPropertyType.Character:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<char>(element, property);
                    return element;
                case SerializedPropertyType.AnimationCurve:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<AnimationCurve>(element, property);
                    return element;
                case SerializedPropertyType.Bounds:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<Bounds>(element, property);
                    return element;
                case SerializedPropertyType.Gradient:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<Gradient>(element, property);
                    return element;
                case SerializedPropertyType.Quaternion:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<Quaternion>(element, property);
                    return element;
                case SerializedPropertyType.ExposedReference:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<object>(element, property);
                    return element;
                case SerializedPropertyType.FixedBufferSize:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<object>(element, property);
                    return element;
                case SerializedPropertyType.Vector2Int:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createVector2iField(property, unit);
                    addCallbacks<Vector2Int>(element, property);
                    return element;
                case SerializedPropertyType.Vector3Int:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createVector3iField(property, unit);
                    addCallbacks<Vector3Int>(element, property);
                    return element;
                case SerializedPropertyType.RectInt:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<RectInt>(element, property);
                    return element;
                case SerializedPropertyType.BoundsInt:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<BoundsInt>(element, property);
                    return element;
                case SerializedPropertyType.ManagedReference:
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<object>(element, property);
                    return element;
                default:
                    Debug.LogWarning("Unknown Property Type: " + property.propertyType +
                                     ", PLEASE UPDATE THIS SWITCH!");
                    Debug.Log("No Custom Field implemented, fallback to default");
                    element = createPropertyField(property);
                    addCallbacks<object>(element, property);
                    return element;
            }
        }

        #endregion

        #region Callbacks
        /// <summary>
        /// Change event callback for generic widgets
        /// </summary>
        /// <param name="evt">Change Event</param>
        /// <param name="property">Property to bind the event to</param>
        /// <typeparam name="T">inner type of the change event</typeparam>
        static void genericChangeEventCB<T>(ChangeEvent<T> evt, SerializedProperty property)
        {
            
            UnityEngine.Object callTarget = property.serializedObject.targetObject;

            if (typeof(ExtendedMonoBehaviour).IsAssignableFrom(callTarget.GetType()))
            {
                ((ExtendedMonoBehaviour)callTarget).OnInspectorValueChanged(property.propertyPath, evt.previousValue, evt.newValue);
            }
        }

        #endregion

        #region Labels & Foldouts

        /// <summary>
        /// Creates a Label
        /// </summary>
        /// <param name="label">text of the label</param>
        /// <param name="style">Font Style of the label</param>
        /// <returns>The Label</returns>
        public static Label createLabel(string label, FontStyle style = FontStyle.Normal)
        {
            Label lbl = new Label(label);
            lbl.style.unityFontStyleAndWeight = style;
            return lbl;
        }

        /// <summary>
        /// Creates a Label in header format
        /// </summary>
        /// <param name="label">text of the label</param>
        /// <param name="style">Font Style of the label</param>
        /// <returns>The Label</returns>
        public static Label createHeaderLabel(string label, FontStyle style = FontStyle.Bold)
        {
            Label lbl = new Label(label);
            lbl.style.unityFontStyleAndWeight = style;
            lbl.style.fontSize = HEADER_TEXT_SIZE;
            return lbl;
        }

        /// <summary>
        /// Creates a Foldout
        /// </summary>
        /// <param name="label">text of the label</param>
        /// <param name="style">Font Style of the label</param>
        /// <returns>The Foldout</returns>
        public static Foldout createFoldout(string label, FontStyle style = FontStyle.Bold)
        {
            Foldout foldout = new Foldout();
            foldout.text = label;
            foldout.style.unityFontStyleAndWeight = style;

            return foldout;
        }

        #endregion
        
        #region Property Fields

        #region BaseFields

        /// <summary>
        /// Prepares this field base
        /// </summary>
        /// <param name="field">Field to prepare</param>
        /// <param name="property">Property belonging to this field</param>
        /// <param name="value">Value of this field</param>
        /// <param name="unit">Unit name of this fielt</param>
        /// <typeparam name="T">Type param of the value</typeparam>
        /// <returns></returns>
        private static BaseField<T> prepareBaseField<T>(BaseField<T> field, SerializedProperty property, T value,
            string unit)
        {
            field.label = property.displayName;
            field.value = value;
            field.BindProperty(property);

            field.tooltip = property.tooltip;

            field.style.unityFontStyleAndWeight = FontStyle.Normal;

            unitWrapper(field, unit);

            return field;
        }

        /// <summary>
        /// Creates a generic property field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>The Property Field for this property</returns>
        public static PropertyField createPropertyField(SerializedProperty property, string unit = null)
        {
            PropertyField field = new PropertyField();

            field.label = property.displayName;
            field.tooltip = property.tooltip;
            field.BindProperty(property);
            unitWrapper(field, unit);
            
            return field;
        }
        
        /// <summary>
        /// Creates a generic property field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>The Property Field for this property</returns>
        public static TagField createTagField(SerializedProperty property, string unit = null)
        {
            TagField field = new TagField();

            field.label = property.displayName;
            field.tooltip = property.tooltip;
            
            field.BindProperty(property);
            unitWrapper(field, unit);
            
            return field;
        }

        /// <summary>
        /// Creates a field for a Color property
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <returns>the created ColorField</returns>
        public static ColorField createColorField(SerializedProperty property, string unit = "")
        {
            ColorField fld = new ColorField();
            prepareBaseField(fld, property, property.colorValue, unit);

            return fld;
        }

        /// <summary>
        /// Creates a Text field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <param name="multiline">If True, the field should be a multi line field</param>
        /// <returns>the created TextField</returns>
        public static TextField createTextField(SerializedProperty property, string unit = "", int multiline = 1)
        {
            TextField field = new TextField();
            prepareBaseField(field, property, property.stringValue, unit);

            if (multiline > 1)
            {
                field.multiline = true;
                field.style.height = multiline * field.style.fontSize.value.value;
            }

            return field;
        }

        /// <summary>
        /// Creates a object field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="fieldType">Type of the Field value</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>The created ObjectField</returns>
        public static ObjectField createObjectField(SerializedProperty property, Type fieldType, string unit = "")
        {
            ObjectField field = new ObjectField();
            prepareBaseField(field, property, property.objectReferenceValue, unit);

            field.objectType = fieldType;
            return field;
        }

        #endregion

        #region Scalar Numbers
        
        /// <summary>
        /// Prepares a TextValueField
        /// </summary>
        /// <param name="field">Field to prepare</param>
        /// <param name="property">Property to prepare the field for</param>
        /// <param name="value">Value of the field</param>
        /// <param name="unit">Unit of the field</param>
        /// <typeparam name="T">Type of the fields value</typeparam>
        /// <returns>the prepared TextValueField</returns>
        private static TextValueField<T> prepareTextValueField<T>(TextValueField<T> field, SerializedProperty property,
            T value, string unit)
        {
            field.label = property.displayName;
            field.value = value;
            field.BindProperty(property);

            field.tooltip = property.tooltip;
            //field.style.unityTextAlign = NUMERIC_FIELD_TEXT_ALIGN;
            field.style.unityFontStyleAndWeight = FontStyle.Normal;

            unitWrapper(field, unit);

            return field;
        }

        /// <summary>
        /// Creates a toggle field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <returns>the created Toggle field</returns>
        public static Toggle createToggle(SerializedProperty property, string unit = null)
        {
            Toggle fld = new Toggle();
            prepareBaseField(fld, property, property.boolValue, unit);

            return fld;
        }

        /// <summary>
        /// Creates an int field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <returns>the created IntegerField</returns>
        public static IntegerField createIntField(SerializedProperty property, string unit = "")
        {
            IntegerField fld = new IntegerField();
            prepareTextValueField(fld, property, property.intValue, unit);
            
            
            
            return fld;
        }

        /// <summary>
        /// Creates a long field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <returns>the created LongField</returns>
        public static LongField createLongField(SerializedProperty property, string unit = "")
        {
            LongField fld = new LongField();
            prepareTextValueField(fld, property, property.longValue, unit);

            return fld;
        }
        
        /// <summary>
        /// Creates a float field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <returns>the created FloatField</returns>
        public static FloatField createFloatField(SerializedProperty property, string unit = "")
        {
            FloatField fld = new FloatField();
            prepareTextValueField(fld, property, property.floatValue, unit);

            return fld;
        }

        /// <summary>
        /// Creates a double field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field to create</param>
        /// <returns>the created DoubleField</returns>
        public static DoubleField createDoubleField(SerializedProperty property, string unit = "")
        {
            DoubleField fld = new DoubleField();

            prepareTextValueField(fld, property, property.doubleValue, unit);

            return fld;
        }

        #endregion

        #region Slider
        
        /// <summary>
        /// Prepares a BaseSlider
        /// </summary>
        /// <param name="slider">Field to prepare</param>
        /// <param name="property">Property to prepare the field for</param>
        /// <param name="value">Value of the field</param>
        /// <param name="min">Minimal value of the slider</param>
        /// <param name="max">Maximal value of the slider</param>
        /// <param name="unit">Unit of the field</param>
        /// <typeparam name="U">Type of the fields value</typeparam>
        /// <returns>the prepared TextValueField</returns>
        private static BaseSlider<U> prepareSlider<U>(BaseSlider<U> slider, SerializedProperty property, U value, U min,
            U max, string unit = null) where U : IComparable<U>
        {
            slider.label = property.displayName;
            slider.value = value;

            slider.lowValue = min;
            slider.highValue = max;

            slider.BindProperty(property);

            slider.tooltip = property.tooltip;

            slider.showInputField = true;

            slider.style.unityFontStyleAndWeight = FontStyle.Normal;

            unitWrapper(slider, unit);

            return slider;
        }

        /// <summary>
        /// Creates a float slider
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">Property the slider is for</param>
        /// <param name="unit">Unit of the slider</param>
        /// <returns>the created Slider</returns>
        public static Slider createSlider(Type objectType, SerializedProperty property, string unit = "")
        {
            Slider fld = new Slider();


            RangeAttribute range =
                (RangeAttribute) AttributeTools.getFieldAttribute(objectType, property.propertyPath,
                    typeof(RangeAttribute));

            if (range != null)
            {
                prepareSlider(fld, property, property.floatValue, range.min, range.max, unit);
            }
            else
            {
                prepareSlider(fld, property, property.floatValue, 0, 10, unit);
            }

            return fld;
        }

        /// <summary>
        /// Creates a float slider
        /// </summary>
        /// <param name="property">Property the slider is for</param>
        /// <param name="min">Minimal slider value</param>
        /// <param name="max">Maximal slider value</param>
        /// <param name="unit">Slider unit</param>
        /// <returns>the created float slider</returns>
        public static Slider createSlider(SerializedProperty property, float min, float max, string unit = "")
        {
            Slider fld = new Slider();

            prepareSlider(fld, property, property.floatValue, min, max, unit);

            return fld;
        }

        /// <summary>
        /// Creates an int slider
        /// </summary>
        /// <param name="objectType">Type containing the property</param>
        /// <param name="property">Property the slider is for</param>
        /// <param name="unit">Unit of the slider</param>
        /// <returns>the created IntSlider</returns>
        public static SliderInt createIntSlider(Type objectType, SerializedProperty property, string unit = "")
        {
            SliderInt fld = new SliderInt();

            RangeAttribute range =
                (RangeAttribute) AttributeTools.getFieldAttribute(objectType, property.propertyPath,
                    typeof(RangeAttribute));

            if (range != null)
            {
                prepareSlider(fld, property, property.intValue, (int) range.min, (int) range.max, unit);
            }
            else
            {
                prepareSlider(fld, property, property.intValue, 0, 10, unit);
            }

            return fld;
        }

        /// <summary>
        /// Creates an int slider
        /// </summary>
        /// <param name="property">Property the slider is for</param>
        /// <param name="min">Minimal slider value</param>
        /// <param name="max">Maximal slider value</param>
        /// <param name="unit">Slider unit</param>
        /// <returns>the created int slider</returns>
        public static SliderInt createIntSlider(SerializedProperty property, int min, int max, string unit = "")
        {
            SliderInt fld = new SliderInt();

            prepareSlider(fld, property, property.intValue, min, max, unit);

            return fld;
        }

        #endregion

        #region DropDown Fields
        

        
    #if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Creates a DropDown field with the given choices for the given property
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="choices">List of choices to display in the dropdown field</param>
        /// <param name="unit">Unit of the values in the drop down field.</param>
        /// <returns>the created drop down field</returns>
        public static DropdownField createDropdownField(SerializedProperty property, string[] choices, string unit = "")
        {
            DropdownField field = new DropdownField();
            field.label = property.displayName;
            field.choices =  new List<string>(choices);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                Debug.Log("Setting Property Field by Index !");
                field.index = property.intValue;
                field.value = choices[property.intValue];
            }
            else
            {
                field.value = property.stringValue;
            }
            
            field.BindProperty(property);

            field.tooltip = property.tooltip;
            field.style.unityFontStyleAndWeight = FontStyle.Normal;
            
            unitWrapper(field, unit);
            
            return field;
        }
#else
        /// <summary>
        /// Creates a DropDown field with the given choices for the given property
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="choices">List of choices to display in the dropdown field</param>
        /// <param name="unit">Unit of the values in the drop down field.</param>
        /// <returns>a label to inform the user to update their Unity version</returns>
        public static VisualElement createDropdownField(SerializedProperty property, string[] choices, string unit = "")
        {
            return createLabel("Please Update to Unity 2021.3 or newer for DropdownField support");
        }
#endif
        

        #endregion
        
        #region Vectors

        /// <summary>
        /// prepares a BaseCompositeField
        /// </summary>
        /// <param name="field">The field to prepare</param>
        /// <param name="property">The property to prepare the field for</param>
        /// <param name="value">Value of the field</param>
        /// <param name="unit">Unit of the field</param>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <typeparam name="V">Type of the fields contained in this composite field</typeparam>
        /// <typeparam name="U">Inner Type of the fields within the composite field</typeparam>
        /// <returns>the prepared composite field</returns>
        private static BaseCompositeField<T, V, U> prepareCompositeField<T, V, U>(BaseCompositeField<T, V, U> field,
            SerializedProperty property,
            T value, string unit) where V : TextValueField<U>, new()
        {
            field.label = property.displayName;
            field.value = value;
            field.BindProperty(property);

            field.tooltip = property.tooltip;
            field.style.unityFontStyleAndWeight = FontStyle.Normal;

            //field.style.unityTextAlign = NUMERIC_FIELD_TEXT_ALIGN;

            unitWrapper(field, unit);

            return field;
        }

        /// <summary>
        /// Creates a new Vector2 field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>the created Vector2Field</returns>
        public static Vector2Field createVector2Field(SerializedProperty property, string unit = "")
        {
            Vector2Field fld = new Vector2Field();
            prepareCompositeField(fld, property, property.vector2Value, unit);

            return fld;
        }

        /// <summary>
        /// Creates a new Vector2int field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>the created Vector2IntField</returns>
        public static Vector2IntField createVector2iField(SerializedProperty property, string unit = "")
        {
            Vector2IntField fld = new Vector2IntField();
            prepareCompositeField(fld, property, property.vector2IntValue, unit);

            return fld;
        }

        /// <summary>
        /// Creates a new Vector3 field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>the created Vector3Field</returns>
        public static Vector3Field createVector3Field(SerializedProperty property, string unit = "")
        {
            Vector3Field fld = new Vector3Field();
            prepareCompositeField(fld, property, property.vector3Value, unit);

            return fld;
        }

        /// <summary>
        /// Creates a new Vector3Int field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>the created Vector3IntField</returns>
        public static Vector3IntField createVector3iField(SerializedProperty property, string unit = "")
        {
            Vector3IntField fld = new Vector3IntField();
            prepareCompositeField(fld, property, property.vector3IntValue, unit);

            return fld;
        }

        /// <summary>
        /// Creates a new Vector4 field
        /// </summary>
        /// <param name="property">Property to create the field for</param>
        /// <param name="unit">Unit of the field</param>
        /// <returns>the created Vector4Field</returns>
        public static Vector4Field createVector4Field(SerializedProperty property, string unit = "")
        {
            Vector4Field fld = new Vector4Field();
            prepareCompositeField(fld, property, property.vector4Value, unit);

            return fld;
        }

        #endregion

        #region Popups
        
        /// <summary>
        /// Prepares a BasePopupField
        /// </summary>
        /// <param name="field">Field to prepare</param>
        /// <param name="property">Property to prepare the field for</param>
        /// <param name="value">Value of the property</param>
        /// <param name="unit">Unit of the property field</param>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <typeparam name="V">Value Choice Type</typeparam>
        /// <returns>the prepared BasePopupField</returns>
        private static BasePopupField<T, V> prepareBasePopupField<T, V>(BasePopupField<T, V> field, SerializedProperty property, T value,
            string unit)
        {
            field.label = property.displayName;
            field.value = value;
            field.BindProperty(property);
            
            field.tooltip = property.tooltip;

            field.style.unityFontStyleAndWeight = FontStyle.Normal;

            unitWrapper(field, unit);

            return field;
        }
        
        #endregion
        
        #endregion

        #region Wrappers

        #region Callbacks

        /// <summary>
        /// Adds the given callback to the given visual element
        /// </summary>
        /// <param name="element">Visual Element to add the callback to</param>
        /// <param name="property">Property whose callback to add to the Visual Element</param>
        /// <typeparam name="T">Type of the change event</typeparam>
        /// <returns>The visual element with the added callback</returns>
        private static VisualElement addCallbacks<T>(VisualElement element, SerializedProperty property)
        {
            element.RegisterCallback<ChangeEvent<T>, SerializedProperty>(genericChangeEventCB, property, useTrickleDown: TrickleDown.NoTrickleDown);
            return element;
        }


        #endregion
        
        #region Units
        
        /// <summary>
        /// Adds the given Unit decorator to the given field
        /// </summary>
        /// <param name="element">Visual Element to add the decorator to</param>
        /// <param name="unit">Unit to add to the field</param>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <typeparam name="V">Type of the fields contained in this composite field</typeparam>
        /// <typeparam name="U">Inner Type of the fields within the composite field</typeparam>
        /// <returns>The modified BaseCompositeField</returns>
        public static BaseCompositeField<T, V, U> unitWrapper<T, V, U>(BaseCompositeField<T, V, U> element, string unit)
            where V : TextValueField<U>, new()
        {
            if (unit != null)
            {
                int count = element.contentContainer[1].childCount;
                for (int i = 0; i < count; i++)
                {
                    element.contentContainer[1]
                        .Insert((2 * i) + 1, createLabel(" [" + unit + "]", FontStyle.BoldAndItalic));
                }

            }

            return element;
        }
        
        /// <summary>
        /// Adds the given Unit decorator to the given field
        /// </summary>
        /// <param name="element">Visual Element to add the decorator to</param>
        /// <param name="unit">Unit to add to the field</param>
        /// <returns>The modified VisualElement</returns>
        public static VisualElement unitWrapper(VisualElement element, string unit)
        {
            if (unit != null)
            {
                element.Add(createLabel(" [" + unit + "]", FontStyle.BoldAndItalic));
            }

            return element;
        }

        #endregion

        #endregion

    }
}