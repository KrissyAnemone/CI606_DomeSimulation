using Unity.VisualScripting;
using UnityEngine;

public class ZF_NPC_Spawner : MonoBehaviour
{
    [SerializeField] GameObject NPC_Prefab;
    [SerializeField] GameObject[] Goals;
    [SerializeField] BoxCollider SpawnVolume;
    [SerializeField] int NumNPCs = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Random.InitState((int)System.DateTime.Now.Ticks);
        if (!SpawnVolume) SpawnVolume = GetComponent<BoxCollider>();
        if (NPC_Prefab)
        {
            for (int i = 0; i < NumNPCs; i++)
            {
                SpawnNPC();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetSpawnPosition()
    {
        return new Vector3(
        Random.Range(SpawnVolume.bounds.min.x, SpawnVolume.bounds.max.x),
        Random.Range(SpawnVolume.bounds.min.y, SpawnVolume.bounds.max.y),
        Random.Range(SpawnVolume.bounds.min.z, SpawnVolume.bounds.max.z)
    );
    }

    GameObject GetRandomGoal()
    {
        int index = Random.Range(0, Goals.Length);
        return Goals[index];
    }

    void SpawnNPC()
    {
        GameObject newNPC = Instantiate(NPC_Prefab, GetSpawnPosition(), transform.rotation);
        ZF_NPC_Pathfinding newAI = newNPC.GetComponent<ZF_NPC_Pathfinding>();
        newAI.SetGoal(GetRandomGoal());
    }
}
