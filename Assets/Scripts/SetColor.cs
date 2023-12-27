using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class SetColor : MonoBehaviour
{
    public enum ColorType
    {
        green,
        red,
        blue
    }

    public ColorType _colorType;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (_colorType == ColorType.green)
        {
            _meshRenderer.material.color = Color.green;
        }
        else if (_colorType == ColorType.blue)
        {
            _meshRenderer.material.color = Color.blue;
        }
        else
        {
            _meshRenderer.material.color = Color.red;
        }
    }
}