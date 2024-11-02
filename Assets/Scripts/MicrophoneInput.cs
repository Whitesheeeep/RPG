using UnityEngine;

[DefaultExecutionOrder(200)]
public class MicrophoneInput : MonoBehaviour
{
    private AudioClip micClip;
    private const int sampleSize = 256; // 采样大小
    private string selectedMic;
    private Player player;

    private void Awake()
    {
        
    }

    void Start()
    {
        player = PlayerManager.instance.player;
        // 获取可用的麦克风设备
        string[] micDevices = Microphone.devices;


        if (micDevices.Length > 0)
        {
            foreach(string device in micDevices)
            {
                Debug.Log("可用的麦克风设备: " + device);
            }
            selectedMic = micDevices[2]; // 默认选择第一个麦克风
            Debug.Log("使用的麦克风: " + selectedMic);

            // 启动麦克风
            micClip = Microphone.Start(selectedMic, true, 10, 44100);
            if (micClip == null)
            {
                Debug.LogError("无法启动麦克风！");
                return;
            }
        }
        else
        {
            Debug.LogError("没有找到可用的麦克风设备！");
            return;
        }
    }

    void Update()
    {
        if (micClip != null)
        {
            float volume = GetMicrophoneVolume();
            Debug.Log("麦克风音量: " + volume);

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

        if (position < 0) return 0; // 防止负值

        // 从 AudioClip 中获取音频样本
        micClip.GetData(samples, position);

        // 计算音量大小
        float sum = 0f;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample); // 使用绝对值
        }
        return sum / sampleSize; // 返回平均音量
    }

    void OnDestroy()
    {
        // 停止麦克风
        if (micClip != null)
        {
            Microphone.End(selectedMic);
        }
    }
}
