using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HairStudio))]
public class Editor_HairStudio : Editor {

    // Editing state Enum
    private enum EditingState {
        notEditing,
        creatingStrandStartPoint,
        creatingStrandControlPoint,
        editingStrandPoints
    }

    // Object
    private HairStudio _target;
    private SerializedObject _serial;
    private SerializedProperty _serialProperty;

    // Strand Creation Button
    private bool isPickingStrandStartPoint;
    private string strandStartPointNotPickingString = "Create Hair Strand (C)";
    private string strandStartPointPickingString = "Done (Esc)";
    private string strandStartPointButtonString;

    // Editing State
    private EditingState currentEditingState;

    Strand currentStrand;

    // TEMPORARY STUFF
    Vector3 temp_vector;
    string texture;
    Texture2D inputTexture;

    void OnEnable(){
        texture = "GUI/blue_pellet";
        inputTexture = (Texture2D)Resources.Load(texture);

        _target = (HairStudio)target;
        // _serial = new SerializedObject(target);

        strandStartPointButtonString = strandStartPointNotPickingString;
        currentEditingState = EditingState.notEditing;
    }

    public override void OnInspectorGUI(){
        GUILayout.BeginVertical();

        //#############################################################################################
        // Emission section
        //#############################################################################################
        GUILayout.Label ("Hair Strands", EditorStyles.boldLabel);

        // Emission Point Picking Button
        if(GUILayout.Button(strandStartPointButtonString)){
            if(currentEditingState == EditingState.notEditing){
                beginCreatingHairStrand();
            } else {
                endCreatingHairStrand();
            }
        }

        if(GUILayout.Button("Edit Hair Strand (D)")){

        }

        // Emission Points
        // Loop over these, maybe? For more granular control, etc
        // _serialProperty = _serial.FindProperty("strands");
        // EditorGUILayout.PropertyField(_serialProperty, new GUIContent(" Strands"), true);
        // _serial.ApplyModifiedProperties();

        //#############################################################################################
        // Rendering section
        //#############################################################################################
        GUILayout.Label ("Rendering", EditorStyles.boldLabel);

        //#############################################################################################
        // Physics section
        //#############################################################################################
        GUILayout.Label ("Physics", EditorStyles.boldLabel);

        GUILayout.EndVertical();

        if(GUI.changed){
            EditorUtility.SetDirty(_target);
        }
    }

    public void OnSceneGUI() {

        // Handling Input
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        Event currentEvent = Event.current;
        EventType currentEventType = currentEvent.GetTypeForControl(controlID);

        if(currentEventType == EventType.MouseDown){
            if(currentEvent.button == 0 && currentEditingState != EditingState.notEditing){
                clickedOnSceneWhileEditing();
                GUIUtility.hotControl = controlID;
                currentEvent.Use();
            }
        } else if (currentEventType == EventType.MouseUp){
            if(currentEvent.button == 0 && currentEditingState != EditingState.notEditing){
                GUIUtility.hotControl = 0;
                currentEvent.Use();
            }
        } else if (currentEventType == EventType.KeyDown){
            // Keyboard shortcuts
            if(currentEvent.keyCode == KeyCode.C){
                beginCreatingHairStrand();
            } else if (currentEvent.keyCode == KeyCode.D){
                Debug.Log("D Pressed event");
            } else if (currentEvent.keyCode == KeyCode.Escape){
                if(currentEditingState == EditingState.creatingStrandControlPoint || currentEditingState == EditingState.creatingStrandStartPoint){
                    endCreatingHairStrand();
                }
                // Other states, like saving edited strands
            }

            EditorUtility.SetDirty(_target);
        }

        // Drawing GUI
        Handles.BeginGUI();
        // get vector3's screenspace
        Vector3 screenPos = Camera.current.WorldToScreenPoint(temp_vector);
        GUI.DrawTexture(new Rect(screenPos.x - 3, Screen.height - screenPos.y - 40, 5, 5), inputTexture);
        Handles.EndGUI();
    }

    private void beginCreatingHairStrand(){
        strandStartPointButtonString = strandStartPointPickingString;
        currentEditingState = EditingState.creatingStrandStartPoint;
    }

    private void endCreatingHairStrand(){
        strandStartPointButtonString = strandStartPointNotPickingString;
        currentEditingState = EditingState.notEditing;

        // Other special things like saving strand
    }

    private void clickedOnSceneWhileEditing(){
        switch(currentEditingState){
            case EditingState.creatingStrandStartPoint:
                createStrandStartPoint();
                break;
            case EditingState.creatingStrandControlPoint:
                createStrandControlPoint();
                break;
        }
    }

    private void createStrandStartPoint(){
        currentEditingState = EditingState.creatingStrandControlPoint;
        getWorldCoordinateOnModel();
    }

    private void createStrandControlPoint(){
        Debug.Log("Creating Strand Control Point");

        // Other stuff too
    }

    private Vector3 getWorldCoordinateOnModel(){
        // Add a mesh collider component so we can register click hits
        _target.AddMeshColliderComponent();

        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Debug.Log(Event.current.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast (worldRay, out hit)){
            // Undo.RegisterUndo (target, "Add Path Node");
            // ((Path)target).AddNode (hitInfo.point);
            Debug.Log("Hit something!");
            temp_vector = hit.point;
        }

        // Clean up
        _target.RemoveMeshColliderComponent();

        return new Vector3(0, 0, 0);
    }
}
