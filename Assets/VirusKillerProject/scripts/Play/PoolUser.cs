using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolUser : MonoBehaviour
{
    protected bool isUse;

    public bool GetIsUse()
    {
        return isUse;
    }

    public void SetIsUse(bool value)
    {
        isUse = value;
    }
}