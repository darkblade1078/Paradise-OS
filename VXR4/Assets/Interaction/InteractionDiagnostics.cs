using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;

// Simple editor diagnostics to help find null references and instances
// of Oculus.Interaction.InteractorActiveState in the active scene.
// Usage: Tools -> Diagnostics -> Find Null References in Scene
public static class InteractionDiagnostics
{
    [MenuItem("Tools/Diagnostics/Find Null References in Scene")]
    public static void FindNullReferencesInScene()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            Debug.LogWarning("No active scene loaded to scan.");
            return;
        }

        int nullFieldCount = 0;
        int missingScriptCount = 0;
        int interactorActiveInstances = 0;

        var roots = scene.GetRootGameObjects();
        foreach (var root in roots)
        {
            Traverse(root, ref nullFieldCount, ref missingScriptCount, ref interactorActiveInstances);
        }

        Debug.Log($"Diagnostic complete. Null fields: {nullFieldCount}, Missing scripts: {missingScriptCount}, InteractorActiveState instances: {interactorActiveInstances}");
    }

    private static void Traverse(GameObject go, ref int nullFieldCount, ref int missingScriptCount, ref int interactorActiveInstances)
    {
        var comps = go.GetComponents<Component>();
        foreach (var c in comps)
        {
            if (c == null)
            {
                Debug.LogWarning($"Missing script on GameObject: {GetFullPath(go)}");
                missingScriptCount++;
                continue;
            }

            var type = c.GetType();

            // Check for InteractorActiveState by full name (avoid hard dependency)
            if (type.FullName == "Oculus.Interaction.InteractorActiveState" || (type.FullName != null && type.FullName.EndsWith("InteractorActiveState")))
            {
                Debug.LogError($"Found InteractorActiveState on {GetFullPath(go)} (component type: {type.FullName}). Inspect in Inspector.");
                interactorActiveInstances++;
                // Inspect fields on this component as a priority
                InspectFields(c, go, ref nullFieldCount);
                continue;
            }

            // Inspect serialized fields for null references
            InspectFields(c, go, ref nullFieldCount);
        }

        foreach (Transform child in go.transform)
        {
            Traverse(child.gameObject, ref nullFieldCount, ref missingScriptCount, ref interactorActiveInstances);
        }
    }

    private static void InspectFields(Component c, GameObject go, ref int nullFieldCount)
    {
        var type = c.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var fields = type.GetFields(flags);
        foreach (var f in fields)
        {
            bool isPublic = f.IsPublic;
            bool hasSerializeField = f.GetCustomAttribute<System.SerializableAttribute>() != null || f.GetCustomAttribute<SerializeField>() != null;
            // Only inspect fields that Unity would serialize (public or [SerializeField])
            if (!isPublic && !hasSerializeField)
                continue;

            object value = null;
            try
            {
                value = f.GetValue(c);
            }
            catch
            {
                // Some fields may throw on GetValue (native pointers etc.) - ignore
                continue;
            }

            if (value == null)
            {
                Debug.LogWarning($"Null serialized field on {GetFullPath(go)}: Component={type.FullName}, Field={f.Name}");
                nullFieldCount++;
            }
        }
    }

    private static string GetFullPath(GameObject go)
    {
        string path = go.name;
        var t = go.transform;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
