using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace QbGameLib_Utils.Editor.Utils
{
    public class WebPlayerStarter : EditorWindow
    {
        private Process _process;
        private const string DEFAULT_URL = "http://localhost:" + DEFAULT_PORT;
        private const string DEFAULT_PORT = "8080";
        private const string WEB_SERVER_PATH = @"Data\PlaybackEngines\WebGLSupport\BuildTools\SimpleWebServer.exe";
        private const string MONO_PATH = @"Data\MonoBleedingEdge\bin\mono.exe";

        private string _lastBuildPath;
        private string _port;

        [MenuItem("Tools/WebGL Server")]
        public static void SetGameBuild()
           => GetWindow<WebPlayerStarter>("WebGL Server");

        private void DrawField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Build Path", GUILayout.Width(150));
            _lastBuildPath = GUILayout.TextField(_lastBuildPath, GUILayout.Height(30));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Port", GUILayout.Width(150));
            _port = GUILayout.TextField(_port, GUILayout.Height(30));
            GUILayout.EndHorizontal();
        }

        private void OnGUI()
        {
            GUILayout.Label($"Last Build Path: {GetLastBuildPath()}", GUILayout.Height(30));
            DrawField();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Start Server"))
                StartServer();

            if (GUILayout.Button("Terminate Server"))
                TerminateServer();

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Launch Browser"))
                LaunchBrowser();
        }

        private GUIStyle GetHeadingStyle()
        {
            return new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                },

                fontSize = 18
            };
        }

        private string GetLastBuildPath()
        {
            if (!string.IsNullOrEmpty(_lastBuildPath))
                return _lastBuildPath;

            return EditorUserBuildSettings.GetBuildLocation(BuildTarget.WebGL);
        }

        private void StartServer()
        {
            
            string apppath = Path.GetDirectoryName(EditorApplication.applicationPath);
            string lastBuildPath = GetLastBuildPath();
            string servetPath = Path.Combine(apppath, WEB_SERVER_PATH);
            _process = new Process();
            _process.StartInfo.FileName = Path.Combine(apppath, MONO_PATH);
            _process.StartInfo.Arguments = "\""+servetPath + "\" \"" +lastBuildPath + $"\" {DEFAULT_PORT}";
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.Start();
            Debug.Log($"[{nameof(WebPlayerStarter)}]: Starting server.... lastBuildPath: {lastBuildPath}");
            string error = _process.StandardError.ReadToEnd();
            if(error!=null) Debug.LogError($"[{nameof(WebPlayerStarter)}]: Starting error: {error}");
        }

        private void TerminateServer()
        {
            try
            {
                _process.Kill();
            }
            catch (System.Exception e)
            {
                Debug.Log($"[{nameof(WebPlayerStarter)}]: {e.Message}");
            }
        }

        private void LaunchBrowser()
        {
            try
            {
                if (_process.HasExited)
                {
                    StartServer();
                }
            }
            catch (System.Exception)
            {
                StartServer();
            }

            Process b = new Process();
            b.StartInfo.FileName = DEFAULT_URL;
            b.Start();
        }
    }
}