using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class EnemyFOVEditor : FieldOfViewEditor
{
    public override void OnSceneGUI()
    {
        base.OnSceneGUI();
        
        EnemyFOV enemyFOV;
        enemyFOV = target as EnemyFOV;
        Vector3 enemyFOVOrigin = enemyFOV.PursuitOrigin != default ? enemyFOV.PursuitOrigin : enemyFOV.transform.position;

        // Draw Pursuit Radius
        Handles.color = Color.blue ;
        Handles.DrawWireArc(enemyFOVOrigin, Vector3.up, Vector3.forward, 360, enemyFOV.MaxPursuitRadius);

        // Draw Pursuit Indicator
        if (enemyFOV.InPursuit && !FOV.CanSeeTarget)
        {
            Handles.color = Color.green;
            Handles.DrawLine(FOVOrigin.position, FOV.Target.transform.position);
        }
    }
}
