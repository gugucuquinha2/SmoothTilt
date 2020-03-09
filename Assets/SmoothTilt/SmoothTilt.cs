//  Copyright(c) 2020 Gustavo Carneiro
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification,
//  are permitted provided that the following conditions are met:
//
//  1. Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//
//  2. Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//
//  3. Neither the name of the copyright holder nor the names of its contributors
//     may be used to endorse or promote products derived from this software without
//     specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
//  EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
//  OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT
//  SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT
//  OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
//  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
//  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
//  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using UnityEngine;

/// <summary>
/// This script is to be added to the object we want to apply the "Tilt" effect.
/// </summary>
public class SmoothTilt : MonoBehaviour
{
    private Transform thisTransf;
    private Vector3 centerRotation;

    private Renderer ren;
    private RectTransform rect;

    private float yRot = 0;
    private float xRot = 0;

    private float xInput = 0;
    private float yInput = 0;

    #region PUBLIC PROPERTIES
    [Space]

    [Header("MANAGEMENT")]

    [Tooltip("What is the camera to be used for the effect. If no camera is attached, the main camera will be used.")]
    [SerializeField]
    private Camera cam;
    /// <summary>
    /// If true, the effect will be applied relative to the object bounds. If false, the effect will applied relative to the whole screen.
    /// </summary>
    public Camera Cam
    {
        get { return cam; }
        set { cam = value; }
    }

    [Tooltip("If true, the effect will be applied relative to the object bounds. If false, the effect will applied relative to the whole screen. Can only be used, if the GameObject has a RectTransform or a Renderer.")]
    [SerializeField]
    [DrawCondition("useAxis", eComparisonType.NotEqual, true)]
    private bool isLocal = false;
    /// <summary>
    /// If true, the effect will be applied relative to the object bounds. If false, the effect will applied relative to the whole screen. Can only be used, if the GameObject has a RectTransform or a Renderer.
    /// </summary>
    public bool IsLocal
    {
        get { return isLocal; }
        set { isLocal = value; }
    }

    [Space]

    [SerializeField]
    [Tooltip("Invert the tilt in the X axis.")]
    private bool xInverse = false;
    /// <summary>
    /// Invert the tilt in the X axis.
    /// </summary>
    public bool X_Inverse
    {
        get { return xInverse; }
        set { xInverse = value; }
    }

    [SerializeField]
    [Tooltip("Invert the tilt in the Y axis.")]
    private bool yInverse = false;
    /// <summary>
    /// Invert the tilt in the Y axis.
    /// </summary>
    public bool Y_Inverse
    {
        get { return yInverse; }
        set { yInverse = value; }
    }

    [Space]

    [Tooltip("If true, the effect will support the use of Gamepad/Keyboard Axis keys.")]
    [SerializeField]
    [DrawCondition("isLocal", eComparisonType.NotEqual, true)]
    private bool useAxis = false;
    /// <summary>
    /// If true, the effect will support the use of Gamepad/Keyboard Axis keys.
    /// </summary>
    public bool UseAxis
    {
        get { return useAxis; }
        set { useAxis = value; }
    }

    [DrawCondition("useAxis", eComparisonType.NotEqual, false)]
    [SerializeField]
    [Tooltip("The name of the Input Axis to be used when rotating on the Y axis.")]
    private string horizontalAxis = "Horizontal";
    /// <summary>
    /// The name of the Input Axis to be used when rotating on the Y Axis.
    /// </summary>
    public string HorizontalAxis
    {
        get { return horizontalAxis; }
        set { horizontalAxis = value; }
    }

    [DrawCondition("useAxis", eComparisonType.NotEqual, false)]
    [SerializeField]
    [Tooltip("The name of the Input Axis to be used when rotating on the X axis.")]
    private string verticalAxis = "Vertical";
    /// <summary>
    /// The name of the Input Axis to be used when rotating on the X Axis.
    /// </summary>
    public string VerticalAxis
    {
        get { return verticalAxis; }
        set { verticalAxis = value; }
    }

    [Tooltip("If true, the tilt effect will reset back to the center once there's no input detected.")]
    [DrawCondition("useAxis", eComparisonType.NotEqual, false)]
    [SerializeField]
    private bool resetOnNoInput = false;
    /// <summary>
    /// If true, the tilt effect will reset back to the center once there's no input detected.
    /// </summary>
    public bool ResetOnNoInput
    {
        get { return resetOnNoInput; }
        set { resetOnNoInput = value; }
    }

    [Space]

    [Header("PROPERTIES")]

    [DrawCondition("useAxis", eComparisonType.NotEqual, false)]
    [SerializeField]
    [Tooltip("The speed multiplier to the horizontal Input Axis.")]
    private float horizontalSpeed = 0.5f;
    /// <summary>
    /// The speed multiplier to the horizontal Input Axis.
    /// </summary>
    public float HorizontalSpeed
    {
        get { return horizontalSpeed; }
        set { horizontalSpeed = value; }
    }

    [DrawCondition("useAxis", eComparisonType.NotEqual, false)]
    [SerializeField]
    [Tooltip("The speed multiplier to the vertical Input Axis.")]
    private float verticalSpeed = 0.5f;
    /// <summary>
    /// The speed multiplier to the vertical Input Axis.
    /// </summary>
    public float VerticalSpeed
    {
        get { return verticalSpeed; }
        set { verticalSpeed = value; }
    }

    [Space]

    [SerializeField]
    [Tooltip("How smooth is the effect.")]
    [Range(0f, 10f)]
    private float smoothness = 1f;
    /// <summary>
    /// How smooth is the effect.
    /// </summary>
    public float Smoothness
    {
        get { return smoothness; }
        set { smoothness = value; }
    }

    [SerializeField]
    [Tooltip("The amount of tilt the effect will have. This amount is applied equally to all vertical and horizontal directions of the tilt.")]
    private float tiltRange = 5;
    /// <summary>
    /// The amount of tilt the effect will have. This amount is applied equally to all vertical and horizontal directions of the tilt.
    /// </summary>
    public float TiltRange
    {
        get { return tiltRange; }
        set { tiltRange = value; }
    }
#endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        SetupComponents();
        SetCenterRotation(thisTransf.eulerAngles);
    }

    void OnValidate()
    {
        if (string.IsNullOrEmpty(horizontalAxis) || string.IsNullOrEmpty(verticalAxis))
        {
            Debug.LogWarning("One or more Input Axis names are not filled.");
        }
    }

    void Update()
    {
        TiltEffect();
    }

#region EFFECT MODES
#region Public Methods
    /// <summary>
    /// Automatic application of the tilt. It will check for the main modes (Local or Global) of the effect.
    /// </summary>
    public void TiltEffect()
    {
        if (isLocal)
        {
            LocalTilt();
        }
        else
        {
            GlobalTilt();
        }
    } 
    
    /// <summary>
    /// Tilt effect that is applied relative to the whole screen. Can be used either with mouse/touch input or keyboard/gamepad input.
    /// </summary>
    public void GlobalTilt()
    {
        if (useAxis)
        {
            // store input values
            xInput = Input.GetAxisRaw(horizontalAxis);
            yInput = Input.GetAxisRaw(verticalAxis);

            AxisTilt(xInput, yInput);
        }
        else
        {
            MouseTouchTilt(Vector3.zero, new Vector3(Display.main.renderingWidth, Display.main.renderingHeight));
        }

        // apply the tilt
        ApplyTilt(xRot, yRot);
    }

    /// <summary>
    /// Tilt effect that is only applied relative to the target UI position. Only supports mouse/touch input.
    /// </summary>
    public void LocalTilt()
    {
        if (useAxis)
        {
            isLocal = false;
            return;
        }

        Vector3 minToScreen = Vector3.zero;
        Vector3 maxToScreen = Vector3.zero;

        // use RectTransform (for UI)
        if (rect != null)
        {
            // get world corners
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);

            Vector3 minCorner = corners[0];
            Vector3 maxCorner = corners[0];

            // find the min and max corners to calculate bounds
            for (int i = 0; i < corners.Length; i++)
            {
                Vector3 v = corners[i];
                minCorner = Vector3.Min(minCorner, v);
                maxCorner = Vector3.Max(maxCorner, v);
            }

            // create the bounds
            Bounds bounds = new Bounds();
            bounds.SetMinMax(minCorner, maxCorner);

            // calculate screen bounds considering camera perspective, object rotation and scale
            FindMinAndMaxOfBounds(bounds, ref minToScreen, ref maxToScreen);
        }
        // use Renderer (for 2D/3D objects)
        else
        {
            // calculate screen bounds considering camera perspective, object rotation and scale
            FindMinAndMaxOfBounds(ren.bounds, ref minToScreen, ref maxToScreen);
        }

        MouseTouchTilt(minToScreen, maxToScreen);

        // apply the tilt
        ApplyTilt(xRot, yRot);
    }
#endregion

#region Private Methods
    /// <summary>
    /// Tilt effect based on keyboard/gamepad input.
    /// </summary>
    private void AxisTilt(float _xInput, float _yInput)
    {
#if UNITY_EDITOR
        Debug.Log("INPUT: " + _xInput + " , " + _yInput);
#endif

        // calculate the rotation based on the speed
        if (yInverse)
            yRot -= horizontalSpeed * _xInput;
        else yRot += horizontalSpeed * _xInput;
        if (xInverse)
            xRot += verticalSpeed * _yInput;
        else xRot -= verticalSpeed * _yInput;

        if (ResetOnNoInput && _xInput == 0 && _yInput == 0)
        {
            ResetToCenterRotation();
        }

        // clamp the tilt values to the calculated limit 
        yRot = Mathf.Clamp(yRot, centerRotation.y - tiltRange, centerRotation.y + tiltRange);
        xRot = Mathf.Clamp(xRot, centerRotation.x - tiltRange, centerRotation.x + tiltRange);
    }

    /// <summary>
    /// Tilt effect based on mouse/touch movement.
    /// </summary>
    private void MouseTouchTilt(Vector3 _minToScreen, Vector3 _maxToScreen)
    {
        Vector3 mousePos = Input.mousePosition;

#if UNITY_ANDROID || UNITY_IOS
        mousePos = Vector3.zero;
        if (Input.touchCount > 0)
        {
            mousePos = Input.touches[0].position;
        }
        else
        {
            ResetToCenterRotation();
        }
#endif

        //Debug.Log("MOUSE POS: " + mousePos + " // MIN: " + _minToScreen + " // MAX: " + _maxToScreen);

        // map the mouse position to the rect limits
        if (mousePos.x > _minToScreen.x && mousePos.x < _maxToScreen.x &&
            mousePos.y > _minToScreen.y && mousePos.y < _maxToScreen.y)
        {
            if (yInverse)
                yRot = Map(mousePos.x, _minToScreen.x, _maxToScreen.x, centerRotation.y + tiltRange, centerRotation.y - tiltRange);
            else yRot = Map(mousePos.x, _minToScreen.x, _maxToScreen.x, centerRotation.y - tiltRange, centerRotation.y + tiltRange);

            if (xInverse)
                xRot = Map(mousePos.y, _minToScreen.y, _maxToScreen.y, centerRotation.x - tiltRange, centerRotation.x + tiltRange);
            else xRot = Map(mousePos.y, _minToScreen.y, _maxToScreen.y, centerRotation.x + tiltRange, centerRotation.x - tiltRange);
        }
        else
        {
            ResetToCenterRotation();
        }
    }

#endregion
#endregion

#region EFFECT MANAGEMENT
    private void ApplyTilt(float _xRot, float _yRot)
    {
        Quaternion targetRot = Quaternion.Euler(_xRot, _yRot, 0.0f);
        thisTransf.rotation = Quaternion.Slerp(thisTransf.rotation, targetRot, smoothness * Time.deltaTime);
    }

    /// <summary>
    /// Set the new center rotation for the camera. The tilt rotation will be made considering this rotation as its new center.
    /// </summary>
    public void SetCenterRotation(Vector3 _newRotation)
    {
        centerRotation = _newRotation;

        // update the stored input rotation
        yRot = thisTransf.eulerAngles.y;
        xRot = thisTransf.eulerAngles.x;
    }

    private void ResetToCenterRotation()
    {
        xRot = centerRotation.x;
        yRot = centerRotation.y;
    }
#endregion

#region MISC
    /// <summary>
    /// Calculate the interaction boundaries of the object on the screen. Considering camera perspective, object rotation and scale.
    /// </summary>
    private void FindMinAndMaxOfBounds(Bounds _bounds, ref Vector3 _minToScreen, ref Vector3 _maxToScreen)
    {
#if UNITY_EDITOR
        debugBounds = _bounds;
#endif
        Vector3 cen = _bounds.center;
        Vector3 ext = _bounds.extents;
        Vector2[] extentPoints = new Vector2[8]
        {
                 cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
                 cam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
        };
        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];

        for (int i = 0; i < extentPoints.Length; i++)
        {
            Vector2 v = extentPoints[i];
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }

        // define the min and max positions on the screen
        _minToScreen = new Vector3(min.x, min.y, 0);
        _maxToScreen = new Vector3(max.x, max.y, 0);
    }
    
    /// <summary>
    /// Maps a range of values to another.
    /// </summary>
    private float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void SetupComponents()
    {
        thisTransf = transform;

        // check if can use local mode
        rect = GetComponent<RectTransform>();
        ren = GetComponent<Renderer>();
        if (isLocal)
        {
            if (ren == null && rect == null)
            {
                Debug.LogWarning("The local mode of the tilt effect requires a 'RectTransform' or a 'Renderer'. The effect will revert back to global mode.");
                isLocal = false;
            }
        }
    }
#endregion

#if UNITY_EDITOR
    Bounds debugBounds = new Bounds();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(debugBounds.center, debugBounds.size);
    }
#endif
}
