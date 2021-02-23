using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private int skipToLastPointCount = 3;

    private List<Vector2> openList;
    private List<Vector2> closedList;

    private void Awake()
    {
        points = this.transform.GetComponentsInChildren<Transform>(true).ToList();
        points.RemoveAt(0);

        openList = points.Select(it => new Vector2(it.position.x, it.position.y)).ToList();
        closedList = new List<Vector2>();
    }

    public Vector2 GetRandomPoint(Vector2 targetPosition)
    {
        Vector2 point = points[Random.Range(0, points.Count)].position;

        do
        {
            if (Random.Range(float.MinValue, float.MaxValue) > 0)
            {
                if (closedList.Count > 0)
                {
                    bool isPositiveXCountMore = closedList.Count(it => it.x > 0) >= 2;
                    List<Vector2> compiledPoints = openList.Where(it => isPositiveXCountMore ? it.x < 0 : it.x > 0).ToList();

                    float closeX = openList.Min(it => (it - targetPosition).magnitude);
                    point = openList.Single(it => (it - targetPosition).magnitude == closeX);
                }
                else
                {
                    float closeX = openList.Min(it => (it - targetPosition).magnitude);
                    point = openList.FirstOrDefault(it => (it - targetPosition).magnitude == closeX);
                }
            }

            if (closedList.Contains(point))
            {
                point = openList[Random.Range(0, openList.Count)];
            }

        } while (closedList.Contains(point));

        if (closedList.Count == skipToLastPointCount)
            closedList.RemoveAt(0);

        if (closedList.Count < skipToLastPointCount)
            closedList.Add(point);

        return point;
    }
}
