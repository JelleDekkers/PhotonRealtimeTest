using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTest : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
{
    private readonly LoadBalancingClient client = new LoadBalancingClient();

    [SerializeField] private string appID = "63eab3e6-2560-42c1-98cd-0f13c41ab8bd";
    [SerializeField] private string roomName = "New Room";
    [SerializeField] private string ip = "10.0.0.2";
    [SerializeField] private int port = 5055;

    private void Awake()
    {
        SubscribeToCallbacks();
    }

    private void OnGUI()
    {
        GUILayout.Label("isconnected: " + client.IsConnected);
        GUILayout.Label("isconnected and ready: " + client.IsConnectedAndReady);
        if (client.IsConnected)
        {
            if (client.CurrentRoom != null)
            {
                GUILayout.Label("CurrentRoom name: " + client.CurrentRoom.Name);
                GUILayout.Label("CurrentRoom player count: " + client.CurrentRoom.PlayerCount);
            }

            roomName = GUILayout.TextField(roomName);

            if (GUILayout.Button("Create & Join Room"))
                CreateRoom(roomName);

            if (GUILayout.Button("Join Room"))
                JoinRoom(roomName);
        }
        GUILayout.Space(2);

        if (GUILayout.Button("Connect"))
            StartClient();

        if (GUILayout.Button("Disconnect"))
            Disconnect();
    }

    private void Update()
    {
        client.Service();
    }

    private void OnDestroy()
    {
        Disconnect();
        UnsubscribeFromCallbacks();
    }

    private void SubscribeToCallbacks()
    {
        client.AddCallbackTarget(this);
    }

    private void UnsubscribeFromCallbacks()
    {
        client.RemoveCallbackTarget(this);
    }

    public void StartClient()
    {
        client.StateChanged += OnStateChange;
		client.UserId = SystemInfo.deviceUniqueIdentifier;
        AppSettings settings = new AppSettings();
        settings.NetworkLogging = ExitGames.Client.Photon.DebugLevel.ALL;
        //settings.AppIdRealtime = appID;
        settings.Server = ip;
        settings.Port = port;
        //client.LoadBalancingPeer.SerializationProtocolType = ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
        client.MasterServerAddress = ip + ":" + port;
        client.ConnectToMasterServer();
        //client.ConnectUsingSettings(settings);
    }

    private void Disconnect()
    {
        if (client.IsConnected)
            client.Disconnect();
    }

    private void CreateRoom(string roomName)
    {
		Debug.Log("creating room: " + roomName);
        EnterRoomParams roomParams = new EnterRoomParams();
        roomParams.RoomName = roomName;
        roomParams.RoomOptions = new RoomOptions();
        client.OpCreateRoom(roomParams);
    }

    private void JoinRoom(string roomName)
    {
		Debug.Log("attempting to join room: " + roomName);
        EnterRoomParams roomParams = new EnterRoomParams();
        roomParams.RoomName = roomName;
        client.OpJoinRoom(roomParams);
    }

    #region events
    private void OnStateChange(ClientState arg1, ClientState arg2)
    {
        Debug.Log(arg1 + " -> " + arg2);
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster Server: " + client.LoadBalancingPeer.ServerIpAddress);
    }

    public void OnConnected()
    {
        Debug.Log("OnConnected()");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected()");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed " + returnCode + " msg: " +  message);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed " + message + "returnCode: " + returnCode);
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        Debug.Log("ONLeftRoom()");
    }

    void IMatchmakingCallbacks.OnJoinedRoom()
    {
        Debug.Log("onJoinedRoom " + client.CurrentRoom.Name);
    }
    #endregion
}