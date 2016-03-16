using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HairStudio))]
public class Editor_HairStudio : Editor {

    private HairStudio _target;
    private SerializedObject _serial;
    private SerializedProperty _serialProperty;

    // Strand Creation Button
    private bool isPickingStrandStartPoint;
    private string strandStartPointNotPickingString = "Create Hair Strand (C)";
    private string strandStartPointPickingString = "Done (Esc)";
    private string strandStartPointButtonString;

    // Editing state
    private enum EditingState {
        notEditing,
        creatingStrandStartPoint,
        creatingStrandControlPoint,
        editingStrandPoints
     }

     private EditingState currentEditingState;

    void OnEnable(){
        _target = (HairStudio)target;
        _serial = new SerializedObject(target);

        isPickingStrandStartPoint = false;
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
            isPickingStrandStartPoint = !isPickingStrandStartPoint;
            if(isPickingStrandStartPoint){
                strandStartPointButtonString = strandStartPointPickingString;
                currentEditingState = EditingState.creatingStrandStartPoint;
            } else {
                strandStartPointButtonString = strandStartPointNotPickingString;
                currentEditingState = EditingState.notEditing;
            }
        }

        // Emission Points
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

        // Lots of spare GUI crap - use it or lose it

        // _target.isDelicious =  EditorGUILayout.Toggle("Is it Delicous Cake?", _target.isDelicious); // Our bool
        // _target.amountOfChocolate = EditorGUILayout.Slider("How much Chocolate?", _target.amountOfChocolate, 0.0f, 10.0f); // A slider to make thing better looking
        //
        // // There is now ay to have a cake without chocolate
        // if(_target.amountOfChocolate == 0)
        // {
        //  EditorGUILayout.HelpBox("THERE IS NO CHOCOLATE IN THIS CAKE", MessageType.Error);
        // }
        //
        // _target.randomNumber = EditorGUILayout.IntField("Just a number", _target.randomNumber); // Common INT field
        // _target.cakeColor = EditorGUILayout.ColorField("Color", _target.cakeColor); // Color Field
        // _target.cakeT = (HairStudio.cakeTypes)EditorGUILayout.EnumPopup("Cake type", _target.cakeT); // Enum Field - It needs proper casting
        //
        // if(GUILayout.Button("DO CAKE"))
        // {
        //   _target.BakeTheCake();
        // }

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
        }
        // TODO add an "esc" button as well (maybe keyboard shortcuts all around?)
    }

    private void clickedOnSceneWhileEditing(){
        switch(currentEditingState){
            case EditingState.creatingStrandStartPoint:
                createStrandStartPoint();
                break;
            case EditingState.creatingStrandControlPoint:
                Debug.Log("Creating Strand Control Point");
                break;
        }
    }

    private void createStrandStartPoint(){
        Debug.Log("Creating Strand Start Point");
        currentEditingState = EditingState.creatingStrandControlPoint;
        
    }
}
