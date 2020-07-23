using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData
{
    public int[,] grid_1 = new int[100,100];
    public int[,] grid_2 = new int[100,100];
    public int[,] gridProgression_1 = new int[100,100];
    public int[,] gridProgression_2 = new int[100,100];
    public GridData(GridSystem g1, GridSystem g2) {
    	// int currentSpot = 0;

    	for (int i =0;i<100;i++) {
            for (int j = 0;j<100;j++) {
                if (g1.checkGrid(i,j, 1, 1)) {
                    var c = g1.getItem(i,j).GetComponent<OnUseItem>();
                    grid_1[i,j] = c.itemID;
                    gridProgression_1[i,j] = c.growthNum;
                }
                else {
                    grid_1[i,j]=-1;
                    gridProgression_1[i,j]=-1;
                }
                if (g2.checkGrid(i,j, 1, 1)) {
                    var c = g2.getItem(i,j).GetComponent<OnUseItem>();
                    grid_2[i,j] = c.itemID;
                    gridProgression_2[i,j] = c.growthNum;
                }
                else {
                    grid_2[i,j]=-1;
                    gridProgression_2[i,j]=-1;
                }
            }
    	}
    }
}