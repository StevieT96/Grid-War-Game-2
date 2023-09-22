using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using static ActionManager;

public class BaseUnit : MonoBehaviour 
{
    public string UnitName;
    public Faction Faction;
    public GameObject highlight;
    public LineRenderer path;
    MenuManager EndTurn;

    [Space(10)]
    public Tile OccupiedTile;
    
    [HideInInspector] public int currentHealth;

    [Space(10)]
    public int maxHealth;
    public int speed;
    public int attackRange;
    public int damage;
    public int defence;

    [Space(10)]
    public bool turnDone = false;
   

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void HasMoved()
    {
        highlight.SetActive(true);
        turnDone = true;
       

    }
    public void ResetAction()
    {
        highlight.SetActive(false);
        turnDone = false;
    }

    public void SetDamage(int damage)
    {
        currentHealth -= (damage - defence);
        
 

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
   
        }

        if (currentHealth <= 0)
        {
            RemoveUnit();
        }
    }
    public void RemoveUnit()
    {
        if (Faction == Faction.Hero)
        {
            UnitManager.Instance.playerUnits.Remove(this);
        }
        if (Faction == Faction.Enemy)
        {
            UnitManager.Instance.enemyUnits.Remove(this);
        }
        OccupiedTile.CleanUnit(this);
        this.gameObject.SetActive(false);
    }

    public void Attack(BaseUnit target)
    {
        target.SetDamage(damage);
     

    }
    public void EndDefence(bool HasMoved)
    {
        {
            GameManager.Instance.ChangeState(GameState.EnemiesTurn);
            if (HasMoved == false)
            {
                defence = defence + 1;
            }
            else if (HasMoved == true)
            {
                defence = 0;
            }
        }
    
        


    }
    public void SetStats(ScriptableUnit data)
    {
        maxHealth = data.maxHealth;
        speed = data.speed;
        attackRange = data.attackRange;
        damage = data.damage;
        defence = data.defence;
    }
    }


