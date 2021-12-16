using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTarget : MonoBehaviour
{
    [SerializeField] private GameObject vfxExplosion;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Magic") {
			if (vfxExplosion) {
                Instantiate(vfxExplosion, transform.position, Quaternion.identity);
			}
            Destroy(this.gameObject);    
		}
    }

    public void kill(){
        // delay for lightning
        StartCoroutine(Delay(0.5f));
    }

    IEnumerator Delay(float t)
    {
        yield return new WaitForSeconds(t);
        if (vfxExplosion) {
            Instantiate(vfxExplosion, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);   
    }
}
