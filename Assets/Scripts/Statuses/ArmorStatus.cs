using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorStatus : Status
{

    public int armor;

    public ArmorStatus()
    {
        m_StatusType = StatusType.Buff;
    }

    public override void UpdateCount()
    {
        base.UpdateCount();
    }

    public override void DoEffect()
    {
        base.DoEffect();
    }
    public override System.Type GetGenericType()
    {
        return this.GetType();
    }
}