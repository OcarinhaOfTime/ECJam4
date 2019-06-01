using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsymptoticCamera))]
public class AsymptoticCameraEditor : Editor {
    AsymptoticCamera cam;
    private const float handle_size = 1;
    private Rect limits_rect;
    private Vector2[] handles = new Vector2[4];
    private Event guiEvent;
    private SelectionInfo selectionInfo = new SelectionInfo();
    private bool needsRepaint;

    private Vector2 cam_pos {
        get {
            return cam.transform.position;
        }
    }
    private void OnEnable() {
        cam = target as AsymptoticCamera;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("Jump to target")) {
            cam.JumpToTarget();
        }
    }

    public void OnSceneGUI() {
        Init();

        if (guiEvent.type == EventType.Repaint) {
            Draw();
        } else if (guiEvent.type == EventType.Layout) {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        } else {
            HandleInput();
        }
    }

    private void Init() {
        guiEvent = Event.current;
        limits_rect = cam.limit;
        handles[0] = new Vector2(limits_rect.xMin - handle_size / 2, limits_rect.yMin + limits_rect.height / 2);
        handles[1] = new Vector2(limits_rect.xMin + limits_rect.width / 2, limits_rect.yMax - handle_size / 2);
        handles[2] = new Vector2(limits_rect.xMax - handle_size / 2, limits_rect.yMin + limits_rect.height / 2);
        handles[3] = new Vector2(limits_rect.xMin + limits_rect.width / 2, limits_rect.yMin - handle_size / 2);        
    }
    private void Draw() {
        for (int i = 0; i < handles.Length; i++) {
            DrawHandle(handles[i], i == selectionInfo.pointIndex ? Color.white : Color.red);
        }

        var drawRect = new Rect(limits_rect.x, limits_rect.y, limits_rect.width, limits_rect.height);
        Handles.DrawSolidRectangleWithOutline(drawRect, Color.clear, Color.red);
    }

    private void HandleInput() {
        guiEvent = Event.current;
        var mpos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None) {
            HandleLeftMouseDown(mpos);
        }

        if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None) {
            HandleLeftMouseUp(mpos);
        }

        if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None) {
            HandleLeftMouseDrag(mpos);
        }

        if (!selectionInfo.pointIsSelected)
            HandleMouseOver(mpos);

        if (needsRepaint) {
            HandleUtility.Repaint();
        }
    }

    private void HandleLeftMouseDown(Vector2 mpos) {
        selectionInfo.pointIsSelected = selectionInfo.mouseIsOverPoint;
    }

    private void HandleLeftMouseUp(Vector2 mpos) {
        selectionInfo.pointIsSelected = false;
    }

    private void HandleLeftMouseDrag(Vector2 mpos) {
        if (!selectionInfo.pointIsSelected)
            return;

        handles[selectionInfo.pointIndex] = mpos - cam_pos;
        UpdateRect();
        needsRepaint = true;
    }

    private void UpdateRect() {
        cam.limit = new Rect(
            handles[0].x + handle_size / 2,
            handles[3].y + handle_size / 2,
            handles[2].x - handles[0].x,
            handles[1].y - handles[3].y
            );
    }

    private void HandleMouseOver(Vector2 mpos) {
        int mouseOverIndex = -1;
        for (int i = 0; i < handles.Length; i++) {
            if (Vector3.Distance(mpos, handles[i]) <= 1) {
                mouseOverIndex = i;
                break;
            }
        }

        if (mouseOverIndex != selectionInfo.pointIndex) {
            selectionInfo.pointIndex = mouseOverIndex;
            selectionInfo.mouseIsOverPoint = mouseOverIndex >= 0;
            needsRepaint = true;
        }
    }

    private void DrawHandle(Vector2 pos, Color color) {
        var r1 = new Rect(pos, Vector2.one * handle_size);
        Handles.DrawSolidRectangleWithOutline(r1, color, color);
    }

    public class SelectionInfo {
        public int pointIndex = -1;
        public bool mouseIsOverPoint = false;
        public bool pointIsSelected = false;
    }
}
