using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previousCountdownNumber;
    private const string NUMBER_POPUP = "NumberPopup";

    private void Awake() {
        animator = GetComponent<Animator>();
        Hide();
    }

    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Show();
        } else {
            Hide();
        }
  
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countDownText.text = countdownNumber.ToString();

        if (previousCountdownNumber != countdownNumber && KitchenGameManager.Instance.IsCountdownToStartActive()) {
            previousCountdownNumber = countdownNumber;

            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountDownSound();
        }
    }
    private void Show() {
        countDownText.gameObject.SetActive(true);
    }
    private void Hide() {
        countDownText.gameObject.SetActive(false);

    }
    
}
