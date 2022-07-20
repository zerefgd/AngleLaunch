using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startCenterPos;

    [SerializeField]
    private float _rotateRadius, _rotateSpeed, _moveSpeed;

    private Vector3 centerPos;

    private bool canMove;
    private bool canRotate;
    private bool canShoot;

    private float rotateAngle;
    private Vector3 moveDirection;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioClip _moveClip, _pointClip, _loseClip;

    private void Awake()
    {
        canRotate = true;
        canShoot = true;
        canMove = false;
        centerPos = _startCenterPos;
        rotateAngle = 0f;
    }

    private void Update()
    {
        if(canShoot && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if(canRotate)
        {
            rotateAngle += _rotateSpeed * Time.fixedDeltaTime;
            moveDirection = new Vector3(Mathf.Cos(rotateAngle * Mathf.Deg2Rad)
                , Mathf.Sin(rotateAngle * Mathf.Deg2Rad), 0f).normalized;
            transform.position = centerPos + _rotateRadius * moveDirection;
            if (rotateAngle >= 360f) rotateAngle = 0f;
        }
        else if(canMove)
        {
            transform.position += _moveSpeed * Time.fixedDeltaTime * moveDirection;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.Tags.Obstacle))
        {
            EndGame();
        }
        if(collision.gameObject.CompareTag(Constants.Tags.SCORE))
        {

            AudioManager.Instance.PlaySound(_pointClip);
            centerPos = collision.gameObject.transform.position;
            canMove = false;
            canRotate = true;
            canShoot = true;
            float tangent = (transform.position.y - centerPos.y) / (transform.position.x - centerPos.x);
            rotateAngle = Mathf.Atan(tangent);
            int id = collision.gameObject.GetComponent<Score>().Id;
            GameManager.Instance.UpdateScore(id);
        }
    }

    private void EndGame()
    {
        AudioManager.Instance.PlaySound(_loseClip);
        Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity), 5f);
        GameManager.Instance.EndGame();
        Destroy(gameObject);
    }

    private void Shoot()
    {
        AudioManager.Instance.PlaySound(_moveClip);
        canRotate = false;
        canShoot = false;
        canMove = true;
        moveDirection = (transform.position - centerPos).normalized;
    }

}