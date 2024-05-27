using UnityEngine;

public class KameraTakip : MonoBehaviour
{

//kameranın arabayı takip etme işlemlerinin olduğu script
    [SerializeField] public Transform player;

    //public float smoothTime = 0.5f;
    public float verticalSmoothTime = 0.1f;
    public float horizontalSmoothTime = 0.3f;

    private Vector3 velocity;

    public Vector3 offset;


    private void OnEnable()
    {
        offset = transform.position - player.position;
    }
    public float rotSpeed;
    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y, player.position.z + offset.z), ref velocity, verticalSmoothTime);
    }
}