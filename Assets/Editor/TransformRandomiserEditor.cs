using UnityEngine;
using UnityEditor;

public class TransformRandomiserEditor : EditorWindow
{
    bool randomX, randomY, randomZ;
    bool randomScale;
    float minScale, maxScale;

    [MenuItem("Utilities/Transform Randomiser")]
    static void Init()
    {
        TransformRandomiserEditor window = (TransformRandomiserEditor)GetWindow(typeof(TransformRandomiserEditor));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Randomise Selected Objects", EditorStyles.whiteLargeLabel);
        GUILayout.Space(10);

        GUILayout.Label("Rotations", EditorStyles.boldLabel);
        randomX = EditorGUILayout.Toggle("    Randomise X", randomX);
        randomY = EditorGUILayout.Toggle("    Randomise Y", randomY);
        randomZ = EditorGUILayout.Toggle("    Randomise Z", randomZ);
        GUILayout.Space(5);

        GUILayout.Label("Scaling", EditorStyles.boldLabel);
        randomScale = EditorGUILayout.Toggle("    Randomise Scale", randomScale);
        minScale = EditorGUILayout.FloatField("    Minimum Scale", minScale);
        maxScale = EditorGUILayout.FloatField("    Maximum Scale", maxScale);
        GUILayout.Space(5);

        if (GUILayout.Button("Randomise"))
        {
            foreach(GameObject go in Selection.gameObjects)
            {
                go.transform.rotation = Quaternion.Euler(GetRandomRotations(go.transform.rotation.eulerAngles));

                if (randomScale)
                {
                    float scaleVal = Random.Range(minScale, maxScale);
                    go.transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
                }
            }
        }
    }

    private Vector3 GetRandomRotations(Vector3 currentRotation)
    {
        float x = randomX ? Random.Range(0f, 360f) : currentRotation.x;
        float y = randomY ? Random.Range(0f, 360f) : currentRotation.y;
        float z = randomZ ? Random.Range(0f, 360f) : currentRotation.z;

        return new Vector3(x, y, z);
    }
}
