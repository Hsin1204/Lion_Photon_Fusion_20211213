using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class Bullet : NetworkBehaviour
{
    #region 欄位
    [Header("子彈移動速度")]
    public float speed = 5f;
    [Header("子彈存活時間")]
    public float lifeTime = 2;
    [Header("子彈尺寸")]
    public float bulletScale = 1f;

   
    #endregion

    #region 屬性
    //Networked 連線用屬性資料
    /// <summary>
    /// 存活時間計時器
    /// </summary>
    [Networked]
    private TickTimer life { get; set; }


    #endregion

    private void Start()
    {
        Init();
    }
    #region 方法
    public void Init()
    {
        //存活計時器 = 計時器從秒數建立(連線執行器,存活時間)
        life = TickTimer.CreateFromSeconds(Runner, lifeTime);
        transform.localScale *= bulletScale;
    }

    /// <summary>
    /// Network Behaivor 父類別提供的事件
    /// 連線用固定更新 50FPS
    /// </summary>
    public override void FixedUpdateNetwork()
    {
     
        //Expried() : 是否到期
        if (life.Expired(Runner))
        {
            //Despawn() : 刪除 Object:連線物件
            Runner.Despawn(Object);
            
        }else
        {
            //否則就移動
            transform.Translate(0, 0, speed * Runner.DeltaTime);
        }
    }

 

    #endregion
}
