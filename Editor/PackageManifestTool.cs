using UnityEditor;
using UnityEngine;
using System.IO;
#if UNITY_2022_1_OR_NEWER
using Newtonsoft.Json;
#endif
using System.Collections.Generic;

//TODO: Add buttons to remove keywords and dependencies individually.
public class JsonData
{
    public string name;
    public string version;
    public string displayName;
    public string description;
    public string unity;
    public string unityRelease;
    public string documentationUrl;
    public string changelogUrl;
    public string licensesUrl;
    public string[] keywords;
#if UNITY_2022_1_OR_NEWER
    public Dictionary<string, string> dependencies;
    public Author author;
    public class Author
    {
        public string name;
        public string email;
        public string url;
    }
#endif
}

public class CreateJsonFile
{
    [MenuItem("Assets/Create/JSON File")]
    static void CreateJson()
    {
        JsonData jsonData = new JsonData()
        {
            name = "com.[company-name].[package-name]",
            version = "1.2.3",
            displayName = "Package Example",
            description = "This is an example package",
            unity = "2019.1",
            unityRelease = "0b5",
            documentationUrl = "https://example.com/",
            changelogUrl = "https://example.com/changelog.html",
            licensesUrl = "https://example.com/licensing.html",
            keywords = new string[] { "keyword1", "keyword2", "keyword3" },
#if UNITY_2022_1_OR_NEWER
            dependencies = new Dictionary<string, string>(),
            author = new JsonData.Author()
            {
                name = "Unity",
                email = "unity@example.com",
                url = "https://www.unity3d.com"
            }
#endif
        };
#if UNITY_2018_1_OR_NEWER && !UNITY_2022
        string jsonString = JsonUtility.ToJson(jsonData, true);
#endif

#if UNITY_2022_2_OR_NEWER
        string jsonString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
#endif
        string filePath = EditorUtility.SaveFilePanel("Save package.json", "Assets", "package.json", "json");
        File.WriteAllText(filePath, jsonString);
        AssetDatabase.Refresh();
    }
}

public class JsonEditorUtility : EditorWindow
{
    private string jsonFilePath;
    private JsonData jsonData;

    [MenuItem("Tools/Package Manifest Tool")]
    static void ShowWindow()
    {
        var window = GetWindow<JsonEditorUtility>("Package Manifest Tool");
        window.Show();
    }

    // Lists to store the key-value pairs for the dependencies
    public List<string> dependenciesKey = new List<string>();
    public List<string> dependenciesValue = new List<string>();

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("...", GUILayout.MaxWidth(24)))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");

#if UNITY_2018_1_OR_NEWER && !UNITY_2022
            jsonData = JsonUtility.FromJson<JsonData>(File.ReadAllText(jsonFilePath));
#endif

#if UNITY_2022_1_OR_NEWER
            jsonData = JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(jsonFilePath));
#endif
        }
        EditorGUILayout.EndHorizontal();

        if (jsonData != null)
        {
            jsonData.name = EditorGUILayout.TextField("Name", jsonData.name);
            jsonData.version = EditorGUILayout.TextField("Version", jsonData.version);
            jsonData.displayName = EditorGUILayout.TextField("Display Name", jsonData.displayName);
            jsonData.description = EditorGUILayout.TextField("Description", jsonData.description);
            jsonData.unity = EditorGUILayout.TextField("Unity", jsonData.unity);
            jsonData.unityRelease = EditorGUILayout.TextField("Unity Release", jsonData.unityRelease);
            jsonData.documentationUrl = EditorGUILayout.TextField("Documentation URL", jsonData.documentationUrl);
            jsonData.changelogUrl = EditorGUILayout.TextField("Changelog URL", jsonData.changelogUrl);
            jsonData.licensesUrl = EditorGUILayout.TextField("Licensing URL", jsonData.licensesUrl);

#if UNITY_2022_1_OR_NEWER

            EditorGUILayout.LabelField("Dependencies");
            // Add a new dependency button
            if (GUILayout.Button("Add Dependency"))
            {
                dependenciesKey.Add("com.[company-name].dependency");
                dependenciesValue.Add("1.0.0");
            }

            // Show the current dependencies
            for (int i = 0; i < dependenciesKey.Count; i++)
            {
                GUILayout.BeginHorizontal();
                dependenciesKey[i] = EditorGUILayout.TextField("Dependency Name", dependenciesKey[i]);
                dependenciesValue[i] = EditorGUILayout.TextField("Version", dependenciesValue[i]);
                GUILayout.EndHorizontal();
            }
#endif
            EditorGUILayout.LabelField("Keywords");
            for (int i = 0; i < jsonData.keywords.Length; i++)
            {
                jsonData.keywords[i] = EditorGUILayout.TextField("Keyword " + (i + 1), jsonData.keywords[i]);
            }
#if UNITY_2022_1_OR_NEWER
            EditorGUILayout.LabelField("Author");
            jsonData.author.name = EditorGUILayout.TextField("Name", jsonData.author.name);
            jsonData.author.email = EditorGUILayout.TextField("Email", jsonData.author.email);
            jsonData.author.url = EditorGUILayout.TextField("URL", jsonData.author.url);
#endif
            if (GUILayout.Button("Save JSON"))
            {
                SaveJson();
            }
        }
    }

    private void LoadJson()
    {
        string jsonString = File.ReadAllText(jsonFilePath);
#if UNITY_2018_1_OR_NEWER && !UNITY_2022
        jsonData = JsonUtility.FromJson<JsonData>(File.ReadAllText(jsonString));
#endif

#if UNITY_2022_1_OR_NEWER
            jsonData = JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(jsonString));
#endif
        if (!IsJsonValid())
        {
            jsonData = null;
            EditorUtility.DisplayDialog("Error", "Invalid JSON file format.", "OK");
        }
    }

    private void SaveJson()
    {
#if UNITY_2022_1_OR_NEWER

        // Clear existing dependencies
        jsonData.dependencies.Clear();

        // Add new dependencies to jsonData object
        for (int i = 0; i < dependenciesKey.Count; i++)
        {
            jsonData.dependencies.Add(dependenciesKey[i], dependenciesValue[i]);
        }
#endif


        // Save json string to file
#if UNITY_2018_1_OR_NEWER && !UNITY_2022
        string jsonString = JsonUtility.ToJson(jsonData, true);
#endif

#if UNITY_2022_1_OR_NEWER
        string jsonString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
#endif
        File.WriteAllText(jsonFilePath, jsonString);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "JSON file saved successfully.", "OK");
    }

    private bool IsJsonValid()
    {
        // Check if jsonData contains all the required properties
        if (jsonData.name == null || jsonData.version == null || jsonData.displayName == null
            || jsonData.description == null || jsonData.unity == null || jsonData.unityRelease == null
            || jsonData.documentationUrl == null || jsonData.changelogUrl == null || jsonData.licensesUrl == null
#if UNITY_2022_1_OR_NEWER
            || jsonData.keywords == null || jsonData.author == null
#endif
            )
        {
            return false;
        }

        return true;
    }
}
