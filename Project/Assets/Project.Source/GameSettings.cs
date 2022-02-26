using UnityEngine;

namespace Project.Source
{
    public class GameSettings : SingletonBehaviour<GameSettings>
    {
        // Used for checking if attacks have hit an enemy
        public LayerMask entityDamageLayerMask;

        // Used for checking if there is a nearby entity
        public LayerMask entityWorldLayerMask;
    }
}