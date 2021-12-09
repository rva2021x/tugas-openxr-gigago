using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Magic{
        fire,
        ice,
        wind,
        lightning
    }

    public Magic magicType;
    public float destroyTime = 5f;

    void Start(){
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
