using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class PrefabCreator : MonoBehaviour
{

    public GameObject modelPrefab;

    [SerializeField]
    private ColourSkinChanger _colourSkinChanger;

    public HairElementChanger hairs;
    public FaceElementChanger faces;
    public AccessoriesElementChanger accessories;
    public BodyElementChanger bodies;
    public LegsElementChanger legs;


    public string standartMaterialPath = "Assets/HYPERCASUAL - Stickman Customization/MaterialsCreator/";
    private string _materialPath;

    private GameObject _objForSave = null;

    private void Start()
    {
        _materialPath = standartMaterialPath;
        //_colourSkinChanger = gameObject.GetComponent<ColourSkinChanger>();
    }

    public void CreatePrefabButtonClick()
    {
        GameObject newPrefab = (GameObject) Instantiate(modelPrefab);
   
        var number = RandomNumber();

        string prefabName = "NewCharacter_" + number;
        newPrefab.name = prefabName;
        newPrefab.transform.position = Vector3.zero;
        newPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        newPrefab.transform.localScale = Vector3.one;

        //_materialPath += prefabName + "/";

        ////if (!Directory.Exists(_materialPath + prefabName))
        //    //AssetDatabase.CreateFolder(_materialPath, prefabName);
        //AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(_materialPath, prefabName));

        var skinMaterial = _colourSkinChanger.CreateNewSkinMaterial(number.ToString());

        CreateAllSkinMaterials(newPrefab, skinMaterial);

        var Animator = newPrefab.GetComponent<Animator>();
        Animator.Play("Save");

        _objForSave = newPrefab;
        _materialPath = standartMaterialPath;
        
        Invoke("SaveAsPrefab", 0.1f);
    }

    private void SaveAsPrefab()
    {
        bool result = false;
        PrefabUtility.SaveAsPrefabAsset(_objForSave, "Assets/HYPERCASUAL - Stickman Customization/PrefabCreator/" + _objForSave.name + ".prefab", out result);
        Destroy(_objForSave);
    }

    private string RandomNumber()
    {
        string str = "";
        for(int i = 0; i < 10; i++)
        {
            str += Random.Range(0, 9).ToString();
        }
        return str;
    }

    private void CreateAllSkinMaterials(GameObject newPrefab, Material newSkin)
    {
        var allRenderers = newPrefab.GetComponentsInChildren<SkinnedMeshRenderer>();

        CreateBody(allRenderers[0], newSkin);
        CreateAccessory(allRenderers[1]);
        CreateHair(allRenderers[2]);
        CreateHead(allRenderers[3], newSkin);
        CreateLegs(allRenderers[4], newSkin);
    }

    private void CreateBody(SkinnedMeshRenderer bodyMesh, Material skinMaterial)
    {
        bool isReverse = false;
        Material bodyMaterial = bodies.GetBodyMaterial(out isReverse);

        
        var mats = bodyMesh.materials;
        if (bodyMaterial != null)
        {
            if (isReverse)
            {
                mats[1] = CreateNewMaterial("BodyUp", bodyMaterial);
                mats[0] = skinMaterial;
            }
            else
            {
                mats[0] = CreateNewMaterial("BodyUp", bodyMaterial);
                mats[1] = skinMaterial;
            }
        }
        else
        {
            mats[0] = skinMaterial;
            mats[1] = null;
        }

        bodyMesh.materials = mats;
    }

    private void CreateAccessory(SkinnedMeshRenderer accMesh)
    {
        var accessoryMat = accessories.GetAccessoryMaterial();

        if (accMesh != null && accessoryMat != null)
        {
            var mats = accMesh.materials;
            mats[0] = CreateNewMaterial("Accessory", accessoryMat);
            accMesh.materials = mats;
        }
        else
        {
            Destroy(accMesh.gameObject);
        }
    }

    private void CreateHair(SkinnedMeshRenderer hairMesh)
    {
        var hairMat = hairs.GetHairMaterial();
        if (hairMesh != null && hairMat != null)
        {
            var mats = hairMesh.materials;
            mats[0] = CreateNewMaterial("Hair", hairMat);
            hairMesh.materials = mats;
        } 
        else
        {
            Destroy(hairMesh.gameObject);
        }
    }

    private void CreateHead(SkinnedMeshRenderer headMesh, Material skinMaterial)
    {
        Material bodyMaterial = faces.GetFaceMaterial();

        var mats = headMesh.materials;

        if (bodyMaterial != null) mats[1] = CreateNewMaterial("Face", bodyMaterial);
        else mats[1] = null;
        mats[0] = skinMaterial;
         

        headMesh.materials = mats;
    }

    private void CreateLegs(SkinnedMeshRenderer legsMesh, Material skinMaterial)
    {
        bool isReverse = false;
        bool isDouble = false;
        Material bodyMaterial = legs.GetLegsMaterial(out isReverse, out isDouble);

        var mats = legsMesh.materials;
        if (bodyMaterial != null)
        {
            if (isDouble)
            {
                if (isReverse)
                {
                    mats[1] = CreateNewMaterial("Legs", bodyMaterial);
                    mats[0] = skinMaterial;
                }
                else
                {
                    mats[0] = CreateNewMaterial("Legs", bodyMaterial);
                    mats[1] = skinMaterial;
                }
            }
            else
            {
                mats[0] = CreateNewMaterial("Legs", bodyMaterial);
                mats[1] = null;
            }
        }
        else
        {
            mats[0] = skinMaterial;
            mats[1] = null;
        }

        legsMesh.materials = mats;
    }

    private Material CreateNewMaterial(string gameObjName, Material mat)
    {
        Material newMaterial = new Material(mat);

        AssetDatabase.CreateAsset(newMaterial, _materialPath + gameObjName + "_" + RandomNumber() + ".mat"); // _materialPath + gameObjName + "/mat_" + RandomNumber() + ".mat");

        return newMaterial;
    }

    public bool CheckMaterialTextures(Material mat1)
    {
        if (!mat1.mainTexture && !_colourSkinChanger.skinMaterial.mainTexture)
        {
            return true;
        }
        else return false;
    }
}
