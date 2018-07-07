using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace SixteenFifty.Editor {
  using Behaviours;
  using TileMap;

  public class MapEditor : EditorWindow {
    HexMap targetMap;

    IDataSet<TileButton> tiles;

    AssetInfo<HexTile> selectedTile;
    HexGridManager hexGridManager;

    bool MapLoaded => null != hexGridManager?.CurrentGrid;
    bool ReadyToLoad => null != hexGridManager && null != targetMap && !MapLoaded;

    [MenuItem("Window/Map Editor")]
    public static void ShowWindow() {
      EditorWindow.GetWindow(typeof(MapEditor));
    }

    private void OnEnable() {
      if(null == tiles)
        tiles = new TileList();
      tiles.Refresh();

      SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    private void OnDisable() {
      tiles.Dispose();
      tiles = null;
      SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    private void Update() {

    }

    void SelectTileForEditing(AssetInfo<HexTile> tileInfo) {
      selectedTile = tileInfo;
    }

    void DrawTileButton(TileButton t) {
      var g = new GUIContent(t.texture, t.assetInfo.path);
      if(GUILayout.Button(g, GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) {
        SelectTileForEditing(t.assetInfo);
      }
    }

    void DrawTileButtons() {
      var w = Screen.width;
      var rowWidth = w / 70;
      Debug.Assert(0 < rowWidth, "button row count is a positive number");

      foreach(var tbn in tiles.Numbering()) {
        int i = tbn.number / rowWidth;
        int j = tbn.number % rowWidth;

        if(j == 0) {
          if(i > 0)
            EditorGUILayout.EndHorizontal();
          EditorGUILayout.BeginHorizontal();
        }

        DrawTileButton(tbn.value);
      }
      EditorGUILayout.EndHorizontal();
    }

    void OnGUI() {
      targetMap = EditorGUILayout.ObjectField(
        "Target",
        targetMap,
        typeof(HexMap),
        false) as HexMap;

      hexGridManager = EditorGUILayout.ObjectField(
        "Hex Grid Manager",
        hexGridManager,
        typeof(HexGridManager),
        true) as HexGridManager;

      EditorGUILayout.LabelField(
        "Selected tile",
        selectedTile?.path ?? "<none>");

      EditorGUILayout.BeginHorizontal();

      if(GUILayout.Button("Refresh", GUILayout.MaxWidth(65))) {
        tiles.Refresh();
      }

      // when both the target map and the hexgridmanager are set,
      // then we can load the map for editing.
      GUI.enabled = ReadyToLoad;
      var loadClicked = GUILayout.Button("Load", GUILayout.MaxWidth(50));
      GUI.enabled = true;

      if(loadClicked) {
        var grid = hexGridManager.LoadMap(targetMap);
        grid.Setup();
      }

      GUI.enabled = MapLoaded;
      var unloadClicked = GUILayout.Button("Unload", GUILayout.MaxWidth(50));
      GUI.enabled = true;

      EditorGUILayout.EndHorizontal();

      if(unloadClicked)
        hexGridManager.DestroyMap(immediate: true);

      DrawTileButtons();
    }

    void OnSceneGUI(SceneView sceneView) {
      if(!MapLoaded)
        return;

      // the Z coordinate might be bogus because we're taking the origin
      var mousePosition =
        HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin
        .Downgrade();

      var cell = hexGridManager.CurrentGrid.GetCellAt(mousePosition);

      if(Event.current.type == EventType.KeyDown)
        DispatchKeyEvent(Event.current.keyCode, cell);
    }

    /**
      * \brief
      * Dispatches on the `keyCode` to the appropriate function.
      */
    void DispatchKeyEvent(KeyCode keyCode, HexCell cell) {
      switch(keyCode) {
      case KeyCode.D:
        DeleteTile(cell);
        break;
      case KeyCode.Z:
        PlaceTile(cell);
        break;
      }
    }

    /**
      * \brief
      * Places the currently selected tile at the cursor location.
      */
    void PlaceTile(HexCell cell) {
      if(!MapLoaded || null == selectedTile || null == cell)
        return;
      var t = cell.coordinates.ToOffsetCoordinates();
      hexGridManager.CurrentGrid.Map[t] = selectedTile.asset;
    }

    /**
      * \brief
      * Deletes the tile under the cursor.
      */
    void DeleteTile(HexCell cell) {
      Debug.Log(String.Format("DELETE {0}.", cell));
      if(null == cell) {
        return;
      }
      var t = cell.coordinates.ToOffsetCoordinates();
      hexGridManager.CurrentGrid.Map[t] = null;
    }
  }
}
