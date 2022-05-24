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
        Vector3 enemyFOVOrigin = enemyFOV.PursuitOrigin;

        if (enemyFOVOrigin != new Vector3(0, 0, 0))
        {
            Handles.color = Color.cyan;
            Handles.DrawWireArc(enemyFOVOrigin, Vector3.up, Vector3.forward, 360, enemyFOV.MaxPursuitRadius);

            if (enemyFOV.InPursuit && !FOV.CanSeeTarget)
            {
                Handles.color = Color.green;
                Handles.DrawLine(FOVOrigin.position, FOV.Target.transform.position);
            }
        }
        else if (enemyFOV != null)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireArc(enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV.MaxPursuitRadius);
        }
    }
}
