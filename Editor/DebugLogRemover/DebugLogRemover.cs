using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class DebugLogRemover : MonoBehaviour
{
    // "Tools" 메뉴에 "Remove All Debug.Log" 항목을 추가
    [MenuItem("Tool/Remove All Debug.Log")]
    public static void RemoveAllDebugLogs()
    {
        // 프로젝트의 모든 에셋 경로를 가져오기
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();

        int filesModified = 0;

        foreach (var path in allAssetPaths)
        {
            // C# 스크립트 파일(.cs)만 대상으로 한다
            if (path.EndsWith(".cs"))
                return;

            // 에셋 경로를 시스템 전체 경로로 반환
            var scriptPath = Path.GetFullPath(path);
    
            // 스크립트 파일의 모든 라인을 읽어온다
            string[] lines = File.ReadAllLines(scriptPath);

            // "Debug.Log(...);" 패턴을 포함하지 않는 라인만 필터링한다.
            // 정규식을 사용하여 다양한 형태의 Debug.Log 구문 찾기
            var newLines = lines.Where(line => !Regex.IsMatch(line, @"Debug\.log\s*\(.*\);")).ToArray();
            
            // 변경된 내용이 있다면 (즉, Debug.Log 구문이 하나라도 사용됐다면
            if (newLines.Length < lines.Length)
            {
                // 변경된 내용으로 파일을 다시 씁니다.
                File.WriteAllLines(scriptPath, newLines);
                
                // 어떤 파일이 수정됐는지 콘솔에 로그를 남기기
                Debug.Log($"Remove Debug.Log statements from : {path}");
            }
        }
        
        // 스크립트 파일 변경 후에는 Unity 에셋 데이터 베이스를 새로고침 해야 함
        AssetDatabase.Refresh();

        if (filesModified > 0)
        {
            EditorUtility.DisplayDialog("Result", $"Successfully removed Debug.Log from {filesModified}files.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Result", "No Debug.Log statements found.", "OK");
        }
    }
}
