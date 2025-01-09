using System.Collections;
using UnityEngine;

public class OrcClass : MonoBehaviour
{
    bool boosted = false;
    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Axe")){
            GetComponentInChildren<AxeThrow>().RequeueAxe(other.gameObject);
            AxePickupBoost();
        }
    }

    public void AxePickupBoost()
    {
        if(!boosted){
            boosted = true;
            GetComponent<IPlayer>().SetActiveMovementSpeed(GetComponent<IPlayer>().GetActiveMovementSpeed() + new Vector2(2.0f, 2.0f));
            StartCoroutine(BoostFinished(2));
        }
    }

    private IEnumerator BoostFinished(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        boosted = false;
        GetComponent<IPlayer>().SetActiveMovementSpeed(GetComponent<IPlayer>().GetActiveMovementSpeed() - new Vector2(2.0f, 2.0f));
    }
}
