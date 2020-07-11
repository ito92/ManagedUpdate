using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaBois.Tools
{
    public class UpdateManager : MonoBehaviour
    {
        private static UpdateManager _instance;
        public static UpdateManager Instance
        {
            get
            {
                if (_quitting)
                {
                    return null;
                }
                if (!_instance)
                {
                    GameObject updateManager = new GameObject("UpdateManager");
                    _instance = updateManager.AddComponent<UpdateManager>();
                }

                return _instance;
            }
        }
        private static bool _quitting;
        private List<IUpdateable> _updateables = new List<IUpdateable>();
        private List<IUpdateable> _lateUpdateables = new List<IUpdateable>();

        private void Awake()
        {
            if (_instance)
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            _instance = this;
        }

        private void OnApplicationQuit()
        {
            _quitting = true;
        }

        public void AddUpdateable(IUpdateable updateable, bool lateUpdate = false)
        {
            if (lateUpdate)
            {
                if (!_lateUpdateables.Contains(updateable))
                {
                    _lateUpdateables.Add(updateable);
                }
            }
            else
            {
                if (!_updateables.Contains(updateable))
                {
                    _updateables.Add(updateable);
                }
            }
        }

        public void RemoveUpdateable(IUpdateable updateable)
        {
            if (_updateables.Contains(updateable))
            {
                _updateables.Remove(updateable);
            }
            else if (_lateUpdateables.Contains(updateable))
            {
                _lateUpdateables.Remove(updateable);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _updateables.Count; i++)
            {
                _updateables[i].ManagedUpdate();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _updateables.Count; i++)
            {
                _updateables[i].ManagedFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _lateUpdateables.Count; i++)
            {
                _lateUpdateables[i].ManagedUpdate();
            }
        }
    }
}