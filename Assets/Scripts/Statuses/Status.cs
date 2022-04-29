using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[System.Serializable]
public class Status
{
    public int m_Count = 0;
    public enum StatusType { Buff, Debuff, Unique, NA };
    public StatusType m_StatusType;

    public Status()
    {

    }

    public virtual void UpdateCount()
    {

    }

    public virtual void DoEffect()
    {

    }
    public virtual System.Type GetGenericType()
    {
        return this.GetType();
    }
}


