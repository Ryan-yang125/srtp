using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        SwitchAnim();
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
    }

    public void Movement(float horizontalMove)
    {
        //float horizontalMove=Input.GetAxis("Horizontal");
        //float facedirection=Input.GetAxisRaw("Horizontal"); //保留-1，0，1

        //if(horizontalMove!=0.0f)
        //{
        rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);

        //}
        // if(facedirection!=0.0f)
        //{
        //   transform.localScale=new Vector3(facedirection,1,1);
        //}

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    public void Jump()
    {
        if (isJump == false)
        {
            isJump = true;
            print("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            anim.SetBool("jumping", true);
        }
    }
    void SwitchAnim()
    {
        anim.SetBool("idle", false);
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            isJump = false;
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadLine")
        {
            Invoke("Restart", 1f);//延迟时间
        }
    }


    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
