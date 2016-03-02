using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HairStudio))]
public class Editor_HairStudio : Editor {

    private HairStudio _target;
    private SerializedObject _serial;
    private SerializedProperty _serialProperty;

    // Emission Point Picking
    private bool isPickingEmissionPoint;
    private string emissionPointNotPickingString = "Add Emission Point(s)";
    private string emissionPointPickingString = "Done";
    private string emissionPointButtonString;

    void OnEnable() {
        _target = (HairStudio)target;
        _serial = new SerializedObject(target);

        isPickingEmissionPoint = false;
        emissionPointButtonString = emissionPointNotPickingString;
    }

    public override void OnInspectorGUI() {
        GUILayout.BeginVertical();

        //#############################################################################################
        // Emission section
        //#############################################################################################
        GUILayout.Label ("Emission", EditorStyles.boldLabel);

        // Emission Point Picking Button
        if(GUILayout.Button(emissionPointButtonString)) {
            isPickingEmissionPoint = !isPickingEmissionPoint;
            if(isPickingEmissionPoint) {
                emissionPointButtonString = emissionPointPickingString;
            } else {
                emissionPointButtonString = emissionPointNotPickingString;
            }
        }

        // Emission Points
        _serialProperty = _serial.FindProperty("emissionPoints");
        EditorGUILayout.PropertyField(_serialProperty, new GUIContent(" Emission Points"), true);
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

        if(GUI.changed) {
            EditorUtility.SetDirty(_target);
        }
    }

    public void OnSceneGUI(){
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        Event currentEvent = Event.current;

        if(currentEvent.GetTypeForControl(controlID) == EventType.MouseDown && currentEvent.button == 0){
            if(isPickingEmissionPoint) {
                GUIUtility.hotControl = controlID;
                SelectedEmissionPoint();
                currentEvent.Use();
            }
        }
    }

    public void SelectedEmissionPoint(){
        Debug.Log("Clicked while picking with LMB!");
    }
}
