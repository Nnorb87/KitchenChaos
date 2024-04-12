using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
       public KitchenObjectSO kitchenObjectSO;
    }


    public void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {

        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Not a valid Ingredient
            return false;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Already has this type
            return false;
            } else { 
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;
    }
}
