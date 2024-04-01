using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using UnityEngine.Events;


//[System.Serializable]
//public class EventVector3: UnityEvent<Vector3>{}
public class MouseManager : Singleton<MouseManager>
{

    public Texture2D point,doorway,attack,target,arrow;
    RaycastHit hitinfo;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        SetCursorTexture();
        MouseControl();
    }


    void SetCursorTexture()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hitinfo))
        {
            //切换鼠标贴图
            switch(hitinfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
                    break;
                 case "Enemy":
                    Cursor.SetCursor(attack,new Vector2(16,16),CursorMode.Auto);
                    break;
                
            }
        }
    }

    void MouseControl()
    {
        if(Input.GetMouseButtonDown(0) && hitinfo.collider != null)
        {
            if(hitinfo.collider.gameObject.CompareTag("Ground"))
                OnMouseClicked?.Invoke(hitinfo.point);
            if(hitinfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hitinfo.collider.gameObject);
            if(hitinfo.collider.gameObject.CompareTag("Attackable"))
                OnEnemyClicked?.Invoke(hitinfo.collider.gameObject);
        }
    }


    // Start is called before the first frame update

}
