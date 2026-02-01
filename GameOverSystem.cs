using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // İçime giren şeyin etiketi "Player" mı?
        if (other.CompareTag("Player"))
        {
            Debug.Log("YAKALANDIN! (KillZone)");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("LoseScene");
        }
    }
}