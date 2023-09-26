using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class NetworkHandler : NetworkBehaviour
{     
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
    }

    private bool hasPrinted = false;
    private void PrintMe ()
    {
        if (hasPrinted)
        {
            return;
        }
        Debug.Log("I AM");
        hasPrinted = true;
        if (IsServer)
        {
            Debug.Log($"  the Server! {NetworkManager.ServerClientId}");
        }
        if (IsHost)
        {
            Debug.Log($"  the Host! {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");
        }
        if (IsServer)
        {
            Debug.Log($"  the Server! {NetworkManager.LocalClientId}");
        }
        if (IsClient)
        {
            Debug.Log("  the Client!");
        }
        if (!IsServer && !IsClient)
        {
            Debug.Log("  Nothing Yet!");
            hasPrinted = false;
        }
    }

    private void OnClientStarted()
    {
        Debug.Log("!! Client Started !!");
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDisconnected;
        NetworkManager.OnServerStopped += ClientOnClientStopped;
        PrintMe();
    }

    //Client Actions

    private void ClientOnClientConnected(ulong clientId) 
    {
        Debug.Log($"I {clientId} have connected to the server");
        PrintMe();
        
    }

    private void ClientOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"I {clientId} have disconnected from the server");
    }

    private void ClientOnClientStopped(bool indicator)
    {
        Debug.Log("!! Client Stopped !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDisconnected;
        NetworkManager.OnServerStopped -= ClientOnClientStopped;
    }
        
        //Server Actions

    private void OnServerStarted()
    {
        Debug.Log("!! Server Started !!");
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDisconnected;
        NetworkManager.OnServerStopped += SeverOnServerStopped;
        PrintMe();
    }

    private void ServerOnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected to the server");
    }
    private void ServerOnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected to the server");
    }
    private void SeverOnServerStopped (bool indicator)
    {
        Debug.Log("!! Server Stopped !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDisconnected;
        NetworkManager.OnServerStopped -= SeverOnServerStopped;
    }
}
