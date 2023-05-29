using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurvePoint : MonoBehaviour
{

 [SerializeField] private int id;
 public int Id => id;
  
   [SerializeField] private CurvePoint previewPoint;
  // [SerializeField] private CurvePointType previewType;
   [SerializeField] private CurvePoint nextPoint;
   [SerializeField] private CurvePointType nextType;

 
   public CurvePoint PreviewPoint => previewPoint;
  // public CurvePointType PreviewType => previewType;
   
   public CurvePoint NextPoint => nextPoint;
   public CurvePointType NextType => nextType;
}



public enum CurvePointType
{
   LINE,
   REFERENCE_Qu_BEZIER,
   CONTROL_Qu_BEZIER,
   
}
