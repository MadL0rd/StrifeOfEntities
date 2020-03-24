using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBirdScript : MonoBehaviour {

    private GameObject bird;
	void Start ()
    {
        bird = this.transform.parent.gameObject;
	}

    private void StartNextStep()
    {
        bird.GetComponent<BirdScript>().NextStep();
    }
}
