using UnityEngine;
using System.Collections;

public class ProximityAudio : MonoBehaviour
{
    [Header("Ses Klipleri")]
    [SerializeField] private AudioClip yakinSes;      // Kalp atışı / Çok yüksek gerilim
    [SerializeField] private AudioClip ortaSes;       // Yakınlaşma sesi
    [SerializeField] private AudioClip uzakSes;       // Rüzgar / Ambiyans

    [Header("Referanslar")]
    [SerializeField] private Transform oyuncu;
    [SerializeField] private Transform canavar;
    [SerializeField] private MaskSystem maskSystem;   // MASKE SİSTEMİNİ BURAYA BAĞLA

    [Header("Mesafe Ayarları")]
    [SerializeField] private float yakinMesafe = 5f;
    [SerializeField] private float ortaMesafe = 15f;

    private AudioSource audioSource;
    private AudioClip hedefClip;
    private bool isFading = false; // Fade işlemi sırasında Update'in karışmasını engeller

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Müziği 2D yapıyoruz ki kafanın içinde çalsın (Korku için daha iyidir)
        audioSource.volume = 0.5f;
    }

    void Update()
    {
        if (oyuncu == null || canavar == null) return;

        float gercekMesafe = Vector3.Distance(oyuncu.position, canavar.position);

        // --- MASKE MANTIĞI ---
        // Eğer Spirit (TROS) modundaysak, canavar bizi daha zor duyar.
        // Bu yüzden mesafeyi olduğundan "daha uzakmış" gibi hesaplatıyoruz.
        float algilananMesafe = gercekMesafe;

        if (maskSystem != null && maskSystem.IsInSpiritWorld()) // Bu fonksiyonu aşağıda MaskSystem'e ekleyeceğiz
        {
            algilananMesafe += 5.0f; // Maske takılıyken +5 metre bonus kazan (Güvenli alan artar)
        }
        // ---------------------

        // Hangi ses çalmalı?
        DetermineAudioClip(algilananMesafe);

        // Fade işlemi yapmıyorsak ses şiddetini mesafeye göre ayarla
        if (!isFading)
        {
            AdjustVolume(algilananMesafe);
        }
    }

    void DetermineAudioClip(float mesafe)
    {
        AudioClip yeniClip = uzakSes; // Varsayılan

        if (mesafe < yakinMesafe)
        {
            yeniClip = yakinSes;
        }
        else if (mesafe < ortaMesafe)
        {
            yeniClip = ortaSes;
        }

        // Eğer çalması gereken ses şu an çalan sesten farklıysa değiştir
        if (yeniClip != audioSource.clip && !isFading)
        {
            StartCoroutine(FadeTrack(yeniClip));
        }
        // Eğer hiç ses çalmıyorsa başlat
        else if (!audioSource.isPlaying)
        {
            audioSource.clip = yeniClip;
            audioSource.Play();
        }
    }

    void AdjustVolume(float mesafe)
    {
        // Canavar çok yakınsa ses %100, uzaksa %30
        // Mathf.InverseLerp kullanarak 0 ile 1 arasında yumuşak geçiş yapıyoruz
        float volumeFactor = 1f - Mathf.Clamp01(mesafe / (ortaMesafe + 5f));
        // En az 0.2 ses olsun, en fazla 1.0
        float targetVolume = Mathf.Clamp(volumeFactor, 0.2f, 1.0f);

        // Sesi yumuşakça hedefe çek (Anlık patlamaları önler)
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * 2f);
    }

    IEnumerator FadeTrack(AudioClip newClip)
    {
        isFading = true;
        float startVolume = audioSource.volume;

        // Sesi kıs (Fade Out)
        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / 0.5f);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Sesi aç (Fade In) - Ama Update'teki hedef volume'a kadar
        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            // O anki mesafeye göre olması gereken ses seviyesini tahmin et
            float targetVol = 0.5f;
            audioSource.volume = Mathf.Lerp(0f, targetVol, t / 0.5f);
            yield return null;
        }

        isFading = false;
    }
}