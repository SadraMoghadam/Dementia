using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MovingPoints : MonoBehaviour
{
    [SerializeField] private GameObject pointsContainer;
    [SerializeField] private int nearestPointPickRange = 3;
    private List<Transform> points;
    private static int pointIndex;
    private static Transform _currentPoint;
    private int recentlyViewedPointIndex;

    private void Awake()
    {
        points = pointsContainer.GetComponentsInChildren<Transform>().ToList();
        points.RemoveAt(0);
        pointIndex = Random.Range(0, points.Count);
        _currentPoint = points[pointIndex];
        recentlyViewedPointIndex = pointIndex;
    }

    public Transform GetNext(NavMeshAgent agent)
    {
        int rnd = Random.Range(0, points.Count);
        pointIndex = rnd;
        if (pointIndex == rnd)
        {
            if (rnd == points.Count - 1)
            {
                pointIndex = rnd - 1;
            }
            else if (rnd == 0)
            {
                pointIndex = 1;
            }
            else
            {
                pointIndex = rnd + 1;
            }
        }
        _currentPoint = points[pointIndex];
        return _currentPoint;
        // Dictionary<Transform, float> nearestPoints = new Dictionary<Transform, float>();
        // for (int i = 0; i < points.Count; i++)
        // {
        //     float distance = Vector3.Distance(points[i].position, points[recentlyViewedPointIndex].position);
        //     nearestPoints.Add(points[i], distance);
        // }
        //
        // nearestPoints = nearestPoints.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        // recentlyViewedPointIndex = Random.Range(1, nearestPointPickRange + 1);
        // _currentPoint = nearestPoints.ElementAt(recentlyViewedPointIndex).Key;
        // return _currentPoint;
    }

    public bool HasReached(NavMeshAgent agent)
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance != Mathf.Infinity && Vector3.Distance(agent.transform.position, _currentPoint.position) < agent.stoppingDistance * 2)
        {
            return true;
        }

        return false;
    }
    
    public bool HasReached(NavMeshAgent agent, Transform playerTransform)
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance != Mathf.Infinity && Vector3.Distance(agent.transform.position, playerTransform.position) < agent.stoppingDistance * 2)
        {
            return true;
        }

        return false;
    }
}
