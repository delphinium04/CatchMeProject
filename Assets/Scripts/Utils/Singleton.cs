using System;
using UnityEngine;

// 훌륭한 gpt-4o의 산출물
namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        void Awake()
        {
            _applicationIsQuitting = false;
        }

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       "  - there should never be more than 1 singleton!" +
                                       "  Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance != null) return _instance;

                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singleton);
                    return _instance;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            Debug.Log("Destroyed");
            _applicationIsQuitting = true;
        }
    }
}