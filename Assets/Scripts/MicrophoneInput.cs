using UnityEngine;

[DefaultExecutionOrder(200)]
public class MicrophoneInput : MonoBehaviour
{
    private AudioClip micClip;
    private const int sampleSize = 256; // ������С
    private string selectedMic;
    private Player player;

    private void Awake()
    {
        
    }

    void Start()
    {
        player = PlayerManager.instance.player;
        // ��ȡ���õ���˷��豸
        string[] micDevices = Microphone.devices;


        if (micDevices.Length > 0)
        {
            foreach(string device in micDevices)
            {
                Debug.Log("���õ���˷��豸: " + device);
            }
            selectedMic = micDevices[2]; // Ĭ��ѡ���һ����˷�
            Debug.Log("ʹ�õ���˷�: " + selectedMic);

            // ������˷�
            micClip = Microphone.Start(selectedMic, true, 10, 44100);
            if (micClip == null)
            {
                Debug.LogError("�޷�������˷磡");
                return;
            }
        }
        else
        {
            Debug.LogError("û���ҵ����õ���˷��豸��");
            return;
        }
    }

    void Update()
    {
        if (micClip != null)
        {
            float volume = GetMicrophoneVolume();
            Debug.Log("��˷�����: " + volume);

            if (volume > 0.2)
            {
                player.rb.velocity = new Vector2(player.moveSpeed, player.jumpForce);
            }
        }
    }

    float GetMicrophoneVolume()
    {
        float[] samples = new float[sampleSize];
        int position = Microphone.GetPosition(selectedMic) - sampleSize;

        if (position < 0) return 0; // ��ֹ��ֵ

        // �� AudioClip �л�ȡ��Ƶ����
        micClip.GetData(samples, position);

        // ����������С
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample); // ʹ�þ���ֵ
        }
        return sum / sampleSize; // ����ƽ������
    }

    void OnDestroy()
    {
        // ֹͣ��˷�
        if (micClip != null)
        {
            Microphone.End(selectedMic);
        }
    }
}
