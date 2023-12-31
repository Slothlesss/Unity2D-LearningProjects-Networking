using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ConnectionApprovalHandler : MonoBehaviour
{
    private const int MaxPlayers = 10;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("Connect Approval");
        response.Approved = true;
        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;
        if (NetworkManager.Singleton.ConnectedClients.Count >= MaxPlayers)
        {
            response.Approved = false;
            response.Reason = "Server is Full";
        }

        response.Pending = false;
    }
}
