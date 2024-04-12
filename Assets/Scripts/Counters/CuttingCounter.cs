using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress{

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData() {
        OnAnyCut = null;
    }


    private int cuttingProgress;

    

    public override void Interact(Player player) {

        if (!HasKitchenObject()) {
            // There is no kitchenObject on the counter
            if (player.HasKitchenObject()) {

                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    // Player is carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {

                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    }
                    );
                }
            }
        } else {
            // There is a kitchenObject already
            if (player.HasKitchenObject()) {
                // the player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
                } else {
                // the player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }

    }
    public override void InteractAlternate(Player player) {

        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {

            // There is a kitchenObject here and can be cut

            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {

                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            }
            );

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {

                KitchenObjectSO outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObject, this);

            }

        }

    }


    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;

    }

    public KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {

        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
   
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }
        else {
            return null;
        }
    }

    public CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}
