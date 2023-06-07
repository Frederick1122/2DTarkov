using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private List<Exit> _exits;

    public List<Exit> GetExits() => _exits;
}
