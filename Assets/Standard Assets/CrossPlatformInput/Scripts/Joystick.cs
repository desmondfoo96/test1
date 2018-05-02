using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;




namespace UnityStandardAssets.CrossPlatformInput
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public enum AxisOption
        {
            // Options for which axes to use
            Both, // Use both
            OnlyHorizontal, // Only horizontal
            OnlyVertical // Only vertical
        }

        int stTime = 0;
        public int MovementRange = 100;
        public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
        public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

        Vector3 m_StartPos;
        bool m_UseX; // Toggle for using the x axis
        bool m_UseY; // Toggle for using the Y axis
        CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

        public Text x, y, startPosx, startPosy;
        private int direct;

        void OnEnable()
        {
            CreateVirtualAxes();
            if(stTime>0)
            {
                transform.position = m_StartPos;
                UpdateVirtualAxes(m_StartPos);
            }
        }

        void Start()
        {
            m_StartPos = transform.position;


        }


        void UpdateVirtualAxes(Vector3 value)
        {
            var delta = m_StartPos - value;
            delta.y = -delta.y;
            delta /= MovementRange;
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Update(-delta.x);
            }

            if (m_UseY)
            {
                m_VerticalVirtualAxis.Update(delta.y);
            }
        }


        void Update()
        {
            

            //x.text = transform.position.x.ToString();
            //y.text = transform.position.y.ToString();
            //startPosx.text = m_StartPos.x.ToString();

            //startPosy.text = m_StartPos.y.ToString();


            if (transform.position.x < m_StartPos.x + 1 && transform.position.x > m_StartPos.x - 1)
            {
                if (transform.position.y < m_StartPos.y + 1 && transform.position.y > m_StartPos.y - 1)
                {
                    direct = 0;
                }
            }
            else if (transform.position.x < m_StartPos.x && transform.position.y > m_StartPos.y)
            {
                direct = 1;

            }
            else if (transform.position.x > m_StartPos.x && transform.position.y > m_StartPos.y)
            {
                direct = 2;

            }
            else if (transform.position.x < m_StartPos.x && transform.position.y < m_StartPos.y)
            {
                direct = 3;

            }
            else if (transform.position.x > m_StartPos.x && transform.position.y < m_StartPos.y)
            {
                direct = 4;

            }

           // Debug.Log(direct);

          
            //Direction.text = direct;
        }
        void CreateVirtualAxes()
        {
            // set axes to use
            m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

            // create new axes based on axes to use
            if (m_UseX)
            {
                m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
            }
            if (m_UseY)
            {
                m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
            }
        }


        public void OnDrag(PointerEventData data)
        {
            Vector3 newPos = Vector3.zero;

            if (m_UseX)
            {
                int delta = (int)(data.position.x - m_StartPos.x);
                //delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
                newPos.x = delta;
            }

            if (m_UseY)
            {
                int delta = (int)(data.position.y - m_StartPos.y);
                //delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
                newPos.y = delta;
            }
            transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
            UpdateVirtualAxes(transform.position);
        }


        public void OnPointerUp(PointerEventData data)
        {
            transform.position = m_StartPos;
            UpdateVirtualAxes(m_StartPos);
        }


        public void OnPointerDown(PointerEventData data) { }

        void OnDisable()
        {
            // remove the joysticks from the cross platform input
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Remove();
            }
            if (m_UseY)
            {
                m_VerticalVirtualAxis.Remove();
            }
            stTime++;
        }

        public int getDirection()
        {
            return this.direct;
        }
    }
}