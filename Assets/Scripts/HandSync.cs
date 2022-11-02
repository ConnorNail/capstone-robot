using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class HandSync : MonoBehaviour, IOnEventCallback
{
    public const byte updatePoseEventCode = 1;
    public const byte updateButtonsEventCode = 2;
    public const byte updateJointStateEventCode = 3;

    public GameObject rightHand;

    private float[] rightPose;
    private float[] jointState;

    public bool rButtonHand;
    public bool rButtonIndex;
    public bool rButtonA;

    public float deltaX;
    // Start is called before the first frame update
    void Start()
    {
        rightPose = new float[6];
        jointState = new float[6];
    }

    void Update()
    {
        //sendJointStateUpdate();
    }

    public void sendJointStateUpdate(double[] states)
    {
       // if (!PhotonNetwork.IsMasterClient)
       // {
        jointState[0] = (float)states[0];
        jointState[1] = (float)states[1];
        jointState[2] = (float)states[2];
        jointState[3] = (float)states[3];
        jointState[4] = (float)states[4];
        jointState[5] = (float)states[5];
        RaiseEventOptions raiseEvent = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(updateJointStateEventCode, jointState, raiseEvent, SendOptions.SendReliable);
       // }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == updatePoseEventCode)
        {
            Debug.Log("Updating location from master...");
            float[] poseData = (float[])photonEvent.CustomData;
            rightHand.transform.position = new Vector3(poseData[0], poseData[1], poseData[2]);
            rightHand.transform.rotation = Quaternion.Euler(new Vector3(poseData[3], poseData[4], poseData[5]));
        }

        if (eventCode == updateButtonsEventCode)
        {
            Debug.Log("Updating buttons from master...");
            bool[] buttonData = (bool[])photonEvent.CustomData;
            rButtonHand = buttonData[0];
            rButtonIndex = buttonData[1];
            rButtonA = buttonData[2];
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
