using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Broadcasting on")]
    [Tooltip("Fires a GameOver event on this channel")]
    [SerializeField] private VoidEventChannelSO _gameOverChannel = default;
    

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

        if (collision.gameObject.tag == "DeathObstacle")
        {
            Debug.Log("Raising game over event...");
            _gameOverChannel.RaiseEvent();
        }
    }
}
