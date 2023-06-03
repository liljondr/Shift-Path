using System;
using System.Collections;
using System.Collections.Generic;
using Script.Mover;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
  
    private List<BallItem> ballsList = new List<BallItem>();
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            BallItem ballItem = collider.GetComponent<BallItem>();
            if (ballItem != null)
            {
                ballsList.Add(ballItem);
            }
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            BallItem ballItem = collider.GetComponent<BallItem>();
            if (ballItem != null)
            {
                ballsList.Remove(ballItem);
            }
        }
    }
}
