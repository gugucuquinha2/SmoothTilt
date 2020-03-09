using UnityEngine;
using System;

/// <summary>
/// Draws the field/property ONLY if the copared property compared by the comparison type with the value of comparedValue returns true.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DrawConditionAttribute : PropertyAttribute
{
    public string ComparedPropertyName { get; private set; }
    public object ComparedValue { get; private set; }
    public eDisablingType DisablingType { get; private set; }
    public eComparisonType ComparisonType { get; private set; }

    /// <summary>
    /// Only draws the field only if a condition is met. Supports enum and bools.
    /// </summary>
    /// <param name="_comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
    /// <param name="_comparedValue">The value the property is being compared to.</param>
    /// <param name="_disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
    public DrawConditionAttribute(string _comparedPropertyName, eComparisonType _comparisonType, object _comparedValue, eDisablingType _disablingType = eDisablingType.DontDraw)
    {
        this.ComparedPropertyName = _comparedPropertyName;
        this.ComparedValue = _comparedValue;
        this.DisablingType = _disablingType;
        this.ComparisonType = _comparisonType;
    }
}

/// <summary>
/// Types of comparisons.
/// </summary>
public enum eComparisonType
{
    Equals = 1,
    NotEqual = 2
}

/// <summary>
/// Types of comparisons.
/// </summary>
public enum eDisablingType
{
    ReadOnly = 2,
    DontDraw = 3
}
