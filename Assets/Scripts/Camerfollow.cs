using UnityEngine;

public class Camerfollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 3, -8);
    [SerializeField] float smooth = 5f;

    // Update is called once per frame
    void LateUpdate()
    {
      if(!target) return;
      Vector3 desiredPosition = target.position + offset;
      transform.position = Vector3.Lerp(transform.position, desiredPosition, smooth * Time.deltaTime); 
    }
}
