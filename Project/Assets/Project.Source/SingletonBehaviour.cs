using System;
using UnityEngine;

namespace Project.Source
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        private static T instance;
        
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
                Destroy(this);

                return;
            }

            instance = (T)this;
        }
    }
}