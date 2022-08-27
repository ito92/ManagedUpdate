using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                    if (_lastScene == SceneManager.GetActiveScene().path)
                    {
                        return null;
                    }

                    GameObject updateManager = new GameObject("UpdateManager");
                    _instance = updateManager.AddComponent<UpdateManager>();
                }

                return _instance;
            }
        }
        private static bool _quitting;
        private List<IUpdateable> _updateables = new List<IUpdateable>();
        private List<IUpdateable> _fixedUpdateables = new List<IUpdateable>();
        private List<IUpdateable> _lateUpdateables = new List<IUpdateable>();
        private static string _lastScene;

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

        private void OnDestroy()
        {
            _lastScene = SceneManager.GetActiveScene().path;
            _instance = null;
        }

        public void AddUpdateable(IUpdateable updateable, updateMode mode)
        {
            if ((mode & updateMode.Update) == updateMode.Update)
            {
                if (!_updateables.Contains(updateable))
                {
                    _updateables.Add(updateable);
                }
            }
            if ((mode & updateMode.FixedUpdate) == updateMode.FixedUpdate)
            {
                if (!_fixedUpdateables.Contains(updateable))
                {
                    _fixedUpdateables.Add(updateable);
                }
            }
            if ((mode & updateMode.LateUpdate) == updateMode.LateUpdate)
            {
                if (!_lateUpdateables.Contains(updateable))
                {
                    _lateUpdateables.Add(updateable);
                }
            }
        }

        public void RemoveUpdateable(IUpdateable updateable)
        {
            if (_updateables.Contains(updateable))
            {
                _updateables.Remove(updateable);
            }
            else if (_fixedUpdateables.Contains(updateable))
            {
                _fixedUpdateables.Remove(updateable);
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
            for (int i = 0; i < _fixedUpdateables.Count; i++)
            {
                _fixedUpdateables[i].ManagedFixedUpdate();
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

[Flags]
public enum updateMode : byte
{
    Update = 1,
    FixedUpdate = 2,
    LateUpdate = 4,
    Update_FixedUpdate = Update | FixedUpdate,
    Update_LateUpdate = Update | LateUpdate,
    FixedUpdate_LateUpdate = FixedUpdate | LateUpdate,
    All = Update | FixedUpdate | LateUpdate
}