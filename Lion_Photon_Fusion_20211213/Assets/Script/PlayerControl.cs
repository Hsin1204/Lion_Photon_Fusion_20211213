using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerControl : NetworkBehaviour
{
    #region 欄位
    [Header("移動速度")]
    public float speed = 7.5f;
    [Header("子彈發射間隔")]
    public float fireInterval = 0.35f;
    [Header("子彈物件")]
    public Bullet bullet;
    [Header("砲塔")]
    public Transform towerTrans;
    [Header("子彈生成位置")]
    public Transform firePoint;

    private InputField inputMessage;
    private Text textMessage;
    /// <summary>
    /// 連線腳色控制器
    /// </summary>
    private NetworkCharacterController ncc;
    #endregion

    #region 屬性
    /// <summary>
    /// 開槍間隔計時器
    /// </summary>
    public TickTimer interval { get; set; }
    #endregion
    #region 事件
    private void Awake()
    {
        ncc = GetComponent<NetworkCharacterController>();
        textMessage = GameObject.Find("ChatContent").GetComponent<Text>();
        
        inputMessage = GameObject.Find("IF_ChatInput").GetComponent<InputField>();
        inputMessage.onEndEdit.AddListener((string message) => { InputMessage(message); });
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("Bullet"))
        {
            Destroy(gameObject);
        }
    }
    #endregion
    #region 方法
    /// <summary>
    /// Fusion 固定更新事件
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        Move();
        Fire();
    }
    private void Move()
    {
        //如果有輸入資料
        if(GetInput(out NetworkInputData dataInput))
        {
            //連線角色控制器.移動(速度*方向*連線一幀時間)
            ncc.Move(speed * dataInput.direction * Runner.DeltaTime);
            Vector3 cursorPosition = dataInput.cursorPosition;
            cursorPosition.y = towerTrans.position.y;

            towerTrans.forward = cursorPosition - towerTrans.position;
        }
    }
    private void Fire()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            if (interval.ExpiredOrNotRunning(Runner))
            {
                if (inputData.isFire)
                {
                    interval = TickTimer.CreateFromSeconds(Runner, fireInterval);

                    Runner.Spawn(
                        bullet,
                        firePoint.position,
                        firePoint.rotation,
                        Object.InputAuthority,
                        (runner, objectSpawn) =>
                        {
                            objectSpawn.GetComponent<Bullet>();
                        }
                        );
                }
            }
        }
    }

    private void InputMessage(string message)
    {
        if(Object.HasInputAuthority)
        {
            RPC_SendMessage(message);
        }
    }

    [Rpc(RpcSources.InputAuthority,RpcTargets.All)]
    private void RPC_SendMessage(string message,RpcInfo rpcInfo= default)
    {
        textMessage.text += message+"\n";
        
    }
    #endregion
}
