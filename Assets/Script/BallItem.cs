using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Script.Mover
{
    public class BallItem : MonoBehaviour, IDragHandler
    {
        [SerializeField] private MeshRenderer myRenderer;
        [SerializeField] private List<MaterailCatalog> materialCatalog ;
         public event Action<int,Direction> OnIsDrag;
        
        public int ID { get; private set; }
        public int PathID { get; private set; }
        public int PathIndex { get; private set; }
        
        private float moverTime = 0.17f;

        public float Diametr
        {

            get
            {
                return myRenderer.bounds.size.x;
            }
        }

       

        public void OnDrag(PointerEventData eventData)
        {
            Direction dragDirection = CalculateDirection.GetDirection(eventData.delta);
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
        }


        public void SetDataForMove(int pathIndex, PathData position, Action callBack)
        {
            transform.DOMove(position.Position, moverTime).SetEase(Ease.Linear).OnComplete(()=>
            {
                PathIndex=pathIndex;
                callBack();
            });
        }

        public void SetColor(ColorType colorType)
        {
            MaterailCatalog materailCatalog = materialCatalog.Find(c => c.ColorType == colorType);
            if (materailCatalog == null)
            {
                Debug.LogError("Didn`t find color "+colorType + " in sphera prefab");
            }
            myRenderer.material = materailCatalog.Material;
        }

       
    }
}

[Serializable]
public class MaterailCatalog
{
    [SerializeField]private ColorType colorType;
    public ColorType ColorType => colorType;
    [SerializeField]private Material material;
    public Material Material => material;
}