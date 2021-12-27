using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

//INetworkRunnerCallbacks �s�u�����^�G�����ARunner ���澹�B�z�欰��|�^�I����������k
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region ���
    [Header("�ЫػP�[�J�ж������")]
    public InputField if_Create;
    public InputField if_Search;
    [Header("���a�����")]
    public NetworkPrefabRef go_Player;

    [Header("�ͦ���m")]
    public Transform[] spawnPoints;
    [Header("�D�e��UI")]
    public GameObject ui_Main;
    [Header("������r")]
    public Text txt_Version;

    private string input_RoomName;
    private NetworkRunner runner;
    private string version = "Hsin Copyright 2022.| Version  ";
    private Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();
    #endregion

    private void Awake()
    {
        txt_Version.text = version + Application.version;
    }
    #region ��k
    /// <summary>
    /// ���s�I���I�s : �Ыةж�
    /// </summary>
    public void Btn_CreateRoom()
    {
        input_RoomName = if_Create.text;
        print(input_RoomName);
        StartGame(GameMode.Host);
    }
    /// <summary>
    /// ���s�I���I�s : �j�M�å[�J�ж�
    /// </summary>
    public void Btn_SearchRoom()
    {
        input_RoomName = if_Search.text;
        print(input_RoomName);
        StartGame(GameMode.Client);
    }

    //async �D�P�B�B�z :����t�ήɳB�z�s�u
    /// <summary>
    /// �}�l�s�u�C��
    /// </summary>
    /// <param name="mode">�s�u�覡:�D���B�Ȥ�</param>
    private async void StartGame(GameMode mode)
    {
        print("<color=lime>�}�l�s�u</color>");
        //�K�[�s�u���澹
        runner = gameObject.AddComponent<NetworkRunner>();
        //�O�_���ѿ�J�\��
        runner.ProvideInput = true;

        //���ݳs�u: �C���s�u�Ҧ��B�ж��W�١B�s�u�᪺�����B�����޲z��
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = input_RoomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        print("<color=lime>�s�u���\</color>");
        ui_Main.SetActive(false);
    }

    #region �^�I�禡��

    
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        
    }

    /// <summary>
    /// ���a�s�u��J�欰
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();

        #region �Ȼs�ƿ�J
        if (Input.GetKey(KeyCode.W)) inputData.direction += Vector3.forward;
        if (Input.GetKey(KeyCode.D)) inputData.direction += Vector3.right;
        if (Input.GetKey(KeyCode.S)) inputData.direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputData.direction += Vector3.left;

        inputData.isFire = Input.GetKeyDown(KeyCode.Mouse0);
        #endregion

        #region �ƹ��y�гB�z
        inputData.cursorPosition = Input.mousePosition;
        inputData.cursorPosition.z = 60;

        Vector3 cursorToWorld = Camera.main.ScreenToWorldPoint(inputData.cursorPosition);
        inputData.cursorPosition = cursorToWorld;
        #endregion
        //��J��T.�]�w(�s�u��J���)
        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    /// <summary>
    /// ���a���\�[�J�ж���
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length + 1);
        //�s�u���澹.�ͦ�(����,�y��,����,���a��T)
        NetworkObject playerObj = runner.Spawn(go_Player, spawnPoints[randomIndex].position, Quaternion.identity, player);

        players.Add(player, playerObj);

    }

    /// <summary>
    /// ���a�h�X�ж���
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       if(players.TryGetValue(player,out NetworkObject playeretworkObject))
        {
            runner.Despawn(playeretworkObject);
            players.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
    #endregion
    #endregion
}
