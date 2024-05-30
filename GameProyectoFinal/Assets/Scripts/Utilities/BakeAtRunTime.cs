using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class BakeAtRunTime : MonoBehaviour
{
    [SerializeField]
    public NavMeshSurface navElement {  get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<NavMeshSurface>().BuildNavMeshAsync();
    }

}
