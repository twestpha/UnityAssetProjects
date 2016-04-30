using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HairStudio))]
public class Editor_HairStudio : Editor {

    // Enums
    private enum EditingState {
        notEditing,
        creatingStrandControlPoint,
        editingStrandControlPoints
    }

    private enum RenderingState {
        LineRendering,
        GeometryRendering,
        TexturedRendering
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

    // States
    private EditingState currentEditingState;
    private RenderingState currentRenderingState;

    Strand currentStrand;

    void OnEnable(){

        _target = (HairStudio)target;
        // _serial = new SerializedObject(target);

        currentStrand = new Strand();

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
        string[] text = {"Line", "Wireframe", "Textured"};

        currentRenderingState = (RenderingState)GUILayout.SelectionGrid((int)currentRenderingState, text, 3);
        switch(currentRenderingState){
        case RenderingState.LineRendering:
            // something
            break;
        }

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
                Debug.Log("D Pressed event"); // Toggle editing (later)
            } else if (currentEvent.keyCode == KeyCode.Escape){
                // Handle "cancel" operations depending on state
                if(currentEditingState == EditingState.creatingStrandControlPoint){
                    endCreatingHairStrand();
                }
                // Other states, like saving edited strands
            }

            EditorUtility.SetDirty(_target);
        }

        // Drawing GUI
        Handles.BeginGUI();
            if(currentStrand.HasControlPoints()){

                // Draw current strand (Wireframe)
                currentStrand.drawWireframeToGUI();
            }

        Handles.EndGUI();
    }

    private void clickedOnSceneWhileEditing(){
        switch(currentEditingState){
            case EditingState.creatingStrandControlPoint:
                createStrandControlPoint();
                break;
        }
    }

    private void beginCreatingHairStrand(){
        strandStartPointButtonString = strandStartPointPickingString;
        currentEditingState = EditingState.creatingStrandControlPoint;
    }

    private void endCreatingHairStrand(){
        strandStartPointButtonString = strandStartPointNotPickingString;
        currentEditingState = EditingState.notEditing;

        // Other special things like saving strand to the instance data
    }

    private void createStrandControlPoint(){
        // Add a mesh collider component so we can register click hits
        _target.AddMeshColliderComponent();

        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 normal = worldRay.direction * -1;
        RaycastHit hit;

        if(Physics.Raycast (worldRay, out hit)){
            currentStrand.AddControlPointAndNormal(hit.point, normal);
        }

        // Clean up
        _target.RemoveMeshColliderComponent();
    }
}
