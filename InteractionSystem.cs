using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Settings")]
    public float etkilesimMesafesi = 3f;
    public LayerMask etkilesimKatmani;
    public TextMeshProUGUI bilgiYazisi;



    [Header("Audio (Sesler)")] // --- YENİ ---
    public AudioSource sesKaynagi; // Kamerana bir AudioSource eklemeyi unutma!
    public AudioClip anahtarSesi;
    public AudioClip kapiSesi;

    [Header("Inventory (Keys)")]
    public bool hasRedKey = false;
    public bool hasBlueKey = false;
    public bool hasGreenKey = false;
    public bool hasFinalKey = false;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (bilgiYazisi != null) bilgiYazisi.text = "";

        if (Physics.Raycast(ray, out hit, etkilesimMesafesi, etkilesimKatmani))
        {
            // ---------------- KEY PICKUP ----------------
            if (hit.collider.CompareTag("Key"))
            {
                string objeIsmi = hit.collider.gameObject.name;
                if (bilgiYazisi != null) bilgiYazisi.text = "[E] Pick up " + objeIsmi;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (objeIsmi.Contains("Red")) hasRedKey = true;
                    else if (objeIsmi.Contains("Blue")) hasBlueKey = true;
                    else if (objeIsmi.Contains("Green")) hasGreenKey = true;
                    else if (objeIsmi.Contains("Final")) hasFinalKey = true;

                    // --- SESİ ÇAL ---
                    if (sesKaynagi && anahtarSesi) sesKaynagi.PlayOneShot(anahtarSesi);

                    Destroy(hit.collider.gameObject);
                }
            }
            // ---------------- DOOR OPENING ----------------
            else if (hit.collider.CompareTag("Door"))
            {
                string kapiIsmi = hit.collider.gameObject.name;
                bool acabilirMiyim = false;

                if (kapiIsmi.Contains("Red") && hasRedKey) acabilirMiyim = true;
                else if (kapiIsmi.Contains("Blue") && hasBlueKey) acabilirMiyim = true;
                else if (kapiIsmi.Contains("Green") && hasGreenKey) acabilirMiyim = true;
                else if (kapiIsmi.Contains("Final") && hasFinalKey) acabilirMiyim = true;

                if (acabilirMiyim)
                {
                    if (bilgiYazisi != null) bilgiYazisi.text = "[E] Open Door";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // --- SESİ ÇAL ---
                        if (sesKaynagi && kapiSesi) sesKaynagi.PlayOneShot(kapiSesi);

                        Destroy(hit.collider.gameObject);
                    }
                }
                else
                {
                    if (bilgiYazisi != null) bilgiYazisi.text = "Locked (Need Key)";
                }
            }
        }
    }
}