using UnityEngine;
using UnityEngine.InputSystem;

public class JumpButtonTest : MonoBehaviour
{
    private void Update()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;

        // キーボード接続チェック
        if (current == null)
            return;

        // スペースキーが押された瞬間かどうか
        if (current.spaceKey.wasPressedThisFrame)
            Debug.Log("スペースキーが押された！");
    }
}