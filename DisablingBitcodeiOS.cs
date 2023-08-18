using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor;
using UnityEngine;
using System.IO;

public class DisablingBitcodeiOS : MonoBehaviour
{
    [PostProcessBuild(1000)]
    public static void PostProcessBuildAttribute(BuildTarget target, string pathToBuildProject)
    {
        if (target == BuildTarget.iOS)
        {
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuildProject);

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
#if UNITY_2019_3_OR_NEWER
            var targetGuid = pbxProject.GetUnityMainTargetGuid();
#else
                var targetName = PBXProject.GetUnityTargetName();
                var targetGuid = pbxProject.TargetGuidByName(targetName);
#endif
            pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            pbxProject.WriteToFile(projectPath);

            var projectInString = File.ReadAllText(projectPath);

            projectInString = projectInString.Replace("ENABLE_BITCODE = YES;",
              $"ENABLE_BITCODE = NO;");
            File.WriteAllText(projectPath, projectInString);
        }
    }
}
