using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake(){
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn(){

        //Check if checkpoint available
        if (currentCheckpoint == null)
        {
            //Show game over screen
            uiManager.GameOver();

            return; //Don't execute the rest of this function
        }

        transform.position = currentCheckpoint.position; // Move player to checkpoint pos
        playerHealth.Respawn(); //Restore player Health and reset animation

        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent); //Move camera to the checkpoint room
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.transform.tag == "Checkpoint"){
            currentCheckpoint = collision.transform; //Store the checkpoint that we activated as the current checkpoint
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //Deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear"); //Trigger checkpoint animation
        }
    }
}
