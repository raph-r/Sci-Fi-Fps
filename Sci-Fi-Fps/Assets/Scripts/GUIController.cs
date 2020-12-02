using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public Text endGameMsg;
    public Text killsCounter;
    public Text healthPoints;
    [SerializeField] private GameObject _aim;

    public void setHealthPoints(int healthPoints)
    {
        this.healthPoints.text = healthPoints.ToString();
    }

    public void setKills(int kills)
    {
        this.killsCounter.text = kills.ToString();
    }

    public void setEndGameMsg(string msg)
    {
        this.endGameMsg.text = msg;
    }

    public void showAim(bool status)
    {
        this._aim.GetComponent<Image>().enabled = status;
    }
}
