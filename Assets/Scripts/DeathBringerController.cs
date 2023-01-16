using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI
    ;
public class DeathBringerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator bringerAnim;
    public LayerMask layerMask;

    //TODO.プレイヤーを追いかける
    //TODO.一定距離に近づいたら攻撃する
    //TODO.ジャンプの実装
    //TODO.自分からステージ上から落ちないようにする
    //TODO.攻撃を受けたらHPが減る＆アニメーター起動

    public int hp = 25;
    Transform target;
    public Transform attackPoint;
    public float attackRadius;

    [SerializeField]
    private GameObject damageEffect;


    Vector2 moveDir;
    public PolygonCollider2D moveArea;


    float chaseSpeed, rangeToAttack;
    [SerializeField]
    float waitAfterHitting;
    public int at;
    //  public float coolTime;

    float knockTime, knockCounter, knockPower;
    Vector2 knockDir;
    bool isKnockBack;


    enum STATE
    {
        IDLE,
        HURT,
        WALK,
        SPELL,
        ATTACK,
        CAST,
        DEATH
    }

    STATE state = STATE.IDLE;
    private void Start()
    {
        bringerAnim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        target = GameObject.Find("Wizard").transform;
        bringerAnim.SetBool("Idle", true);
    }


    private void Update()
    {
        TurnOffTrigger();

        Chase();



        transform.position = new Vector3(Mathf.Clamp(transform.position.x, moveArea.bounds.min.x + 1, moveArea.bounds.max.x - 1),
        Mathf.Clamp(transform.position.y, moveArea.bounds.min.y + 1, moveArea.bounds.max.y - 1), transform.position.z);
    }



    void Chase()
    {


        moveDir = target.transform.position - transform.position;
        moveDir.Normalize();

        rb2d.velocity = moveDir * chaseSpeed;
        bringerAnim.SetBool("Walk", true);


        if (DistanceToPlayer() < rangeToAttack)
        {
            Attack();
        }
    }




    public void Attack()
    {
        TurnOffTrigger();
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, layerMask);
        foreach (Collider2D hit in hitPlayer)
        {
            hit.GetComponent<WizardController>().OnDamage(at);
        }

        if (DistanceToPlayer() < 2)
        {
            bringerAnim.SetTrigger("attack");
        }
    }



    void Die()
    {
        hp = 0;
        bringerAnim.SetTrigger("Death");
        Destroy(this.gameObject, 1f);
    }


    public void TurnOffTrigger()
    {
        bringerAnim.SetBool("Hurt", false);
        bringerAnim.SetBool("Attack", false);
        bringerAnim.SetBool("Walk", false);
        bringerAnim.SetBool("Spell", false);
        bringerAnim.SetBool("Death", false);
        bringerAnim.SetBool("Cast", false);
    }
    float DistanceToPlayer()
    {

        return Vector3.Distance(transform.position, target.transform.position);
    }
    public void OnDamage(int damage)
    {
        hp -= damage;
        bringerAnim.SetTrigger("Hurt");

        var obj = Instantiate(damageEffect, transform.position, transform.rotation);

        Destroy(obj, 1.4f);

        if (hp <= 0)
        {
            Die();
        }
    }
    bool IsGround()
    {
        Vector3 fromLeftRay = transform.position - Vector3.right * 0.2f;
        Vector3 fromRightRay = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;

        return Physics2D.Linecast(fromLeftRay, endPoint, layerMask) || Physics2D.Linecast(fromRightRay, endPoint, layerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            WizardController wizard = collision.gameObject.GetComponent<WizardController>();

            wizard.KnockBack(transform.position);
            wizard.OnDamage(at);

            bringerAnim.SetBool("Walk", false);
        }
    }



    public void KnockBack(Vector3 position)
    {
        isKnockBack = true;
        knockCounter = knockTime;
        knockDir = transform.position - position;
        knockDir.Normalize();
        bringerAnim.SetBool("Walk", false);
    }

    public void TakeDamage(int damage, Vector3 position)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            KnockBack(position);
        }
    }
}
