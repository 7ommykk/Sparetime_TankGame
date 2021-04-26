using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 属性值，移动速度，计时器，被动
    public float moveSpeed = 3;
    private Vector3 bulletEulerAngles;
    private float timeVal;
    private float defendTimeVal = 3;
    private bool isDefended = true;

    // 引用：Sprite对象，坦克移动方向，顺序：上 右 下 左
    private SpriteRenderer sr;
    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;

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
        // 保护是否处于无敌, 这里2020版有个坑，需要把Player下面的Animator关掉就行
        if(isDefended)
        {
            defendEffectPrefab.SetActive(true);
            defendTimeVal -= Time.deltaTime;
            if( defendTimeVal <= 0 )
            {
                isDefended = false;
                defendEffectPrefab.SetActive(false);
            }
        }
    }

    // 固定物理帧，0.02秒执行一次，Update后运行
    private void FixedUpdate()
    {
        // 如果游戏失败，禁止玩家一切行为
        if(PlayerManager.Instance.isDefeat)
        {
            return;
        }
        Move();
        
        // 攻击CD
        if(timeVal >= 0.4f)
        {
            Attack();
        }
        else
        {
            timeVal += Time.fixedDeltaTime;
        }
    }

    // 坦克的攻击方法
    private void Attack()
    {
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            // 子弹产生的角度：
            // Instantiate(bulletPrefab, transform.position, transform.rotation);
            //子弹产生的角度： 当前坦克的角度+子弹应该旋转的角度
            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
            timeVal = 0;
        }
    }

    // 坦克移动方法
    private void Move()
    {
        // Time.deltaTime 每一秒移动， 否则按每一帧移动。
        // Space.World 以世界坐标轴移动
        float v = Input.GetAxisRaw("Vertical");
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

        float h = Input.GetAxisRaw("Horizontal");
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
        if(isDefended)
        {
            return;
        }
        PlayerManager.Instance.isDead = true;

        // 产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        // 死亡
        Destroy(gameObject);
    }
}
