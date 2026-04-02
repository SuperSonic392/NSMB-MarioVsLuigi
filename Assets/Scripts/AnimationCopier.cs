using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCopier : MonoBehaviour
{
    public Transform target;

    private Transform[] ourBones, targetBones;

    private Dictionary<string, Transform> targetBoneDict; 
    private Dictionary<string, Vector3> targetRestLocalPos;
    private Dictionary<string, Vector3> ourRestLocalPos;

    private void Awake()
    {
        ourBones = GetComponentsInChildren<Transform>();
        targetBones = target.GetComponentsInChildren<Transform>();

        targetBoneDict = new Dictionary<string, Transform>();
        targetRestLocalPos = new Dictionary<string, Vector3>();
        ourRestLocalPos = new Dictionary<string, Vector3>();

        foreach (var t in targetBones)
        {
            targetBoneDict[t.name] = t;
            targetRestLocalPos[t.name] = t.localPosition;
        }

        foreach (var t in ourBones)
        {
            ourRestLocalPos[t.name] = t.localPosition;
        }
    }

    private void LateUpdate()
    {
        foreach (var ourBone in ourBones)
        {
            if (targetBoneDict.TryGetValue(ourBone.name, out var targetBone))
            {
                MergeBonePosition(ourBone, targetBone);
            }
        }
    }

    public void MergeBonePosition(Transform our, Transform target)
    {
        if(our.name == "skl_root")
        {
            our.localPosition = target.localPosition;
        }
        else
        {
            if (targetRestLocalPos.TryGetValue(target.name, out var targetRest) && ourRestLocalPos.TryGetValue(our.name, out var ourRest))
            {
                Vector3 delta = target.localPosition - targetRest;
                our.localPosition = ourRest + delta;
            }
        }

        our.localRotation = target.localRotation;
        our.localScale = target.localScale;
    }
}
