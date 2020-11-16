using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Startup startup;

    private GameManager gameManager;
    private UserInterfaceManager userInterfaceManager;
    private CameraController cameraController;
    internal Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private TrailRenderer trailRenderer;
    private Animation anim;
    private ParticleSystem particle;
    public float playerMovementSpeedMultiplier = 5.0f;
    public float minArrowDistance = 3.0f;
    public float maxArrowDistance = 12.0f;
    public float slowMotionScale = .1f;
    internal bool canMove = false;
    internal bool scoreCombo = false;

    public Gradient belowMinMovementPlayerSpeedGradient, defaultMoveGradient, moreThanMaxMovementPlayerSpeedGradient;

    private Vector2 startMousePosition, dirMousePosition, endMousePosition;
    private bool isMousePressed = false;
    private bool canDrawArrow = false;

    private void Awake()
    {
        this.startup = GameObject.Find("Startup").GetComponent<Startup>();
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.userInterfaceManager = GameObject.Find("UserInterfaceManager").GetComponent<UserInterfaceManager>();
        this.cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

        this.rb = this.gameObject.GetComponent<Rigidbody2D>();
        this.lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        this.trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
        this.anim = this.gameObject.GetComponent<Animation>();
        this.particle = this.gameObject.GetComponent<ParticleSystem>();

        this.GetComponent<SpriteRenderer>().sprite = startup.currentPlayerSprite.sprite;
        this.GetComponent<SpriteRenderer>().material = startup.currentPlayerSprite.material;

        this.GetComponent<TrailRenderer>().material = startup.currentPlayerTrail.material;
        this.GetComponent<TrailRenderer>().textureMode = startup.currentPlayerTrail.TextureMode;
    }

    private void FixedUpdate()
    {
        if (this.rb.bodyType == RigidbodyType2D.Dynamic)
            this.rb.velocity = Vector2.ClampMagnitude(this.rb.velocity, maxArrowDistance * playerMovementSpeedMultiplier * 1.5f);

        if(isMousePressed)
        {
            Vector3 playerPosition = this.gameObject.transform.position;

            float arrowDistance = Vector2.Distance(playerPosition, playerPosition + ((Vector3)dirMousePosition) / 35);

            if (arrowDistance >= minArrowDistance)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                canMove = false;

                if (arrowDistance < maxArrowDistance)
                {
                    rb.velocity = dirMousePosition.normalized * arrowDistance * playerMovementSpeedMultiplier;
                }
                else
                {
                    rb.velocity = dirMousePosition.normalized * maxArrowDistance * playerMovementSpeedMultiplier;
                }

                scoreCombo = false;
                gameManager.SetDefaultMultiplier();
            }
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isMousePressed = false;
        }

        rb.rotation += Time.timeScale * rb.velocity.magnitude / 5;
    }

    private void Update()
    {
        if(canMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                startMousePosition.x *= Screen.width;
                startMousePosition.y *= Screen.height;

                Time.timeScale = slowMotionScale;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                canDrawArrow = true;
            }

            if(canDrawArrow)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 playerPosition = this.gameObject.transform.position;

                    endMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    endMousePosition.x *= Screen.width;
                    endMousePosition.y *= Screen.height;

                    dirMousePosition = endMousePosition - startMousePosition;

                    DrawArrow(playerPosition, playerPosition + ((Vector3)dirMousePosition) / 35);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    this.lineRenderer.enabled = false;

                    isMousePressed = true;
                    canDrawArrow = false;
                    cameraController.defaultZoom = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        canMove = true;
        if (coll.gameObject.tag == "Wall" && rb.velocity.y < .8f)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void DrawArrow(Vector2 startPosition, Vector3 endMousePosition)
    {
        float distance = Vector2.Distance(startPosition, endMousePosition);

        if (distance <= minArrowDistance)
            this.lineRenderer.colorGradient = belowMinMovementPlayerSpeedGradient;
        else if (distance > minArrowDistance && distance < maxArrowDistance)
            this.lineRenderer.colorGradient = defaultMoveGradient;
        else if (distance >= maxArrowDistance)
            this.lineRenderer.colorGradient = moreThanMaxMovementPlayerSpeedGradient;

        Vector3[] positions = new Vector3[] { new Vector3(startPosition.x, startPosition.y, -1.0f), new Vector3(endMousePosition.x, endMousePosition.y, -1.0f) };
        this.lineRenderer.enabled = true;
        this.lineRenderer.SetPositions(positions);

        cameraController.Zoom(rbVelocity: distance + cameraController.minZoomOut);

    }

    public void GameOver()
    {
        canMove = false;
        this.lineRenderer.enabled = false;
        cameraController.DefaultZoom();

        Time.timeScale = 0.3f;

        this.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        this.trailRenderer.emitting = false;

        this.particle.Play();
        this.anim.Play("PlayerDestroy");

        Destroy(this.gameObject, particle.main.duration + particle.main.startLifetimeMultiplier);

        PlayerPrefsManager.SetHighScore(gameManager.score);
        startup.IncreaseNeonBlocks(gameManager.neonBlocks);

        userInterfaceManager.GameInterfaceDisable();
        userInterfaceManager.GameOverInterfaceEnable(gameManager.score, gameManager.brokenBlocks);
    }
}