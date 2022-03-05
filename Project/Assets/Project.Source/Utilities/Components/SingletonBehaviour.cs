using System;
using UnityEngine;

namespace Project.Source.Utilities.Components
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        private static T instance;

        public bool dontDestroyOnLoad;
        
        public static T Instance
        {
            get
            {
                if (instance)
                {
                    return instance;
                }

                throw new NullReferenceException($"{typeof(T).Name} was null. There should be a {typeof(T).Name} object somewhere in your scene.");
            }
        }

        protected virtual void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);

                return;
            }
            
            instance = (T)this;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
    }
}