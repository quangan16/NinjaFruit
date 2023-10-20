using System.Collections;
using UnityEngine;

public class G80_SwipeRaycast : MonoBehaviour
{
    // Store the starting and ending positions of the swipe.
    private Vector3 swipeStartPos;
    private Vector3 swipeEndPos;

  

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
              
                if (hit.collider.CompareTag("HorizontalSlice"))
                {
                  
                    if (hit.collider.gameObject.transform.GetChild(0).TryGetComponent(out G80_Fruit fruit))
                    {
                    
                        fruit.OnSliced(G80_InputManager.Instance.GetTouchDirectionNomalized(),G80_InputManager.Instance.GetTouchPosition(),6.0f);
                    }
                    
                }
            }
        }
    }

  
}