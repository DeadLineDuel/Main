using System;
using System.Collections;
using System.Collections.Generic;
using KTA.Test;
using Unity.Netcode;
using UnityEngine;

public class TESTPlayerController : NetworkBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Move = Animator.StringToHash("CanMove");
    [SerializeField] private TESTPlayerCore PlayerCore;
    
    [SerializeField] private QuadViewCinemachine _cameraController;
    [SerializeField] private Camera _playerCamera;
    
    [SerializeField] private GameObject _moveIndicatorPrefab;
    private GameObject _moveIndicator;
    
    [SerializeField] private LayerMask _groundLayer;
    
    private bool CanMove = true;
    private bool CanSkill = false;

    [SerializeField] private TESTPlayerSkill NormalAttack;
    [SerializeField] private TESTPlayerSkill QAttack;

    private TESTPlayerSkill currentSkill;

    [SerializeField] private List<TransparencyController> _transparencyControllers = new List<TransparencyController>();
    
    private void Awake()
    {
        _moveIndicator = Instantiate(_moveIndicatorPrefab);
        _moveIndicator.SetActive(false);
        //SetupCamera();        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


        PlayerCore.NavMeshAgent.speed = 50f;
        PlayerCore.NavMeshAgent.acceleration = 500f;
        PlayerCore.NavMeshAgent.angularSpeed = 500f;
        
        if (IsOwner)
        {
            SetupCamera();
        }
        else
        {
            // 다른 플레이어의 카메라는 꺼준다
            if (_cameraController != null)
                _cameraController.gameObject.SetActive(false);
            if (_playerCamera != null)
                _playerCamera.gameObject.SetActive(false);
        }

        SetTransparency();

    }

    private void SetTransparency()
    {
        if (IsOwner)
        {
            foreach (var transparencyController in _transparencyControllers)
            {
                transparencyController.SetToOpaque();
            }
        }
        else
        {
            foreach (var transparencyController in _transparencyControllers)
            {
                transparencyController.SetToTransparent();
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        float speedPercent = PlayerCore.NavMeshAgent.velocity.magnitude / PlayerCore.NavMeshAgent.speed;
        PlayerCore.NetworkAnimator.Animator.SetFloat(Speed, speedPercent, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanSkill) CanSkill = false;
            else CanSkill = true;
        }
        
        if (CanMove) HandleMovementInput();
        if (CanSkill) HandleSkillInput();
    }


    private void SetupCamera()
    {
        if (!IsOwner) return;

        _cameraController = GetComponentInChildren<QuadViewCinemachine>(true); // 비활성화 포함해서 찾기

        if (_cameraController != null)
        {
            _cameraController.SetTarget(transform);
            _playerCamera = _cameraController.GetCamera();

            if (_playerCamera != null)
            {
                _playerCamera.gameObject.SetActive(true);
                Debug.Log($"카메라 설정 성공: {_playerCamera.name}");
            }
        }
        else
        {
            Debug.LogError("카메라 컨트롤러를 찾을 수 없습니다.");
        }
    }

    private void HandleMovementInput()
    {
        // 마우스 우클릭 감지
        if (Input.GetMouseButtonDown(1))
        {
            if (_playerCamera == null)
            {
                Debug.LogError("카메라를 찾을 수 없습니다. 이동 불가.");
                return;
            }

            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, _groundLayer))
            {
                // 이동 표시자 표시
                if (_moveIndicator != null)
                {
                    _moveIndicator.transform.position = hit.point + Vector3.up * 0.5f;
                    _moveIndicator.SetActive(true);
                    StartCoroutine(HideMoveIndicator(0.5f));
                }
                PlayerCore.NavMeshAgent.SetDestination(hit.point);
            }
        }
    }

    private IEnumerator HideMoveIndicator(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_moveIndicator != null)
        {
            _moveIndicator.SetActive(false);
        }
    }
    
    private void HandleSkillInput()
    {
        if (!CanSkill) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            CanMove = false;
            CanSkill = false;
            currentSkill = NormalAttack;
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, _groundLayer))
            {
                transform.LookAt(hit.point);
            }
            currentSkill.ActivateSkill();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CanMove = false;
            CanSkill = false;
            currentSkill = QAttack;
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, _groundLayer))
            {
                transform.LookAt(hit.point);
            }
            currentSkill.ActivateSkill();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
    }

    private void OnSkillEffect()
    {
        if (currentSkill == null) return;
        
        currentSkill.PlayEffect();
        currentSkill.SkillHitServerRpc();
        CheatManager.Instance.MyBossMinusHealth();
    }
    
    private void OnAnimationEnd()
    {
        CanMove = true;
        CanSkill = true;
        PlayerCore.NetworkAnimator.Animator.SetBool(Move, true);
        currentSkill = null;
    }
    
    
}
