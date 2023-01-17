using UnityEditor;
using UnityEngine;
using System.IO;
#if UNITY_2020_1_OR_NEWER
using Newtonsoft.Json;
#endif
using System.Collections.Generic;

// TODO: Consider adding buttons to remove keywords individually (convert keywords to List).

public class PackageManifestData
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
#if UNITY_2020_1_OR_NEWER
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

public class CreatePackageManifestFile
{
    [MenuItem("Assets/Create/Package Manifest", priority = 1)]
    static void CreateJson()
    {
        PackageManifestData packageManifestData = new PackageManifestData()
        {
            name = "com.[company-name].[package-name]",
            version = "1.2.3",
            displayName = "Package Example",
            description = "This is an example package",
            unity = "2019.1",
            unityRelease = "0b5",
            documentationUrl = "https://example.com/",
            changelogUrl = "https://example.com/changelog.html",
            licensesUrl = "https://example.com/licenses.html",
            keywords = new string[] { "keyword1", "keyword2", "keyword3" },
#if UNITY_2020_1_OR_NEWER
            dependencies = new Dictionary<string, string>(),
            author = new PackageManifestData.Author()
            {
                name = "Unity",
                email = "unity@example.com",
                url = "https://www.unity3d.com"
            }
#endif
        };
#if !UNITY_2020_1_OR_NEWER
        string jsonString = JsonUtility.ToJson(packageManifestData, true);
#endif

#if UNITY_2020_1_OR_NEWER
        string jsonString = JsonConvert.SerializeObject(packageManifestData, Formatting.Indented);
#endif
        string filePath = EditorUtility.SaveFilePanel("Save package.json", "Assets", "package.json", "json");
        File.WriteAllText(filePath, jsonString);
        AssetDatabase.Refresh();
        PackageManifestEditorUtility window = (PackageManifestEditorUtility)EditorWindow.GetWindow(typeof(PackageManifestEditorUtility));
        window.jsonFilePath = filePath;
        window.LoadJson();
        //        window.packageManifestData = JsonUtility.FromJson<PackageManifestData>(File.ReadAllText(filePath));
    }
}

public class PackageManifestEditorUtility : EditorWindow
{
    public string jsonFilePath;
    public PackageManifestData packageManifestData;

    [MenuItem("Tools/Package Manifest Tool")]
    public static void ShowWindow()
    {
        var window = GetWindow<PackageManifestEditorUtility>("Package Manifest Tool");
        window.Show();
    }

    // Lists to store the key-value pairs for the dependencies
    public List<string> dependenciesKey = new List<string>();
    public List<string> dependenciesValue = new List<string>();

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        jsonFilePath = EditorGUILayout.TextField("Package Manifest Path", jsonFilePath);
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("...", GUILayout.MaxWidth(24)))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select Package Manifest", "", "json");

#if !UNITY_2020_1_OR_NEWER
            packageManifestData = JsonUtility.FromJson<PackageManifestData>(File.ReadAllText(jsonFilePath));
#endif

#if UNITY_2020_1_OR_NEWER
            packageManifestData = JsonConvert.DeserializeObject<PackageManifestData>(File.ReadAllText(jsonFilePath));
#endif
        }
        EditorGUILayout.EndHorizontal();

        if (packageManifestData != null)
        {
            packageManifestData.name = EditorGUILayout.TextField("Name", packageManifestData.name);
            packageManifestData.version = EditorGUILayout.TextField("Version", packageManifestData.version);
            packageManifestData.displayName = EditorGUILayout.TextField("Display Name", packageManifestData.displayName);
            packageManifestData.description = EditorGUILayout.TextField("Description", packageManifestData.description);
            packageManifestData.unity = EditorGUILayout.TextField("Unity", packageManifestData.unity);
            packageManifestData.unityRelease = EditorGUILayout.TextField("Unity Release", packageManifestData.unityRelease);
            packageManifestData.documentationUrl = EditorGUILayout.TextField("Documentation URL", packageManifestData.documentationUrl);
            packageManifestData.changelogUrl = EditorGUILayout.TextField("Changelog URL", packageManifestData.changelogUrl);
            packageManifestData.licensesUrl = EditorGUILayout.TextField("Licenses URL", packageManifestData.licensesUrl);

#if UNITY_2020_1_OR_NEWER
            EditorGUILayout.LabelField("Dependencies");
            // Show the current dependencies
            for (int i = 0; i < dependenciesKey.Count; i++)
            {
                GUILayout.BeginHorizontal();
                dependenciesKey[i] = EditorGUILayout.TextField("Dependency Name & Version", dependenciesKey[i]);
                dependenciesValue[i] = EditorGUILayout.TextField(dependenciesValue[i], GUILayout.MaxWidth(40));
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    dependenciesKey.RemoveAt(i);
                    dependenciesValue.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            // Add Dependency Button
            if (GUILayout.Button("Add Dependency"))
            {
                dependenciesKey.Add("com.[company-name].dependency");
                dependenciesValue.Add("1.0.0");
            }

#endif
            EditorGUILayout.LabelField("Keywords");
            for (int i = 0; i < packageManifestData.keywords.Length; i++)
            {
                packageManifestData.keywords[i] = EditorGUILayout.TextField("Keyword " + (i + 1), packageManifestData.keywords[i]);
            }
#if UNITY_2020_1_OR_NEWER
            EditorGUILayout.LabelField("Author");
            packageManifestData.author.name = EditorGUILayout.TextField("Name", packageManifestData.author.name);
            packageManifestData.author.email = EditorGUILayout.TextField("Email", packageManifestData.author.email);
            packageManifestData.author.url = EditorGUILayout.TextField("URL", packageManifestData.author.url);
#endif
            if (GUILayout.Button("Save JSON"))
            {
                SaveJson();
            }
        }
    }

    public void LoadJson()
    {
#if !UNITY_2020_1_OR_NEWER
        packageManifestData = JsonUtility.FromJson<PackageManifestData>(File.ReadAllText(jsonFilePath));
#endif

#if UNITY_2020_1_OR_NEWER
            packageManifestData = JsonConvert.DeserializeObject<PackageManifestData>(File.ReadAllText(jsonFilePath));
#endif
        if (!IsJsonValid())
        {
            packageManifestData = null;
            EditorUtility.DisplayDialog("Error", "Invalid Package Manifest format.", "OK");
        }
    }

    private void SaveJson()
    {
#if UNITY_2020_1_OR_NEWER

        // Clear existing dependencies
        packageManifestData.dependencies.Clear();

        // Add new dependencies to packageManifestData object
        for (int i = 0; i < dependenciesKey.Count; i++)
        {
            packageManifestData.dependencies.Add(dependenciesKey[i], dependenciesValue[i]);
        }
#endif


        // Save json string to file
#if !UNITY_2020_1_OR_NEWER
        string jsonString = JsonUtility.ToJson(packageManifestData, true);
#endif

#if UNITY_2020_1_OR_NEWER
        string jsonString = JsonConvert.SerializeObject(packageManifestData, Formatting.Indented);
#endif
        File.WriteAllText(jsonFilePath, jsonString);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "Package Manifest saved successfully.", "OK");
    }

    private bool IsJsonValid()
    {
        // Check if packageManifestData contains all the required properties
        if (packageManifestData.name == null || packageManifestData.version == null || packageManifestData.displayName == null
            || packageManifestData.description == null || packageManifestData.unity == null || packageManifestData.unityRelease == null
            || packageManifestData.documentationUrl == null || packageManifestData.changelogUrl == null || packageManifestData.licensesUrl == null
#if UNITY_2020_1_OR_NEWER
            || packageManifestData.keywords == null || packageManifestData.author == null
#endif
            )
        {
            return false;
        }

        return true;
    }
}
