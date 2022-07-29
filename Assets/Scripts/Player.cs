using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Broadcasting on")]
    [Tooltip("Fires a GameOver event on this channel")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;
    [Tooltip("Fires an event on this channel when scoring points")]
    [SerializeField] private FloatEventChannelSO _scoreIncreaseChannel = default;

    public ParticleSystem explosionPrefab;

    //private float gravity;
    private Rigidbody2D rb;
    //private Vector2 startPos;

    private bool dead;

    void Start()
    {
        //startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        //gravity = rb.gravityScale;
    }

    void Update()
    {
        RotatePlayerTowardsVelocity();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with trigger event");

        if (collision.gameObject.tag == "DeathObstacle")
        {
            Debug.Log("Raising game over event...");
            _gameOverChannel.RaiseEvent();
        }

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with collision event");

        switch (collision.gameObject.tag)
        {
            case "DeathObstacle":
                explosionPrefab.startColor = Color.red;
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                Debug.Log("Raising game over event...");
                _gameOverChannel.RaiseEvent();
                break;
            case "Destructible":
            {
                if (collision.gameObject.TryGetComponent<SpriteRenderer>(out var renderer))
                {
                    explosionPrefab.startColor = renderer.color;
                }
                Destroy(collision.gameObject);
                Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
                _scoreIncreaseChannel.RaiseEvent(1);
                break;
            }
        }
    }

    void RotatePlayerTowardsVelocity()
    {
        var playerVelocity = rb.velocity;

        var rotateRight = 10;
        if (playerVelocity.x < 0)
        {
            rotateRight = -10;
        }

        
        var angleDegrees = Mathf.Atan2(playerVelocity.y, rotateRight) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleDegrees));
    }
}
