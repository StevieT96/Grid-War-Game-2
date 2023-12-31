using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    [HideInInspector] public enum Action { select, move, attack, defend};
    [HideInInspector] public Action action;

    public Color moveColor;
    public Color attackColor;
    private BaseHero selectedHero;
    void Awake()
    {
        Instance = this;
        action = Action.select;
    }

    public void RemoveDefenceBuff(BaseHero hero)
    {
        if (hero.DefenceBuffApplied)
        {
            hero.RemoveDefenceBuff();
        }
    }
    public async void MouseDown(Tile tile)
    {
        if (GameManager.Instance.gameState != GameState.HeroesTurn) return;
        Vector3 tilePos = tile.transform.position;

        //ATTACK TURN
        if (UnitManager.Instance.SelectedHero != null && action == Action.attack)
        {
            //check all adjacent tiles

            if (tile.inRange == true && tile.OccupiedUnit != null && tile.OccupiedUnit.Faction == Faction.Enemy)
            {
                var enemy = (BaseEnemy)tile.OccupiedUnit;
                UnitManager.Instance.SelectedHero.Attack(enemy);


                UnitManager.Instance.SelectedHero.HasMoved();
                UnitManager.Instance.SetSelectedHero(null);
                MovementManager.Instance.CleanMovementTiles();


            }


            MovementManager.Instance.CleanMovementTiles();
            action = Action.select;
            UnitManager.Instance.SetSelectedHero(null);
            return;
        }

        // MOVEMENT TURN
        if (UnitManager.Instance.SelectedHero != null && action == Action.move)
        {
            BaseHero selectedHero = UnitManager.Instance.SelectedHero;
            MovementManager.Instance.CleanPath();

            if (UnitManager.Instance.SelectedHero.transform.position == tile.transform.position)
            {
                MovementManager.Instance.CleanMovementTiles();

                MovementManager.Instance.SetAreaTiles(new Vector2(tilePos.x, tilePos.y), selectedHero.attackRange, attackColor);
                action = Action.attack;
            }

            if (tile.Walkable == true && tile.inRange == true)
            {
                // Check if the selected hero has the defense buff applied and remove it if necessary
                if (selectedHero.DefenceBuffApplied)
                {
                    RemoveDefenceBuff(selectedHero);
                }
                //implement animation
                await MoveAnimationManager.Instance.MoveInPath(selectedHero, tile);
                // !!! tile.SetUnit(UnitManager.Instance.SelectedHero);

                MovementManager.Instance.CleanMovementTiles();

                //check for enemy
                MovementManager.Instance.SetAreaTiles(new Vector2(tilePos.x, tilePos.y), selectedHero.attackRange, attackColor);
                action = Action.attack;
            }
            else
            {
                return;
            }


            //if no enemy detected
            if (MovementManager.Instance.CheckRangeTiles() == false)
            {
                MovementManager.Instance.CleanMovementTiles();
                action = Action.select;
                UnitManager.Instance.SelectedHero.HasMoved();
                UnitManager.Instance.SetSelectedHero(null);
            }
            return;
        }

        //SELECT TURN
        if (tile.OccupiedUnit != null && action == Action.select)
        {
            // OCCUPIED
            if (tile.OccupiedUnit.Faction == Faction.Hero && tile.OccupiedUnit.turnDone == false)
            {

                BaseHero selectedHero = (BaseHero)tile.OccupiedUnit;

                if (!selectedHero.DefenceBuffApplied)
                {
                    // Apply the defense buff and symbol when selecting the hero
                    selectedHero.ApplyDefenseBuff();

                    // Mark that the defense buff has been applied
                    selectedHero.DefenceBuffApplied = true;
                }

                // select self
                UnitManager.Instance.SetSelectedHero((BaseHero)tile.OccupiedUnit);
                //MOVEMENT RANGE

                MovementManager.Instance.SetMovementTiles(new Vector2(tilePos.x, tilePos.y), selectedHero.speed, moveColor);

                action = Action.move;
            }

            return;
        }
    }
}
