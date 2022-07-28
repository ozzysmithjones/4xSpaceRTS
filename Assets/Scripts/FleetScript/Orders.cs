using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoint : IOrder
{
    private Star star;
    private Transform point;
    private List<Star> preferedPath;

    public MoveToPoint(Star star, Transform point, List<Star> preferedPath = null)
    {
        this.star = star;
        this.point = point;
        this.preferedPath = preferedPath;
    }

    public Star TargetStar => star;

    public Transform TargetPoint => point;

    public List<Star> PreferedPath => preferedPath;

    public void OnReachPoint(Fleet fleet, Transform point)
    {
    }

    public void OnReachStar(Fleet fleet, Star star)
    {
    }
}