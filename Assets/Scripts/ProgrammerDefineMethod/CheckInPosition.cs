using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInPosition : MonoBehaviour
{
    public bool TouchToRect(Rect rt, Vector2 pos)
    {
        if (rt.x <= pos.x && rt.width >= pos.x
            && rt.y <= pos.y && rt.height >= pos.y)
            return true;


        return false;
    }

    public bool TouchToRect(Rect rt, Vector3 pos)
    {
        if (rt.x <= pos.x && rt.width >= pos.x
            && rt.y <= pos.y && rt.height >= pos.y)
            return true;


        return false;
    }
}
