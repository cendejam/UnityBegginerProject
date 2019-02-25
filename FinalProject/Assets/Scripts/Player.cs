using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D player;
    private bool can_jump = false, gotHit = false, right = true;
    private Animator anim;
    //private Text score_text;
    //private Text health_text;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float airtime = 5.0f;
    [SerializeField] private int health = 3;
    [SerializeField] private int score = 0;
    [SerializeField] private int invincibleDelay = 3;
    private SpriteRenderer sprite;
    //public GameObject scoreGameObject;
    //public GameObject healthGameObject;
    public GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = player.GetComponentInChildren<Animator>();
        sprite = player.GetComponentInChildren<SpriteRenderer>();
        //score_text = scoreGameObject.GetComponent<Text>();
        //health_text = healthGameObject.GetComponent<Text>();
        //health_text.text = "Lives: III";
        for (float x = -8.45f; x < 9.00f; x += 0.88f)
        {
            Vector3 position = new Vector3(x, -4.6f, 0);
            Instantiate(floor, position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal_translation = player.velocity.x;//Input.GetAxis("Horizontal") * speed;
        if (System.Math.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            flipSprite(Input.GetAxis("Horizontal"));
            anim.SetFloat("Speed", System.Math.Abs(Input.GetAxis("Horizontal")));
            horizontal_translation = Input.GetAxis("Horizontal") * speed;
        }
        else 
            horizontal_translation = 0;
        float vertical_translation = player.velocity.y;//Input.GetAxis("Jump");
        if (Input.GetAxis("Vertical") > 0 && can_jump)
        {
            print("jump!!!");
            anim.SetBool("Ground", true);
            can_jump = false;
            player.AddForce(Vector2.up * airtime, ForceMode2D.Impulse);
        }
        if (Input.GetAxis("Vertical") < 0 && !can_jump)
        {
            print("jump!!!");
            player.AddForce(Vector2.down * airtime, ForceMode2D.Force);
        }
        if (player.position.x >= 8.25 && horizontal_translation > 0)
            horizontal_translation = 0;
        if (player.position.x <= -8.25 && horizontal_translation < 0)
            horizontal_translation = 0;
        player.velocity = new Vector2(horizontal_translation, player.velocity.y);
    }
    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.CompareTag("Coin"))
    //    {
    //        print("Coin");
    //        Destroy(collider.gameObject);
    //        ++score;
    //    }
    //    if (!gotHit)
    //    {
    //        if (collider.CompareTag("Enemy"))
    //        {
    //            gotHit = true;
    //            print("Enemy");
    //            Destroy(collider.gameObject);
    //            --health;
    //            if (health == 2)
    //                health_text.text = "Lives: II";
    //            else
    //                health_text.text = "Lives: I";

    //            Invoke("Reset", invincibleDelay);
    //        }
    //    }
    //}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            print("Ground");
            anim.SetBool("Ground", false);
            can_jump = true;
        }
        else if(collision.collider.CompareTag("enemy"))
        {
            player.velocity = new Vector2(player.velocity.x, 10f);
        }
    }
    void flipSprite(float dir)
    {
        print(dir);
        if (dir < 0 && right || (dir > 0 && !right))
        {
            right = !right;
            sprite.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
