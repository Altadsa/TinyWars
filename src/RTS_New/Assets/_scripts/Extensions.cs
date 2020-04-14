using UnityEngine;
using UnityEngine.AI;

public static class Extensions
{
    public static Vector3[] VieportBounds(this Camera camera)
    {
        var viewportBounds = new Vector3[4];
        viewportBounds[0] = camera.ViewportToWorldPoint(Vector3.zero);
        viewportBounds[1] = camera.ViewportToWorldPoint(Vector3.up);
        viewportBounds[2] = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        viewportBounds[3] = camera.ViewportToWorldPoint(Vector3.right);
        return viewportBounds;
    }

    public static Vector3 SwapYZ(this Vector3 vector3)
    {
        var tmp = vector3.y;
        vector3.y = vector3.z;
        vector3.z = tmp;
        return vector3;
    }

    public static float DistanceToPoint(this Entity entity, Vector3 point)
    {
        var bPosition = entity.transform.position;
        return Mathf.Abs((bPosition - point).magnitude);
    }

    public static bool TotallyStopped(this NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude <= Mathf.Epsilon)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
}
