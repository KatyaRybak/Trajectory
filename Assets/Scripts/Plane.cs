using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    Vector2 bl, br, tl, tr;//bottom,top
    public Vector2 startPosition, lastPosition;
    public float planeSpeed;
    float minSpeed, maxSpeed;
    float deployDistance;
    Rigidbody2D planeRb;
    Trajectory trajectory;
    Coroutine timerCoroutine;

    private void Start()
    {
        minSpeed = 0.2f;
        maxSpeed = 0.4f;
        trajectory = (Trajectory)UnityEngine.Random.Range(0, 2);
        Launch();
       // timerCoroutine = StartCoroutine(ChangePlaneVelosity());
        float lenght = (lastPosition.x - startPosition.x);
        
    }

    private void OnEnable()
    {
        trajectory = (Trajectory)UnityEngine.Random.Range(0, 3);
        Launch();
        StartCoroutine(ChangePlaneVelosity());
        float lenght = (lastPosition.x - startPosition.x);
        
    }

    public void Launch()
    {//place the asteroid in top with random x & launch it to bottom with random x
        bl = Camera.main.ViewportToWorldPoint(new Vector2(0, 0.5f));
        br = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f));
        tl = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        tr = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //transform.localScale = Vector2.one * (0.2f + UnityEngine.Random.Range(1f, 3f));
        planeSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        startPosition.x = bl.x;
        startPosition.y = UnityEngine.Random.Range(bl.y, tl.y);
        lastPosition.y = br.y;
        lastPosition.x = br.x;
        transform.position = SetTrajectory(startPosition.x);
 
        deployDistance = Vector3.Distance(startPosition, lastPosition);//after traveling this much distance, return to pool
    }

    IEnumerator ChangePlaneVelosity()
    {
        while (true)
        {
            yield return new WaitForSeconds(planeSpeed);
            Vector2 velocity = planeSpeed * (SetTrajectory(transform.position.x+ planeSpeed) - (Vector2)transform.position).normalized;
            //Vector2 velocity = planeSpeed * ((lastPosition - startPosition).normalized);

            planeRb = GetComponent<Rigidbody2D>();
            planeRb.velocity = velocity;//set a velocity to rigidbody to set it in motion
        }
    }

    public Vector2 SetTrajectory(float x)
    {
        switch (trajectory)
        {
            case Trajectory.Linear:
                return TrajectoryFunction.LineMove(x);
            case Trajectory.Sinusoid:
                return TrajectoryFunction.SinusMove(x);
            case Trajectory.Parabola:
                return TrajectoryFunction.ParabolaMove(x);
            default:
                return (Vector2)transform.position+new Vector2(1,1);
        }
               
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, startPosition) > deployDistance)
        {//once we have traveled the set distance, return to pool
            ReturnToPool();
            GetComponentInParent<PlaneLauncher>().SpawnPlane();
        }
        transform.up = GetComponent<Rigidbody2D>().velocity;
    }

    private void ReturnToPool()
    {
        SimplePool.Despawn(gameObject);
    }

    void OnTriggerEnter2D(Collider2D projectile)
    {
        if (projectile.gameObject.CompareTag("rocket"))
        {//check collision with rocket, return to pool
            ReturnToPool();
            GetComponentInParent<PlaneLauncher>().SpawnPlane();
        }
    }

}
