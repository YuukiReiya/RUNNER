using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapConverter : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    Image Icon;

    [SerializeField]
    float xRate;
    [SerializeField]
    float yRate;
    [SerializeField]
    Vector2 offset;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        float dir = -player.transform.eulerAngles.y;

        Icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, dir));

        Vector3 cPos = player.gameObject.transform.position;
        Debug.Log("ワールド" + cPos);
        cPos.x = cPos.x / xRate+offset.x;
        cPos.y = cPos.z / yRate+offset.y;
        cPos.z = 0;
        Icon.rectTransform.localPosition = cPos;
	}
}
