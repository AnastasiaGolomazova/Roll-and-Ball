using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Player _target;

    [Space(10)]
    [Header("LoseMenu")]
    [SerializeField] private GameObject _loseMenu;
    [SerializeField] private TMP_Text _enemyName;

    private NavMeshAgent _agent;
    private SetColor _color;

    private void Start()
    {
        _loseMenu.SetActive(false);

        _agent = GetComponent<NavMeshAgent>();
        _color = GetComponent<SetColor>();

        _agent.speed = _speed;

    }

    private void Update()
    {
        _agent.SetDestination(_target.transform.position);    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _loseMenu.SetActive(true);
            player.enabled = false;
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (_color._colorType == SetColor.ColorType.green)
            {
                _enemyName.color = Color.green;
                _enemyName.text = "Green Enemy catch";
            }
               
            else if (_color._colorType == SetColor.ColorType.red)
            {
                _enemyName.color = Color.red;
                _enemyName.text = "Red Enemy catch";
            }   
        }
    }
}