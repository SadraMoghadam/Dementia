using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolPoints : MonoBehaviour
{
    public List<Transform> points;
    [SerializeField] private int nearestPointPickRange = 3;
    private Transform _currentPoint;
    private int recentlyViewedPointIndex;

    private void Awake()
    {
        int rnd = Random.Range(0, points.Count);
        _currentPoint = points[rnd];
        recentlyViewedPointIndex = rnd;
    }

    public Transform GetNext()
    {
        Dictionary<Transform, float> nearestPoints = new Dictionary<Transform, float>();
        for (int i = 0; i < points.Count; i++)
        {
            float distance = Vector3.Distance(points[i].position, points[recentlyViewedPointIndex].position);
            nearestPoints.Add(points[i], distance);
        }

        nearestPoints = nearestPoints.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        recentlyViewedPointIndex = Random.Range(1, nearestPointPickRange + 1);
        _currentPoint = nearestPoints.ElementAt(recentlyViewedPointIndex).Key;
        return _currentPoint;
    }

    public bool HasReached(NavMeshAgent agent)
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            return true;
        }

        return false;
    }
}
