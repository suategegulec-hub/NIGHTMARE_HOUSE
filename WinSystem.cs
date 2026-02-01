using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // İçimden geçen şey Oyuncu mu?
        if (other.CompareTag("Player"))
        {
            Debug.Log("KAÇTIN!");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // "WinScene" sahnesini aç
            SceneManager.LoadScene("WinScene");
        }
    }
}