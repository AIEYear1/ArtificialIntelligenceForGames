using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public Vector2 lerpEnds = new Vector2();
    public Vector2 speedEnds = new Vector2();
    public Vector2 spawnIntervalRange = new Vector2();
    public float[] lanes;
    public GameObject[] carFabs;

    List<Car> cars = new List<Car>();
    Timer spawnTimer = new Timer();

    private void Start()
    {
        spawnTimer = new Timer(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
    }

    private void FixedUpdate()
    {
        if (spawnTimer.Check())
        {
            spawnTimer = new Timer(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
            Spawn();
        }

        for (int x = 0; x < cars.Count; ++x)
        {
            if (!cars[x].carSpeed.IsComplete())
            {
                cars[x].carSpeed.CountByTime();

                Vector3 tmpVector = cars[x].transform.position;

                if (cars[x].travelRight)
                    tmpVector.x = Mathf.Lerp(lerpEnds.x, lerpEnds.y, cars[x].carSpeed.PercentComplete);
                else
                    tmpVector.x = Mathf.Lerp(lerpEnds.y, lerpEnds.x, cars[x].carSpeed.PercentComplete);

                cars[x].transform.position = tmpVector;
            }
            else
            {
                Destroy(cars[x].transform.gameObject);
                cars.RemoveAt(x);
            }
        }
    }

    void Spawn()
    {
        Car car = new Car();
        int laneIndex = Random.Range(0, lanes.Length);

        car.transform = Instantiate(carFabs[Random.Range(0, carFabs.Length)]).transform;

        Vector3 tmpPos = car.transform.position;
        tmpPos.z = lanes[laneIndex];
        tmpPos.y = -8;
        car.transform.position = tmpPos;

        car.travelRight = laneIndex < 2;

        car.transform.rotation = Quaternion.Euler(0, ((car.travelRight) ? 270 : 90), 0);

        car.carSpeed = new Timer(Random.Range(speedEnds.x, speedEnds.y));

        cars.Add(car);
    }
}
