using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _pleyerHealth;
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    private Rigidbody rb;
    public Animator _horseAnim;
    public Animator _playerAnim;
    private GameManager _gameManager;
    public TextMeshProUGUI _healthTmp;

    public Collider mainCollider;
    public GameObject thisGuyRig;
    public Animator thisGuyAnimator;
    public Rigidbody shieldRb;
    public Rigidbody spearRb;
    public Collider shieldCol;
    public Collider spearCol;

    public float Get_PlayerHealth()
    {
        return _pleyerHealth;
    }

    public void Set_PlayerHealth(float _playerHealth)
    {
        this._pleyerHealth = _playerHealth;
        WriteHealth();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
        GetRagDollBits();
        RagdollModeOff();
        WriteHealth();
        Upgrades();
    }

    void Upgrades()
    {
        _pleyerHealth += PlayerPrefs.GetInt("horsearmorlevl") * 100;
        _pleyerHealth += PlayerPrefs.GetInt("heroarmorlevel") * 100;
        _pleyerHealth += PlayerPrefs.GetInt("spearlevel") * 100;
        _pleyerHealth += PlayerPrefs.GetInt("shieldlevel") * 100;
    }

    void WriteHealth()
    {
        _healthTmp.text = _pleyerHealth.ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        if (_forwardSpeed < _maxSpeed)
        {
            _forwardSpeed += _acceleration * Time.deltaTime;
        }
        rb.velocity = new Vector3(0f, 0f, _forwardSpeed);
    }

    public void RunPlayer()
    {
        _maxSpeed = 50;
        _playerAnim.SetBool("run",true);
        _horseAnim.SetBool("run",true);
    }

    public void SpeedUpPlayer()
    {
        _maxSpeed = 50;
        _forwardSpeed = 50;
        _playerAnim.speed = 1f;
        _horseAnim.speed = 1f;
    }
    public void StopPlayer()
    {
        _maxSpeed = 4;
        _forwardSpeed = 0;
        _playerAnim.speed = 0.07f;
        _horseAnim.speed = 0.07f;
    }

    public void LastStop()
    {
        _playerAnim.SetBool("attack",true);
        _maxSpeed = 0;
        _forwardSpeed = 0;
        _playerAnim.speed = 1f;
        _horseAnim.speed = 1f;
        _horseAnim.SetBool("run",false);
        StartCoroutine(EndAttack());
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.3f);
        _playerAnim.SetBool("run",false);
        _playerAnim.SetBool("attack",false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _gameManager.HorsesCollision();
        }
    }
    
    #region Ragdoll

    private Collider[] ragDollColliders;
    private Rigidbody[] limbsRigidbodies;

    void GetRagDollBits()
    {
        ragDollColliders = thisGuyRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = thisGuyRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollModeOn()
    {
        thisGuyAnimator.enabled = false;
        _horseAnim.SetBool("lose",true);
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
            rigid.AddForce(Vector3.forward * 50);
        }
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Physics.gravity = new Vector3(0, -50.0F, -20);
    }
    
    void RagdollModeOff()
    {
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }

        shieldCol.enabled = true;
        spearCol.enabled = true;
        spearRb.isKinematic = true;
        shieldRb.isKinematic = true;
        thisGuyAnimator.enabled = true;
        mainCollider.enabled = true;
        thisGuyRig.GetComponent<Rigidbody>().isKinematic = false;
    }
    
    #endregion
}
