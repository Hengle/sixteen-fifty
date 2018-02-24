using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
    public int width = 6;
    public int height = 6;

    public HexCell cellPrefab;

    public Text cellLabelPrefab;

    Canvas gridCanvas;
    HexMesh hexMesh;

    HexCell[] cells;

    // start gets called *after* awake, so at this point, `cells` will be initialized
    void Start () {
        hexMesh.Triangulate(cells);
    }

    void Awake () {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        Debug.Log("hello world");

        cells = new HexCell[height * width];

        for (int y = 0, i = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                CreateCell(x, y, i++);
            }
        }
    }

    void CreateCell (int x , int y, int i) {
        // define the position for our tile
        Vector3 position;
        position.y = (y + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.z = 0f;

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.y);
        label.text = x.ToString() + "\n" + y.ToString();
    }

	  // Update is called once per frame
	  void Update () {

	  }
}
