using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class UpdateManagerEditor : Editor
{
    static UpdateManagerEditor()
    {
        AddDefineIfNecessary("DaBois_UpdateManager", BuildTargetGroup.Standalone);
        AddDefineIfNecessary("DaBois_UpdateManager", BuildTargetGroup.Android);
        AddDefineIfNecessary("DaBois_UpdateManager", BuildTargetGroup.iOS);
    }

    private static void AddDefineIfNecessary(string _define, BuildTargetGroup _buildTargetGroup)
    {
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(_buildTargetGroup);

        if (defines == null) { defines = _define; }
        else if (defines.Length == 0) { defines = _define; }
        else { if (defines.IndexOf(_define, 0) < 0) { defines += ";" + _define; } }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(_buildTargetGroup, defines);
    }


    private static void RemoveDefineIfNecessary(string _define, BuildTargetGroup _buildTargetGroup)
    {
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(_buildTargetGroup);

        if (defines.StartsWith(_define + ";"))
        {
            // First of multiple defines.
            defines = defines.Remove(0, _define.Length + 1);
        }
        else if (defines.StartsWith(_define))
        {
            // The only define.
            defines = defines.Remove(0, _define.Length);
        }
        else if (defines.EndsWith(";" + _define))
        {
            // Last of multiple defines.
            defines = defines.Remove(defines.Length - _define.Length - 1, _define.Length + 1);
        }
        else
        {
            // Somewhere in the middle or not defined.
            var index = defines.IndexOf(_define, 0, System.StringComparison.Ordinal);
            if (index >= 0) { defines = defines.Remove(index, _define.Length + 1); }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(_buildTargetGroup, defines);
    }
}
