using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Grants access to various stat presets
public static class Stats
{
    public static MassPresets Mass { get; }
    public static ForcePresets Force { get; }
    public static FontPresets Font { get; }

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