using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerCounter : BaseCounter {

   
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player) {

        if (!player.HasKitchenObject()) {

            // The player is not carrying anything

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }

}
