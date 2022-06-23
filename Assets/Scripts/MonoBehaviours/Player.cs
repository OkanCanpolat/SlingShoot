using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Characters
{
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public Inventory inventoryPrefab;
    Inventory inventory;
    public HitPoints hitPoints;

    private void Start()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.
                GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                
                bool shouldDisappear = false;

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;

                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;

                    default:
                        break;
                }
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }

            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted HP by: " + amount + ". New value: " +
            hitPoints.value);
            return true;
            
        }

        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float
                                                            interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());

            hitPoints.value = hitPoints.value - damage;

            if (hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }

            else
            {
                break;
            }
        }
    }

    public override void KillCharacter()
    {
        base.KillCharacter();

        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }

    
}
