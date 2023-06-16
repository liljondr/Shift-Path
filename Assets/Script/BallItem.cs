using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



    
    public class BallItem : MonoBehaviour, IDragHandler
    {
        [SerializeField] private MeshRenderer myRenderer;
        [SerializeField] private TextMeshPro idText;
         public event Action<int,Direction> OnIsDrag;
         public Action<int> OnRemoveFromPath;
         private ColorType colorType;
         public ColorType ColorType => colorType;
        
         //public Action<int,int> OnSetInNewPath;
        
        public int ID { get; private set; }
        public int PathID { get; private set; }
        public int PathIndex { get; private set; }
        
        private float moverTime = 0.17f;
        private bool isBlock =false; //чи є режим очікування
        public bool IsBlock => isBlock;
        public float Diametr
        {

            get
            {
                return myRenderer.bounds.size.x;
            }
        }

       


        public void OnDrag(PointerEventData eventData)
        {
           // Direction dragDirection = CalculateDirection.GetDirectionOf8(eventData.delta);
            Direction dragDirection = CalculateDirection.GetDirectionOf4(eventData.delta);
            //Debug.Log("dragDirection ="+dragDirection);
            OnIsDrag?.Invoke(ID,dragDirection);
            
        }
        public void SetPathID(int id)
        {
            PathID = id;
        }

        public void SetPathIndex(int i)
        {
            PathIndex = i;
        }

        public void SetID(int id)
        {
            ID = id;
            idText.text = id.ToString();
        }


        public void SetDataForMove(int pathIndex, PathData position, Action callBack)
        {
            transform.DOMove(position.Position, moverTime).SetEase(Ease.Linear).OnComplete(()=>
            {
                PathIndex=pathIndex;
                callBack();
            });
        }

        public void SetColor(ColorType colorType, Material materail)
        {
            myRenderer.material = materail;
            this.colorType = colorType;
        }


        public void SwitchBlock(bool b)
        {
            isBlock = b;
        }
    }


