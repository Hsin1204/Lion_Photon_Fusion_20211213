using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// �����s�u��J���
/// </summary>
public struct NetworkInputData : INetworkInput
{
    /// <summary>
    /// �Z�J���ʤ�V
    /// </summary>
    public Vector3 direction;
    /// <summary>
    /// �O�_���U�o�g����
    /// </summary>
    public bool isFire;

    /// <summary>
    /// �ƹ��y��
    /// </summary>
    public Vector3 cursorPosition;
}
