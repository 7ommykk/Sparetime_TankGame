using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    // 引用
    private SpriteRenderer sr;

    public GameObject explosionPrefab;

    public Sprite BrokenSprite;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent < SpriteRenderer > ();
    }


    public void Die()
    {
        sr.sprite = BrokenSprite;
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        PlayerManager.Instance.isDefeat = true;
    }

}
