using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Rigidbody2D enemyRb;

    public float speed = 20f;
    // Start is called before the first frame update
    
    void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.velocity = transform.right * speed;
    }
}
