using Assets.Scripts.Common.Collisions;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class TriggerCollisionHandlerBehaviour : MonoBehaviour
    {
        [SerializeField] private bool _onlyTriggerOnCollisionWithPlayer = false;
        [SerializeField] private UnityEvent<CollisionData> _onCollision = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_onlyTriggerOnCollisionWithPlayer && !other.gameObject.CompareTag("Player")){
                return;
            }

            _onCollision.Invoke(new CollisionData
            {
                CollidedWith = other.gameObject
            });
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_onlyTriggerOnCollisionWithPlayer && !other.gameObject.CompareTag("Player")){
                return;
            }

            _onCollision.Invoke(new CollisionData
            {
                CollidedWith = other.gameObject
            });
        }
    }
}
