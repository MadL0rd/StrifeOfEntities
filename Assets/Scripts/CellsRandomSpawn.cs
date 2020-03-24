using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsRandomSpawn : MonoBehaviour {

    [Header("cellsMaxWaveCount")]
    [Range(0, 5)]
    public int cellsMaxWaveCount;
    private float cellsTimeout;
    private List<GameObject> cells;
    private List<float> cellsTimeouts;
    void Start ()
    {
        cellsTimeout = 0;
    }
    private void Update()
    {
        if (cells != null)
        {
            if (cells.Count > 0)
            {
                cellsTimeout += Time.deltaTime;
                while (cells.Count > 0 && cellsTimeout > cellsTimeouts[0])
                {
                    int buff = Random.Range(0, cells.Count);
                    cells[buff].GetComponent<Animator>().SetTrigger("Alive");
                    cells.RemoveAt(buff);
                    cellsTimeouts.RemoveAt(0);
                }
            }
            else
            {
                this.gameObject.GetComponent<GameControlScript>().StartGame();
                this.enabled = false;
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Cell")!=null)
            {
                cells = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cell"));
                cellsTimeouts = new List<float>();
                for (int i = 0; i < cells.Count; i++)
                {
                    float buff = Random.Range(0, cellsMaxWaveCount);
                    cellsTimeouts.Add(buff);
                }
                cellsTimeouts.Sort();
            }
        }
    }
}
