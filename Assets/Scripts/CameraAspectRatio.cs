using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionAdjuster : MonoBehaviour
{
    public Transform target; // 场景目标对象，可根据实际情况设置
    public Vector3 offset;   // 相机相对于目标对象的偏移量

    void Start()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}

