using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : MainItem
{
    public override void OnTriggerEnter2D (Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            player.AddHealth(bonus);
        }
    }
}
