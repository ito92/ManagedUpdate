using System.Collections;
using System.Collections.Generic;
using DaBois.Tools;
using UnityEngine;

public class ManagedUpdateExample : MonoBehaviour, IUpdateable
{
    private void OnEnable()
    {
        if (UpdateManager.Instance)
        {
            UpdateManager.Instance.AddUpdateable(this);
        }
    }

    private void OnDisable()
    {
        if (UpdateManager.Instance)
        {
            UpdateManager.Instance.RemoveUpdateable(this);
        }
    }
    
    public void ManagedUpdate()
    {
        Debug.Log(this + " is Updating", this);
    }

    public void ManagedFixedUpdate()
    {
    }
}
