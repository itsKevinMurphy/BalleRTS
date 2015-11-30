using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    float cameraDistanceMax = 50f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;
    float scrollSpeed = 5f;
    public Transform player;

    void Update()
    {
        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        if (Input.GetMouseButton(0))
        {            
            transform.RotateAround(player.position, Vector3.up, Input.GetAxis("Mouse X") * scrollSpeed);
            transform.LookAt(player);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraDistance, player.transform.position.z - cameraDistance);
            transform.LookAt(player);
        }
        // set camera position
    }
}
