using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Warning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HideWarning());
    }

    void OnEnable()
    {
        StartCoroutine(HideWarning());
    }

    IEnumerator HideWarning()
    {
        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
    }
}
