using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Custom_Tasks
{
    [TaskCategory("Custom")]
    [TaskDescription("To get away from an object")]
    public class GetAwayFrom : Action
    {
        public SharedTransform avoidObject;
        public SharedFloat safeDist;
        public override TaskStatus OnUpdate()
        {
            Vector3 movePos = (transform.position - avoidObject.Value.position).normalized * 2 + transform.position;
            movePos.y = 0;
            if ((transform.position - avoidObject.Value.position).magnitude > safeDist.Value)
            {
                return TaskStatus.Success;
            }
                
            else
            {
                var position = transform.position;
                var delta = (movePos - position).normalized; 
                transform.forward = Vector3.Lerp(transform.forward, delta, 0.2f);
                transform.position = Vector3.Lerp(position, movePos, Time.fixedDeltaTime);
            }
            return TaskStatus.Running;
        }
    }
}
