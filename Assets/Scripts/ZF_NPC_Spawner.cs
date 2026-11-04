using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ZF_NPC_Spawner : MonoBehaviour
{
    // Editor Variables
    [Header("Editor Variables")]
    [SerializeField] GameObject[] Npc_Prefabs;
    [SerializeField] ZF_Goal[] Goals;
    [SerializeField] BoxCollider SpawnVolume;
    [SerializeField] GameObject NpcParent;
    [SerializeField] int NumNpcs = 5;
    [SerializeField] int MaxGroupSize = 4;

    // Internal Variables
    private int npcCount = 0; // Number of NPCs currently spawned
    private int nextGroupSize = 0;
    private int currentGroupSize = 0;
    private List<ZF_NPC_Pathfinding> npcList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npcList = new List<ZF_NPC_Pathfinding>();

        //Random.InitState((int)System.DateTime.Now.Ticks);
        if (!SpawnVolume) SpawnVolume = GetComponent<BoxCollider>();
        if (Npc_Prefabs.Length > 0)
        {
            for (int i = 0; i < NumNpcs; i++)
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

    ZF_Goal GetRandomGoal()
    {
        int index = Random.Range(0, Goals.Length);
        return Goals[index];
    }

    GameObject GetRandomNpcPrefab()
    {
        if (Npc_Prefabs.Length == 1) return Npc_Prefabs[0];

        int chance = Random.Range(0, 100);
        if (chance < 90) return Npc_Prefabs[0]; // 90% able bodied
        else return Npc_Prefabs[1]; // 10% slower
    }

    void SpawnNPC()
    {
        if (nextGroupSize == 0)
        {
            nextGroupSize = Random.Range(1, MaxGroupSize + 1); // Random group size
            if (npcCount + nextGroupSize > NumNpcs) nextGroupSize = NumNpcs - npcCount; // Limit group size to total number of npcs

            currentGroupSize = nextGroupSize;
            //Debug.Log($"Group Size: {currentGroupSize}");

        }


        // Create NPC
        GameObject newNPC = Instantiate(GetRandomNpcPrefab(), GetSpawnPosition(), transform.rotation);
        ZF_NPC_Pathfinding newAI = newNPC.GetComponent<ZF_NPC_Pathfinding>();

        newNPC.transform.SetParent(NpcParent.transform); // Parent NPC to npcParent
        npcList.Add(newAI); // Add NPC to list

        newAI.SetGoal(GetRandomGoal()); // Set NPC Goal

        // Set NPC ID
        newAI.npcID = npcCount;
        npcCount++;

        // Update Group
        GroupNPC(newAI);

        if (nextGroupSize == 0 && currentGroupSize > 1)
        {
            newAI.TriggerGroupLeaderCatchup();
            //newAI.Invoke(nameof(newAI.TriggerGroupLeaderCatchup), 1);
        }
    }

    void GroupNPC(ZF_NPC_Pathfinding newAI)
    {
        if (currentGroupSize > 1)
        {
            // Group newAI
            if (currentGroupSize != nextGroupSize) // Add to existing group
            {
                int difference = currentGroupSize - nextGroupSize;
                npcList[npcList.Count - difference - 1].AddToGroup(newAI);
            }
            else // Create new group
            {
                newAI.npcGroup = new List<ZF_NPC_Pathfinding>();
                newAI.npcGroup.Add(newAI);
                newAI.inGroup = true;
                newAI.leader = newAI.gameObject;
            }

            // Trigger sorting of leader behaviour within group
            newAI.UpdateLeader();
        }
        nextGroupSize--;
    }    
}
