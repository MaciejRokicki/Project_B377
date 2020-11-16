using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;

    private Camera cam;
    private Vector2 cameraPositionVelocity;
    public float smoothTimeX, smoothTimeY;
    public bool isMinPositionY;
    public float minCameraPosY;
    internal bool defaultZoom;
    private float cameraZoomSizingVelocity;
    public float minZoomOut, maxZoomOut, smoothZoomOut, smoothZoomIn;

    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private float chromaticAberrationIntensityVelocity;
    private ColorAdjustments colorAdjustments;
    private float colorAdjustmentsSaturationVelocity;
    public float defaultChromaticAberration, targetValueChromaticAberration, defaultSaturationValue, targetSaturationValue;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        cam = this.gameObject.GetComponent<Camera>();
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        if (player)
        {
            float posX = Mathf.SmoothDamp(this.transform.position.x, player.transform.position.x, ref cameraPositionVelocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(this.transform.position.y, player.transform.position.y, ref cameraPositionVelocity.y, smoothTimeY);

            this.transform.position = new Vector3(posX, posY, this.transform.position.z);

            if (isMinPositionY)
            {
                this.transform.position = new Vector3(Mathf.Clamp(this.gameObject.transform.position.x, -(gameManager.chunkSizeX / 2 - 7.0f), gameManager.chunkSizeX / 2 - 7.0f),
                                                      Mathf.Clamp(this.transform.position.y, minCameraPosY, this.gameObject.transform.position.y + 100.0f),
                                                      this.gameObject.transform.position.z);
            }

            if (defaultZoom)
            {
                DefaultZoom();
            }
        }
    }

    public void Zoom(float rbVelocity)
    {
        float targetSize = Mathf.Clamp(rbVelocity, minZoomOut, maxZoomOut);
        float size = Mathf.SmoothDamp(cam.orthographicSize, targetSize, ref cameraZoomSizingVelocity, smoothZoomOut);
        chromaticAberration.intensity.value = Mathf.SmoothDamp(chromaticAberration.intensity.value, targetValueChromaticAberration, ref chromaticAberrationIntensityVelocity, smoothZoomOut);
        colorAdjustments.saturation.value = Mathf.SmoothDamp(colorAdjustments.saturation.value, targetSaturationValue, ref colorAdjustmentsSaturationVelocity, smoothZoomOut);
        cam.orthographicSize = size;
    }

    public void DefaultZoom()
    {
        float size = Mathf.SmoothDamp(cam.orthographicSize, minZoomOut, ref cameraZoomSizingVelocity, smoothZoomIn);
        chromaticAberration.intensity.value = Mathf.SmoothDamp(chromaticAberration.intensity.value, defaultChromaticAberration, ref chromaticAberrationIntensityVelocity, smoothZoomIn);
        colorAdjustments.saturation.value = Mathf.SmoothDamp(colorAdjustments.saturation.value, defaultSaturationValue, ref colorAdjustmentsSaturationVelocity, smoothZoomIn);
        cam.orthographicSize = size;

        if (cam.orthographicSize == minZoomOut && colorAdjustments.saturation.value == defaultSaturationValue && chromaticAberration.intensity.value == defaultChromaticAberration)
            defaultZoom = false;
    }
}
