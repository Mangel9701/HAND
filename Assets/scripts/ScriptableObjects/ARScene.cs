using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "ARScene", menuName = "AR/ARScene", order = 0), System.Serializable]
public class ARScene : ScriptableObject
{
    [SerializeField] public ARObject[] objects;

    public string ConvertARSceneToJSON()
    {
        return JsonUtility.ToJson(this, true);
    }


}

[System.Serializable]
public struct ARObject
{
    private struct CustomVector
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public CustomVector(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
            w = 0;
        }
        public CustomVector(Quaternion vector4)
        {
            x = vector4.x;
            y = vector4.y;
            z = vector4.z;
            w = vector4.w;
        }
    }
    private struct CustomColor
    {
        public float r;
        public float g;
        public float b;
        public float a;
        public CustomColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }
    }
    //object info
    [JsonIgnore]
    public Vector3 position;
    [JsonProperty("position")]
    private CustomVector _position { get { return new CustomVector(position); } }

    [JsonIgnore]
    public Vector3 scale;
    [JsonProperty("scale")]
    private CustomVector _scale { get { return new CustomVector(scale); } }

    [JsonIgnore]
    public Quaternion rotation;
    [JsonProperty("rotation")]
    private CustomVector _rotation { get { return new CustomVector(rotation); } }

    public ARObjectType type;

    //is text
    public string text;
    public string textureUrl;
    public int textureId;

    [JsonIgnore]
    public Color color;
    [JsonProperty("color")]
    private CustomColor _color { get { return new CustomColor(color); } }

    public float material;
    public float roughness;

    //is Object
    public string objectUrl;
    public int objectId;

public ARObject(ARObjectType _type, Vector3 _scale, Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        scale = _scale;
        rotation = _rotation;
        type = _type;
        text = default;
        material = default;
        roughness = default;
        textureUrl = default;
        textureId = default;
        color = default;
        objectUrl = default;
        objectId = default;
    }
    public ARObject(ARObjectType _type, Vector3 _scale, Vector3 _position, Quaternion _rotation, string _objectUrl, int _objectId)
    {
        position = _position;
        scale = _scale;
        rotation = _rotation;
        type = _type;
        text = default;
        material = default;
        roughness = default;
        textureUrl = default;
        textureId = default;
        color = default;
        objectUrl = _objectUrl;
        objectId = _objectId;
    }
    public ARObject(ARObjectType _type, Vector3 _scale, Vector3 _position, Quaternion _rotation, string _text, string _textureURL, int _textureId, float _material, float _roughness, Color _color)
    {
        position = _position;
        scale = _scale;
        rotation = _rotation;
        type = _type;
        text = _text;
        material = _material;
        roughness = _roughness;
        textureUrl = _textureURL;
        textureId = _textureId;
        color = _color;
        objectUrl = default;
        objectId = default;
    }
    public ARObject(ARObjectType _type, Vector3 _scale, Vector3 _position, Quaternion _rotation, string _text, string _textureURL, int _textureId, float _material, float _roughness, Color _color, string _objectUrl, int _objectId)
    {

        position = _position;
        scale = _scale;
        rotation = _rotation;
        type = _type;
        text = _text;
        material = _material;
        roughness = _roughness;
        textureUrl = _textureURL;
        textureId = _textureId;
        color = _color;
        objectUrl = _objectUrl;
        objectId = _objectId;
    }

}

public enum ColorType { Texture, Color }

public enum ARObjectType { Object, Text }