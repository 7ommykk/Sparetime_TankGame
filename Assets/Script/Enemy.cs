using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 属性值，移动速度，计时器，被动
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float v = -1;
    private float h;

    // 引用：Sprite对象，坦克移动方向，顺序：上 右 下 左
    private SpriteRenderer sr;
    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    //计时器
    private float timeVal;
    private float timeValChangeDirection = 0;

    private void Awake()
    {
        sr = GetComponent < SpriteRenderer > ();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 攻击的时间间隔
        if(timeVal >= 3)
        {
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    // 固定物理帧，0.02秒执行一次，Update后运行
    private void FixedUpdate()
    {
        Move();
    }

    // 坦克的攻击方法
    private void Attack()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
        timeVal = 0;
    }

    // 坦克移动方法
    private void Move()
    {
        if(timeValChangeDirection >= 4)
        {
            int num = Random.Range(0, 8);
            if(num > 5)
            {
                v = -1;
                h = 0;
            }
            else if(num == 0)
            {
                v = 1;
                h = 0;
            }
            else if(num > 0 && num <= 2)
            {
                h = -1;
                v = 0;
            }
            else if(num >2 && num <= 4)
            {
                h = 1;
                v = 0;
            }

            timeValChangeDirection = 0;

        }
        else
        {
            timeValChangeDirection += Time.fixedDeltaTime;
        }

        transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);
        if( v < 0 )
        {
            sr.sprite = tankSprite[2];
            bulletEulerAngles = new Vector3(0, 0, -180);
        }

        else if ( v > 0 )
        {
            sr.sprite = tankSprite[0];
            bulletEulerAngles = new Vector3(0, 0, 0);
        }
        // 禁止斜着走
        if( v != 0 )
        {
            return;
        }

        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);
        // 改坦克方向
        if( h < 0 )
        {
            sr.sprite = tankSprite[3];
            bulletEulerAngles = new Vector3(0, 0, 90);
        }

        else if ( h > 0 )
        {
            sr.sprite = tankSprite[1];
            bulletEulerAngles = new Vector3(0, 0, -90);
        }
    }

    // 坦克死亡方法
    private void Die()
    {
        // 产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        // 死亡
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Enemy")
        {
            timeValChangeDirection = 4;
        }
        else if(collision.gameObject.tag == "Barrier")
        {
            timeValChangeDirection = 4;
        }
        else if(collision.gameObject.tag == "Wall")
        {
            timeValChangeDirection = 4;
        }
    }

}
