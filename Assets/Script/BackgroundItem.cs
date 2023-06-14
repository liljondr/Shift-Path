using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundItem : MonoBehaviour
{
   [SerializeField] private MeshRenderer myMeshRenderer;
  


   public void SetColor(Color color)
   {
      color.a = 1;
      myMeshRenderer.sharedMaterial.color = color;
     
   }
}
