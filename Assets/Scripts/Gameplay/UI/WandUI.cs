using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WandUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> uis = new List<GameObject>();
    private List<SpriteRenderer> instantiatedCanvas = new List<SpriteRenderer>();
    [SerializeField] private float yOffset = 0.4f;
    [SerializeField] private float duration = 0.5f;

    private float rotateBy;
    private float curRotation = 0;
    private int index = 0;

    private void Start()
    {
        rotateBy = 360 / uis.Count;
        foreach (var go in uis)
        {
            GameObject o = Instantiate(go);
            o.transform.parent = transform;
            o.transform.localPosition = new Vector3(0, yOffset, 0);
            o.transform.parent = transform.parent;
            SpriteRenderer c = o.GetComponent<SpriteRenderer>();
            if (c == null)
            {
                Debug.LogError("Prefab does not have Canvas Group");
            }
            instantiatedCanvas.Add(c);
        }
        foreach (var c in instantiatedCanvas)
        {
            c.transform.parent = transform;
            c.color = new Color(c.color.r, c.color.g, c.color.b, 0);
            transform.Rotate(rotateBy, 0, 0);
        }
        instantiatedCanvas[index].DOFade(1, duration);
    }

    public void Cycle()
    {
        instantiatedCanvas[index].DOFade(0, duration);
        index = (index + 1) % uis.Count;
        curRotation += rotateBy;
        transform.DOLocalRotate(new Vector3(curRotation, 0, 0), duration, RotateMode.Fast).SetEase(Ease.InOutCirc);
        curRotation %= 360;
        instantiatedCanvas[index].DOFade(1, duration);
    }
}
