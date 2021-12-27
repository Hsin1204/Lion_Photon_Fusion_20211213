using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// 紀錄連線輸入資料
/// </summary>
public struct NetworkInputData : INetworkInput
{
    /// <summary>
    /// 坦克移動方向
    /// </summary>
    public Vector3 direction;
    /// <summary>
    /// 是否按下發射按鍵
    /// </summary>
    public bool isFire;

    /// <summary>
    /// 滑鼠座標
    /// </summary>
    public Vector3 cursorPosition;
}
