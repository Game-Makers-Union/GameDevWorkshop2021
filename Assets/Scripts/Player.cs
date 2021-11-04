using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Vector2 currentDirection;
    private float velocity = 10f;
    private Camera mainCamera;

    public Transform shadow;

    public Transform gunPoint;
    public Transform indicator;
    public Bullet bullet;

    private static Vector2 minPosition = new Vector2(-21f, -14f);
    private static Vector2 maxPosition = new Vector2(21f, 14f);

    public Trail trailPrefab;
    private Trail trail;

    private int health;
    private const int MaxHealth = 5;
    public Text healthText;
    private int ammo;
    private const int MaxAmmo = 20;
    public Text ammoText;
    public int score;
    public Text scoreText;

    private CameraShake cameraShake;
    private GameController gameController;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        cameraShake = mainCamera.GetComponent<CameraShake>();
        gameController = FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        trail = Instantiate(trailPrefab, transform.position, Quaternion.identity).GetComponent<Trail>();
        trail.target = transform;

        health = MaxHealth;
        ammo = MaxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        currentDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        shadow.position = (Vector2)transform.position + new Vector2(-1f, -1.5f);
        indicator.position = (Vector2)transform.position + new Vector2(0f, 1.5f);
        indicator.rotation = Quaternion.identity;

        healthText.text = "Health: " + health.ToString();
        ammoText.text = "Ammo: " + ammo.ToString();
        scoreText.text = "Score: " + score.ToString();

        LookAtMouse();

        if (Input.GetButtonDown("Fire1")) Shoot();
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

    private void LookAtMouse()
    {
        if (gameController.isPaused) return;

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = (mousePosition - (Vector2)transform.position).normalized;

        transform.up = Vector2.Lerp(transform.up, lookDirection, 0.2f);
    }

    private void Shoot()
    {
        if (ammo <= 0 || gameController.isPaused) return;

        Instantiate(bullet, gunPoint.position, transform.rotation).GetComponent<Bullet>().currentDirection = transform.up;
        ammo--;

        cameraShake.Shake(0.05f, 0.1f);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        cameraShake.Shake();

        if (health <= 0) Die();
    }

    public void Die()
    {
        gameController.GameOver();
        Destroy(trail);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoCollectable"))
        {
            ammo += 5;
            if (ammo > MaxAmmo) ammo = MaxAmmo;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HealthCollectable"))
        {
            health++;
            if (health > MaxHealth) health = MaxHealth;
            Destroy(other.gameObject);
        }
    }
}
