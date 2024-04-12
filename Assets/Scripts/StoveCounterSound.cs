using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class StoveCounterSound : MonoBehaviour{


    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;


    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
       
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEvenetArgs e) {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound) {
            audioSource.Play();
        }
        else
        {
             audioSource.Pause();
        }
    }

    public void Update() {

        if (playWarningSound) {
            warningSoundTimer -= Time.deltaTime;

            if (warningSoundTimer <= 0f) {
                float warningSoundTimerMax = 0.1f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }

}
