using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualButton : MonoBehaviour, IVirtualButtonEventHandler
{

    Material mat;
    Vector3 des;
    bool hasbeenchanged = false;

    public Animator Cat;

    Quaternion q = Quaternion.identity;

    //按钮按下
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        string str = vb.VirtualButtonName;
        string num = str.Substring (14, str.Length - 14);
        int buttonNum = int.Parse(num);
    
        hasbeenchanged = false;
        Cat.SetBool("walk", true);
        des = transform.GetChild(buttonNum + 1).localPosition; //将当前vb位置设置为终点位置

        mat.color = Color.red;

        GameObject pfb = Resources.Load("Hit_01") as GameObject;
        GameObject prefabInstance = Instantiate(pfb);
        prefabInstance.transform.parent = transform.GetChild(buttonNum+1);
        prefabInstance.transform.localPosition = new Vector3(0, 0, 0);
        prefabInstance.transform.localScale = new Vector3(1, 0.001f, 1);
        Destroy(prefabInstance, 1f);
        

        if(buttonNum == 17) //eat
        {
            Cat.SetBool("walk_to_eat", true);
            Cat.SetBool("eat_to_idle", true);
            Cat.SetBool("walk_to_jump", false);
            Cat.SetBool("walk_to_sound", false);
        }
        else if( buttonNum == 18) //jump
        {
            Cat.SetBool("walk_to_jump", true);
            Cat.SetBool("jump_to_idle", true);
            Cat.SetBool("walk_to_eat", false);
            Cat.SetBool("walk_to_sound", false);
        }
        else if( buttonNum == 19) //sound
        {
            Cat.SetBool("walk_to_sound", true);
            Cat.SetBool("sound_to_idle", true);
            Cat.SetBool("walk_to_jump", false);
            Cat.SetBool("walk_to_eat", false);
        }
        else
        {
            Cat.SetBool("walk_to_eat", false);
            Cat.SetBool("walk_to_jump", false);
            Cat.SetBool("walk_to_sound", false);
        }

     
    }


    //按钮释放
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        mat.color = Color.yellow;
    }

    void Start()
    {
        
        mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;

        des = transform.GetChild(1).localPosition;
        des = new Vector3(des.x, des.y , des.z);

       //注册所有的vb事件
        VirtualButtonBehaviour[] vbBehaviours = this.GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbBehaviours.Length; i++)
        {
            vbBehaviours[i].RegisterEventHandler(this);
           
        }
    }
    void Update()
    {
        //same
        if (transform.GetChild(1).localPosition == des)
        {
            Cat.SetBool("walk", false);
            Cat.SetBool("walk_to_idle", true);
            mat.color = Color.black;
            if (hasbeenchanged == false)
            {
                transform.GetChild(1).localEulerAngles = new Vector3(0, -180, 0);
                hasbeenchanged = true;
            }
        }
    
        else
        {
            Vector3 src = transform.GetChild(1).localPosition;
            Vector3 rotateVector = des - src;
            q = Quaternion.LookRotation(rotateVector);
            transform.GetChild(1).rotation = q;
      
            transform.GetChild(1).localPosition = Vector3.MoveTowards(transform.GetChild(1).localPosition, des, 0.1f * Time.deltaTime);
        }
    }



}