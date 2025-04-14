using UnityEngine;

public class ArrowLook : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target;
    public Transform LookAtTarget { get { return m_Target; } }

    [SerializeField]
    private Transform m_Spinner;
    public Transform Spinner { get { return m_Spinner; } }
    [SerializeField]
    private Transform m_Scaler;
    public Transform Scaler { get { return m_Scaler; } }

    public float speed;

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    private void FixedUpdate()
    {
        if (LookAtTarget)
            transform.LookAt(LookAtTarget);

        if (Spinner)
            Spinner.transform.Rotate(0, 0, speed * Time.deltaTime);
    }

}
