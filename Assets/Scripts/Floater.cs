using System.Collections;
using UnityEngine;
using UnityStandardAssets.Water;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
    [SerializeField] private float _ejectionTime;
    [SerializeField] private float _ejectionForce;
    [SerializeField] private float floatUpSpeedLimit = 1.15f;
    [SerializeField] private float floatUpSpeed = 1f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Water water))
        {
            StartCoroutine(Ejection());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Water water))
        {
            float difference = (other.transform.position.y - transform.position.y) * floatUpSpeed;
            _rigidbody.AddForce(new Vector3(0f, Mathf.Clamp((Mathf.Abs(Physics.gravity.y) * difference), 0, Mathf.Abs(Physics.gravity.y) * floatUpSpeedLimit), 0f), ForceMode.Acceleration);
            _rigidbody.drag = 0.99f;
            _rigidbody.angularDrag = 0.8f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Water water))
        {
            _rigidbody.drag = 0f;
            _rigidbody.angularDrag = 0f;
            StopCoroutine(Ejection());
        }
    }

    private IEnumerator Ejection()
    {
        yield return new WaitForSeconds(_ejectionTime);
        _rigidbody.AddForce(Vector3.up * _ejectionForce, ForceMode.Impulse);
    }
}
