using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            // "Assets" 폴더 내의 ".cs" 파일만 처리
            if (!path.StartsWith("Assets") || !path.EndsWith(".cs"))
                continue;

            // 에셋 경로를 시스템 전체 경로로 반환
            var scriptPath = Path.GetFullPath(path);

            // 디렉토리인 경우는 건너뛴다
            if (Directory.Exists(scriptPath))
                continue;

            // 한글 깨짐 수정(UTF-8 BOM 추가)
            var utf8 = new UTF8Encoding(true);

            // 스크립트 파일의 모든 라인을 읽어온다
            // 파일을 UTF-8로 읽어오도록 시도
            string[] lines = File.ReadAllLines(scriptPath, Encoding.Default);

            // 정규식을 사용하여 다양한 형태의 Debug.Log 구문 찾기
            var newLines = lines
                .Where(line => !Regex.IsMatch(line, @"Debug\.Log\s*\(.*\);"))
                .ToArray();

            // 변경된 내용이 있다면 (즉, Debug.Log 구문이 하나라도 사용됐다면)
            if (newLines.Length < lines.Length)
            {
                // 변경된 내용으로 파일을 다시 씁니다.
                // UTF-8 BOM이 포함된 인코딩으로 저장하여 한글 깨짐 방지
                File.WriteAllLines(scriptPath, newLines, utf8);

                // 변경된 파일 수 증가
                filesModified++;
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

    #region 확장형 구조

    /* 
    void RemoveLogs(string pattern)
    {
        var encoding = new UTF8Encoding(true);

        foreach (var path in allAssetPaths)
        {
            if (!path.StartsWith("Assets") || !path.EndsWith(".cs"))
                continue;

            var scriptPath = Path.GetFullPath(path);

            if (Directory.Exists(scriptPath))
                continue;

            string[] lines = File.ReadAllLines(scriptPath, encoding);

            var newLines = lines
                .Where(line => !Regex.IsMatch(line, pattern))
                .ToArray();

            if (newLines.Length < lines.Length)
            {
                File.WriteAllLines(scriptPath, newLines, encoding);
            }
        }
    }

    RemoveLogs(@"Debug\.Log\s*\(.*?\);");
    RemoveLogs(@"GameLogger\.Log\w*\s*\(.*?\);");
    */

    #endregion
}
