using UnityEngine;

public class BlockBase : MonoBehaviour
{
    internal GameManager gameManager;
    internal ParticleSystem particle;
    internal Animation anim;
    internal PlayerController playerController;
    internal Rigidbody2D playerRigidbody;

    public int points;

    public virtual void Awake()
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.particle = this.gameObject.GetComponent<ParticleSystem>();
        this.anim = this.gameObject.GetComponent<Animation>();
        this.playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        this.playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (this.gameObject.GetComponent<ParticleSystem>())
            {
                this.particle.Play();

                var ps = this.particle.main;
                float totalDuration = (ps.duration + ps.startLifetimeMultiplier) * .8f;

                anim.Play("BlockSizing");

                this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

                gameManager.brokenBlocks++;
                gameManager.IncreaseScore(this.gameObject.transform.position, points);

                playerController.scoreCombo = true;

                if (playerController.scoreCombo)
                    gameManager.IncreaseScoreMultiplier(1);

                Destroy(this.gameObject, totalDuration);
            }
        }
    }
}
