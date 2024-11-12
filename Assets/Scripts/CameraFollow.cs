using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = player.position.x + offset.x;

        transform.position = player.transform.position + offset;
    }
}
