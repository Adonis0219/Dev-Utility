# Dev Utility (Collection)

Unity development utilities to improve productivity, including logging control, editor tools, and reusable helper modules.



## 🧑‍💻 About

- Unity 기반 개발 과정에서 만든 **개발 생산성 향상 유틸리티 모음**
- 반복 작업을 줄이고 **디버깅 및 구조 개선**을 목표로 제작
- 실제 프로젝트에서 사용한 코드 기반
</br>


## 🚀 Features

### 🔹 GameLogger
- Conditional Attribute 기반 로그 제어
- 빌드 환경에서 Debug.Log 자동 제거

```csharp
[Conditional("UNITY_EDITOR")]
public static void Log(object message)
{
    Debug.Log(message);
}
```
### 🔹 DebugLogRemover
- 프로젝트 전체 Debug.Log 제거 자동화
- Editor Tool (One-Click)
### 🔹 TMP_FontChanger
- TextMeshPro 폰트 일괄 변경
### 🔹 PlayerPrefsHelper
- PlayerPrefs의 bool 저장 문제 해결
###  🔹 Util
- Time, Transform, UI 등 공통 기능 모듈
- 일부 기능 직접 구현 및 구조 정리
</br>

## 🛠 Features
- Unity (C#)
- Unity Editor Extension
- Conditional Attribute
- 일부 Reflection 활용
