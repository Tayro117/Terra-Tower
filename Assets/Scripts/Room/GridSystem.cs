using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int row;
    private int col;
    private int tileSize;

    private Dictionary<(int, int),GameObject> objects;

    public GridSystem(int r, int c, int t) {
    	row=r;
    	col=c;
    	tileSize=t;
    	objects = new Dictionary<(int, int),GameObject>();
    }

    public void addToGrid(int x, int y, GameObject obj, int length, int w) {
        for (int i = x; i < x+w; i++) {
            for (int j = y;j < y+length;j++) {
                objects.Add((i,j), obj);
            }
        }
    }
    public void removeItem(int x, int y, int length, int w) {
        for (int i = x; i < x+w; i++) {
            for (int j = y;j < y+length;j++) {
                objects.Remove((i,j));
            }
        }

    }
    public bool checkGrid(int x, int y, int length, int w) {
        for (int i = x; i < x+w; i++) {
            for (int j = y;j < y+length;j++) {
                if (objects.ContainsKey((i,j))) {
                    return true;
                }
            }
        }
        return false;
    }
    public GameObject getItem(int x, int y) {
        if (!objects.ContainsKey((x,y))) {
            return null;
        }
        return objects[(x,y)];
    }

    public int getRowCount() {
    	return row;
    }
    public int getColCount() {
    	return col;
    }
    public int getTileSize() {
    	return tileSize;
    }
    public void setRowCount(int n) {
    	row = n;
    }
    public void setColCount(int n) {
    	col = n;
    }
    public void setTileSize(int n) {
    	tileSize = n;
    }
    public void emptyGrid() {
        objects = new Dictionary<(int, int), GameObject>();
    }
}
