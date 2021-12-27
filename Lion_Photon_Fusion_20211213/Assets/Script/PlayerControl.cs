using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerControl : NetworkBehaviour
{
    #region ���
    [Header("���ʳt��")]
    public float speed = 7.5f;
    [Header("�l�u�o�g���j")]
    public float fireInterval = 0.35f;
    [Header("�l�u����")]
    public Bullet bullet;
    [Header("����")]
    public Transform towerTrans;
    [Header("�l�u�ͦ���m")]
    public Transform firePoint;

    private InputField inputMessage;
    private Text textMessage;
    /// <summary>
    /// �s�u�}�ⱱ�
    /// </summary>
    private NetworkCharacterController ncc;
    #endregion

    #region �ݩ�
    /// <summary>
    /// �}�j���j�p�ɾ�
    /// </summary>
    public TickTimer interval { get; set; }
    #endregion
    #region �ƥ�
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
    #region ��k
    /// <summary>
    /// Fusion �T�w��s�ƥ�
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        Move();
        Fire();
    }
    private void Move()
    {
        //�p�G����J���
        if(GetInput(out NetworkInputData dataInput))
        {
            //�s�u���ⱱ�.����(�t��*��V*�s�u�@�V�ɶ�)
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
