using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    public Vector2 currentDirection;
    private float velocity = 40f;
    public GameObject damageIndicator;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Fly();
    }

    private void Fly()
    {
        if (currentDirection == Vector2.zero) return;
        rigidbody2D.MovePosition(rigidbody2D.position + currentDirection * velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Border":
                break;

            case "Enemy":
                Instantiate(damageIndicator, transform.position, Quaternion.identity);
                other.GetComponent<Enemy>().TakeDamage(1);
                break;

            case "Player":
                Instantiate(damageIndicator, transform.position, Quaternion.identity);
                other.GetComponent<Player>().TakeDamage(1);
                break;

            default:
                return;
        }

        Destroy(gameObject);
    }
}
