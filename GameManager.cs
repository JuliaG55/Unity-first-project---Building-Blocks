using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Block
{
    public Transform blockTransform;
}

public enum BlockColor
{
    White = 0,
    Red = 1,
    Green = 2,
    Blue = 3
}

public class GameManager : MonoBehaviour 
{
    private float blockSize = 0.25f;

    public Block[,,] blocks = new Block[20, 20, 20];
    public GameObject blockPrefab;

    public BlockColor selectedColor;
    public Material[] blockMaterials;

    private GameObject fundationObject;
    private Vector3 blockOffset;
    private Vector3 fundationCenter = new Vector3(1.25f, 0, 1.25f);
   

    private void Start()
    {
        fundationObject = GameObject.Find("Fundation");
        blockOffset = (Vector3.one * 0.5f) / 4;
        selectedColor = BlockColor.White;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 30.0f)) 
            {
                Vector3 index = BlockPosition(hit.point); //index where the blox should go

                int x = (int)index.x
                  , y = (int)index.y
                  , z = (int)index.z;

                if(blocks[x,y,z] == null) //if there is no cube in the position
                                          // Instantiate the cube
                {
                    GameObject go = CreateBlock();
                    go.transform.localScale = Vector3.one * blockSize;
                    PositionBlock(go.transform, index);

                    blocks[x, y, z] = new Block //use array and assign it
                    {
                        blockTransform = go.transform
                    };
                }
                else 
                {
                    GameObject go = CreateBlock();
                    go.transform.localScale = Vector3.one * blockSize;

                    Vector3 newIndex = BlockPosition(hit.point + hit.normal * blockSize);
                    PositionBlock(go.transform, newIndex);
                }
            }
        }
    }
    private GameObject CreateBlock()
    {
        GameObject go = Instantiate(blockPrefab) as GameObject;
        go.GetComponent<Renderer>().material = blockMaterials[(int)selectedColor];
        return go;

    }

    private Vector3 BlockPosition(Vector3 hit)
    {
        int x = (int)(hit.x / blockSize);
        int y = (int)(hit.y / blockSize);
        int z = (int)(hit.z / blockSize);

        return new Vector3(x, y, z);
    }

    private void PositionBlock(Transform t, Vector3 index)
    {
        t.position = ((index * blockSize) + blockOffset) + (fundationObject.transform.position - fundationCenter);
    }

    public void ChangeBlockColor(int color)
    {
        selectedColor = (BlockColor)color;

    }

}
