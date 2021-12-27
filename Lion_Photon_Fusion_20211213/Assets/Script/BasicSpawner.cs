using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

//INetworkRunnerCallbacks 連線執行氣回乎介面，Runner 執行器處理行為後會回呼此介面的方法
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region 欄位
    [Header("創建與加入房間的欄位")]
    public InputField if_Create;
    public InputField if_Search;
    [Header("玩家控制物件")]
    public NetworkPrefabRef go_Player;

    [Header("生成位置")]
    public Transform[] spawnPoints;
    [Header("主畫面UI")]
    public GameObject ui_Main;
    [Header("版本文字")]
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
    #region 方法
    /// <summary>
    /// 按鈕點擊呼叫 : 創建房間
    /// </summary>
    public void Btn_CreateRoom()
    {
        input_RoomName = if_Create.text;
        print(input_RoomName);
        StartGame(GameMode.Host);
    }
    /// <summary>
    /// 按鈕點擊呼叫 : 搜尋並加入房間
    /// </summary>
    public void Btn_SearchRoom()
    {
        input_RoomName = if_Search.text;
        print(input_RoomName);
        StartGame(GameMode.Client);
    }

    //async 非同步處理 :執行系統時處理連線
    /// <summary>
    /// 開始連線遊戲
    /// </summary>
    /// <param name="mode">連線方式:主機、客戶</param>
    private async void StartGame(GameMode mode)
    {
        print("<color=lime>開始連線</color>");
        //添加連線執行器
        runner = gameObject.AddComponent<NetworkRunner>();
        //是否提供輸入功能
        runner.ProvideInput = true;

        //等待連線: 遊戲連線模式、房間名稱、連線後的場景、場景管理器
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = input_RoomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneObjectProvider = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        print("<color=lime>連線成功</color>");
        ui_Main.SetActive(false);
    }

    #region 回呼函式區

    
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
    /// 玩家連線輸入行為
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();

        #region 客製化輸入
        if (Input.GetKey(KeyCode.W)) inputData.direction += Vector3.forward;
        if (Input.GetKey(KeyCode.D)) inputData.direction += Vector3.right;
        if (Input.GetKey(KeyCode.S)) inputData.direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputData.direction += Vector3.left;

        inputData.isFire = Input.GetKeyDown(KeyCode.Mouse0);
        #endregion

        #region 滑鼠座標處理
        inputData.cursorPosition = Input.mousePosition;
        inputData.cursorPosition.z = 60;

        Vector3 cursorToWorld = Camera.main.ScreenToWorldPoint(inputData.cursorPosition);
        inputData.cursorPosition = cursorToWorld;
        #endregion
        //輸入資訊.設定(連線輸入資料)
        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    /// <summary>
    /// 當玩家成功加入房間後
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length + 1);
        //連線執行器.生成(物件,座標,角度,玩家資訊)
        NetworkObject playerObj = runner.Spawn(go_Player, spawnPoints[randomIndex].position, Quaternion.identity, player);

        players.Add(player, playerObj);

    }

    /// <summary>
    /// 當玩家退出房間後
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
