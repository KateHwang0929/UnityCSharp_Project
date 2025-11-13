using UnityEngine;

public class Obstacle2D : MonoBehaviour
{
     [SerializeField] private Transform player;
    private GameManager gameManager;
    private bool outcomeResolved;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (player == null)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (outcomeResolved || player == null || gameManager == null || gameManager.IsGameOver)
        {
            return;
        }

        if (transform.position.y < player.position.y)
        {
            outcomeResolved = true;
            gameManager.Win();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
         TryLose(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TryLose(other.gameObject);
    }

    void TryLose(GameObject target)
    {
        if (!target.CompareTag("Player"))
        {
            return;
        }

       if (gameManager != null && !gameManager.IsGameOver)
        {
            outcomeResolved = true;
            gameManager.Lose();
        }
    }   
}
