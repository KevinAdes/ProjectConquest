using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Net.Mail;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField]
    float maxHealth, currentHealth, damage, defense, attackModifier, speed, jump, blood;

    [SerializeField]
    int cash;
    
    [SerializeField]
    InventoryObject inventory;

    [SerializeField]
    EnemySkill[] skills;

    public void AddBlood(float f)
    {
        blood += f;
    }

    public void AddCash(int i)
    {
        cash += i;
    }

    //Setters and Getters

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float f)
    {
        maxHealth = f;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float f)
    {
        currentHealth = f;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float f)
    {
        damage = f;
    }

    public float GetDefense()
    {
        return defense;
    }

    public void SetDefense(float f)
    {
        defense = f;
    }

    public float GetAttackModifier()
    {
        return attackModifier;
    }

    public void SetAttackModifier(float f)
    {
        attackModifier = f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float f)
    {
        speed = f;
    }

    public float GetJump()
    {
        return jump;
    }

    public void SetJump(float f)
    {
        jump = f;
    }

    public void SetBlood(float f)
    {
        blood = f;
    }
    public float GetBlood()
    {
        return blood;
    }

    public int GetCash()
    {
        return cash;
    }

    public void SetCash(int i)
    {
        cash = i;
    }

    public InventoryObject GetInventory()
    {
        return inventory;
    }

    public EnemySkill[] GetSkills()
    {
        return skills;
    }
}
