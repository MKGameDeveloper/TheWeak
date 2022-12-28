using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healparticle : MonoBehaviour
{

    IEnumerator acitvetime()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(acitvetime());
    }
}
