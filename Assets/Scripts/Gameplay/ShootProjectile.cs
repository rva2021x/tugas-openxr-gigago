using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject fireProjectile;
    public GameObject iceProjectile;
    public GameObject windProjectile;
    public GameObject lightningProjectile;
    [SerializeField] private GameObject currentProjectile;
    public Transform firePoint;
    public float projectileSpeed = 5f;
    private Vector3 destination;
    [SerializeField] private float maxDistance = 100f;

    public void useFireMagic(){
        currentProjectile = fireProjectile;
    }
    public void useIceMagic(){
        currentProjectile = iceProjectile;
    }

    public void useWindMagic(){
        currentProjectile = windProjectile;
    }

    public void useLightningMagic(){
        currentProjectile = lightningProjectile;
    }

    public void Shoot(){
        if (this.GetComponentInChildren<Projectile>() == null){
            var projectileObj = Instantiate(currentProjectile, firePoint.position, Quaternion.identity) as GameObject;
            Ray ray = new Ray(firePoint.position, firePoint.transform.right);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, maxDistance)){
                destination = hit.point;
            }else{
                destination = ray.GetPoint(maxDistance);
            }

            if(currentProjectile.GetComponent<Projectile>().magicType == Projectile.Magic.lightning){
                projectileObj.transform.GetChild(0).transform.position = firePoint.position;
                projectileObj.transform.GetChild(1).transform.position = destination;
            }else if(currentProjectile.GetComponent<Projectile>().magicType == Projectile.Magic.wind){
                projectileObj.transform.localRotation = Quaternion.LookRotation((destination - firePoint.position).normalized, Vector3.up);
                projectileObj.transform.parent = firePoint.parent;
                projectileObj.transform.localPosition += new Vector3(4, 0, 0);
            }
            else{
                projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
            }
        }  
    }
}
