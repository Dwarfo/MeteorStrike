using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//testit
public static class BoundsInteraction {

    public static bool Contains(IForm first, IForm second)
    {
        return second.Min.x >= first.Min.x && second.Max.x <= first.Max.x && second.Min.y >= first.Min.y && second.Max.y <= first.Max.y;
    }

    public static bool Intersects(IForm first, IForm second)
    {
        return first.Max.x > second.Min.x && first.Min.x < second.Max.x && first.Max.y > second.Min.y && first.Min.y < second.Max.y;
    }

    public static float GetSize(IForm node)
    {
        return (node.Max.x - node.Min.x) * (node.Max.y - node.Min.y);
    }
    /*
    public AABB AddTwoForms(IForm form1, IForm form)
    {

    } 

    public static float GetPotentionalSize(IForm form1, IForm form)
    {

    }
    */
    public static int CheckN2(List<AABB> elements)
    {
        int checks = 0;
        for (int i = 0; i < elements.Count - 1; i++)
            for (int j = i + 1; j < elements.Count; j++)
            {
                checks++;
                if(AABB.TwoIntersect(elements[i],elements[j]))
                    Debug.Log("Intersection " + elements[i].gameObject.name + "  and  " + elements[j].gameObject.name);
            }

        return checks;
    }

    public static void CheckN2(List<AABB> elements, ActOnCollision action)
    {
        for (int i = 0; i < elements.Count - 1; i++)
            for (int j = i + 1; j < elements.Count; j++)
            {
                if (AABB.TwoIntersect(elements[i], elements[j]))
                {
                    Debug.Log("Intersection " + elements[i].gameObject.name + "  and  " + elements[j].gameObject.name);
                    action();
                }
            }
    }

    public static float GetDistance(AABB a1, AABB a2)
    {
        return Mathf.Sqrt(Mathf.Pow((a1.transform.position.x - a2.transform.position.x), 2) + Mathf.Pow((a1.transform.position.y - a2.transform.position.y), 2));
    }

    public static bool CheckOverlap1D(AABB a1, AABB a2, bool axis)
    {
        if(axis)
            return a1.max.x > a2.min.x && a1.min.x < a2.max.x;
        else
            return a1.max.y > a2.max.y && a1.min.y < a2.max.y;
    }

}

public delegate void ActOnCollision();
