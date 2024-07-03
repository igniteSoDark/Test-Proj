using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera mode: 1 for follow player, 0 for room camera")]
    [SerializeField] private int cameraMode;

    //Room Camera
    [Header("Room Camera")]
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follow player
    [Header("Follow Player Camera")]
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;
    

    private void Update()
    {
        switch(cameraMode){
            case 0:
                //Room Camera
                transform.position = Vector3.SmoothDamp(transform.position, 
                new Vector3(currentPosX, transform.position.y,transform.position.z), ref velocity, speed);
                break;
            case 1:
                //Follow Player
                transform.position = new Vector3(player.position.x + lookAhead, transform.position.y,transform.position.z);
                lookAhead = Mathf.Lerp(lookAhead, aheadDistance*player.localScale.x, Time.deltaTime*cameraSpeed);
                break;
            default:
                break;
        }





    }
    public void MoveToNewRoom(Transform _newRoom){
        currentPosX = _newRoom.position.x;
    }
}
