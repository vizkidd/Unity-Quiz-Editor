using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableColor
{
    /// <summary>
    /// x component
    /// </summary>
    public float r,g,b,a;

    /// <summary>
    /// y component
    /// </summary>

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rR"></param>
    /// <param name="rG"></param>
    /// <param name = "rB" ></ param >
    /// <param name="rA"></param>
    public SerializableColor(float rR, float rG, float rB, float rA)
    {
        r = rR;
        g = rG;
        b = rB;
        a = rA;

    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2} , {3}]", r, g, b, a);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Color32(SerializableColor rValue)
    {
        return new Color(rValue.r, rValue.g, rValue.b, rValue.a);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableColor(Color32 rValue)
    {
        return new SerializableColor(rValue.r, rValue.g, rValue.b, rValue.a);
    }
}
