using UnityEngine;

//������ʱҲ����Ч��
[ExecuteInEditMode]
//��Ļ������Чһ�㶼��Ҫ�����������
[RequireComponent(typeof(Camera))]
//�ṩһ������Ļ��࣬��Ҫ��������ֱ��ͨ��Inspector�������shader������shader��Ӧ�Ĳ���
public class ScreenPostEffectBase : MonoBehaviour
{

    //Inspector�����ֱ������
    public Shader shader = null;
    private Material _material = null;
    public Material _Material
    {
        get
        {
            if (_material == null)
                _material = GenerateMaterial(shader);
            return _material;
        }
    }

    //����shader����������Ļ��Ч�Ĳ���
    protected Material GenerateMaterial(Shader shader)
    {
        if (shader == null)
            return null;
        //��Ҫ�ж�shader�Ƿ�֧��
        if (shader.isSupported == false)
            return null;
        Material material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if (material)
            return material;
        return null;
    }

}
