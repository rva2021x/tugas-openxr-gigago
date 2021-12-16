using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Magic{
        fire,
        ice,
        wind,
        lightning,
        enemy
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
        if(magicType == Magic.enemy) {
            if(other.transform.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour player)) {
                player.health -= 10;
			}
		}
    }

	private void OnTriggerStay(Collider other) {
		if(magicType == Magic.wind) {
            if(other.TryGetComponent<MonsterBehaviour>(out MonsterBehaviour mb)) {
                mb.TakeDamage(5f * Time.deltaTime);
			}
		}
	}
}
