using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FinalGunmanBehaviour : GunmanBehaviour
{
    public override void DeleteCharacter()
    {
        base.DeleteCharacter();
        GameManager._instance.EndGame(false);
    }
}
