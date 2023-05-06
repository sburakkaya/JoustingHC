using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomizationShop : MonoBehaviour
{
    private LevelManager _levelManager;
    
    [SerializeField] private GameObject[] _itemsBuy;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private GameObject[] _nameOfArmor;

    #region MeshChange

    [SerializeField] private GameObject[] _playerMeshes;
    [SerializeField] private GameObject[] _fireArmorMeshes;
    [SerializeField] private GameObject[] _lightningArmorMeshes;
    [SerializeField] private GameObject[] _poisonArmorMeshes;
    [SerializeField] private GameObject[] _madnessArmorMeshes;
    [SerializeField] private GameObject[] _curseArmorMeshes;
    [SerializeField] private GameObject[] _iceArmorMeshes;

    #endregion
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
        ActiveCheck();

        /*for (int i = 0; i < _playerMeshes.Length; i++)
        {
            if (_fireArmorMeshes[i] != null)
            {
                _playerMeshes[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = _fireArmorMeshes[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                _playerMeshes[i].transform.localScale = Vector3.one;
                _playerMeshes[i].transform.localPosition = Vector3.zero;
                _playerMeshes[i].GetComponent<Renderer>().material = _fireArmorMeshes[i].GetComponent<Renderer>().material;
            }
            else
            {
                _playerMeshes[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
        }*/
    }

    public void ActiveCheck()
    {
        for (int i = 0; i < _itemsBuy.Length; i++)
        {
            string x = "Item"+i.ToString();
            if (PlayerPrefs.GetInt(x) == 1)
            {
                _itemsBuy[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void EquipCheck()
    {
        for (int i = 0; i < _playerMeshes.Length; i++)
        {
            if (_nameOfArmor[i] != null)
            {
                _playerMeshes[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = _nameOfArmor[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                _playerMeshes[i].transform.localScale = Vector3.one;
                _playerMeshes[i].transform.localPosition = Vector3.zero;
                _playerMeshes[i].GetComponent<Renderer>().material = _nameOfArmor[i].GetComponent<Renderer>().material;
            }
            else
            {
                _playerMeshes[i].GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
            }
        }
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            
            m_Raycaster.Raycast(m_PointerEventData, results);
            
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Items") && result.gameObject.transform.GetChild(1).gameObject.activeSelf == true)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    Debug.Log(PlayerPrefs.GetInt("money"));
                    if (PlayerPrefs.GetInt("money") >= 1000)
                    {
                        PlayerPrefs.SetInt("money",PlayerPrefs.GetInt("money")-1000);
                        PlayerPrefs.SetInt(result.gameObject.name,1);
                        _levelManager.WriteMoney();
                        ActiveCheck();
                    }
                }
                if (result.gameObject.CompareTag("Items") && result.gameObject.transform.GetChild(1).gameObject.activeSelf == false)
                {
                    if (result.gameObject.name == "Item0")
                    {
                        _nameOfArmor = _fireArmorMeshes;
                    }
                    if (result.gameObject.name == "Item1")
                    {
                        _nameOfArmor = _lightningArmorMeshes;
                    }
                    if (result.gameObject.name == "Item2")
                    {
                        _nameOfArmor = _poisonArmorMeshes;
                    }
                    if (result.gameObject.name == "Item3")
                    {
                        _nameOfArmor = _madnessArmorMeshes;
                    }
                    if (result.gameObject.name == "Item4")
                    {
                        _nameOfArmor = _curseArmorMeshes;
                    }
                    if (result.gameObject.name == "Item5")
                    {
                        _nameOfArmor = _iceArmorMeshes;
                    }
                    EquipCheck();
                }
            }
        }
    }
    
    
}
