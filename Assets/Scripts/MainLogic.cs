using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject launcher, leadPointPrefab;

    float aiPollTime, groundProximity;
    float rocketSpeed;

    GameObject leadPoint;
    GameObject currentPlane;

    Vector2 leadPointPosition;
    public static MainLogic instance;

    public GameObject CurrentPlane
    {
        get { return currentPlane; }
        set { currentPlane = value; }
    }

    public Vector2 LeadPoint
    {
        get { return leadPointPosition; }
    }

    private void Awake()
    {
        if (FindObjectsOfType<MainLogic>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
  
    private void Start()
    {
        aiPollTime = 0;
        groundProximity = 0;
        StartCoroutine(FindLeadPoint());
    }

    public float SetRotation()
    {
        float turretAngle = Mathf.Atan2(leadPointPosition.y - launcher.transform.position.y, leadPointPosition.x - launcher.transform.position.x) * Mathf.Rad2Deg;
        return turretAngle -= 90;//art correction
       
    }


    IEnumerator FindLeadPoint()
    {
        yield return new WaitForSeconds(0.1f);
        while (currentPlane)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            Plane planeScript = currentPlane.GetComponent<Plane>();
            Vector2 targetVelocity = planeScript.GetComponent<Rigidbody2D>().velocity;

            rocketSpeed = rocketPrefab.GetComponent<Rocket>().speed;
            float a = (targetVelocity.x * targetVelocity.x) + (targetVelocity.y * targetVelocity.y) - rocketSpeed * rocketSpeed;
            float b = 2 * (targetVelocity.x * (currentPlane.gameObject.transform.position.x - launcher.transform.position.x)
                + targetVelocity.y * (currentPlane.gameObject.transform.position.y - launcher.transform.position.y));

            //distanse betwene plane and launcher : 
            float c = ((currentPlane.gameObject.transform.position.x - launcher.transform.position.x) * (currentPlane.gameObject.transform.position.x - launcher.transform.position.x)) +
                ((currentPlane.gameObject.transform.position.y - launcher.transform.position.y) * (currentPlane.gameObject.transform.position.y - launcher.transform.position.y));

            float disc = b * b - (4 * a * c);
            if (disc < 0 )
            {
                Debug.LogError("No possible hit!");
            }
            else
            {
                float t1 = (-1 * b + Mathf.Sqrt(disc)) / (2 * a);
                float t2 = (-1 * b - Mathf.Sqrt(disc)) / (2 * a);
                float t = Mathf.Max(t1, t2);// let us take the larger time value 
                float aimX = (targetVelocity.x * t) + currentPlane.gameObject.transform.position.x;
                float aimY = currentPlane.gameObject.transform.position.y + (targetVelocity.y * t);
                leadPointPosition = new Vector2(aimX, aimY);
                if (leadPoint && !FindObjectOfType<Rocket>())
                {
                    leadPoint.transform.position = currentPlane.GetComponent<Plane>().SetTrajectory(leadPointPosition.x);
                }
                else if (!leadPoint)
                {
                    leadPoint = Instantiate(leadPointPrefab, leadPointPosition, Quaternion.identity);

                }

            }
        }
    }

}
