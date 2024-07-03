using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationdelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;
    
    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; //when the trap gets triggered
    private bool active; // when the trap is active

    private Health player;

    private void Awake(){
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player"))
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap());
            
            player = collision.GetComponent<Health>();
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision){
        player = null;
    }

    private void Update(){
        if (active && player != null){
            player.TakeDamage(damage);
            player = null;
        }
    }

    private IEnumerator ActivateFiretrap(){
        // turn the sprite red to notify the player and trigger the trap
        triggered = true;
        spriteRend.color = Color.red; 

        // wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationdelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; 
        active = true;
        anim.SetBool("activated", true);

        // wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        active = triggered = false;
        anim.SetBool("activated", false);
    }
}
