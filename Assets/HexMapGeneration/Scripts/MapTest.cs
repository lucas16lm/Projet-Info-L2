using TMPro;
using UnityEngine;



public class MapTest : MonoBehaviour
{
    public MapGenerator gridGenerator;
    public TMP_InputField xField;
    public TMP_InputField yField;
    public TMP_InputField zField;
    

    public void CreateMapButton(){
        gridGenerator.PlaceTiles(gridGenerator.width);
    }

    public void ClearMapButton(){
        gridGenerator.ClearTiles();
    }

    public void SelectTile(){
        Tile tile = Tile.GetTile(int.Parse(xField.text), int.Parse(yField.text), int.Parse(zField.text));
        if(tile!=null) tile.transform.GetComponent<Renderer>().material.color=Color.blue;
    }

    public void SelectNeighbors(){
        Tile tile = Tile.GetTile(int.Parse(xField.text), int.Parse(yField.text), int.Parse(zField.text));
        if(tile!=null){
            foreach(Tile neighbor in tile.GetNeighbors()){
                if(neighbor!=null) neighbor.transform.GetComponent<Renderer>().material.color=Color.red;
            }
        }
    }

    
}
