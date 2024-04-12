using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress {

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEvenetArgs> OnStateChanged;
    public class OnStateChangedEvenetArgs : EventArgs {
        public State state;
    }

    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }

    private State state;

    public void Start() {
        state = State.Idle;
    }

    public void Update() {

        if (HasKitchenObject()) {

            switch(state) {

                case State.Idle:
                    break;

                case State.Frying:

                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        //Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        burningTimer = 0f;
                        state = State.Fried;

                        OnStateChanged?.Invoke(this, new OnStateChangedEvenetArgs() {
                            state = state
                        });
                    }

                    break;

                case State.Fried:

                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        //Burned
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });

                        OnStateChanged?.Invoke(this, new OnStateChangedEvenetArgs() {
                            state = state
                        });
                    }

                    break;

                case State.Burned:
                    break;
            }
        }
        
      

    }



    public override void Interact(Player player) {

        if (!HasKitchenObject()) {
            // There is no kitchenObject on the counter
            if (player.HasKitchenObject()) {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    
                    // Player is carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEvenetArgs() {
                        state = state
                    });
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
                    

                    state = State.Idle;

                    OnStateChanged?.Invoke(this, new OnStateChangedEvenetArgs() {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = 0f
                    });

                    }

                }


                } else {
                // the player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEvenetArgs() {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }

    public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        return fryingRecipeSO != null;

    }

    public KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {

        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
   
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        else {
            return null;
        }
    }

    public FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    public BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {

        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }

        return null;
    }

    public bool IsFried() {
        return state == State.Fried;
    }
}
