using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject ShieldBuff;

    // Start is called before the first frame update
    void start()
    {
       GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }
    // Update is called once per frame
    void Update()
    {
        ShieldBuff.SetActive(false);
    }
}
