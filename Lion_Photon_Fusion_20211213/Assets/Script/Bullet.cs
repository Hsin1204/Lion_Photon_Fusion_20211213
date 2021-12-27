using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class Bullet : NetworkBehaviour
{
    #region ���
    [Header("�l�u���ʳt��")]
    public float speed = 5f;
    [Header("�l�u�s���ɶ�")]
    public float lifeTime = 2;
    [Header("�l�u�ؤo")]
    public float bulletScale = 1f;

   
    #endregion

    #region �ݩ�
    //Networked �s�u���ݩʸ��
    /// <summary>
    /// �s���ɶ��p�ɾ�
    /// </summary>
    [Networked]
    private TickTimer life { get; set; }


    #endregion

    private void Start()
    {
        Init();
    }
    #region ��k
    public void Init()
    {
        //�s���p�ɾ� = �p�ɾ��q��ƫإ�(�s�u���澹,�s���ɶ�)
        life = TickTimer.CreateFromSeconds(Runner, lifeTime);
        transform.localScale *= bulletScale;
    }

    /// <summary>
    /// Network Behaivor �����O���Ѫ��ƥ�
    /// �s�u�ΩT�w��s 50FPS
    /// </summary>
    public override void FixedUpdateNetwork()
    {
     
        //Expried() : �O�_���
        if (life.Expired(Runner))
        {
            //Despawn() : �R�� Object:�s�u����
            Runner.Despawn(Object);
            
        }else
        {
            //�_�h�N����
            transform.Translate(0, 0, speed * Runner.DeltaTime);
        }
    }

 

    #endregion
}
