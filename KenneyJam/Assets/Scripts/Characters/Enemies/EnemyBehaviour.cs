using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : CharacterBehaviour
{
    public override void DeleteCharacter()
    {
        base.DeleteCharacter();
        EnemyList.instance.allEnemies.Remove(this.GetComponent<EnemyBehaviour>());
    }

    public override void Die()
    {
        base.Die();
        GameManager._instance.IncreaseEnemiesKilled();
    }
}
