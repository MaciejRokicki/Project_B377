using UnityEngine;

public class Chunk : MonoBehaviour
{
    private GameManager gameManager;

    public bool canGenerate = true;
    public Vector2 chunkPosition;

    private void Awake()
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.chunkPosition = this.gameObject.transform.position;
    }

    private void Start()
    {
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(gameManager.chunkSizeX, 1.0f);
        this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, -(gameManager.chunkSizeY / 2));

        GenerateBlocks();
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (canGenerate && coll.gameObject.name == "Player")
        {
            if (gameManager.chunkCount >= 3)
            {
                float deadLinePositionY = GameObject.Find($"Chunk_{gameManager.chunkCount - 3}").GetComponent<Chunk>().chunkPosition.y - 5.0f;
                gameManager.SpawnDeadLine(new Vector2(0.0f, deadLinePositionY));

                if (gameManager.chunkCount >= 4)
                {
                    Destroy(GameObject.Find($"Chunk_{gameManager.chunkCount - 4}"));
                }
            }

            gameManager.GenerateChunk(new Vector2(0.0f, this.chunkPosition.y + gameManager.chunkSizeY));
            canGenerate = false;
        }
    }

    private void GenerateBlocks()
    {
        float chanceSpawnBounceBlock = gameManager.chanceSpawnBounceBlock;
        float chanceSpawnAttractionBlockOut = chanceSpawnBounceBlock + gameManager.chanceSpawnAttractionBlockOut;
        float chanceSpawnAttractionBlockIn = chanceSpawnAttractionBlockOut + gameManager.chanceSpawnAttractionBlockIn;
        float chanceSpawnBoosterBlock = chanceSpawnAttractionBlockIn + gameManager.chanceSpawnBoosterBlock;
        float chanceSpawnScoreBlock = chanceSpawnBoosterBlock + gameManager.chanceSpawnScoreBlock;
        float chanceSpawnKillerBlock = chanceSpawnScoreBlock + gameManager.chanceSpawnKillerBlock;
        float chanceSpawnRainbowBlock = chanceSpawnKillerBlock + gameManager.chanceSpawnNeonBlock;

        for (int i = 0; i < gameManager.blocksPerChunk; i++)
        {
            BlockBase blockType = gameManager.blocks[0];

            Vector2 randomVector = new Vector2(Random.Range(chunkPosition.x - (gameManager.chunkSizeX / 2 - 5.0f), chunkPosition.x + (gameManager.chunkSizeX / 2 - 5.0f)),
                                               Random.Range(chunkPosition.y - (gameManager.chunkSizeY / 2), chunkPosition.y + (gameManager.chunkSizeY / 2))
                                               );

            RaycastHit2D hit = Physics2D.CircleCast(randomVector, 7.0f, Vector2.zero);

            while (hit.collider)
            {
                randomVector = new Vector2(Random.Range(chunkPosition.x - (gameManager.chunkSizeX / 2 - 5.0f), chunkPosition.x + (gameManager.chunkSizeX / 2 - 5.0f)),
                                               Random.Range(chunkPosition.y - (gameManager.chunkSizeY / 2), chunkPosition.y + (gameManager.chunkSizeY / 2))
                                               );

                hit = Physics2D.CircleCast(randomVector, 6.0f, Vector2.zero);
            }

            float rdn = Random.Range(0.0f, 100.0f);

            if (rdn <= chanceSpawnBounceBlock)
            {
                blockType = gameManager.blocks[1];
            }
            else if (rdn > chanceSpawnBounceBlock && rdn <= chanceSpawnAttractionBlockOut)
            {
                blockType = gameManager.blocks[2];
            }
            else if (rdn > chanceSpawnAttractionBlockOut && rdn <= chanceSpawnAttractionBlockIn)
            {
                blockType = gameManager.blocks[3];
            }
            else if (rdn > chanceSpawnAttractionBlockIn && rdn <= chanceSpawnBoosterBlock)
            {
                blockType = gameManager.blocks[4];
            }
            else if (rdn > chanceSpawnBoosterBlock && rdn <= chanceSpawnScoreBlock)
            {
                blockType = gameManager.blocks[5];
            }
            else if (rdn > chanceSpawnScoreBlock && rdn <= chanceSpawnKillerBlock)
            {
                blockType = gameManager.blocks[6];
            }
            else if(rdn > chanceSpawnKillerBlock && rdn <= chanceSpawnRainbowBlock)
            {
                blockType = gameManager.blocks[7];
            }

            BlockBase instBlock = Instantiate(blockType, randomVector, Quaternion.identity);
            instBlock.name = $"{blockType.name}({i})";
            instBlock.transform.parent = this.gameObject.transform;
        }
    }
}
