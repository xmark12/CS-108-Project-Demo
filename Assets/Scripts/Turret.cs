using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float speed = 1.0f;
    public float aggroRange = 0;
    public float lineOfSight = 0;
    public float fireRate = 0.5f;
    private float nextFireTime;
    public GameObject bullet;
    public GameObject bulletOrigin;
    private Transform player;
    public FieldOfView fov;

    private Transform aimTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aimTransform = transform.Find("Aim");
    }

    // Update is called once per frame
    void Update()
    {
        aggroRange = Vector2.Distance(player.position, transform.position);

        if (aggroRange < lineOfSight && nextFireTime < Time.time && fov.playerDetected)
        {
            //transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            Shoot();
            nextFireTime = Time.time + fireRate;

            Vector3 aimDirection = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, bulletOrigin.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }
}
