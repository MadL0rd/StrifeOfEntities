using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepStarter : MonoBehaviour {

    GameObject gameController;
	void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        //SpawnCell();
	}

    private void OnMouseDown()
    {
        gameController.GetComponent<GameControlScript>().SetTarget(this.gameObject);
    }

    private IEnumerator SpawnCell()
    {
        float timeout = Random.Range(0, 3);
        yield return new WaitForSeconds(timeout);
        this.GetComponent<Animator>().SetTrigger("Alive");
    }


}
