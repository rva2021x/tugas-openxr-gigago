using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private GameObject currentProjectile;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    private Vector3 destination;
    [SerializeField] private float maxDistance = 100f;

    public void setProjectile(GameObject projectile)
    {
        currentProjectile = projectile;
    }
    public void setFirePoint(Vector3 pos)
    {
        firePoint.localPosition = pos;
    }

    public void Shoot()
    {
        if (this.GetComponentInChildren<Projectile>() == null)
        {
            var projectileObj = Instantiate(currentProjectile, firePoint.position, Quaternion.identity) as GameObject;
            Ray ray = new Ray(firePoint.position, firePoint.transform.right);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                destination = hit.point;
            }
            else
            {
                destination = ray.GetPoint(maxDistance);
            }

            if (currentProjectile.GetComponent<Projectile>().magicType == Projectile.Magic.lightning)
            {
                projectileObj.transform.GetChild(0).transform.position = firePoint.position;
                projectileObj.transform.GetChild(1).transform.position = destination;
                if(hit.collider != null && hit.collider.gameObject.tag == "TrainingTarget"){
                    TrainingTarget target = hit.collider.GetComponent<TrainingTarget>();
                    target.kill();
                }
            }
            else if (currentProjectile.GetComponent<Projectile>().magicType == Projectile.Magic.wind)
            {
                projectileObj.transform.parent = firePoint.parent;
                projectileObj.transform.rotation = Quaternion.LookRotation((destination - firePoint.position).normalized, Vector3.up);
            }
            else
            {
                projectileObj.transform.rotation = Quaternion.LookRotation((destination - firePoint.position).normalized, Vector3.up);
                projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
            }
        }
    }
}
