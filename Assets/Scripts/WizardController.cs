using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    WizardController controller;
    Rigidbody2D rb2d;
    Animator animator;

    
    [SerializeField] LayerMask layerMask;

    public float movePower;
    int direction;

    bool jumpKey;
    private int moveSpeed;
    public float jumpForce = 200f;
    public float firstSpeed = 16.0f;
    float gravity = 30.0f;
    float timer = 0f;

    public Transform attackPoint;
    public float attackRadius;
    public LayerMask enemyLayer;
    public int hp = 50;
    [System.NonSerialized]
    public int maxHp = 50;
   
   public int at = 4;

    static readonly float coolTime = 1f;
    float leftCoolTime;

    // 吹き飛び判定、吹き飛ぶ方向、吹き飛び時間、吹き飛び力、タイマー
    // 無敵時間、タイマー

    bool isKnockBack;
    Vector2 knockDirec;

    float knockTime, knockPower, knockCounter;
    float invincibleTime, invincibleTimeCounter;
    public bool push = false;
    public bool run = false;
    public float nextButtonDownTime = 0.3f;
    float nowTime = 0f;

    public float limitAngle = 3f;
    Vector3 runDirec = Vector3.zero;

    public float dashSpeed;
    private float dashCounter, activeMoveSpeed;


    enum STATUS
    {
        UP,
        GROUND,
        DOWN
    }

    STATUS status = STATUS.GROUND;


    void Start()
    {
        controller = GetComponent<WizardController>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        leftCoolTime = 0;

        activeMoveSpeed = moveSpeed;

        GameManager.instance.UpdateWizardHealthUI();
    }

    void Update()
    {
        // ステータスメニューが開いている時はゲームを一時停止する
        if (GameManager.instance.statusMenu.activeInHierarchy)
        {
            return;
        }

        jumpKey = Input.GetKey(KeyCode.W);

       
        
        if (invincibleTimeCounter > 0)
        {
            invincibleTimeCounter -= Time.deltaTime;
        }

        if (isKnockBack)
        {
            knockCounter -= Time.deltaTime;
            rb2d.velocity = knockDirec * knockPower;

            if (knockCounter <= 0)
            {
                isKnockBack = false;
            }
            else
            {
                return;
            }
        }
        
             
        
        leftCoolTime -= Time.deltaTime;

        if (leftCoolTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("attack");
                leftCoolTime = coolTime;
                Attack();
            }
        }
        Run();
        Jump();
    }

    void Jump()
    {
        Vector2 newVec = Vector2.zero;


        // 地面についている
        if (IsGround())
        {

            // Wキーを押したとき
            if (jumpKey && rb2d.velocity.y >= 0)
            {
                timer = Time.deltaTime;

                newVec.y = firstSpeed;
                newVec.y -= gravity * timer;
                // rb2d.AddForce(Vector2.up * jumpForce);
                animator.SetBool("isJump", true);

                WizardSE.instance.SE(WizardSE.WIZARDSE.JUMP);
            }
            else　　// 地面にはついているが、Wキーをおしていないとき
            {
                animator.SetBool("isJump", false);
            }
            rb2d.velocity = newVec;
        }
    }
    void Run()
    {
        Vector3 moveVelocity = Vector3.zero;
        animator.SetBool("isRun", false);

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            direction = -1;
            moveVelocity = Vector3.left;

            transform.localScale = new Vector3(direction, 1,1);
            animator.SetBool("isRun", true);
            if (!IsGround())
            {
                animator.SetBool("isJump",true);
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            direction = 1;
            moveVelocity = Vector3.right;

            transform.localScale = new Vector3(direction, 1,1);
            animator.SetBool("isRun", true);
            if (!IsGround())
            {
                animator.SetBool("isJump", true);
            }
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    // アニメーションのイベントで登録済
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
        foreach(Collider2D hitEnemy in hitEnemies)
        {
            Debug.Log("hit");
            hitEnemy.GetComponent<DeathBringerController>().OnDamage(at);
            WizardSE.instance.SE(WizardSE.WIZARDSE.ATTACK);

            GameManager.instance.UpdateBringerHealthUI();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

  
    
    public void OnDamage(int damage)
    {
        if(invincibleTime < 0)
        {
            hp -= Mathf.Clamp(hp - damage, 0, maxHp);
            invincibleTimeCounter = invincibleTime;

            if(hp == 0)
            {
                gameObject.SetActive(false);
            }
        }
        
        GameManager.instance.UpdateWizardHealthUI();
    }

    /// <summary>
    /// 吹き飛ばし用関数
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        knockCounter = knockTime;
        isKnockBack = true;

        knockDirec = transform.position - position;
        knockDirec.Normalize();
    }
    bool IsGround()
    {
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.left * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;

        Debug.DrawLine(leftStartPoint, endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);

        return Physics2D.Linecast(leftStartPoint, endPoint,layerMask) || Physics2D.Linecast(rightStartPoint,endPoint,layerMask);
      
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(rb2d.velocity.y == 0 && collision.gameObject.tag == "Ground")
        {
            timer = 0;
        }
    }    
}
