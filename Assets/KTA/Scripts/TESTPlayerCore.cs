using System.Collections;
using System.Collections.Generic;
using KTA.Test;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;

public class TESTPlayerCore : MonoBehaviour
{
    [field: SerializeField] public TESTPlayerController PlayerController { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent  { get; private set; }
    [field: SerializeField] public NetworkAnimator NetworkAnimator { get; private set; }
    [field: SerializeField] public TESTCPController CPSController { get; private set; }
}
