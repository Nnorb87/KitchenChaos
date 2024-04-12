using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PorgressBarUI : MonoBehaviour
{

    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;

    private IHasProgress hasProgress;

    public void Start() {

        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        if (hasProgress == null) {
            Debug.LogError("Game Object" + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {

        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();

        } else {

            Show();

        }
    }

    public void Show() {
        gameObject.SetActive(true);

    }

    public void Hide() {
        gameObject.SetActive(false);

    }
}
