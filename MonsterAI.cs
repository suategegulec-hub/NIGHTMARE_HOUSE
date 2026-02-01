using UnityEngine;
using UnityEngine.AI; // NavMesh için bu kütüphane ŞART

public class MonsterAI : MonoBehaviour
{
    public Transform player; // Oyuncuyu (Target) buraya sürükleyeceğiz
    private NavMeshAgent agent;

    void Start()
    {
        // Canavarın üzerindeki NavMeshAgent bileşenini otomatik bul
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Eğer oyuncu varsa sürekli onu takip et
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}
