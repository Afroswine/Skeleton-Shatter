using UnityEditor;
using UnityEngine;

// When "FieldOfView" is active, this editor will start running
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private FieldOfView _fov;
    protected FieldOfView FOV => _fov;
    private Transform _fovOrigin;
    protected Transform FOVOrigin => _fovOrigin;

    public virtual void OnSceneGUI()
    {
        // Access the target's FieldOfView
        _fov = target as FieldOfView;
        _fovOrigin = _fov.ViewOrigin;
        float currentRadius = _fov.CurrentRadius != default ? _fov.CurrentRadius : _fov.Radius;
        float currentAngle = _fov.CurrentAngle != default ? _fov.CurrentAngle : _fov.Angle;

        // In the event that fovOrigin has not been set...
        if(_fovOrigin != null)
            Handles.color = Color.white;
        else
        {
            _fovOrigin = _fov.gameObject.transform;
            Handles.color = Color.magenta;
        }

        // Draw Radius visualizer
        Handles.DrawWireArc(_fovOrigin.position, Vector3.up, Vector3.forward, 360, currentRadius);

        // Draw View Angle visualizer
        Vector3 viewAngle01 = DirectionFromAngle(_fovOrigin.eulerAngles.y, -currentAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(_fovOrigin.eulerAngles.y, currentAngle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(_fovOrigin.position, _fovOrigin.position + viewAngle01 * currentRadius, 2f);
        Handles.DrawLine(_fovOrigin.position, _fovOrigin.position + viewAngle02 * currentRadius, 2f);
        Handles.DrawWireArc(_fovOrigin.position, Vector3.up, viewAngle01, currentAngle, currentRadius, 2f);

        // Dray Line indicating that a target is within the FOV
        if (_fov.CanSeeTarget)
        {
            Handles.color = Color.red;
            //for (int i = 0; i < fov.Targets.Count; i++)
            //    Handles.DrawLine(fovOrigin.position, fov.Targets[i].transform.position); 
            Handles.DrawLine(_fovOrigin.position, _fov.Target.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
