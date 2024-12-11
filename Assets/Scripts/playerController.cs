using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class playerController : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float forceJump;
    private bool faceingRight;
    private bool isjumping;
    private bool isGrounded;
    public bool isDefense;

    public float punchDemage;
    public float kickDemage;
    public bool isDie;

    private NewBehaviourScript myHealth;
    private EnemyControler GetEnemy;

    private Camera mainCamera;

    private ChracterAnimation myAnim;
    [SerializeField]
    private BarStat healthBar;


    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<ChracterAnimation>();
        myHealth = GetComponent<NewBehaviourScript>();
        mainCamera = Camera.main;
        healthBar.bar = GameObject.FindGameObjectWithTag(Tags.Player_Health_Bar).GetComponent<BarScript>();
        healthBar.Initialize();
    }

    void Start()
    {
        healthBar.MaxVal = myHealth.maxHealth;
        faceingRight = true;
        GetEnemy = GameObject.FindGameObjectWithTag(Tags.Enemy_Tag).GetComponent<EnemyControler>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.CurrentVal = myHealth.healt;
        CheckUserInput();
        DeadChecker();
        ClampPositionWithinCamera();
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
        Flip(horizontal);
    }
    private void HandleMovement(float horizontal)
    {
        if (isDie)
        {
            return;
        }
        myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        myAnim.Walk(horizontal);
    }

    private void ClampPositionWithinCamera()
    {
        Vector3 pos = transform.position;
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        pos.x = Mathf.Clamp(pos.x, -screenBounds.x, screenBounds.x);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y, screenBounds.y);
        transform.position = pos;
    }

    private void Flip(float horizontal)
    {
        if (isDie)
        {
            return;
        }
        if (horizontal > 0 && !faceingRight || horizontal < 0 && faceingRight)
        {
            faceingRight = !faceingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    void DeadChecker()
    {

        if (isDie)
        {
            return;
        }

        if (myHealth.healt <= 0) 
        {
            isDie = true;
            myAnim.Die(isDie);
        }

    }
    private void CheckUserInput()
    {
        if (isDie)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            myRigidbody.AddForce(new Vector2(0, forceJump));
            myAnim.Jump(true);
            isjumping = true;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            isDefense = true;
            myAnim.Defense(isDefense);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isDefense = false;
            myAnim.Defense(isDefense);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Ground")
        {
            myAnim.Jump(false);
            isGrounded = true;
            isjumping = false;
        }

        if (isDie)
        {
            return;
        }

        if (collision.tag == Tags.Punch_Attack_Tag && !isDefense)
        {
            myAnim.Hurt();
            myHealth.healt -= GetEnemy.punchDemage;

        }
        if (collision.tag == Tags.Kick_Attack_Tag && !isDefense)
        {
            myAnim.Hurt();
            myHealth.healt -= GetEnemy.kickDemage;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGrounded = false;
        }
    }

}
