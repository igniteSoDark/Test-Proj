using JetBrains.Annotations;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote time")]
    [SerializeField] private float coyoteTime; //How much time can player still hang in the air and be able to jump
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private int wallJumpY; //Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    //private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        //L-R Flip
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        //Animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        
        //Adjustable Jump Height
        if(Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x,body.velocity.y/2);

        if(onWall()){
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else{
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(isGrounded()){
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter
            }
            else{
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
            }
        }
    }

    private void Jump()
    {
        if(coyoteCounter < 0 && !onWall() && jumpCounter <= 0) return; //No jump :(

        SoundManager.instance.PlaySound(jumpSound);

        if(onWall())
            WallJump();
        else
        {
            if(isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //If not on the ground and coyote counter bigger than 0 do a normal jump
                if(coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else{
                    if (jumpCounter > 0){ //If we have extra jumps then jump and decrease the jump counter
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void WallJump(){
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) *wallJumpX, wallJumpY));
        //wallJumpCooldown = 0;
    }
    public bool canAttack(){
        //Old Version
        //return horizontalInput == 0 && isGrounded() && !onWall();
        return !onWall();
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0,Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
