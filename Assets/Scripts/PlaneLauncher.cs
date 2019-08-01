using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLauncher : MonoBehaviour
{
    [SerializeField] GameObject planePrefab;


    public void SpawnPlane()
    {
        GameObject targetPlane = SimplePool.Spawn(planePrefab, Vector2.one, Quaternion.identity);
        targetPlane.transform.parent = gameObject.transform;
        MainLogic.instance.CurrentPlane = targetPlane;

    }


    private void Start()
    {
        SpawnPlane();

    }

}
