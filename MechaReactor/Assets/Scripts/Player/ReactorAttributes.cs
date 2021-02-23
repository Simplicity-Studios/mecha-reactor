using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attribute
{
    public string name;
    public float minValue, maxValue;

    private int points;
    private const int size = 3;
    private float value;
    private float step;

    public int pointsAllocated 
    { 
        get => points; 
        set 
        {
            points = (int) Mathf.Clamp(value, 0, size);
            this.value = minValue + points * step;
        }
    }

    public float GetValue()
    {
        return value;
    }

    public void Initialize()
    {
        pointsAllocated = 0;
        step = (maxValue - minValue) / size;
    }
}

public class ReactorAttributes : MonoBehaviour
{
    [SerializeField]
    private Attribute[] initialAttributes;
    private Dictionary<string, Attribute> m_attributes;

    void Start()
    {
        m_attributes = new Dictionary<string, Attribute>();
        foreach (Attribute attr in initialAttributes)
        {
            m_attributes[attr.name] = attr;
            m_attributes[attr.name].Initialize();
        }
    }
    
    // Operator [] overload to get a specific attribute from the m_attributes Dictionary.
    public Attribute this[string attribute] 
    {
        get => m_attributes[attribute];
    }
}
