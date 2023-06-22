using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StartNetwork : MonoBehaviour
{
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
