using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attribute
{
    public string name;
    public float minValue, maxValue;

    private int points, hiddenPoints;
    private const int size = 3;
    private float value, step;
    private bool enabled = true;

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

    public void Disable()
    {
        hiddenPoints = pointsAllocated;
        pointsAllocated = 0;
        enabled = false;
    }

    public void Enable()
    {
        pointsAllocated = hiddenPoints;
        hiddenPoints = 0;
        enabled = true;
    }

    public bool IsEnabled()
    {
        return enabled;
    }

    public int GetHiddenPoints()
    {
        return hiddenPoints;
    }
}

public class ReactorAttributes : MonoBehaviour
{
    public int maxElectricity;
    [Range(1, 10)]
    public int electricityDecreaseRate = 1;
    public AudioSource electricityPenalty;
    public AudioSource electricityBackOnline;

    [SerializeField]
    private Attribute[] initialAttributes;
    private Dictionary<string, Attribute> m_attributes;
    
    private int m_maxPoints = 9, m_points;
    private float m_electricity;
    private bool m_penalty = false;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            points += attr.pointsAllocated + attr.GetHiddenPoints();
        m_points = points;

        if(!gameManager.isPaused)
        {
            m_electricity -= m_points * electricityDecreaseRate * 0.005f;
            m_electricity = Mathf.Clamp(m_electricity, 0, maxElectricity);
        }

        if (m_electricity == 0 && !m_penalty)
        {
            electricityPenalty.Play();
            m_penalty = true;
            foreach (Attribute attr in initialAttributes)
                m_attributes[attr.name].Disable();
        }
        else if (m_penalty && m_electricity > 0)
        {
            electricityBackOnline.Play();
            m_penalty = false;
            foreach (Attribute attr in initialAttributes)
                m_attributes[attr.name].Enable();
        }
            
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

    public void AddElectricity(float add)
    {
        m_electricity = Mathf.Min(m_electricity + add, maxElectricity);
    }

    public void ResetPoints()
    {
        foreach (Attribute attr in m_attributes.Values)
            attr.pointsAllocated = 0;
    }
}
