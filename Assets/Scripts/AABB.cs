using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{
    public bool flag;
    public float size;
    public Vector2 min;
    public Vector2 max;
    public bool draw = true;
    public AABB() { }

    public AABB(Vector2 min, Vector2 max)
    {
        this.min = min;
        this.max = max;
        size = (max.x - min.y) / 2;
    }

    public AABB(AABB first, AABB second)
    {
        this.min = new Vector2(first.min.x < second.min.x ? first.min.x : second.min.x, first.min.y < second.min.y ? first.min.y : second.min.y);
        this.max = new Vector2(first.max.x > second.max.x ? first.max.x : second.max.x, first.max.y > second.max.y ? first.max.y : second.max.y);
        size = (max.x - min.y) / 2;
    }

    public AABB(List<AABB> toUnite)
    {
        float minx = toUnite[0].min.x;
        float maxx = toUnite[0].max.x;
        float miny = toUnite[0].min.y;
        float maxy = toUnite[0].max.y;

        foreach (AABB aabb in toUnite)
        {
            if (minx < aabb.min.x)
                minx = aabb.min.x;
            if (maxx > aabb.max.x)
                maxx = aabb.max.x;
            if (miny < aabb.min.y)
                miny = aabb.min.y;
            if (maxy > aabb.max.y)
                maxy = aabb.max.y;
        }
        Vector2 min = new Vector2(minx, miny);
        Vector2 max = new Vector2(maxx, maxy);
        this.min = min;
        this.max = max;
        size = (max.x - min.y) / 2;
    }

    public AABB(Transform tr)
    {
        UpdateAABB(tr);
        size = (max.x - min.y) / 2;
    }

    public bool Contains(AABB aabb)
    {
        return aabb.min.x >= min.x && aabb.max.x <= max.x && aabb.min.y >= min.y && aabb.max.y <= max.y;
    }
    
    public bool Intersects(AABB aabb)
    {
        return max.x > aabb.min.x && min.x < aabb.max.x && max.y > aabb.min.y && min.y < aabb.max.y;
    }

    public void UpdateAABB(Transform tr)
    {
        this.min = new Vector2(tr.position.x - size / 4, tr.position.y - size / 4);
        this.max = new Vector2(tr.position.x + size / 4, tr.position.y + size / 4);

    }


    public static float GetSize(AABB first, AABB second)
    {
        Vector2 newMin = new Vector2(first.min.x < second.min.x ? first.min.x : second.min.x, first.min.y < second.min.y ? first.min.y : second.min.y);
        Vector2 newMax = new Vector2(first.max.x > second.max.x ? first.max.x : second.max.x, first.max.y > second.max.y ? first.max.y : second.max.y);

        return (newMax.x - newMin.x) * (newMax.y - newMin.y);
    }

    private void Start()
    {
        GameManager.Instance.OnOutOfBounds.AddListener(HandleOutOfBounds);
    }

    private void Update()
    {
        UpdateAABB(gameObject.transform);
    }

    private void HandleOutOfBounds(AABB aabb)
    {
        if(aabb == this)
            gameObject.SetActive(false);
    }

    public static float GetSize(AABB first)
    {
        return (first.max.x - first.min.x) * (first.max.y - first.min.y);
    }

    public static bool TwoIntersect(AABB first, AABB second)
    {
        return first.Intersects(second);
    }

    public static AABB AddTwoBounds(AABB first, AABB second)
    {
        return new AABB(first, second);
    }

    private void OnDrawGizmos()
    {
        //Debug.Log("size: " + GetSize(this));
        if (draw)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(max.x - min.x - 0.1f, max.y - min.y - 0.1f, 0.1f));
        }
    }
}

