using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    int speed = 1;
    Rigidbody rb;
    [SerializeField] Transform childBody;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.SetDamage(30);
           
        }

        Destroy(gameObject);
    }
}
