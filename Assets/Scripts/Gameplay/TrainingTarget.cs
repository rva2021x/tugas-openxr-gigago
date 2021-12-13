using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTarget : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Magic")
            Destroy(this.gameObject);    
    }

    public void kill(){
        // delay for lightning
        StartCoroutine(Delay(0.5f));
    }

    IEnumerator Delay(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(this.gameObject);   
    }
}
