using UnityEditor;
using UnityEngine;

// When "FieldOfView" is active, this editor will start running
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private FieldOfView _fov;
    public FieldOfView FOV => _fov;
    private Transform _fovOrigin;
    public Transform FOVOrigin => _fovOrigin;

    public virtual void OnSceneGUI()
    {
        // Access the target's FieldOfView
        _fov = target as FieldOfView;
        _fovOrigin = _fov.ViewOrigin;

        // In the event that fovOrigin has not been set...
        if(_fovOrigin != null)
            Handles.color = Color.white;
        else
        {
            _fovOrigin = _fov.gameObject.transform;
            Handles.color = Color.magenta;
        }

        // Draw Radius visualizer
        Handles.DrawWireArc(_fovOrigin.position, Vector3.up, Vector3.forward, 360, _fov.Radius);

        // Draw View Angle visualizer
        Vector3 viewAngle01 = DirectionFromAngle(_fovOrigin.eulerAngles.y, -_fov.Angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(_fovOrigin.eulerAngles.y, _fov.Angle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(_fovOrigin.position, _fovOrigin.position + viewAngle01 * _fov.Radius);
        Handles.DrawLine(_fovOrigin.position, _fovOrigin.position + viewAngle02 * _fov.Radius);

        // Dray Line indicating that a target is within the FOV
        if (_fov.CanSeeTarget)
        {
            Handles.color = Color.red;
            //for (int i = 0; i < fov.Targets.Count; i++)
            //    Handles.DrawLine(fovOrigin.position, fov.Targets[i].transform.position); 
            Handles.DrawLine(_fovOrigin.position, _fov.Target.transform.position);
        }

        // if target uses EnemyFOV, display the pursuit range as well
        #region EnemyFOV
        /*
        EnemyFOV enemyFOV;
        enemyFOV = target as EnemyFOV;
        Vector3 enemyFOVOrigin = enemyFOV.PursuitOrigin;

        if (enemyFOVOrigin != new Vector3(0, 0, 0))
        {
            Handles.color = Color.cyan;
            Handles.DrawWireArc(enemyFOVOrigin, Vector3.up, Vector3.forward, 360, enemyFOV.MaxPursuitRadius);

            if (enemyFOV.InPursuit && !_fov.CanSeeTarget)
            {
                Handles.color = Color.green;
                Handles.DrawLine(_fovOrigin.position, _fov.Target.transform.position);
            }
        }
        else if (enemyFOV != null)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireArc(enemyFOV.transform.position, Vector3.up, Vector3.forward, 360, enemyFOV.MaxPursuitRadius);
        }
        */
        #endregion
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
