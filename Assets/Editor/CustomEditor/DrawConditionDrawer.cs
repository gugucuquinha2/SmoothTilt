using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DrawConditionAttribute))]
public class DrawConditionDrawer : PropertyDrawer
{
    #region Fields

    // Reference to the attribute on the property.
    DrawConditionAttribute drawIf;

    // Field that is being compared.
    SerializedProperty comparedField;

    // Height of the property.
    private float propertyHeight;

    #endregion

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return propertyHeight;
    }

    /// <summary>
    /// Errors default to showing the property.
    /// </summary>
    private bool IsPropertyShown(SerializedProperty property)
    {
        drawIf = attribute as DrawConditionAttribute;
        // Replace propertyname to the value from the parameter
        string path = property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, drawIf.ComparedPropertyName) : drawIf.ComparedPropertyName;

        comparedField = property.serializedObject.FindProperty(path);

        if (comparedField == null)
        {
            Debug.LogError("Cannot find property with name: " + path);
            return true;
        }

        // get the value & compare based on types
        switch (comparedField.type)
        { // Possible extend cases to support your own type
            case "bool":
                return comparedField.boolValue.Equals(drawIf.ComparedValue);
            case "Enum":
                return comparedField.enumValueIndex.Equals((int)drawIf.ComparedValue);
            default:
                Debug.LogError("Error: " + comparedField.type + " is not supported of " + path);
                return true;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool bIsConditionMet = IsPropertyShown(property);

        // The height of the property should be defaulted to the default height.
        propertyHeight = base.GetPropertyHeight(property, label);

        switch (drawIf.ComparisonType)
        {
            case eComparisonType.Equals:

                if (bIsConditionMet)
                {
                    EditorGUI.PropertyField(position, property);
                } //...check if the disabling type is read only. If it is, draw it disabled
                else
                {
                    switch (drawIf.DisablingType)
                    {
                        case eDisablingType.DontDraw:
                            propertyHeight = 0f;
                            break;
                        case eDisablingType.ReadOnly:
                            GUI.enabled = false;
                            EditorGUI.PropertyField(position, property);
                            GUI.enabled = true;
                            break;
                    }
                }

                break;
            case eComparisonType.NotEqual:

                if (!bIsConditionMet)
                {
                    EditorGUI.PropertyField(position, property);
                } //...check if the disabling type is read only. If it is, draw it disabled
                else
                {
                    switch (drawIf.DisablingType)
                    {
                        case eDisablingType.DontDraw:
                            propertyHeight = 0f;
                            break;
                        case eDisablingType.ReadOnly:
                            GUI.enabled = false;
                            EditorGUI.PropertyField(position, property);
                            GUI.enabled = true;
                            break;
                    }
                }

                break;
        }
    }
}
