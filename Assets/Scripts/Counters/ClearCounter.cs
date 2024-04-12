using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {

        if (!HasKitchenObject()) {
            // There is no kitchenObject on the counter
            if (player.HasKitchenObject()) {
                // Player is carrying Somehting
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                // Player not carrying anything
            }
        } else {
            // There is a kitchenObject here
            if (player.HasKitchenObject()) {
                // the player is carrying a something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                } else {
                    // The player is carrying something else than a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // There is a plate on the counter
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

                } else {
                // the player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }

}
