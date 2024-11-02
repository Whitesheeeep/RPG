using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [MenuItem("Tools/����ѡ��ͼ��ͼƬ")]
    static void ExportSpriteAtlas()
    {
        //�������ѡ�ж���
        var selObjs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        if (selObjs == null || selObjs.Length == 0)
        {
            EditorUtility.DisplayDialog("����", "������ѡ��һ���кõ�ͼ��!", "��֪����");
            return;
        }

        foreach (var obj in selObjs)
        {
            //���ͼ���µ����е�Sprite
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
                EditorUtility.DisplayDialog("����", "��ѡ�еĲ���ͼ�������к�ͼ�����ٴ�ѡ�У�", "��֪����");
                return;
            }

            //���������ļ���
            string outPath = Application.dataPath + "/OutSprite/" + obj.name;
            System.IO.Directory.CreateDirectory(outPath);

            //���Ե���
            foreach (Sprite sprite in spriteList)
            {
                try
                {
                    //��������
                    Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                    texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin, (int)sprite.rect.width, (int)sprite.rect.height));
                    texture.Apply();

                    //��������
                    System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", texture.EncodeToPNG());
                }
                catch
                {
                    EditorUtility.DisplayDialog("����", "�޷�����������ɫ�飬����ͼ����Inspector����������������ݺ����ԡ�\n1.��ѡAdvenced > Read/Write��\n2.ƽ̨���� > Format�޸�ΪRGB 24 bit��", "��֪����");
                    return;
                }
            }
        }

        //ˢ����Դ��ʾ
        AssetDatabase.Refresh();
    }
}
