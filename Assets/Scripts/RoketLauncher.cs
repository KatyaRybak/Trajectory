using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoketLauncher : MonoBehaviour
{
    [SerializeField] GameObject rocketPrefab;
    Vector2 spawnPosition, leadPoint;



    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        int rocketCount = GetComponentsInChildren<Rocket>().Length;
        int planeCount = FindObjectsOfType<Plane>().Length;
        if (rocketCount > 0)
        {
            return;
        } 
        else if (rocketCount <= planeCount)
        {
            RotateAndFire(MainLogic.instance.LeadPoint);
        }
    }

    public void RotateAndFire(Vector2 deployPos)
    {

        float turretAngle = MainLogic.instance.SetRotation(); 
        transform.localRotation = Quaternion.Euler(0, 0, turretAngle); 

        FireRocket(deployPos, turretAngle);//launch rocket
    }

    void FireRocket(Vector3 deployPos, float turretAngle)
    {
        float deployDist = Vector3.Distance(deployPos, transform.position);//how far is our target

        GameObject firedRocket = SimplePool.Spawn(rocketPrefab, transform.position, Quaternion.Euler(0, 0, turretAngle));
        firedRocket.transform.parent = transform;
        Rigidbody2D rocketRb = firedRocket.GetComponent<Rigidbody2D>();
        Rocket rocketScript = firedRocket.GetComponent<Rocket>();
        float rocketSpeed = rocketScript.speed;
        rocketRb.velocity = rocketSpeed * firedRocket.transform.up;//rocket is rotated in necessary direction already

    }
}
