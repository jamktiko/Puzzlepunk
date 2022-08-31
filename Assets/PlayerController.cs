using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMovement))]

public class PlayerController : MonoBehaviour
{
    bool Selected = false;
    public PlayerMovement parent;
    void Awake()
    {
        parent = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        PlayerSelector.main.RegisterCharacter(this);
    }

    private void Update()
    {
        if (Selected)
            HandleControls();
    }
    void HandleControls()
    {
        parent.Move(Input.GetAxis("Horizontal"));
        parent.TryJump();
    }
    public void SetSelected(bool value)
    {
        Selected = value;
        parent.Move(0);
    }
}
