using System.Collections.Generic;
using UnityEngine;

public class ReactorAttributes : MonoBehaviour
{
    private Dictionary<string, float> m_attributes = new Dictionary<string, float>()
    {
        {"movementSpeed", 0.0f},
        {"attack", 0.0f},
        {"defense", 0.0f},
        {"special", 0.0f},
        {"attackSpeed", 0.0f},
    };

    // Operator [] overload to get a specific attribute from the m_attributes Dictionary.
    public float this[string attribute] 
    {
        get { return m_attributes[attribute]; }
        set { m_attributes[attribute] = value; }
    }
}
