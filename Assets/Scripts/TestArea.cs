using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArea : MonoBehaviour
{
    public Collider2D collider;
    public SpriteRenderer renderer;

    public GameObject prefab;
    public GameObject holder;

    public List<Collider2D> colliders;

    void Start()
    {
       
        //Instantiate(prefab, max, Quaternion.identity);
    }


    private void Test()
    {
        Vector2 min = renderer.bounds.min;
        Vector2 max = renderer.bounds.max;

        float x1, x2, y1, y2;
        float rotation = this.transform.eulerAngles.z;

        x1 = (min.x * Mathf.Cos(rotation)) - (min.y * Mathf.Sin(rotation));
        y1 = (min.x * Mathf.Sin(rotation)) + (min.y * Mathf.Cos(rotation));

        x2 = (min.x / 2 * Mathf.Cos(rotation)) - (min.y / 2 * Mathf.Sin(rotation));
        y2 = (min.x / 2 * Mathf.Sin(rotation)) + (min.y / 2 * Mathf.Cos(rotation));
        min = new Vector2(x1 * x2, 0);
        Debug.Log($"x1 {x1}, x2 {x2}, y1 {y1}, y2 {y2}");

        holder.transform.position = min;
    }

    // Update is called once per frame
    void Update()
    {
        //Test();
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            collider = colliders[Random.Range(0, colliders.Count-1)];

            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;
            Color color = Color.white;
            Vector2 randomPoint = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            if (collider.OverlapPoint(randomPoint))
            {
                color = Color.yellow;
            }
            else
            {
                color = Color.red;
            }
            Debug.Log(min + "----" + max + " :::::: " + randomPoint);

            GameObject clone = Instantiate(prefab, randomPoint, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().color = color;
        }

        bool PointInTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
        {
            Vector3 d, e;
            float w1, w2;
            d = b - a;
            e = c - a;
            w1 = (e.x * (a.y - p.y) + e.y * (p.x - a.x)) / (d.x * e.y - d.y * e.x);
            w2 = (p.y - a.y - w1 * d.y) / e.y;
            return (w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
        }
    }
}
