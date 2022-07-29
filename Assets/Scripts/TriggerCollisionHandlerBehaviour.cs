using Assets.Scripts.Common.Collisions;
using UnityEngine;

namespace Assets.Scripts
{
    public class TriggerCollisionHandlerBehaviour : MonoBehaviour
    {
        [SerializeField] private CollisionEvent onCollision = new();

        private void OnTriggerEnter(Collider other)
        {
            onCollision.Invoke(new CollisionData
            {
                CollidedWith = other.gameObject
            });
        }
    }
}
