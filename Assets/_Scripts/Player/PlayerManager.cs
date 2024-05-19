using System.Collections;
using System.Collections.Generic;
using GameArea;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private bool _smoothWalk;
        [SerializeField] private float _MovingSpeed = 5;
        [SerializeField] private GameAreaManager _GameAreaManager;
        [SerializeField] private PlayerView _PlayerView;
        [SerializeField] private Camera _Camera;
        private List<Vector3> _currentPath = new List<Vector3>();
        private Coroutine _movingCoroutineHandle;
        private bool _isMoving;
        public void Move(NativeList<int2> path)
        {
            if (_movingCoroutineHandle == null)
            {
                _movingCoroutineHandle = StartCoroutine(MoveCoroutine(path));
            }
        }
        private void CancelMovement()
        {
            if (_movingCoroutineHandle != null)
            {
                StopCoroutine(_movingCoroutineHandle);
                _movingCoroutineHandle = null;
                _isMoving = false;
            }
            _PlayerView.transform.localPosition = Vector3.zero;
        }
        private void Start()
        {
            _GameAreaManager.PathFound += Move;
            _GameAreaManager.NewAreaCreated += CancelMovement;
        }

        private void OnDestroy()
        {
            _GameAreaManager.PathFound -= Move;
            _GameAreaManager.NewAreaCreated -= CancelMovement;
        }
        private void Update()
        {
            if (!_isMoving && Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = _Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var hitPos = hit.point;
                    hitPos = new Vector3(hitPos.x + 0.5f, 0, hitPos.z + 0.5f); // adjust to better UX
                    _GameAreaManager.FindPath(_PlayerView.transform.position, hitPos);
                }
            }
        }
        private IEnumerator MoveCoroutine(NativeList<int2> path)
        {
            _isMoving = true;
            _currentPath.Clear();
            for (int i = path.Length-1; i >=0; i--)
            {
                _currentPath.Add(new Vector3(path[i].x, 0, path[i].y));
            }

            for (int i = 0; i < _currentPath.Count;)
            {
                var timer = 0f;
                Vector3 prevPos = _PlayerView.transform.position;
                while (timer < 1f)
                {
                    _PlayerView.transform.position = Vector3.Lerp(prevPos, _currentPath[i], timer);
                    timer += Time.deltaTime * _MovingSpeed * (_smoothWalk ? 0.5f : 1f);
                    yield return null;
                }
                if (_smoothWalk)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }
            }
            if (_smoothWalk && _currentPath.Count % 2 > 0)
            {
                var timer = 0f;
                Vector3 prevPos = _PlayerView.transform.position;
                while (timer < 1f)
                {
                    _PlayerView.transform.position = Vector3.Lerp(prevPos, _currentPath[^1], timer);
                    timer += Time.deltaTime * _MovingSpeed * (_smoothWalk ? 0.5f : 1f);
                    yield return null;
                }
            }
            _movingCoroutineHandle = null;
            _isMoving = false;
        }
    }
}
