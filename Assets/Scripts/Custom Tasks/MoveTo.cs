using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Custom_Tasks
{
    [TaskCategory("Custom")]
    [TaskIcon("Assets/Resources/Editor/MoveTo.png")]
    public class MoveTo : Action
    {
        public SharedVector3 movePos;
        public override TaskStatus OnUpdate()
        {
            if ((transform.position - movePos.Value).magnitude < 0.05f)
            {
                Debug.Log("Delta:"+(transform.position - movePos.Value));
                return TaskStatus.Success;
            }
                
            else
            {
                var position = transform.position;
                var delta = (movePos.Value - position).normalized; 
                transform.forward = Vector3.Lerp(transform.forward, delta, 0.2f);
                position = Vector3.Lerp(position, movePos.Value, Time.fixedDeltaTime);
                transform.position = position;
            }
            return TaskStatus.Running;
        }
    }
}
