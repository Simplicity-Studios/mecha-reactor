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
    private float value, step;

    // Value gets changed automatically based on how many points are allocated
    // and the min and max set by the developer beforehand.
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
    public int maxElectricity;
    [Range(1, 10)]
    public int electricityDecreaseRate = 1;

    [SerializeField]
    private Attribute[] initialAttributes;
    private Dictionary<string, Attribute> m_attributes;
    
    private int m_maxPoints = 9, m_points;
    private float m_electricity;

    void Start()
    {
        m_electricity = maxElectricity;

        m_attributes = new Dictionary<string, Attribute>();
        foreach (Attribute attr in initialAttributes)
        {
            m_attributes[attr.name] = attr;
            m_attributes[attr.name].Initialize();
        }
    }

    void Update()
    {
        int points = 0;
        foreach (Attribute attr in m_attributes.Values)
            points += attr.pointsAllocated;
        m_points = points;

        m_electricity -= m_points * electricityDecreaseRate * 0.005f;
        print(m_electricity);
    }
    
    // Operator [] overload to get a specific attribute from the m_attributes Dictionary.
    public Attribute this[string attribute] 
    {
        get => m_attributes[attribute];
    }

    // Getters mainly for UI scripts
    public int GetTotalPointsAllocated()
    {
        return m_points;
    }

    public int GetMaxPoints()
    {
        return m_maxPoints;
    }

    public float GetElectricity()
    {
        return m_electricity;
    }

    public float GetMaxElectricity()
    {
        return maxElectricity;
    }
}
