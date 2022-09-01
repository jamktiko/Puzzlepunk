using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : MonoBehaviour
{
    public PlayerMovement parent;
    void Awake()
    {
        parent = GetComponent<PlayerMovement>();
        parent.HasControl = true;
    }
}
