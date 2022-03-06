using Project.Source.Utilities.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Source
{
    public class GameSettings : SingletonBehaviour<GameSettings>
    {
        public LayerMask ProjectileBlockingLayerMask;
        
        public LayerMask NonWalkableLayerMask;
        
        // Used for checking if attacks have hit an enemy
        [FormerlySerializedAs("entityDamageLayerMask")]
        public LayerMask EntityDamageLayerMask;

        // Used for checking if there is a nearby entity
        [FormerlySerializedAs("entityWorldLayerMask")]
        public LayerMask EntityWorldLayerMask;

        public bool IsHardmode;
    }
}