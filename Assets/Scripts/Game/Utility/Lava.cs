using UnityEngine;

public class Lava : MonoBehaviour
{

    private GameManager gameManager;
    private GameObject player;
    public bool isWalls = false;

    private GameObject bottomWall;

    private string objName;

    private void Awake()
    {
        bottomWall = GameObject.Find("BottomWall");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        this.objName = this.gameObject.name;
    }

    private void Start()
    {
        float scaleBotX = gameManager.chunkSizeX / 2 - bottomWall.transform.localScale.x / 2;
        float posBotX = scaleBotX / 2 + bottomWall.transform.localScale.x / 2;

        switch (this.objName)
        {
            case "BotLeft":
                this.gameObject.transform.localScale = new Vector3(gameManager.chunkSizeX / 2 - bottomWall.transform.localScale.x / 2, 20.0f, 1.0f);
                this.gameObject.transform.position = new Vector3(-(this.gameObject.transform.localScale.x / 2 + bottomWall.transform.localScale.x / 2), 0.0f, 0.0f);
                break;

            case "BotRight":
                this.gameObject.transform.localScale = new Vector3(gameManager.chunkSizeX / 2 - bottomWall.transform.localScale.x / 2, 20.0f, 1.0f);
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.localScale.x / 2 + bottomWall.transform.localScale.x / 2, 0.0f, 0.0f);
                break;

            case "Left":
                this.gameObject.transform.localScale = new Vector3(40.0f, gameManager.chunkSizeY * 2, 1.0f);
                this.gameObject.transform.position = new Vector3(-2 * (posBotX), 0.0f, 0.0f);
                break;

            case "Right":
                this.gameObject.transform.localScale = new Vector3(40.0f, gameManager.chunkSizeY * 2, 1.0f);
                this.gameObject.transform.position = new Vector3(2 * (posBotX), 0.0f, 0.0f);
                break;
        }
    }

    private void Update()
    {
        if (isWalls && player)
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, player.transform.position.y);
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject == player)
        {
            player.GetComponent<PlayerController>().GameOver();
        }
    }
}
