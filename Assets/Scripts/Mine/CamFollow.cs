using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z);
    }
}
