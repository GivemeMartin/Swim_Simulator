using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Custom_Tasks
{
    [TaskCategory("Custom")]
    [TaskIcon("Assets/Resources/Editor/random.png")]
    [TaskDescription("This Task is to find a random place in an area")]
    public class RandomPoint : Action
    {
        public SharedVector3 targetPos;
        private const int TargetRadius = 3;
        public override TaskStatus OnUpdate()
        {
            Debug.Log("RunTime:FindRandomPoint");
            var temp = Random.insideUnitSphere * TargetRadius + transform.position;
            var res = new Vector3(temp.x, 0, temp.z);
            targetPos.SetValue(res);
            return TaskStatus.Success;
        }
    }
}
