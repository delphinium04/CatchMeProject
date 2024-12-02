using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dijkstra
{
    public class DijkstraUIBase : MonoBehaviour
    {
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new();

        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            List<string> debugList = new();
            _objects.Add(typeof(T), objects);
            
            for (int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = FindChild(gameObject, names[i]);
                else
                    objects[i] = FindChild<T>(gameObject, names[i]);
                
                if(objects[i] == null)
                    debugList.Add(names[i]);
            }
            if(debugList.Count > 0)
                Debug.Log($"{string.Join(", ", debugList)} is not exist in this scene");
        }

        private T FindChild<T>(GameObject go, string name = null) where T : UnityEngine.Object
        {
            if (go == null) return null;

            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || name == component.name)
                    return component;
            }

            return null;
        }
        
        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            if(!_objects.TryGetValue(typeof(T), out var objects)) return null;
            return objects[idx] as T;
        }

        private GameObject FindChild(GameObject go, string name = null)
        {
            return FindChild<Transform>(go, name)?.gameObject;
        }
    }
}