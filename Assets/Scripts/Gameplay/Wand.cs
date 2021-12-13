using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Wand : MonoBehaviour
{
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private List<Vector3> firepointPos = new List<Vector3>();
    [SerializeField] private ShootProjectile shooter;
    [SerializeField] private WandUI ui;

    private int index = 0;

    private void Start()
    {
        shooter.setProjectile(projectiles[index]);
    }

    public void Cycle()
    {
        index = (index + 1) % projectiles.Count;
        shooter.setProjectile(projectiles[index]);
        shooter.setFirePoint(firepointPos[index]);
        ui.Cycle();
    }
}
