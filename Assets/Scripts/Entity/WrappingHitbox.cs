using UnityEngine;

public class WrappingHitbox : MonoBehaviour
{

    private Rigidbody2D body;
    private BoxCollider2D[] ourColliders, childColliders;
    private bool cachedDoOffset = false;
    private Vector2 cachedOffset;
    private Vector2 offset;
    private float levelMiddle, levelWidth;

    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        if (!body)
            body = GetComponentInParent<Rigidbody2D>();
        ourColliders = GetComponents<BoxCollider2D>();

        // null propagation is ok w/ GameManager.Instance
        if (!(GameManager.Instance?.loopingLevel ?? false))
        {
            enabled = false;
            return;
        }

        childColliders = new BoxCollider2D[ourColliders.Length];
        for (int i = 0; i < ourColliders.Length; i++)
            childColliders[i] = gameObject.AddComponent<BoxCollider2D>();
        levelWidth = GameManager.Instance.levelWidthTile / 2f;
        levelMiddle = GameManager.Instance.GetLevelMinX() + levelWidth / 2f;
        offset = new(levelWidth, 0);

        FixedUpdate();
    }

    public void FixedUpdate()
    { //I don't care to test if it's one frame delayed or not, it works
        for (int i = 0; i < ourColliders.Length; i++)
            UpdateChildColliders(i);
    }

    private void UpdateChildColliders(int index)
    {
        BoxCollider2D ourCollider = ourColliders[index];
        BoxCollider2D childCollider = childColliders[index];

        if (ourCollider.autoTiling != childCollider.autoTiling)
        {
            childCollider.autoTiling = ourCollider.autoTiling;
        }
        if (ourCollider.edgeRadius != childCollider.edgeRadius)
        {
            childCollider.edgeRadius = ourCollider.edgeRadius;
        }
        if (ourCollider.enabled != childCollider.enabled)
        {
            childCollider.enabled = ourCollider.enabled;
        }
        if (ourCollider.isTrigger != childCollider.isTrigger)
        {
            childCollider.isTrigger = ourCollider.isTrigger;
        }

        if (cachedDoOffset != body.position.x < levelMiddle || childCollider.offset != cachedOffset)
        {
            childCollider.offset = ourCollider.offset + (((body.position.x < levelMiddle) ? offset : -offset) / body.transform.lossyScale);
        }

        if (ourCollider.sharedMaterial != childCollider.sharedMaterial)
        {
            childCollider.sharedMaterial = ourCollider.sharedMaterial;
        }
        if (ourCollider.size != childCollider.size)
        {
            childCollider.size = ourCollider.size;
        }
        if (ourCollider.usedByComposite != childCollider.usedByComposite)
        {
            childCollider.usedByComposite = ourCollider.usedByComposite;
        }
        if (ourCollider.usedByEffector != childCollider.usedByEffector)
        {
            childCollider.usedByEffector = ourCollider.usedByEffector; //HEY IPOD, I FIXED YOUR OOPSIE!
        }

        cachedDoOffset = body.position.x < levelMiddle;
        cachedOffset = ourCollider.offset;
    }
}