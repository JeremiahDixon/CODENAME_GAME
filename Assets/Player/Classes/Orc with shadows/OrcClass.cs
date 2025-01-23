using System.Collections;
using UnityEngine;

public class OrcClass : MonoBehaviour
{
    bool boosted = false;
    const string AXE_TAG = "Axe";
    Vector2 boostSpeed = new Vector2(2.0f, 2.0f);
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag(AXE_TAG)){
            GetComponentInChildren<AxeThrow>().RequeueAxe(other.gameObject);
            AxePickupBoost();
        }
    }

    public void AxePickupBoost()
    {
        if(!boosted){
            boosted = true;
            GetComponent<IPlayer>().SetActiveMovementSpeed(GetComponent<IPlayer>().GetActiveMovementSpeed() + boostSpeed);
            StartCoroutine(BoostFinished(2, boostSpeed));
        }
    }

    IEnumerator BoostFinished(int seconds, Vector2 boost)
    {
        yield return new WaitForSeconds(seconds);
        boosted = false;
        GetComponent<IPlayer>().SetActiveMovementSpeed(GetComponent<IPlayer>().GetActiveMovementSpeed() - boost);
    }
}
