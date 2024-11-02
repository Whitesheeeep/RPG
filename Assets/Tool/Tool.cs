using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [MenuItem("Tools/导出选中图集图片")]
    static void ExportSpriteAtlas()
    {
        //获得所有选中对象
        var selObjs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        if (selObjs == null || selObjs.Length == 0)
        {
            EditorUtility.DisplayDialog("错误", "请至少选中一个切好的图集!", "我知道了");
            return;
        }

        foreach (var obj in selObjs)
        {
            //获得图集下的所有的Sprite
            var resPath = AssetDatabase.GetAssetPath(obj);
            var spriteObjs = AssetDatabase.LoadAllAssetsAtPath(resPath);
            var spriteList = new List<Sprite>();
            foreach (var sprite in spriteObjs)
            {
                if (sprite is Sprite)
                {
                    spriteList.Add(sprite as Sprite);
                }
            }

            if (spriteList.Count == 0)
            {
                EditorUtility.DisplayDialog("错误", "您选中的不是图集！请切好图集后再次选中！", "我知道了");
                return;
            }

            //创建导出文件夹
            string outPath = Application.dataPath + "/OutSprite/" + obj.name;
            System.IO.Directory.CreateDirectory(outPath);

            //尝试导出
            foreach (Sprite sprite in spriteList)
            {
                try
                {
                    //创建纹理
                    Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                    texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin, (int)sprite.rect.width, (int)sprite.rect.height));
                    texture.Apply();

                    //导出精灵
                    System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", texture.EncodeToPNG());
                }
                catch
                {
                    EditorUtility.DisplayDialog("错误", "无法设置像素颜色块，请在图集的Inspector面板中设置以下内容后在试。\n1.勾选Advenced > Read/Write。\n2.平台设置 > Format修改为RGB 24 bit。", "我知道了");
                    return;
                }
            }
        }

        //刷新资源显示
        AssetDatabase.Refresh();
    }
}
