using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public Text playerName;
    SimplePlayerMovement target;
    public Vector3 screenOffset = new Vector3(0f, 30f, 0f);
    public Slider PlayerHealth;

    Transform targetTransform;
    Vector3 targetPosition;
    void Start()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

    }
    public void SetTarget(SimplePlayerMovement _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency
        target = _target;
       // Get data from the Player that won't change during the lifetime of this Component
        
        if (playerName != null)
        {
            playerName.text = target.photonView.Owner.NickName;
        }
    }

    void LateUpdate()
    {
        // #Critical
        // Follow the Target GameObject on screen.
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += 3;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
