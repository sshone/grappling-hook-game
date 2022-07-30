using Assets.Scripts.Behavioural;
using Assets.Scripts.Common.Collisions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Broadcasting on")]
    [Tooltip("Fires a GameOver event on this channel")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;
    [Tooltip("Fires an event on this channel when scoring points")]
    [SerializeField] private FloatEventChannelSO _scoreIncreaseChannel = default;

    private Rigidbody2D rb;

    /// <summary>
    /// Handles a collision occurring, occurs for both Trigger and non Trigger collisions
    /// </summary>
    /// <param name="collision"></param>
    public void HandleCollision(CollisionData collision)
    {
        var collisionObject = collision.CollidedWith.gameObject;

        switch (collisionObject.tag)
        {
            case "DeathObstacle":
                _gameOverChannel.RaiseEvent();
                break;
            case "Destructible":
            {
                if (collisionObject.TryGetComponent<EventsToInvokeOnDestroyBehaviour>(out var destroyEffect))
                {
                    destroyEffect.DoDeathEffects();
                }

                ScreenShakeBehaviour.instance.StartShake(0.5f, 0.5f);

                break;
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RotatePlayerTowardsVelocity();
    }

    void RotatePlayerTowardsVelocity()
    {
        var playerVelocity = rb.velocity;
        var rotationSpeed = 10;

        if (playerVelocity.x < 0)
        {
            rotationSpeed *= -1;
        }
        
        var angleDegrees = Mathf.Atan2(playerVelocity.y, rotationSpeed) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleDegrees));
    }
}
