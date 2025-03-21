using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    private GameObject turret;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    void OnMouseDown()
    {
        // Checks if mouse is over UI element to prevent interacting with game elements behind the UI 
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (buildManager.GetTurretToBuild() == null)
            return;
        
        if (turret != null)
        {
            Debug.Log("Turret Already Built Silly");
            return;
        }

        GameObject turretToBuild = buildManager.GetTurretToBuild();
        turret = Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);

    }

    void OnMouseEnter()
    {
        // Checks if mouse is over UI element to prevent interacting with game elements behind the UI 
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (buildManager.GetTurretToBuild() == null)
            return;
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
