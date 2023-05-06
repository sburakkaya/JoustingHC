using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player _player;
    private Enemy _enemy;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private TextMeshProUGUI _criticXUITMP;
    [SerializeField] private Animator _cameraAnimator;
    [SerializeField] private GameObject _horseStartButton;
    [SerializeField] private GameObject _criticHitUI;
    [SerializeField] private Image _criticHitIcon;
    private float criticHit;
    private float criticHitX;
    public float maxTime;
    int counter;
    public bool hitEnd;
    public bool isHorsesCollided;
    public bool criticUIOnOff;
    public bool isHorsesAttacked;
    public bool isPlaying;
    private LevelManager _levelManager;
    void Start()
    {
        Application.targetFrameRate = 120;
        _player = FindObjectOfType<Player>();
        _enemy = FindObjectOfType<Enemy>();
        _levelManager = FindObjectOfType<LevelManager>();
        isPlaying = true;
        criticUIOnOff = false;
        isHorsesCollided = false;
        isHorsesAttacked = false;
    }
    
    public void HorsesStart()
    {
        _player.RunPlayer();
        _enemy.RunPlayer();
        _horseStartButton.SetActive(false);
        _gamePanel.SetActive(true);
    }

    public void HorsesCollision() { StartCoroutine(HorsesCollisionNumerator());}
    
    public IEnumerator HorsesCollisionNumerator()
    {
        if (isHorsesCollided == false)
        {
            isHorsesCollided = true;
            criticUIOnOff = true;
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            _criticHitUI.SetActive(true);
            _player.StopPlayer();
            _enemy.StopPlayer();
            _cameraAnimator.Play("second");
            while (counter < maxTime)
            {
                counter++;
                if (hitEnd == false)
                {
                    criticHit += 10000 * Time.deltaTime;
                    if (criticHit > 1351)
                    {
                        hitEnd = true;
                    }
                }
                if(hitEnd == true)
                {
                    criticHit -= 10000 * Time.deltaTime;
                    if (criticHit < -1351)
                    {
                        hitEnd = false;
                    }
                }
            
                _criticHitIcon.rectTransform.anchoredPosition = new Vector3(criticHit,0,0);
                yield return new WaitForSeconds(0.00001f);
            }
            CriticUICloser();
        }
    }
    
    public void HorseAttack()
    {
        Debug.Log("attacked");
        isHorsesAttacked = true;
        CriticUICloser();
        _player.LastStop();
        _enemy.LastStop();
        if (_player.Get_PlayerHealth() > _enemy.Get_EnemyHealth())
        {
            _enemy.RagdollModeOn();
            _winPanel.SetActive(true);
            Vibration.Vibrate(100);
            PlayerPrefs.SetInt("money",PlayerPrefs.GetInt("money")+1000);
            _levelManager.WriteMoney();
        }
        else
        {
            _player.RagdollModeOn();
            _losePanel.SetActive(true);
            Vibration.Vibrate(500);
        }
    }

    void CriticUICloser()
    {
        _criticHitUI.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && criticUIOnOff == true && isHorsesAttacked == false)
        {
            if (-1351 < criticHit && criticHit < -550)
            {
                criticHitX = 1.25f;
                _player.Set_PlayerHealth(_player.Get_PlayerHealth() * criticHitX);
                _criticXUITMP.text = "1.25X";
            }
            if (-550 < criticHit && criticHit < 550)
            {
                criticHitX = 1.5f;
                _player.Set_PlayerHealth(_player.Get_PlayerHealth() * criticHitX);
                _criticXUITMP.text = "1.5X";
            }

            if (550 < criticHit && criticHit <1351)
            {
                criticHitX = 2f;
                _player.Set_PlayerHealth(_player.Get_PlayerHealth() * criticHitX);
                _criticXUITMP.text = "2X";
            }
            criticUIOnOff = false;
            CriticUICloser();
            _player.SpeedUpPlayer();
            _enemy.SpeedUpPlayer();
        }
        
        if (criticUIOnOff == true && isHorsesAttacked == false)
        {
            if (-1351 < criticHit && criticHit < -550)
            {
                _criticXUITMP.text = "1.25X";
            }
            if (-550 < criticHit && criticHit < 550)
            {
                _criticXUITMP.text = "1.5X";
            }

            if (550 < criticHit && criticHit <1351)
            {
                _criticXUITMP.text = "2X";
            }
        }
    }
}
