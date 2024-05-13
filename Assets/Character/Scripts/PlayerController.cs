using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    //private PlayerActions playerActions;
    private Rigidbody2D rb;
    private Animator animator;

    //Movement
    private float horizontal;
    public float speed;
    private float speedMultiplier;
    public float acceleration;
    public float airControl;
    public float airSpeed;
    public bool canMove;
    //Jump
    private bool isJumping;
    public float jumpForce;

    //Wall Slide & Jump
    bool isWallTouch;
    private bool isSliding;
    public Transform wallCheck;
    private float slidingSpeed;
    bool wallJumping;
    public Vector2 wallJumpForce;
    public float wallJumpDuration;
    private float wallJumpDirection;

    //Character
    private float health;
    private int mana;
    private int attackMana;
    private int invisibleMana;
    private int dashmana;
    private int teleportmana;
    private int selectedAbility;

    LayerMask groundLayer;
    public Transform groundCheck;
    private bool onGround;

    public GameObject background;

    private GameObject attackObject;
    private GameObject invisibleObject;
    private GameObject teleportObject;

    SpriteRenderer attackRenderer;
    SpriteRenderer invisibleRenderer;
    SpriteRenderer teleportRenderer;

    private Vector2 ability1pos;
    private Vector2 ability2pos;
    private Vector2 ability3pos;

    private Color abilityColor;
    private Color abilityUsedColor;

    private GameObject hpbar;
    private UnityEngine.UI.Image hpbarimage;
    private void Awake()
    {
        //playerActions = new PlayerActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = 6;
        jumpForce = 5.0f;
        groundLayer = LayerMask.GetMask("Ground");
        speedMultiplier = 0;
        wallJumping = false;
        horizontal = 0;
        slidingSpeed = 1;
        canMove = true;

        health = 100;
        attackMana = 3;
        invisibleMana = 5;
        dashmana = 3;
        teleportmana = 5;
        mana = 5;

        attackObject = GameObject.Find("sghudattack1");
        invisibleObject = GameObject.Find("sghudghost1");
        teleportObject = GameObject.Find("sghudfast1");//isimlendirme ayarlanacak

        attackRenderer = attackObject.GetComponent<SpriteRenderer>();
        invisibleRenderer = invisibleObject.GetComponent<SpriteRenderer>();
        teleportRenderer = teleportObject.GetComponent<SpriteRenderer>();

        ability1pos = new Vector3(0, 155, 0);
        ability2pos = new Vector3(-30, 185, 0);
        ability3pos = new Vector3(30, 185, 0);

        abilityColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        abilityUsedColor = new Color(1,1,1);

        hpbar = GameObject.Find("HPBar");
        hpbarimage = hpbar.GetComponent<UnityEngine.UI.Image>();
        hpbarimage.fillAmount = 0;
        HpBarUpdate();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
            if (isJumping)
            {
                Jump();
            }

            if (isSliding)
            {
                Slide();
            }

            if (wallJumping)
            {
                rb.velocity = new Vector2(wallJumpDirection * wallJumpForce.x, wallJumpForce.y);
                wallJumping = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        onGround = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.4f, 0.15f), 0, groundLayer);
        SlideCheck();
        animator.SetBool("isSliding", isSliding);
        animator.SetBool("isOnAir", !onGround);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void updateSpeed()//speed multiplierin deðerini sürekli arttýrarak yumuþak hareket edilmesi saðlanýr.
    {
        if (speedMultiplier < 1)
        {
            speedMultiplier++;
        }
    }

    private void InputCheck()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
        flip();
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Teleport();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Inivisible();
        }

        if (Input.GetMouseButtonDown(1))
        {
            applyDamage(10);
        }
    }

    private void flip()
    {
        if (horizontal < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
        if (horizontal > 0.01f) transform.localScale = new Vector3(1, 1, 1);
    }

    private void Move()
    {
        animator.SetBool("isMove", false);
        if (onGround)
        {
            if (horizontal != 0)
            {
                animator.SetBool("isMove", true);
            }
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        }
        else
        {
            if (!isSliding)
            {
                rb.AddForce(new Vector2(horizontal * airControl, 0));
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -airSpeed, airSpeed), rb.velocity.y);
            }
        }
    }

    public void Jump()
    {
        if (onGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        else if (isSliding)
        {
            wallJumping = true;
            wallJumpDirection = -transform.localScale.x;
            Invoke("StopWallJump", wallJumpDuration);
        }
        isJumping = false;
    }

    private void SlideCheck()
    {
        isWallTouch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.1f, 0.75f), 0, groundLayer);

        if (isWallTouch && !onGround)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }
        flip();
    }

    private void Slide()
    {
        animator.SetBool("isSliding", true);
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
    }

    private void StopWallJump()
    {
        wallJumping = false;
    }

    public void applyDamage(float dmg)
    {
        if (health > 0)
        {
            health -= dmg;
            HpBarUpdate();
        }
        else
        {
            health = 0;
            gameOver();
        }
    }

    private void HpBarUpdate()
    {
        hpbarimage.fillAmount = health;
    }
    public void takeMana(int soul)
    {
        if (mana <= 5)
        {
            mana += soul;
        }
        else
        {
            mana = 5;
        }
    }

    private void loseMana(int soul)
    {
        if (mana > 0)
        {
            mana -= soul;
        }
        else
        {
            mana = 0;
        }
    }

    private void Attack()
    {
       // loseMana(attackMana);
        //animator.SetBool("isAttack", true);
        //selectedAbility = 1;
        //AbilityHudUpdate();
    }

    private void Inivisible()
    {
       // loseMana(invisibleMana);
       // selectedAbility = 2;
       // AbilityHudUpdate();
    }

    private void Dash()
    {
      //  loseMana(dashmana);
      //  selectedAbility = 1;
      //  AbilityHudUpdate();
    }

    private void Teleport()
    {
      //  loseMana(teleportmana);
      //  selectedAbility = 3;
      //  AbilityHudUpdate();
    }

    private void AbilityHudUpdate()
    {
        if(selectedAbility == 1)
        {
            attackRenderer.color = abilityUsedColor;
            invisibleRenderer.color = abilityColor;
            teleportRenderer.color = abilityColor;

            attackObject.transform.localPosition = ability1pos;
            invisibleObject.transform.localPosition = ability2pos;
            teleportObject.transform.localPosition = ability3pos;         
        }
        else if(selectedAbility == 2)
        {
            attackRenderer.color = abilityColor;
            invisibleRenderer.color = abilityUsedColor;
            teleportRenderer.color = abilityColor;

            attackObject.transform.localPosition = ability3pos;
            invisibleObject.transform.localPosition = ability1pos;
            teleportObject.transform.localPosition = ability2pos;
        }
        else if( selectedAbility == 3)
        {
            attackRenderer.color = abilityColor;
            invisibleRenderer.color = abilityColor;
            teleportRenderer.color = abilityUsedColor;

            attackObject.transform.localPosition = ability2pos;
            invisibleObject.transform.localPosition = ability3pos;
            teleportObject.transform.localPosition = ability1pos;
        }
        else
        {

        }
    }
    private void gameOver()
    {

    }
}
