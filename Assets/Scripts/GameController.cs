using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool isPaused;
    public bool isGameOver;
    public PostProcessProfile profile;
    private DepthOfField depthOfField;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public Enemy enemyPrefab;
    private static Vector2 enemySpawnIntervalRange = new Vector2(1.5f, 3f);
    private static Vector2 enemySpawnMinPosition = new Vector2(-21f, -14f);
    private static Vector2 enemySpawnMaxPosition = new Vector2(21f, 14f);

    // Start is called before the first frame update
    void Start()
    {
        profile.TryGetSettings(out depthOfField);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        depthOfField.enabled.value = false;

        Invoke("SpawnEnemy", Random.Range(enemySpawnIntervalRange.x, enemySpawnIntervalRange.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && !isGameOver) Pause();
            else if (isPaused && !isGameOver) Resume();
        }
    }

    private void SpawnEnemy()
    {
        float interval = Random.Range(enemySpawnIntervalRange.x, enemySpawnIntervalRange.y);

        Instantiate(enemyPrefab, new Vector2(Random.Range(enemySpawnMinPosition.x, enemySpawnMaxPosition.x), 
                                             Random.Range(enemySpawnMinPosition.y, enemySpawnMaxPosition.y)), transform.rotation);

        Invoke("SpawnEnemy", interval);
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        pauseMenu.SetActive(true);
        depthOfField.enabled.value = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        pauseMenu.SetActive(false);
        depthOfField.enabled.value = false;
    }

    public void GameOver()
    {
        isGameOver = true;

        gameOverMenu.SetActive(true);
        depthOfField.enabled.value = true;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
