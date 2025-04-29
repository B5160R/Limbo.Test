using Newtonsoft.Json.Linq;

public class JsonHandler {
    private readonly HashSet<string> _parsedStrings = new();

    public List<JToken> InspectForUdi(JToken token) {
        var result = new List<JToken>();

        // Check if the current token is an object and contains a "udi" property
        if (token.Type == JTokenType.Object) {
            var obj = (JObject) token;
            if (obj.TryGetValue("udi", out _)) {
                result.Add(obj); // Add the entire object containing the "udi" property
            }

            // Recursively inspect all properties of the object
            foreach (var prop in obj.Properties()) {
                result.AddRange(InspectForUdi(prop.Value));
            }
        }
        // Check if the current token is an array and inspect each element
        else if (token.Type == JTokenType.Array) {
            foreach (var item in (JArray) token) {
                result.AddRange(InspectForUdi(item));
            }
        }

        return result;
    }

    public List<JToken> InspectForKey(JToken? token) {
        var result = new List<JToken>();
        if (token == null) return result; // Return empty list if token is null

        // Check if the current token is an object and contains a "key" property
        if (token.Type == JTokenType.Object) {
            var obj = (JObject) token;
            if (obj.TryGetValue("key", out _)) {
                result.Add(obj); // Add the entire object containing the "key" property
            }

            // Recursively inspect all properties of the object
            foreach (var prop in obj.Properties()) {
                result.AddRange(InspectForKey(prop.Value));
            }
        }
        // Check if the current token is an array and inspect each element
        else if (token.Type == JTokenType.Array) {
            foreach (var item in (JArray) token) {
                result.AddRange(InspectForKey(item));
            }
        }

        return result;
    }

    public JObject ConvertJsonToJObject(string json) {
        JObject root = JObject.Parse(json);
        RecursivelyParseStrings(root);
        return root;
    }

    private JToken RecursivelyParseStrings(JToken token) {
        if (token.Type == JTokenType.Object) {
            // Traverse all properties in the object
            foreach (var prop in ((JObject) token).Properties()) {
                prop.Value = RecursivelyParseStrings(prop.Value);
            }
        } else if (token.Type == JTokenType.Array) {
            // Traverse all elements in the array
            var arr = (JArray) token;
            for (int i = 0; i < arr.Count; i++) {
                arr[i] = RecursivelyParseStrings(arr[i]);
            }
        } else if (token.Type == JTokenType.String) {
            // Parse strings that might contain JSON
            return ParseIfJsonString(token);
        }

        return token;
    }

    private JToken ParseIfJsonString(JToken token) {
        string str = token.ToString();

        // Skip parsing if the string has already been parsed
        if (_parsedStrings.Contains(str)) return token;

        // Check if the string contains JSON-like characters
        if (str.Contains("{") || str.Contains("[") || str.Contains("]") || str.Contains("}")) {
            try {
                // Attempt to parse the string as JSON
                var parsed = JToken.Parse(str);
                _parsedStrings.Add(str); // Cache the parsed string
                RecursivelyParseStrings(parsed); // Recursively parse the new JSON structure
                return parsed;
            } catch {
                // Not valid JSON, return the original string
            }
        }

        return token;
    }
}