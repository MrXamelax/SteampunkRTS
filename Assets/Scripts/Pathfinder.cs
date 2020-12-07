using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public List<Vector3> findPath(Vector3 start, Vector3 goal)
    {
        List<Vector3> path = new List<Vector3>();

        //TODO add more corners (in abhängigkeit zu den blocked tiles)

        path.Add(goal);

        return path;
    }
}
