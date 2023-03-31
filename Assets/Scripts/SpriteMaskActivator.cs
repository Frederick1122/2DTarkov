using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteMaskActivator : MonoBehaviour
{
    private void Start() => GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
}
