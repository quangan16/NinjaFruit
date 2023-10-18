using System;
using UnityEngine;


public class Blade : MonoBehaviour
{
   [SerializeField] private GameObject trail;
   Vector2 previousPosition;
   Camera cam;
   public GameObject bladeTrailPrefab;
   public void Awake()
   {
      // trail.SetPosition(0, InputManager.Instance.GetTouchPosition());
   }
   public void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         StartCutting();
      }
      else if (Input.GetMouseButtonUp(0))
      {
         StopCutting();
      }

      if (GameManager.Instance.isCutting)
      {
         UpdatePosition();
      }
      
   }

   void Start()
   {
      cam = Camera.main;
  
   }

   public void OnMouseUp()
   {
      Debug.Log("lol");
     
   }


   public void UpdatePosition()
   {
      Vector3 pos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetTouchPosition());
      pos.z = -5.0f;

      transform.position = pos;
      previousPosition = pos;
   }

   void StartCutting()
   {
      GameManager.Instance.isCutting = true;
      trail = Instantiate(bladeTrailPrefab, transform);
      previousPosition = cam.ScreenToWorldPoint(Input.mousePosition);
     
   }

   void StopCutting()
   {
      GameManager.Instance.isCutting = false;
      trail.transform.SetParent(null);
      Destroy(trail, 2f);
    
   }
    
    
  
}
