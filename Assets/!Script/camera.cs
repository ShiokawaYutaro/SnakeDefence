
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class camera : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleMask;
    public Material transparentMat;
    public Material defaultMat;

    private Renderer lastRenderer;

    void Update()
    {
        Vector3 dir = player.position - transform.position;
        float distance = Vector3.Distance(transform.position, player.position);

        if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, distance, obstacleMask))
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                if (lastRenderer != null && lastRenderer != rend)
                {
                    lastRenderer.material = defaultMat;
                }

                rend.material = transparentMat;
                lastRenderer = rend;
            }
        }
        else
        {
            if (lastRenderer != null)
            {
                lastRenderer.material = defaultMat;
                lastRenderer = null;
            }
        }
    }


}
