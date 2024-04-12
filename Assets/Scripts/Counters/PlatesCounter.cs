using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter{


    [SerializeField] private KitchenObjectSO kitchenObjectS0;

    private float spawnPlateTimer;
    private float spawnPlatetimerMax = 4f;
    private float platesSpawnedAmount;
    private float platesSpawnedAmountMax = 4;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;


    public void Update() {

        spawnPlateTimer += Time.deltaTime;

            if (spawnPlateTimer > spawnPlatetimerMax) {

                spawnPlateTimer = 0;

                if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax) {

                    platesSpawnedAmount++;

                    OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
    }

    public override void Interact(Player player) {

        if (!player.HasKitchenObject() && platesSpawnedAmount != 0) {
            //The player is empty handed

            KitchenObject.SpawnKitchenObject(kitchenObjectS0, player);

            platesSpawnedAmount--;

            OnPlateRemoved?.Invoke(this, EventArgs.Empty);

        }
    
    
    }

}
