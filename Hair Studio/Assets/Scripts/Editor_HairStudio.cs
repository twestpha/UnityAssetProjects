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

    void OnEnable(){
        _target = (HairStudio)target;
        _serial = new SerializedObject(target);

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
        _serialProperty = _serial.FindProperty("strands");
        EditorGUILayout.PropertyField(_serialProperty, new GUIContent(" Strands"), true);
        _serial.ApplyModifiedProperties();

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

            }
        }
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
        Debug.Log("Creating Strand Start Point");
        currentEditingState = EditingState.creatingStrandControlPoint;

    }

    private void createStrandControlPoint(){
        Debug.Log("Creating Strand Control Point");

        // Other stuff too
    }
}
