using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Custom_Tasks
{
    [TaskCategory("Custom")]
    [TaskDescription("To detect whether the target object is nearby")]
    public class GameObjectNearby : Conditional
    {
        public SharedTransform avoidObject;
        public SharedFloat avoidDistance;
        public override TaskStatus OnUpdate()
        {
            if ((avoidObject.Value.position - transform.position).magnitude < avoidDistance.Value)
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }
    }
}
