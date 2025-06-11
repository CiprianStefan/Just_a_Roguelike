using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [SerializeField]
    private GameObject player;

    protected void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
