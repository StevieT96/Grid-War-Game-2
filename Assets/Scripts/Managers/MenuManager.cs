using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    public static MenuManager Instance;

    [SerializeField] private GameObject selectedHeroPanel, selectedHeroName, selectedHeroHP, selectedHeroMoveRange, selectedHeroDamage;
    

    [SerializeField] private GameObject tileUnitPanel, tileUnitName, tileUnitHP;

    [Space(10)]
    [SerializeField] private GameObject tilePanel;
    void Awake() {
        Instance = this;
        selectedHeroPanel.SetActive(false);
        tileUnitPanel.SetActive(false);
        tilePanel.SetActive(false);
    }

    public void ShowTileInfo(Tile tile) {
        //EMPTY SPACE
        if (tile == null)
        {
            tilePanel.SetActive(false);
            tileUnitPanel.SetActive(false);
            return;
        }
        //TILE
        tilePanel.GetComponentInChildren<Text>().text = tile.TileName;
        tilePanel.SetActive(true);
        
        //OCCUPIED UNIT
        if (tile.OccupiedUnit) {
            tileUnitName.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            tileUnitHP.GetComponentInChildren<Text>().text = tile.OccupiedUnit.currentHealth.ToString();
            tileUnitPanel.SetActive(true);
        }
    }

    public void ShowSelectedHero(BaseHero hero) {
        if (hero == null) {
            selectedHeroPanel.SetActive(false);
            return;
        }
        //HERO
        selectedHeroPanel.GetComponentInChildren<Text>().text = hero.UnitName;
        selectedHeroHP.GetComponentInChildren<Text>().text = hero.currentHealth.ToString();
        selectedHeroMoveRange.GetComponentInChildren<Text>().text = hero.speed.ToString();
        selectedHeroDamage.GetComponentInChildren<Text>().text = hero.damage.ToString();
        selectedHeroPanel.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }

    public void TurnPass()
    {
        GameManager.Instance.ChangeState(GameState.EnemiesTurn);
        
    }
}