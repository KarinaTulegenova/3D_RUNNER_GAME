using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CharacterMover))]
    public class MovePlayerInput : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField]
        private string m_HorizontalAxis = "Horizontal";
        [SerializeField]
        private string m_VerticalAxis = "Vertical";
        [SerializeField]
        private string m_JumpButton = "Jump";
        [SerializeField]
        private KeyCode m_RunKey = KeyCode.LeftShift;
        [SerializeField]
        private bool m_AlwaysRun = true;

        [Header("Camera")]
        [SerializeField]
        private PlayerCamera m_Camera;
        [SerializeField]
        private string m_MouseX = "Mouse X";
        [SerializeField]
        private string m_MouseY = "Mouse Y";
        [SerializeField]
        private string m_MouseScroll = "Mouse ScrollWheel";

        private CharacterMover m_Mover;

        private Vector2 m_Axis;
        private bool m_IsRun;
        private bool m_IsJump;

        private Vector3 m_Target;
        private Vector2 m_MouseDelta;
        private float m_Scroll;

        private void Awake()
        {
            m_Mover = GetComponent<CharacterMover>();

            if(m_Camera == null ) 
            {
                m_Camera = Camera.main == null ? null : Camera.main.GetComponent<PlayerCamera>();
            }
            if(m_Camera != null) {
                m_Camera.SetPlayer(transform);
            }
        }

        private void Update()
        {
            GatherInput();
            SetInput();
        }

        public void GatherInput()
        {
            m_Axis = new Vector2(Input.GetAxis(m_HorizontalAxis), Input.GetAxis(m_VerticalAxis));
            m_IsJump = Input.GetButton(m_JumpButton);

            if (m_Camera == null)
            {
                m_IsRun = m_AlwaysRun ? m_Axis.sqrMagnitude > Mathf.Epsilon : Input.GetKey(m_RunKey);

                if (m_Axis.sqrMagnitude > Mathf.Epsilon)
                {
                    Vector3 moveDirection = new Vector3(m_Axis.x, 0f, m_Axis.y).normalized;
                    m_Target = transform.position + moveDirection;
                    m_Axis = new Vector2(0f, 1f);
                }
                else
                {
                    m_Target = transform.position + transform.forward;
                    m_Axis = Vector2.zero;
                }
            }
            else
            {
                m_Target = m_Camera.Target;
                m_IsRun = m_AlwaysRun ? m_Axis.sqrMagnitude > Mathf.Epsilon : Input.GetKey(m_RunKey);
            }
            m_MouseDelta = new Vector2(Input.GetAxis(m_MouseX), Input.GetAxis(m_MouseY));
            m_Scroll = Input.GetAxis(m_MouseScroll);
        }

        public void BindMover(CharacterMover mover)
        {
            m_Mover = mover;
        }

        public void SetInput()
        {
            if (m_Mover != null)
            {
                m_Mover.SetInput(in m_Axis, in m_Target, in m_IsRun, m_IsJump);
            }

            if (m_Camera != null)
            {
                m_Camera.SetInput(in m_MouseDelta, m_Scroll);
            }
        }
    }
}
