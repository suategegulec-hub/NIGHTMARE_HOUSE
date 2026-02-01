using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // OYNA butonuna basınca çalışacak
    public void StartGame()
    {
        // Build Settings'deki sıraya göre 1. sahneyi (Oyun Sahnesi) açar
        // Eğer oyun sahnen 1. sırada değilse parantez içine sahne adını yaz: LoadScene("Game");
        SceneManager.LoadScene(1);
    }
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(2); // Direkt oyun sahnesini (2. sıradakini) açar
    }
    // TEKRAR DENE (TRY AGAIN) butonuna basınca çalışacak
    public void RestartGame()
    {
        // HATA BURADAYDI: "GetActiveScene" yerine direkt 1 yazıyoruz.
        // Çünkü Build Settings'de oyun sahnen 1. sırada.
        SceneManager.LoadScene(2);
    }

    // MENÜYE DÖN butonuna basınca çalışacak
    public void GoToMenu()
    {
        SceneManager.LoadScene(0); // Genelde Menü 0. sıradadır
    }

    // ÇIKIŞ butonuna basınca çalışacak
    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkıldı!"); // Editörde kapanmaz, sadece konsola yazar
        Application.Quit();
    }
}