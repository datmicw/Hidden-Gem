using UnityEngine;

public class SquareHandler : MonoBehaviour
{
    private int tapCount = 0; // Đếm số lần chạm
    private float lastTapTime = 0f; // Lưu thời gian lần chạm cuối
    private float tapInterval = 0.5f; // Khoảng thời gian giữa 2 lần chạm để tính là double-tap

    void OnMouseDown()
    {
        float currentTime = Time.time;

        if (currentTime - lastTapTime < tapInterval)
        {
            tapCount++;
            Debug.Log($"Count: {tapCount}");
        }
        else
        {
            tapCount = 1; // Reset nếu thời gian giữa 2 lần chạm quá lâu
        }

        lastTapTime = currentTime;

        if (tapCount == 2)
        {
            // Double-tap xảy ra -> Xoá ô vuông
            Destroy(gameObject);
        }
    }
}
