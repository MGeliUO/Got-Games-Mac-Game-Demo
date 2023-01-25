using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton instance reference
    public static GameManager instance;

    private void Awake()
    {
        // Setting the singleton variable
        instance = this;
    }

    // Speed at which obstacles scroll across the screen
    public float scrollSpeed = 12f;
    public float scrollSpeedIncrease = 0.015f;
    public float scrollSpeedMultiplier = 1;

    // Lists to hold objects that will be scrolled across the screen
    private List<GameObject> scrollingObjects;

    // List to hold objects to be removed from the preceding list
    private List<GameObject> toDestroy;

    // A reference to the tumbleweed obstacle prefab
    public GameObject tumbleweedPrefab;

    // The minimum and maximum delays between obstacle spawning
    public float minSpawnDelay = 2, maxSpawnDelay = 3;

    // Scrolling objects will be destroyed when they pass this X value offscreen
    public float destructionThreshold = -50;

    private bool isGameOver;

    private float score;
    private float scoreIncreaseRate = 10;

    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing lists
        scrollingObjects = new List<GameObject>();
        toDestroy = new List<GameObject>();

        // Starting the obstacle spawning coroutine
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {

        while (true)
        {
            // wait for a random amount of time between minSpawnDelay and maxSpawnDelay
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            // Create a clone of the tumbleweed prefab and add it to the list of scrolling objects
            GameObject t = Instantiate(tumbleweedPrefab);
            scrollingObjects.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isGameOver) return;

        // Loop through all the scrolling objects
        foreach (GameObject g in scrollingObjects)
        {
            // Scroll the object to the left
            g.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime * scrollSpeedMultiplier);

            // If the object passes the destruction threshold, add it to the destruction list
            if (g.transform.position.x < destructionThreshold)
            {
                toDestroy.Add(g);
            }
        }

        // If there are any objects that need to be destroyed, destroy them and
        // remove them from the scrolling objects list
        if (toDestroy.Count > 0)
        {
            foreach (GameObject g in toDestroy)
            {
                Destroy(g);
                scrollingObjects.Remove(g);
            }

            toDestroy = new List<GameObject>();
        }

        // increase the tumbleweed speed overtime
        scrollSpeedMultiplier += Time.deltaTime * scrollSpeedIncrease;

        score += scoreIncreaseRate * Time.deltaTime * scrollSpeedMultiplier;
        scoreText.text = "Score: " + ((int)score).ToString();
    }

    public void OnGameOver()
    {
        // Stop scrolling on game over
        scrollSpeedMultiplier = 0;

        isGameOver = true;
    }
}
