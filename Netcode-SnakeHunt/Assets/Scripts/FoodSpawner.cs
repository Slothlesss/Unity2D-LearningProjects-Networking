using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
    }

    private void SpawnFoodStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
        NetworkObjectPool.Singleton.OnNetworkSpawn();
        for (int i = 0; i < 10; i++)
        {
            SpawnFood();
        }
        StartCoroutine(SpawnOverTime());
    }

    

    private void SpawnFood()
    {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab, GetRandomPositionOnMap(), Quaternion.identity);
        obj.GetComponent<Food>().prefab = prefab;
        if (!obj.IsSpawned) obj.Spawn();
    }
    private Vector3 GetRandomPositionOnMap()
    {
        return new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0f);
    }
    IEnumerator SpawnOverTime()
    {
        while (NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            yield return new WaitForSeconds(5f);
            SpawnFood();
        }
    }

}
