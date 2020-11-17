using UnityEngine;

public class BlockBase : MonoBehaviour
{
    internal GameManager gameManager;
    internal Animation anim;
    internal PlayerController playerController;
    internal Rigidbody2D playerRigidbody;

    public int points;

    public virtual void Awake()
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.anim = this.gameObject.GetComponent<Animation>();
        this.playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        this.playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            anim.Play("BlockSizing");

            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

            gameManager.brokenBlocks++;
            gameManager.IncreaseScore(this.gameObject.transform.position, points);

            playerController.scoreCombo = true;

            if (playerController.scoreCombo)
                gameManager.IncreaseScoreMultiplier(1);

            Destroy(this.gameObject, anim.GetClip("BlockSizing").length);
        }
    }
}
