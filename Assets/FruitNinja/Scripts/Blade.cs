using UnityEngine;


public class Blade : MonoBehaviour
{
   public void Update()
   {
      UpdatePosition();
   }

   public void UpdatePosition()
   {
      Vector3 pos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetTouchPosition());
      pos.z = -5.0f;

      transform.position = pos;

   }
    
    
  
}
