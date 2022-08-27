using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaBois.Tools
{
    public interface IUpdateable
    {
        void ManagedUpdate();
        void ManagedFixedUpdate();
        void LateUpdate();
        updateMode UpdateMode();
    }
}