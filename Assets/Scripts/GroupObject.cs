using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroupObject : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 newVelocity;
    public Vector3 newPosition;
    
    public BoidSpawner groupmanager;
    static List<GroupObject> groupList;
    public List<GroupObject> neighbours;
    public List<GroupObject> collisionRisks;
    public GroupObject closest;
    private ParticleSystem bubbles;
    private void Awake()
    {
        bubbles = GetComponentInChildren<ParticleSystem>();

        if (groupList == null)
            groupList = new List<GroupObject>();
        
        groupList.Add(this);

        //初始化两个List
        neighbours = new List<GroupObject>();
        collisionRisks = new List<GroupObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitObject();
    }

    // Update is called once per frame
    void Update()
    {
        //获取到 当前boid 附近所有的Boids 的表
        neighbours = GetNeighbors(this);
        //使用当前位置和速度初始化新位置和新速度
        newVelocity = velocity;
        newPosition = this.transform.position;
        
        //速度匹配
        //取得于 当前Boid 的速度接近的 所有邻近Boid对象 的平均速度
        Vector3 neighborVel = GetAverageVelocity(neighbours);
        //将 新速度 += 邻近boid的平均速度*velocityMatchingAmt
        newVelocity += neighborVel * groupmanager.velocityMatchingAmt;

        /*
        凝聚向心性：使 当前boid 向 邻近Boid对象 的中心 移动
        */
        //取得于 当前Boid 的三位坐标接近的 所有邻近Boid对象 的平均三位间距
        Vector3 neighborCenterOffset = GetAveragePosition(neighbours) - this.transform.position;
        //将 新速度 += 邻近boid的平均间距*flockCenteringAmt
        newVelocity += neighborCenterOffset * groupmanager.flockCenteringAmt;

        /*
        排斥性：避免撞到 邻近的Boid
        */
        Vector3 dist;
        if (collisionRisks.Count > 0)   //处理 最近的boid 表
        {
            //取得 最近的所有boid 的平均位置
            Vector3 collisionAveragePos = GetAveragePosition(collisionRisks);
            dist = collisionAveragePos - this.transform.position;
            //将 新速度 += 与最近boid的平均间距*flockCenteringAmt
            newVelocity += dist * groupmanager.collisionAvoidanceAmt;
        }

        //跟随鼠标光标：无论距离多远都向鼠标光标移动
        dist = GameObject.FindWithTag("Player").transform.position - transform.position;

        //若距离鼠标光标太远，则靠近；反之离开(修改新速度)
        if (dist.magnitude > groupmanager.AvoiddanceDsit)
            newVelocity += dist * groupmanager.AtrractionAmt;
        else
            newVelocity -= dist.normalized * groupmanager.AvoidanceAmt;

        //至此在Update()内 确定了 新速度和新位置，需要在后续LateUpdate()内应用
        //一般都是Update()内确定参数，在LateUpdate()内实现移动
    }

    private void LateUpdate()
    {
        velocity = (1 - groupmanager.velocityLerpAmt) * velocity + groupmanager.velocityLerpAmt * newVelocity;
        //确保 速度值 在上下限范围内(超过范围就设定为范围值)
        if (velocity.magnitude > groupmanager.maxVelocity)
        {
            velocity = velocity.normalized * groupmanager.maxVelocity;
            StartCoroutine(EmitBubbles(1));
        }
        if (velocity.magnitude < groupmanager.minVelocity)
        {
            velocity = velocity.normalized * groupmanager.minVelocity;
        }
        velocity.y = 0;
        newPosition = this.transform.position + velocity * Time.deltaTime;
        float deltaDist = (GameObject.FindWithTag("Player").transform.position - transform.position).magnitude;
        if (deltaDist < 2)
            bubbles.Stop();
        //Debug.Log(Mathf.Abs(Vector3.Dot(transform.forward, deltaDir)));
        // if (Mathf.Abs(Vector3.Dot(transform.forward, deltaDir)) < 0.8f)
        //     //transform.forward = deltaDir;
        //     transform.forward = Vector3.Lerp(transform.forward, deltaDir, 0.8f);
        // else
        // {
        //     transform.forward = Vector3.Lerp(transform.forward, velocity.normalized, 0.1f);
        // }
        //transform.position = newPosition;
        transform.forward = Vector3.Lerp(transform.forward, velocity.normalized, 0.1f);
        GetComponent<Rigidbody>().velocity = velocity;
    }

    IEnumerator EmitBubbles(float emitTimes)
    {
        bubbles.Play();
        yield return new WaitForSeconds(emitTimes);
        bubbles.Stop();
    }

    void InitObject()
    {
        Vector3 randomPos = Random.insideUnitSphere * groupmanager.spawnRadius;
        randomPos.y = 0;
        transform.position = randomPos;
        
        //Random.onUnitSphere 返回 一个半径为1的 球体表面的点
        velocity = Random.onUnitSphere;
        velocity.y = 0;
        velocity *= groupmanager.spawnVelocity;
    }
    
    //查找那些Boid距离当前Boid距离足够近，可以被当作附近对象
    public List<GroupObject> GetNeighbors(GroupObject boi)
    {
        float closesDist = float.MaxValue;  //最小间距，MaxValue 为浮点数的最大值
        Vector3 delta;              //当前 boid 与其他某个 boid 的三维间距 
        float dist;                 //三位间距转换为的 实数间距

        neighbours.Clear();          //清理上次表的数据
        collisionRisks.Clear();     //清理上次表的数据

        //遍历目前所有的 boid，依据设定的范围值筛选出 附近的boid 与 最近的boid 于各自表中
        foreach (GroupObject b in groupList)
        {
            if (b == boi)   //跳过自身
                continue;

            delta = b.transform.position - boi.transform.position;  //遍历到的 b 与当前持有的 boi(都为boid) 的三维间距
            dist = delta.magnitude;     //实数间距

            if (dist < closesDist)
            {
                closesDist = dist;      //更新最小间距
                closest = b;            //更新最近的 boid 为 b
            }

            if (dist < groupmanager.nearDist) //处在附近的 boid 范围
            {
                neighbours.Add(b);
                
            }

            if (dist < groupmanager.collisionDist) //处在最近的 boid 范围(有碰撞风险)
                collisionRisks.Add(b);
        }

        if (neighbours.Count == 0)   //若没有其他满足邻近范围的boid，则将自身boid纳入附近的boid表中
            neighbours.Add(closest);

        return (neighbours);
    }

    //获取 List<Boid>当中 所有Boid 的平均位置
    public Vector3 GetAveragePosition(List<GroupObject> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (GroupObject b in someBoids)
            sum += b.transform.position;
        Vector3 center = sum / someBoids.Count;

        return (center);
    }

    //获取 List<Boid> 当中 所有Boid 的平均速度
    public Vector3 GetAverageVelocity(List<GroupObject> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (var b in someBoids)
        {
            sum += b.velocity;
        }
            
        Vector3 avg = sum / someBoids.Count;

        return (avg);
    }
}
