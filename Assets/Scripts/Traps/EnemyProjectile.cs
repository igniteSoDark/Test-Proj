using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProjectile : EnemyDamage //Will damage the player every time they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private bool hit;
    private BoxCollider2D coll;

    private void Awake(){
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile(){
        hit = false;
        lifetime =0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update(){
        if (hit) return;
        float movementSpeed = Time.deltaTime * speed;
        transform.Translate(movementSpeed,0,0);

        lifetime += Time.deltaTime;
        if(lifetime>resetTime)
        gameObject.SetActive(false);
    }
    new private void OnTriggerEnter2D(Collider2D collision){
        hit = true;
        base.OnTriggerEnter2D(collision);
        coll.enabled = false;

        if(anim != null){
            anim.SetTrigger("explode"); //When the object is a fireball explode it
        }
        else{
            gameObject.SetActive(false); //When this hits any other object deactivate
        }
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
