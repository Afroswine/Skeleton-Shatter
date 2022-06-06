using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Grants access to various stat presets
public class Stats
{
    public static MassPresets Mass { get; private set; }
    public static ForcePresets Force { get; private set; }
    public static FontPresets Font { get; private set; }
    public static GravityPresets Gravity { get; }
    
    public class MassPresets    // Presets for an object's mass
    {
        public readonly float Light = 0.3f;
        public readonly float Medium = 1f;
        public readonly float Heavy = 3f;
    }
    public class ForcePresets   // Presets for force application
    {
        public readonly float Light = 40f;
        public readonly float Medium = 80f;
        public readonly float Heavy = 120f;
    }
    public class GravityPresets
    {
        [Tooltip("Gravity used by the player. DO NOT TOUCH OUTSIDE OF PlayerMovement !!!")]
        public float PlayerGrav = 6f;
        [Tooltip("MaxFall used by the player. DO NOT TOUCH OUTSIDE OF PlayerMovement !!!")]
        public float PlayerMaxFall = 35f;
    }
    public class FontPresets    // Presets for fonts
    {
        /*
        private readonly Font normalSource = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        public Font normal
        {
            get
            {
                Font clonedFont = (Font)Object.Instantiate(normalSource);
                return clonedFont;
            }
        }
        */
    }
}