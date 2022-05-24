using UnityEngine;

[CreateAssetMenu(fileName = "FXSO", menuName = "ScriptableObjects/FX")]
public class FXSO : ScriptableObject
{
    [SerializeField]
    private ParticleSystem _ps;
    public ParticleSystem PS => _ps;
    [SerializeField]
    private AudioClip _clip;
    public AudioClip Clip => _clip;
}
