using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {

    [SerializeField] StoveCounter stoveCoutner;
    [SerializeField] GameObject stoveOnVisual;
    [SerializeField] GameObject particlesGameObject;

    public void Start() {
        stoveCoutner.OnStateChanged += StoveCoutner_OnStateChanged;
    }

    private void StoveCoutner_OnStateChanged(object sender, StoveCounter.OnStateChangedEvenetArgs e) {
        
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveOnVisual.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual); 

    }
}
