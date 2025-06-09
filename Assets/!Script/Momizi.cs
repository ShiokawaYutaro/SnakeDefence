using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momizi : Player
{
    // Start is called before the first frame update
    protected override void Start()
    {
        RunSpeed = 5;
        MaxHp = 100f;
        MaxStamina = 100;
        StaminaInterval = 5;
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
