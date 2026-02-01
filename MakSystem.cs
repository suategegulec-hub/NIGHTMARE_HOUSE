using UnityEngine;
using System.Collections;

public class MaskSystem : MonoBehaviour
{
    [Header("Kamera Referansı")]
    [SerializeField] private Camera mainCamera;

    [Header("Mod Durumu")]
    [SerializeField] private bool isInSpiritWorld = false;

    [Header("Layer Maskeleri")]
    private int realWorldMask;
    private int spiritWorldMask;

    [Header("Geçiş Efekti")]
    [SerializeField] private float transitionDuration = 0.5f;

    // Şu an çalışan Coroutine'i takip etmek için değişken
    private Coroutine currentTransition;

    private float defaultAmbientIntensity;
    private Color defaultAmbientColor;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        defaultAmbientIntensity = RenderSettings.ambientIntensity;
        defaultAmbientColor = RenderSettings.ambientLight;

        SetupLayerMasks();
        SwitchToRealWorld();
    }

    void Update()
    {
        // Q'ya BASILDIĞINDA
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Eğer hali hazırda bir geçiş (kapanma vs.) varsa onu DURDUR
            if (currentTransition != null) StopCoroutine(currentTransition);

            // Yeni geçişi başlat ve değişkene ata
            currentTransition = StartCoroutine(TransitionProcess(true));
        }

        // Q BIRAKILDIĞINDA
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // Eğer açılma işlemi hala sürüyorsa onu DURDUR
            if (currentTransition != null) StopCoroutine(currentTransition);

            // Kapanma işlemini başlat
            currentTransition = StartCoroutine(TransitionProcess(false));
        }
    }

    void SetupLayerMasks()
    {
        realWorldMask = LayerMask.GetMask("Default", "RealWorld", "UI", "SpiritWorld");
        spiritWorldMask = LayerMask.GetMask("Default", "RealWorld", "UI", "SpiritWorld");
    }

    IEnumerator TransitionProcess(bool toSpiritWorld)
    {
        // isTransitioning değişkenine artık gerek yok, çünkü StopCoroutine kullanıyoruz.

        yield return new WaitForSeconds(transitionDuration);

        if (toSpiritWorld)
            SwitchToSpiritWorld();
        else
            SwitchToRealWorld();
    }

    void SwitchToRealWorld()
    {
        // Maskeyi ayarla (Canavarı gizle)
        mainCamera.cullingMask = realWorldMask & ~LayerMask.GetMask("SpiritWorld");

        mainCamera.clearFlags = CameraClearFlags.Skybox;

        RenderSettings.fog = false;
        RenderSettings.ambientIntensity = defaultAmbientIntensity;
        RenderSettings.ambientLight = defaultAmbientColor;

        isInSpiritWorld = false;
    }

    void SwitchToSpiritWorld()
    {
        mainCamera.cullingMask = spiritWorldMask;
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = Color.black;

        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.5f;

        RenderSettings.ambientIntensity = 0.0f;
        RenderSettings.ambientLight = Color.black;

        isInSpiritWorld = true;
    }

    public bool IsInSpiritWorld()
    {
        return isInSpiritWorld;
    }
}