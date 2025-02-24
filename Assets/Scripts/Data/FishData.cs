using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishData
{
    public string name;       // 鱼类名称
    public int cost;          // 价格
    public Sprite thumbnail;  // 缩略图
    public GameObject prefab; // 鱼的Prefab
}
