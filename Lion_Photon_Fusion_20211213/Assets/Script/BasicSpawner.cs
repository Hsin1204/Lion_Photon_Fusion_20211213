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
    public Transform spawnPoint;
    [Header("�D�e��UI")]
    public GameObject ui_Main;
    private string input_RoomName;
    private NetworkRunner runner;
    #endregion

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

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
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
        runner.Spawn(go_Player, spawnPoint.position, Quaternion.identity, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
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
