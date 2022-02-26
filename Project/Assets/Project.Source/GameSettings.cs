using System;
using UnityEngine;

namespace Project.Source
{
    public class GameSettings : MonoBehaviour
    {
        private static GameSettings instance;

        // Used for checking if attacks have hit an enemy
        public LayerMask entityDamageLayerMask;
        
        // Used for checking if there is a nearby entity
        public LayerMask entityWorldLayerMask;
        
        public static GameSettings Instance
        {
            get
            {
                if (instance)
                {
                    return instance;
                }

                throw new NullReferenceException("GameSettings was null. There should be a GameSettings object somewhere in your scene.");
            }
        }

        private void Awake()
        {
            if (instance)
            {
                Destroy(this);

                return;
            }

            instance = this;
        }
    }
}