/************************************************************************************
Filename    :   OVRGrabber.cs
Author      :   Lingyu Li
Content     :   Adapted from the original OVRGrabber.cs from Oculus SDK
Created     :   Janurary 19th, 2019
MIT License
Copyright (c) 2019 Reality Virtually Hachathon
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:
The above copyright notice and this permission notice (including the next
paragraph) shall be included in all copies or substantial portions of the
Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF
OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
************************************************************************************/

/************************************************************************************

Copyright   :   Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Licensed under the Oculus SDK License Version 3.4.1 (the "License");
you may not use the Oculus SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

https://developer.oculus.com/licenses/sdk-3.4.1

Unless required by applicable law or agreed to in writing, the Oculus SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;

/// <summary>
/// Allows grabbing and throwing of objects with the OVRGrabbable component on them.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class OVRGrabber : MonoBehaviour
{
    #region Parameters

    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;

    // Demonstrates parenting the held object to the hand's transform when grabbed.
    // When false, the grabbed object is moved every FixedUpdate using MovePosition. 
    // Note that MovePosition is required for proper physics simulation. If you set this to true, you can
    // easily observe broken physics simulation by, for example, moving the bottom cube of a stacked
    // tower and noting a complete loss of friction.
    [SerializeField]
    protected bool m_parentHeldObject = false;

    // Child/attached transforms of the grabber, indicating where to snap held objects to (if you snap them).
    // Also used for ranking grab targets in case of multiple candidates.
    [SerializeField]
    protected Transform m_gripTransform = null;
    // Child/attached Colliders to detect candidate grabbable objects.
    [SerializeField]
    protected Collider[] m_grabVolumes = null;

    // Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
    [SerializeField]
    protected OVRInput.Controller m_controller;

    [SerializeField]
    protected Transform m_parentTransform;

   ////////////////////////////////////////////////////////////////


    [SerializeField]
    protected Transform m_hand;

    protected Transform localAvatar;

    [SerializeField]
    protected Transform m_controllerPoint;

    protected OVRCameraRig rig = null;

    protected System.Action<OVRCameraRig> updateAnchor;

    protected System.Action<OVRCameraRig> updateObject;
    ////////////////////////////////////////////////////////////////////



    protected bool m_grabVolumeEnabled = true;
    protected Vector3 m_lastPos;
    protected Quaternion m_lastRot;
    protected Quaternion m_anchorOffsetRotation;
    protected Vector3 m_anchorOffsetPosition;
    protected float m_prevFlex;
	protected OVRGrabbable m_grabbedObj = null;
    protected Vector3 m_grabbedObjectPosOff;
    protected Quaternion m_grabbedObjectRotOff;
	protected Dictionary<OVRGrabbable, int> m_grabCandidates = new Dictionary<OVRGrabbable, int>();
	protected bool operatingWithoutOVRCameraRig = true;

    /// <summary>
    /// The currently grabbed object.
    /// </summary>
    public OVRGrabbable grabbedObject
    {
        get { return m_grabbedObj; }
    }

    #endregion

    #region Unity Functions

   
    protected virtual void Awake()
    {
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;

		// If we are being used with an OVRCameraRig, let it drive input updates, which may come from Update or FixedUpdate.

	
		if (transform.parent != null && transform.parent.parent != null)
			rig = transform.parent.parent.GetComponent<OVRCameraRig>();
		
      
        updateAnchor = (r) => { OnUpdatedAnchors(); };
        updateObject = (r) => { OnUpdateObject(); };

        if (rig != null)
		{
            rig.UpdatedAnchors += updateAnchor;
            rig.UpdatedAnchors += (r) => { GrabOrRelease(); };
            operatingWithoutOVRCameraRig = false;
		}
    }

        #region Other Unity Functions
    protected virtual void Start()
    {
        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
        if(m_parentTransform == null)
        {
            if(gameObject.transform.parent != null)
            {
                m_parentTransform = gameObject.transform.parent.transform;
            }
            else
            {
                m_parentTransform = new GameObject().transform;
                m_parentTransform.position = Vector3.zero;
                m_parentTransform.rotation = Quaternion.identity;
            }
        }
    }

	void FixedUpdate()
	{
		if (operatingWithoutOVRCameraRig)
			OnUpdatedAnchors();
	}

    void OnTriggerEnter(Collider otherCollider)
    {
        // Get the grab trigger
        OVRGrabbable grabbable = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
        if (grabbable == null) return;

        // Add the grabbable
        int refCount = 0;
        m_grabCandidates.TryGetValue(grabbable, out refCount);
        m_grabCandidates[grabbable] = refCount + 1;
    }

    void OnTriggerExit(Collider otherCollider)
    {
        OVRGrabbable grabbable = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
        if (grabbable == null) return;

        // Remove the grabbable
        int refCount = 0;
        bool found = m_grabCandidates.TryGetValue(grabbable, out refCount);
        if (!found)
        {
            return;
        }

        if (refCount > 1)
        {
            m_grabCandidates[grabbable] = refCount - 1;
        }
        else
        {
            m_grabCandidates.Remove(grabbable);
        }
    }

    void OnDestroy()
    {
        if (m_grabbedObj != null)
        {
            GrabEnd();
        }
    }
        #endregion

    #endregion




    // Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
    // This is done instead of parenting to achieve workable physics. If you don't require physics on 
    // your hands or held objects, you may wish to switch to parenting.
    void OnUpdatedAnchors()
    {
        ////////////////////////////////Update Hands Position ////////////////////////////////
        Vector3 handPos = OVRInput.GetLocalControllerPosition(m_controller);
        Quaternion handRot = OVRInput.GetLocalControllerRotation(m_controller);

        Vector3 destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition + handPos);
        Quaternion destRot = m_parentTransform.rotation * handRot * m_anchorOffsetRotation;


        GetComponent<Rigidbody>().MovePosition(destPos);
        GetComponent<Rigidbody>().MoveRotation(destRot);

        //////////////////////////////////////////////////////////////////////////////////////

        if (!m_parentHeldObject)
        {
            MoveGrabbedObject(destPos, destRot);
        }
        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
		
    }

    /// <summary>
    /// Hands will not move and attached to the object,
    /// therefore, we need to update object position(Rotate wheel)
    /// </summary>
    void OnUpdateObject()
    {
        if (m_grabbedObj == null)
        {
            return;
        }
        
        Transform grabbedTransform = m_grabbedObj.transform;
        Vector3 linearVelocity = OVRInput.GetLocalControllerVelocity(m_controller);

        Vector3 centripetal = grabbedTransform.position - transform.position;

        float rotationDirection = Vector3.Dot(Vector3.Cross(centripetal, linearVelocity), grabbedTransform.TransformDirection(new Vector3(0, 1, 0)));
        float rotationAngle = (Mathf.Abs(rotationDirection) < .01f) ? 0f : (Mathf.Abs(rotationDirection) < 0.05f) ?- 1 * rotationDirection / Mathf.Abs(rotationDirection): -1 * rotationDirection / 0.05f;
        grabbedTransform.RotateAround(grabbedTransform.GetChild(0).position, grabbedTransform.GetChild(0).TransformDirection(new Vector3(0, 1, 0)), rotationAngle);
        
    }

    

    protected virtual void GrabBegin()
    {
        float closestMagSq = float.MaxValue;
		OVRGrabbable closestGrabbable = null;
        Collider closestGrabbableCollider = null;

        // Iterate grab candidates and find the closest grabbable candidate
		foreach (OVRGrabbable grabbable in m_grabCandidates.Keys)
        {
            bool canGrab = !(grabbable.isGrabbed && !grabbable.allowOffhandGrab);
            if (!canGrab)
            {
                continue;
            }

            for (int j = 0; j < grabbable.grabPoints.Length; ++j)
            {
                Collider grabbableCollider = grabbable.grabPoints[j];
                // Store the closest grabbable
                Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
                float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;
                if (grabbableMagSq < closestMagSq)
                {
                    closestMagSq = grabbableMagSq;
                    closestGrabbable = grabbable;
                    closestGrabbableCollider = grabbableCollider;
                }
            }
        }

        // Disable grab volumes to prevent overlaps
        GrabVolumeEnable(false);

        if (closestGrabbable != null)
        {
            if (closestGrabbable.isGrabbed)
            {
                closestGrabbable.grabbedBy.OffhandGrabbed(closestGrabbable);
            }

            m_grabbedObj = closestGrabbable;
            m_grabbedObj.GrabBegin(this, closestGrabbableCollider);

            m_lastPos = transform.position;
            m_lastRot = transform.rotation;

            // Set up offsets for grabbed object desired position relative to hand.
            if(m_grabbedObj.snapPosition)
            {
                m_grabbedObjectPosOff = m_gripTransform.localPosition;
                if(m_grabbedObj.snapOffset)
                {
                    Vector3 snapOffset = m_grabbedObj.snapOffset.position;
                    if (m_controller == OVRInput.Controller.LTouch) snapOffset.x = -snapOffset.x;
                    m_grabbedObjectPosOff += snapOffset;
                }
            }
            else
            {
                Vector3 relPos = m_grabbedObj.transform.position - transform.position;
                relPos = Quaternion.Inverse(transform.rotation) * relPos;
                m_grabbedObjectPosOff = relPos;
            }

            if (m_grabbedObj.snapOrientation)
            {
                m_grabbedObjectRotOff = m_gripTransform.localRotation;
                if(m_grabbedObj.snapOffset)
                {
                    m_grabbedObjectRotOff = m_grabbedObj.snapOffset.rotation * m_grabbedObjectRotOff;
                }
            }
            else
            {
                Quaternion relOri = Quaternion.Inverse(transform.rotation) * m_grabbedObj.transform.rotation;
                m_grabbedObjectRotOff = relOri;
            }

          
            

            if(m_grabbedObj.isFixed)
            {
                OVRCameraRig rig = transform.parent.parent.GetComponent<OVRCameraRig>();
                rig.UpdatedAnchors -= updateAnchor;
                localAvatar = m_hand.parent;

                OvrAvatar.lockedHand = m_hand.gameObject;
                OvrAvatar.lockedController = m_controllerPoint.gameObject;

                m_hand.parent = m_grabbedObj.transform;
                m_controllerPoint.parent = m_grabbedObj.transform;
                rig.UpdatedAnchors += updateObject;
            }
            else
            {
                // Note: force teleport on grab, to avoid high-speed travel to dest which hits a lot of other objects at high
                // speed and sends them flying. The grabbed object may still teleport inside of other objects, but fixing that
                // is beyond the scope of this demo.
                MoveGrabbedObject(m_lastPos, m_lastRot, true);

                if (m_parentHeldObject)
                {
                    m_grabbedObj.transform.parent = transform;
                }
            }
           
        }
    }
    

    protected void GrabEnd()
    {
        if (m_grabbedObj != null)
        {
            OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
            OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
            localPose = localPose * offsetPose;

            OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
            Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
            Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);


            if (m_grabbedObj.isFixed)
            {
                OVRCameraRig rig = transform.parent.parent.GetComponent<OVRCameraRig>();
                rig.UpdatedAnchors -= updateObject;
                OvrAvatar.lockedHand = null;
                OvrAvatar.lockedController = null;
                m_hand.parent = localAvatar;
                m_controllerPoint.parent = localAvatar;
                rig.UpdatedAnchors += updateAnchor;
                GrabbableRelease(Vector3.zero, Vector3.zero);
            }
            else
            {
                GrabbableRelease(linearVelocity, angularVelocity);
            }

        }

        // Re-enable grab volumes to allow overlap events
        GrabVolumeEnable(true);
    }


    #region Other Help Functions
    protected virtual void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        if (m_grabbedObj == null)
        {
            return;
        }

        Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;
        Vector3 grabbablePosition = pos + rot * m_grabbedObjectPosOff;
        Quaternion grabbableRotation = rot * m_grabbedObjectRotOff;

        if (forceTeleport)
        {
            grabbedRigidbody.transform.position = grabbablePosition;
            grabbedRigidbody.transform.rotation = grabbableRotation;
        }
        else
        {
            grabbedRigidbody.MovePosition(grabbablePosition);
            grabbedRigidbody.MoveRotation(grabbableRotation);
        }
    }

    protected void GrabOrRelease()
    {
        float prevFlex = m_prevFlex;
        // Update values from inputs
        m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);

        CheckForGrabOrRelease(prevFlex);
    }

    protected void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        m_grabbedObj.GrabEnd(linearVelocity, angularVelocity);
        if(m_parentHeldObject) m_grabbedObj.transform.parent = null;
        m_grabbedObj = null;
    }

    protected virtual void GrabVolumeEnable(bool enabled)
    {
        if (m_grabVolumeEnabled == enabled)
        {
            return;
        }

        m_grabVolumeEnabled = enabled;
        for (int i = 0; i < m_grabVolumes.Length; ++i)
        {
            Collider grabVolume = m_grabVolumes[i];
            grabVolume.enabled = m_grabVolumeEnabled;
        }

        if (!m_grabVolumeEnabled)
        {
            m_grabCandidates.Clear();
        }
    }

    public void ForceRelease(OVRGrabbable grabbable)
    {
        bool canRelease = (
            (m_grabbedObj != null) &&
            (m_grabbedObj == grabbable)
        );
        if (canRelease)
        {
            GrabEnd();
        }
    }

    protected void CheckForGrabOrRelease(float prevFlex)
    {
        if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin))
        {
            GrabBegin();
        }
        else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd))
        {
            GrabEnd();
        }
    }

    protected virtual void OffhandGrabbed(OVRGrabbable grabbable)
    {
        if (m_grabbedObj == grabbable)
        {
            GrabbableRelease(Vector3.zero, Vector3.zero);
        }
    }

    #endregion

}
