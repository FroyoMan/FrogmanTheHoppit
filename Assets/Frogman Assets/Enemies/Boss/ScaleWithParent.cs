using UnityEngine;

public class ScaleWithParent : MonoBehaviour
{
    private void LateUpdate()
    {
        float parentScaleY = transform.parent.lossyScale.y;
        transform.localScale = new Vector3(1f, 1f / parentScaleY, 1f);
    }
}
