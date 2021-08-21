using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer shadow;
    public Sprite[] sprites;

    public Transform gunPoint;
    public Bullet bullet;

    private new Rigidbody2D rigidbody2D;
    private static Vector2 velocityRange = new Vector2(3f, 6f);
    private float velocity;

    private static Vector2 shootIntervalRange = new Vector2(1f, 2f);

    private Player player;

    private int health;
    private static Vector2Int HealthRange = new Vector2Int(2, 5);

    private CameraShake cameraShake;

    private static Vector2 minPosition = new Vector2(-20f, -13f);
    private static Vector2 maxPosition = new Vector2(20f, 13f);
    private Vector2 targetPosition;

    public GameObject ammoCollectablePrefab;
    public GameObject healthCollectablePrefab;
    private static Vector2Int ammoSpawnRange = new Vector2Int(1, 3);
    private static Vector2Int healthSpawnRange = new Vector2Int(1, 3);

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        player = FindObjectOfType<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Generate();
        Randomize();
        Invoke("Shoot", Random.Range(shootIntervalRange.x, shootIntervalRange.y));
    }

    // Update is called once per frame
    void Update()
    {
        shadow.transform.position = (Vector2)transform.position + new Vector2(-1f, -1.5f);
        LookAtPlayer();
    }

    void FixedUpdate()
    {
        Fly();
    }

    private void Generate()
    {
        Sprite sprite = sprites[Random.Range(0, sprites.Length)];
        spriteRenderer.sprite = shadow.sprite = sprite;

        health = Random.Range(HealthRange.x, HealthRange.y);
    }

    private void Randomize()
    {
        targetPosition = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));
        velocity = Random.Range(velocityRange.x, velocityRange.y);
    }

    private void Fly()
    {
        var direction = (targetPosition - (Vector2)transform.position).normalized;
        rigidbody2D.MovePosition(rigidbody2D.position + direction * velocity * Time.fixedDeltaTime);

        if ((targetPosition - (Vector2)transform.position).magnitude < 0.1f) Randomize();
    }

    private void LookAtPlayer()
    {
        if (!player) return;

        Vector2 lookDirection = (player.transform.position - transform.position).normalized;
        transform.up = Vector2.Lerp(transform.up, lookDirection, 0.2f);
    }

    private void Shoot()
    {
        if (!player) return;
        float interval = Random.Range(shootIntervalRange.x, shootIntervalRange.y);

        Instantiate(bullet, gunPoint.position, transform.rotation).GetComponent<Bullet>().currentDirection = transform.up;

        Invoke("Shoot", interval);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        cameraShake.Shake();

        if (health <= 0) Die();
    }

    public void Die()
    {
        player.score++;
        for (int i = 0; i < Random.Range(ammoSpawnRange.x, ammoSpawnRange.y); i++)
            Instantiate(ammoCollectablePrefab, (Vector2)transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
        for (int i = 0; i < Random.Range(healthSpawnRange.x, healthSpawnRange.y); i++)
            Instantiate(healthCollectablePrefab, (Vector2)transform.position + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.identity);
            
        Destroy(gameObject);
    }
}
