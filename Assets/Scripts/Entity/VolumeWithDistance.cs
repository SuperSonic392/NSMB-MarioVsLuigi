using UnityEngine;
using Photon.Pun;
using NSMB.Utils;
using System;

public class VolumeWithDistance : MonoBehaviour {

    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private Transform soundOrigin;
    [SerializeField] private float soundRange = 12f;

    public void Update() {

        GameManager inst = GameManager.Instance;

        GameObject closestLocalPlayer = null;
        if (CameraController.playerControllingCamera.Length > 1)
        {
            float dist = Mathf.Infinity;
            for (int i = 0; i < CameraController.playerControllingCamera.Length; i++)
            {
                GameObject player = PhotonView.Find(CameraController.playerControllingCamera[i]).gameObject;
                float newDist = Vector2.Distance(player.transform.position, soundOrigin.transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    closestLocalPlayer = player;
                }
            }
        }
        else
        {
            closestLocalPlayer = PhotonView.Find(CameraController.playerControllingCamera[0]).gameObject;
        }

        Vector3 listener = (inst != null && closestLocalPlayer != null) ? closestLocalPlayer.transform.position : Camera.main.transform.position;

        float volume = Utils.QuadraticEaseOut(1 - Mathf.Clamp01(Utils.WrappedDistance(listener, soundOrigin.position) / soundRange));

        foreach (AudioSource source in audioSources)
            source.volume = volume;
    }
}