using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    private ChracterAnimation enemyAnim;
    private Rigidbody2D enemyRigidBody;
    [SerializeField]
    private float movementSpeed;
    private Transform playerTransform;
    private bool followPlayer;
    private bool attackPlayer;
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private float chasePlayerAfterAttack;

    private float currentAttackTimer;
    [SerializeField]
    private float deffaultAttackTimer;

    private Collider2D playerCollider;

    [SerializeField]
    private GameObject punch1AttackPoint;
    [SerializeField]
    private GameObject punch2AttackPoint;
    [SerializeField]
    private GameObject kickAttackPoint;



    public float punchDemage;
    public float kickDemage;
    public bool isDie;

    private NewBehaviourScript myHealth;
    private playerController GetPlayer;

    [SerializeField]
    private BarStat healthBar;

    // Start is called before the first frame update
    private void Awake()
    {
        myHealth = GetComponent<NewBehaviourScript>();
        playerCollider = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<Collider2D>();
        enemyAnim = GetComponent<ChracterAnimation>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag(Tags.Player_Tag).transform;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
        healthBar.bar = GameObject.FindGameObjectWithTag(Tags.Enemy_Health_Bar).GetComponent<BarScript>();
        healthBar.Initialize();
    }
    void Start()
    {
        healthBar.MaxVal = myHealth.maxHealth;
        followPlayer = true;
        currentAttackTimer = deffaultAttackTimer;
        GetPlayer = GameObject.FindGameObjectWithTag(Tags.Player_Tag).GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.CurrentVal = myHealth.healt;
        FacingToTarget();
        DeadChecker();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        AttackPlayer();
    }

    void FollowPlayer()
    {
        if (!followPlayer || isDie)
            return;

        if (Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance)
        {
            if (playerTransform.transform.position.x < transform.position.x)
            {
                enemyRigidBody.velocity = new Vector2(-1 * movementSpeed, enemyRigidBody.velocity.y);
            }
            else
            {
                enemyRigidBody.velocity = new Vector2(1 * movementSpeed, enemyRigidBody.velocity.y);
            }
            if (enemyRigidBody.velocity.sqrMagnitude != 0)
            {
                enemyAnim.Walk(1);
            }
        }

        else if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= attackDistance)
        {
            enemyRigidBody.velocity = Vector2.zero;
            enemyAnim.Walk(0);
            followPlayer = false;
            attackPlayer = true;
        }

    }

    void AttackPlayer()
    {
        if (!attackPlayer || isDie)
            return;

        currentAttackTimer -= Time.deltaTime;
        if (currentAttackTimer <= 0)
        {
            Attack(Random.Range(0, 5));
            currentAttackTimer = deffaultAttackTimer;
        }


        if (Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance + chasePlayerAfterAttack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    void FacingToTarget()
    {
        if (isDie)
        {
            return;
        }
        Vector3 theScale = transform.localScale;

        if (playerTransform.position.x < transform.position.x)
        {
            theScale.x = -1;
        }
        else
        {
            theScale.x = 1;
        }

        transform.localScale = theScale;
    }

    IEnumerator Punch_1(float time)
    {
        yield return new WaitForSeconds(time);
        enemyAnim.Punch1();
    }

    IEnumerator Punch_2(float time)
    {
        yield return new WaitForSeconds(time);
        enemyAnim.Punch2();
    }

    IEnumerator Kick(float time) 
    { 
        yield return new WaitForSeconds(time);
        enemyAnim.Kick();
    }

    void Attack(int i)
    {
        switch (i)
        {
            case 0:
                StartCoroutine(Punch_1(0.1f));
                break;
            case 1:
                StartCoroutine(Punch_1(8.1f));
                StartCoroutine(Kick(0.3f));
                StartCoroutine(Punch_2(0.3f));
                break;
            case 2:
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Kick(0.3f));
                StartCoroutine(Punch_2(8.3f));
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Punch_2(8.3f));
                break;
            case 3:
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Punch_2(8.3f));
                StartCoroutine(Kick(0.6f));
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Kick(0.6f));
                break;
            case 4:
                StartCoroutine(Kick(8.1f));
                StartCoroutine(Punch_1(0.1f));
                StartCoroutine(Kick(0.3f));
                StartCoroutine(Punch_1(0.1f));
                break;
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
            enemyAnim.Die(isDie);
        }

    }

    public void ActivatePunch1()
    {
        punch1AttackPoint.SetActive(true);
    }

    public void ActivatePunch2()
    {
        punch2AttackPoint.SetActive(true);
    }

    public void ActivateKick()
    {
        kickAttackPoint.SetActive(true);
    }

    public void DeactivatePunch1()
    {
        punch1AttackPoint.SetActive(false);
    }

    public void DeactivatePunch2()
    {
        punch2AttackPoint.SetActive(false);
    }

    public void DeactivateKick()
    {
        kickAttackPoint.SetActive(false);
    }

    public void DeactivateAllAttack()
    {
        punch1AttackPoint.SetActive(false);
        punch2AttackPoint.SetActive(false);
        kickAttackPoint.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie)
        {
            return;
        }

        if (collision.tag == Tags.Punch_Attack_Tag)
        {
            enemyAnim.Hurt();
            myHealth.healt -= GetPlayer.punchDemage;
        }
        if (collision.tag == Tags.Kick_Attack_Tag)
        {
            enemyAnim.Hurt();
            myHealth.healt -= GetPlayer.kickDemage;
        }

    }
}
