using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private Startup startup;

    public Animation hidePanelAnim;
    //PLAYER
    public GameObject player;
    internal PlayerController playerController;
    public UserInterfaceManager userInterfaceManager;

    //SCORE
    private readonly int defaultScoreMultiplier;
    internal int neonBlocks = 0, score = 0, scoreMultiplier = 1;
    internal int brokenBlocks = 0;

    //CHUNK GENERATOR
    public float baseY = 35.0f;
    public GameObject chunk;

    public float chanceSpawnBounceBlock, chanceSpawnAttractionBlockOut, chanceSpawnAttractionBlockIn,
                 chanceSpawnBoosterBlock, chanceSpawnScoreBlock, chanceSpawnKillerBlock,
                 chanceSpawnNeonBlock;
    internal int chunkCount = 0;
    public float chunkSizeX, chunkSizeY;
    public List<BlockBase> blocks = new List<BlockBase>();
    public int blocksPerChunk;

    public GameObject deadLinePrefab;
    public GameObject currentDeadLine;

    private GameManager()
    {
        defaultScoreMultiplier = 1;
    }

    private void Awake()
    {
        startup = GameObject.Find("Startup").GetComponent<Startup>();

        Instantiate(player, new Vector2(0.0f, 15.0f), Quaternion.identity).name = "Player";
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Time.timeScale = 1.0f;
    }

    private IEnumerator Start()
    {
        GenerateChunk(new Vector2(0.0f, chunkSizeY / 2 + baseY));

        GameObject playerObj = GameObject.Find("Player");

        playerObj.GetComponent<Animation>().Play("PlayerSpawn");
        yield return new WaitForSeconds(playerObj.GetComponent<Animation>().GetClip("PlayerSpawn").length);
        playerObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        playerObj.GetComponent<PlayerController>().canMove = true;
    }

    public void GenerateChunk(Vector2 position)
    {
        GameObject newChunk = Instantiate(chunk, position, Quaternion.identity);
        newChunk.name = $"Chunk_{chunkCount}";
        newChunk.transform.parent = GameObject.Find("Chunks").transform;
        chunkCount++;
    }

    public void IncreaseNeonBlocks(Vector3 pos, int neonBlocks)
    {
        int rb;

        rb = neonBlocks * scoreMultiplier;

        if (rb > 0)
        {
            this.neonBlocks += rb;
            userInterfaceManager.spawnAddNeonBlockGameObject(pos, rb);
        }
    }
    public void IncreaseScore(Vector3 pos, int points)
    {
        int pkt;

        pkt = points * scoreMultiplier;

        if (pkt > 0)
        {
            score += pkt;
            userInterfaceManager.spawnAddScoreGameObject(pos, pkt);
        }

    }

    public void IncreaseScoreMultiplier(int multiplier) => scoreMultiplier += multiplier;

    public void SetDefaultMultiplier() => scoreMultiplier = defaultScoreMultiplier;

    public void SpawnDeadLine(Vector2 position)
    {
        if (currentDeadLine)
            Destroy(currentDeadLine);

        currentDeadLine = Instantiate(deadLinePrefab, position, Quaternion.identity);
        currentDeadLine.name = "DeadLine";
        currentDeadLine.transform.localScale = new Vector3(chunkSizeX, 1.0f, 1.0f);
        currentDeadLine.transform.SetParent(GameObject.Find("Walls").transform);
    }
}
