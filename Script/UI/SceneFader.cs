using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public float fadeInDuration;
    public float fadeOutDuration;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();//获取CanvasGroup组件

        DontDestroyOnLoad(gameObject);
    }//保证该脚本实例在所有场景中都存在

    public IEnumerator FadeOutIn()//淡出淡入效果
    {
        yield return FadeOut(fadeOutDuration);//先淡出
        yield return FadeIn(fadeInDuration);//再淡入
    }
    public IEnumerator FadeOut(float time)//淡出效果
    {
        while (canvasGroup.alpha < 1)//当画布组件的透明度小于1时，执行淡出效果
        {
            canvasGroup.alpha += Time.deltaTime / time;//每帧淡出
            yield return null;
        }
    
    }

    public IEnumerator FadeIn(float time)//IEnumerator类型的FadeIn方法，用于实现淡入效果
    {
        while (canvasGroup.alpha != 0)// 当画布组件的透明度不等于0时执行循环
        {
            canvasGroup.alpha -= Time.deltaTime / time;//逐渐减少画布组件的透明度直至达到目标透明度
            yield return null;
        }
        Destroy(gameObject);//淡入结束后销毁该脚本实例
    }
}
