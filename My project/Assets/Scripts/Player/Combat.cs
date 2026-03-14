using UnityEngine;

public class Combat : MonoBehaviour
{

    public float damage = 45f;
    public float range = 100f;
    public float impactForce = 100f;
    public float playerForce = 200f;
    public float fireRate = 2f;

    private float nextTimeToFire = 0f;

    public Rigidbody player;
    public GameObject impactEffect;
    public ParticleSystem muzzleFlash;
    public Camera fpsCam;

    public UIManager manager;

    public Health health;

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }    
    }

    void Shoot()
    {
        if (Time.timeScale > 0f)
        {
            AudioManager.Instance.PlaySFX("shooting");
            muzzleFlash.Play();
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();

                if (player != null)
                {
                    player.AddForce(-fpsCam.transform.forward * playerForce);
                }

                if (enemy != null)
                {
                    manager.showHitMarker();
                    AudioManager.Instance.PlaySFX("hit");
                    enemy.TakeDamage(damage);
                    health.takeHp(5);
                }



                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2);
            }

        }
    }

}
