using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HairStudio))]
public class Editor_HairStudio : Editor {

    // Editing state Enum
    private enum EditingState {
        notEditing,
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
    string texture;
    Texture2D inputTexture;

    void OnEnable(){
        texture = "GUI/blue_pellet";
        inputTexture = (Texture2D)Resources.Load(texture);

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
                for(int i=0; i < currentStrand.ControlPointsCount(); ++i){
                    Vector3 screenPos = Camera.current.WorldToScreenPoint(currentStrand.getControlPointFromIndex(i));
                    GUI.DrawTexture(new Rect(screenPos.x - 3, Screen.height - screenPos.y - 40, 5, 5), inputTexture);

                    if(i > 0){
                        Vector3 screenPos0 = Camera.current.WorldToScreenPoint(currentStrand.getControlPointFromIndex(i));
                        Vector3 screenPos1 = Camera.current.WorldToScreenPoint(currentStrand.getControlPointFromIndex(i - 1));

                        Vector2 a = new Vector2(screenPos0.x - 3, Screen.height - screenPos0.y - 40);
                        Vector2 b = new Vector2(screenPos1.x - 3, Screen.height - screenPos1.y - 40);

                        // float width = 1.0f;
                        // Color color = Color.black;
                        //
                        // Drawing.DrawLine(a, b, color, width);
                    }
                }
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
