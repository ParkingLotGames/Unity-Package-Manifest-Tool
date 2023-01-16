using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
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
    public Author author;
    public Dictionary<string, string> dependencies;

    public class Author
    {
        public string name;
        public string email;
        public string url;
    }
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
            dependencies = new Dictionary<string, string>(),
            keywords = new string[] { "keyword1", "keyword2", "keyword3" },
            author = new JsonData.Author()
            {
                name = "Unity",
                email = "unity@example.com",
                url = "https://www.unity3d.com"
            }
        };

        string jsonString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        string filePath = EditorUtility.SaveFilePanel("Save JSON File", "Assets", "NewJsonFile.json", "json");
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
            jsonData = JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(jsonFilePath));
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

            EditorGUILayout.LabelField("Keywords");
            for (int i = 0; i < jsonData.keywords.Length; i++)
            {
                jsonData.keywords[i] = EditorGUILayout.TextField("Keyword " + (i + 1), jsonData.keywords[i]);
            }

            EditorGUILayout.LabelField("Author");
            jsonData.author.name = EditorGUILayout.TextField("Name", jsonData.author.name);
            jsonData.author.email = EditorGUILayout.TextField("Email", jsonData.author.email);
            jsonData.author.url = EditorGUILayout.TextField("URL", jsonData.author.url);

            if (GUILayout.Button("Save JSON"))
            {
                SaveJson();
            }
        }
    }

    private void LoadJson()
    {
        string jsonString = File.ReadAllText(jsonFilePath);
        jsonData = JsonConvert.DeserializeObject<JsonData>(jsonString);

        if (!IsJsonValid())
        {
            jsonData = null;
            EditorUtility.DisplayDialog("Error", "Invalid JSON file format.", "OK");
        }
    }

    private void SaveJson()
    {
            // Clear existing dependencies
            jsonData.dependencies.Clear();

            // Add new dependencies to jsonData object
            for (int i = 0; i < dependenciesKey.Count; i++)
            {
                jsonData.dependencies.Add(dependenciesKey[i], dependenciesValue[i]);
            }


            // Save json string to file
        string jsonString = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
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
            || jsonData.keywords == null || jsonData.author == null)
        {
            return false;
        }

        return true;
    }
}
