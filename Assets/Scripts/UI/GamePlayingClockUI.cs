using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour{

    [SerializeField] private Image timerImage;

    public void Update() {
        timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTImerNormalized();
    }
}
