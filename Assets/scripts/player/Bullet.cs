using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float speed;

    private Rigidbody rigBody;
    private Vector3 mySpeed;
    private float lifeTime = 10;

    void Start()
    {
        rigBody = GetComponent<Rigidbody>();
        //rigBody.velocity = transform.forward * speed;
        rigBody.velocity = mySpeed;

        Destroy(gameObject, lifeTime);
    }

    public void setSpawnerSpeed(Vector3 spawnerSpeed)
    {
        mySpeed = spawnerSpeed + (transform.forward * speed);
    }
}