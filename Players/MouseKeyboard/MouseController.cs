﻿using CustomPackages.SilicomPlayer.Interactions;
using UnityEngine;

namespace CustomPackages.SilicomPlayer.Players.MouseKeyboard
{
    public class MouseController : MonoBehaviour
    {

        public RaycastHit Hit => _hit;

        [SerializeField] private new Camera camera;

        [SerializeField] private LayerMask layerMask;

        private Ray _ray;
        private RaycastHit _hit;

        private Collider _hitCollider;
        private IMouseInteract[] _currentInteractions;

        private bool _isHovering;

        public void RayCast(Vector2 mousePosition, float rayLength)
        {
            _ray = camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(_ray, out _hit, rayLength, layerMask))
            {
                Collider hitCollider = _hit.collider;

                if (hitCollider == _hitCollider)
                {
                    if(_isHovering) _currentInteractions.HoverStay(this);
                }
                else
                {
                    if (_isHovering)
                    {
                        _currentInteractions.HoverExit(this);
                        _currentInteractions = null;
                    }

                    _hitCollider = hitCollider;

                    if (_hitCollider)
                    {
                        _isHovering = true;
                        _currentInteractions = hitCollider.GetComponents<IMouseInteract>();
                        _currentInteractions.HoverEnter(this);
                    }
                    else
                    {
                        _isHovering = false;
                    }
                }
            }
            else
            {
                if (_isHovering)
                {
                    _hitCollider = null;
                    _currentInteractions.HoverExit(this);
                    _currentInteractions = null;
                }
                _isHovering = false;
            }
        }

        public void LeftClick()
        {
            if(!_isHovering) return;
            _currentInteractions.LeftClick(this);
        }

        public void RightClick()
        {
            if(!_isHovering) return;
            _currentInteractions.RightClick(this);
        }
    
        public enum MouseButton
        {
            LeftClick,
            RightClick
        }
    
    }
}