using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public float spawnRadius;
    public int spawnNum;
    public GameObject boidGameObject;
    public float spawnVelocity;
    public float nearDist;
    public float collisionDist = 5f;
    
    public float spawnVelcoty = 10f;            //boid 的速度
    public float minVelocity = 0f;
    public float maxVelocity = 30f;
    [Tooltip("与 附近的boid 的平均速度 乘数(影响新速度)")]
    public float velocityMatchingAmt = 0.01f;   //与 附近的boid 的平均速度 乘数(影响新速度)
    [Tooltip("与 附近的boid 的平均三维间距 乘数(影响新速度)")]
    public float flockCenteringAmt = 0.15f;     //与 附近的boid 的平均三维间距 乘数(影响新速度)
    [Tooltip("与 最近的boid 的平均三维间距 乘数(影响新速度)")]
    public float collisionAvoidanceAmt = -0.5f; //与 最近的boid 的平均三维间距 乘数(影响新速度)
    [Tooltip("当 鼠标光标距离 过大时，与其间距的 乘数(影响新速度)")]
    public float AtrractionAmt = 0.01f;    //当 鼠标光标距离 过大时，与其间距的 乘数(影响新速度)
    [Tooltip("当 鼠标光标距离 过小时，与其间距的 乘数(影响新速度)")]
    public float AvoidanceAmt = 0.75f;     //当 鼠标光标距离 过小时，与其间距的 乘数(影响新速度)
    public float AvoiddanceDsit = 2f;
    public float velocityLerpAmt = 0.25f;       //线性插值法计算新速度的 乘数
    private void Awake()
    {
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject temp = Instantiate(boidGameObject,this.transform);
            temp.GetComponent<GroupObject>().groupmanager = this;
        }
    }
}
