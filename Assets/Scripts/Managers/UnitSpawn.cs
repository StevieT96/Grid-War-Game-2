using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
    public static UnitSpawn Instance;
    void Awake()
    {
        Instance = this;
    }

    public void LoadStats( ScriptableUnit data, BaseUnit prefab)
    {
        prefab.maxHealth = data.maxHealth;
        prefab.speed = data.speed;
        prefab.attackRange = data.attackRange;
        prefab.damage = data.damage;
        prefab.defence = data.defence;

    }
}
