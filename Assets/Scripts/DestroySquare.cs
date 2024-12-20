using UnityEngine;
// phá huỷ ô vuông khi nhấn 2 lần vào ô vuông
public class DestroySquare : MonoBehaviour
{
    private float firstTapTime = 0; // thời gian đầu
    private float tapThreshold = 0.3f; // thời gian sau

    void OnMouseDown()
    {
        // kiểm tra nếu người dùng có click khôg
        if (Time.time - firstTapTime < tapThreshold)
        {
            Destroy(gameObject);
        }
        else
        {
            firstTapTime = Time.time;
        }
    }
}
