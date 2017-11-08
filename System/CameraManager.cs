using UnityEngine;
using System.Collections;
using System;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private Transform cameraTransform;
    private bool inFollow = true;
    private Transform followTo;
    public bool tpToTarget = false;

    private float currentDistance = 0;
    private float maxDistance = 1.2f;
    private UnitComponent uComp;
    public CameraManager SetTarget(Transform target)
    {
        followTo = target;
        uComp = followTo.GetComponent<UnitComponent>();
        return this;
    }
    public CameraManager SetPosition(Vector3 pos)
    {
        cameraTransform.position = pos;
        return this;
    }
    public CameraManager SetFollowState(bool inFollow, bool tp)
    {
        this.inFollow = inFollow;
        tpToTarget = tp;
        return this;
    }

    public CameraManager SetFollowMaxDistance(float distance)
    {
        this.maxDistance = distance;
        return this;
    }

    public Transform GetTransform()
    {
        return cameraTransform;
    }
    public Vector2 GetPosition()
    {
        return cameraTransform.position;
    }
    public bool IsInFollow()
    {
        return inFollow;
    }
    public Transform GetTarget()
    {
        return followTo;
    }
    public void Init()
    {
        instance = this;
        cameraTransform = transform;
    }
    void Update()
    {
        if (followTo != null)
        {
            Vector2 tarPos = new Vector2(followTo.position.x, followTo.position.y);
            Vector2 camPos = new Vector2(cameraTransform.position.x, cameraTransform.position.y);
            if (inFollow && tarPos != camPos)
            {
                if (tpToTarget)
                    camPos = Vector2.Lerp(camPos, tarPos, 1);
                else {
                    float dis = Vector2.Distance(tarPos, camPos);
                    if (uComp.isMoving){
                        
                        currentDistance = (dis < maxDistance) ? dis : maxDistance;
                    }
                }
                camPos = tarPos - (tarPos - camPos).normalized * currentDistance;
                cameraTransform.position = new Vector3(camPos.x, camPos.y, -10);

            }
        }


    }
}
