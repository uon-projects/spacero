using UnityEngine;
using System.Collections;

public class InputHelper : MonoBehaviour
{
    private const string INPUT_HORIZONTAL_AXIS = "Horizontal";
    private const string INPUT_VERTICAL_AXIS = "Vertical";
    private const string INPUT_A = "A";
    private const string INPUT_B = "B";
    private const string INPUT_X = "X";
    private const string INPUT_Y = "Y";
    private const string INPUT_L = "L";
    private const string INPUT_R = "R";
    private const string INPUT_LSTICK = "LStick";
    private const string INPUT_RSTICK = "RStick";
    private const string INPUT_START = "Start";
    private const string INPUT_BACK = "Back";

    bool _shouldUpdate = true;

    InputAxisState _isUpPressed = InputAxisState.Idle;
    InputAxisState _isDownPressed = InputAxisState.Idle;
    InputAxisState _isLeftPressed = InputAxisState.Idle;
    InputAxisState _isRightPressed = InputAxisState.Idle;

    ButtonStateRetriever _upStateRetriever = null;
    ButtonStateRetriever _downStateRetriever = null;
    ButtonStateRetriever _holdStateRetriever = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _upStateRetriever = new ButtonUpRetriever(this);
        _downStateRetriever = new ButtonDownRetriever(this);
        _holdStateRetriever = new ButtonHoldRetriever(this);
    }

    void Start()
    {
        _shouldUpdate = true;

        _isUpPressed = InputAxisState.Idle;
        _isDownPressed = InputAxisState.Idle;
        _isLeftPressed = InputAxisState.Idle;
        _isRightPressed = InputAxisState.Idle;
    }

    void LateUpdate()
    {
        _shouldUpdate = true;
    }

    void UpdateAllAxis(bool forceUpdate = false)
    {
        if (!_shouldUpdate && !forceUpdate)
            return;

        _shouldUpdate = false;

        UpdateAxisState((int) Input.GetAxisRaw(INPUT_VERTICAL_AXIS), ref _isUpPressed, ref _isDownPressed);
        UpdateAxisState((int) Input.GetAxisRaw(INPUT_HORIZONTAL_AXIS), ref _isRightPressed, ref _isLeftPressed);
    }

    void UpdateAxisState(int axis, ref InputAxisState positiveState, ref InputAxisState negativeState)
    {
        UpdateAxisState(axis, 1, ref positiveState);
        UpdateAxisState(axis, -1, ref negativeState);
    }

    void UpdateAxisState(int axis, int desiredValue, ref InputAxisState state)
    {
        if (axis == desiredValue)
        {
            if (state == InputAxisState.Idle)
            {
                state = InputAxisState.Pressed;
            }
            else if (state == InputAxisState.Pressed)
            {
                state = InputAxisState.Holding;
            }
        }
        else
        {
            if (state == InputAxisState.Pressed || state == InputAxisState.Holding)
            {
                state = InputAxisState.Released;
            }
            else if (state == InputAxisState.Released)
            {
                state = InputAxisState.Idle;
            }
        }
    }

    public IButtonStateRetriever Up
    {
        get { return _upStateRetriever; }
    }

    public IButtonStateRetriever Down
    {
        get { return _downStateRetriever; }
    }

    public IButtonStateRetriever Hold
    {
        get { return _holdStateRetriever; }
    }

    public bool GetUpArrowState(InputPressState pressState)
    {
        if (pressState == InputPressState.Down)
            return this.UpArrowDown;
        if (pressState == InputPressState.Up)
            return this.UpArrowUp;
        return this.UpArrow;
    }

    public bool GetDownArrowState(InputPressState pressState)
    {
        if (pressState == InputPressState.Down)
            return this.DownArrowDown;
        if (pressState == InputPressState.Up)
            return this.DownArrowUp;
        return this.DownArrow;
    }

    public bool GetLeftArrowState(InputPressState pressState)
    {
        if (pressState == InputPressState.Down)
            return this.LeftArrowDown;
        if (pressState == InputPressState.Up)
            return this.LeftArrowUp;
        return this.LeftArrow;
    }

    public bool GetRightArrowState(InputPressState pressState)
    {
        if (pressState == InputPressState.Down)
            return this.RightArrowDown;
        if (pressState == InputPressState.Up)
            return this.RightArrowUp;
        return this.RightArrow;
    }

    private bool UpArrowUp
    {
        get
        {
            UpdateAllAxis();
            return _isUpPressed == InputAxisState.Released;
        }
    }

    private bool UpArrow
    {
        get
        {
            UpdateAllAxis();
            return _isUpPressed == InputAxisState.Pressed || _isUpPressed == InputAxisState.Holding;
        }
    }

    private bool UpArrowDown
    {
        get
        {
            UpdateAllAxis();
            return _isUpPressed == InputAxisState.Pressed;
        }
    }

    private bool DownArrowUp
    {
        get
        {
            UpdateAllAxis();
            return _isDownPressed == InputAxisState.Released;
        }
    }

    private bool DownArrow
    {
        get
        {
            UpdateAllAxis();
            return _isDownPressed == InputAxisState.Pressed || _isDownPressed == InputAxisState.Holding;
        }
    }

    private bool DownArrowDown
    {
        get
        {
            UpdateAllAxis();
            return _isDownPressed == InputAxisState.Pressed;
        }
    }

    private bool LeftArrowUp
    {
        get
        {
            UpdateAllAxis();
            return _isLeftPressed == InputAxisState.Released;
        }
    }

    private bool LeftArrow
    {
        get
        {
            UpdateAllAxis();
            return _isLeftPressed == InputAxisState.Pressed || _isLeftPressed == InputAxisState.Holding;
        }
    }

    private bool LeftArrowDown
    {
        get
        {
            UpdateAllAxis();
            return _isLeftPressed == InputAxisState.Pressed;
        }
    }

    private bool RightArrowUp
    {
        get
        {
            UpdateAllAxis();
            return _isRightPressed == InputAxisState.Released;
        }
    }

    private bool RightArrow
    {
        get
        {
            UpdateAllAxis();
            return _isRightPressed == InputAxisState.Pressed || _isRightPressed == InputAxisState.Holding;
        }
    }

    private bool RightArrowDown
    {
        get
        {
            UpdateAllAxis();
            return _isRightPressed == InputAxisState.Pressed;
        }
    }

    public enum InputAxisState
    {
        Idle,
        Pressed,
        Holding,
        Released
    }

    public enum InputPressState
    {
        Up,
        Down,
        Hold
    }

    abstract class ButtonStateRetriever : IButtonStateRetriever
    {
        protected InputHelper _helper;
        protected InputPressState _desiredInputPressState;

        public ButtonStateRetriever()
        {
            throw new UnityException("Parameterless constructor not supported!");
        }

        public ButtonStateRetriever(InputHelper helper, InputPressState desiredInputPressState)
        {
            _helper = helper;
            _desiredInputPressState = desiredInputPressState;
        }

        abstract protected bool GetButtonState(string buttonName);

        public bool Up
        {
            get { return _helper.GetUpArrowState(_desiredInputPressState); }
        }

        public bool Down
        {
            get { return _helper.GetDownArrowState(_desiredInputPressState); }
        }

        public bool Left
        {
            get { return _helper.GetLeftArrowState(_desiredInputPressState); }
        }

        public bool Right
        {
            get { return _helper.GetRightArrowState(_desiredInputPressState); }
        }

        public bool A
        {
            get { return GetButtonState(INPUT_A); }
        }

        public bool B
        {
            get { return GetButtonState(INPUT_B); }
        }

        public bool X
        {
            get { return GetButtonState(INPUT_X); }
        }

        public bool Y
        {
            get { return GetButtonState(INPUT_Y); }
        }

        public bool L
        {
            get { return GetButtonState(INPUT_L); }
        }

        public bool R
        {
            get { return GetButtonState(INPUT_R); }
        }

        public bool LStick
        {
            get { return GetButtonState(INPUT_LSTICK); }
        }

        public bool RStick
        {
            get { return GetButtonState(INPUT_RSTICK); }
        }

        public bool Start
        {
            get { return GetButtonState(INPUT_START); }
        }

        public bool Back
        {
            get { return GetButtonState(INPUT_BACK); }
        }
    }

    class ButtonDownRetriever : ButtonStateRetriever
    {
        public ButtonDownRetriever(InputHelper helper) : base(helper, InputPressState.Down)
        {
        }

        override protected bool GetButtonState(string buttonName)
        {
            return Input.GetButtonDown(buttonName);
        }
    }

    class ButtonUpRetriever : ButtonStateRetriever
    {
        public ButtonUpRetriever(InputHelper helper) : base(helper, InputPressState.Up)
        {
        }

        override protected bool GetButtonState(string buttonName)
        {
            return Input.GetButtonUp(buttonName);
        }
    }

    class ButtonHoldRetriever : ButtonStateRetriever
    {
        public ButtonHoldRetriever(InputHelper helper) : base(helper, InputPressState.Hold)
        {
        }

        override protected bool GetButtonState(string buttonName)
        {
            return Input.GetButton(buttonName);
        }
    }
}

public interface IButtonStateRetriever
{
    bool Up { get; }
    bool Down { get; }
    bool Left { get; }
    bool Right { get; }
    bool A { get; }
    bool B { get; }
    bool X { get; }
    bool Y { get; }
    bool L { get; }
    bool R { get; }
    bool LStick { get; }
    bool RStick { get; }
    bool Start { get; }
    bool Back { get; }
}

static internal class InputExtensions
{
    //Disable the unused variable warning
#pragma warning disable 0414
    static private InputHelper _inputHelper = (new GameObject("InputHelper")).AddComponent<InputHelper>();
#pragma warning restore 0414

    static public IButtonStateRetriever Pressed
    {
        get { return _inputHelper.Down; }
    }

    static public IButtonStateRetriever Down
    {
        get { return _inputHelper.Down; }
    }

    static public IButtonStateRetriever Released
    {
        get { return _inputHelper.Up; }
    }

    static public IButtonStateRetriever Up
    {
        get { return _inputHelper.Up; }
    }

    static public IButtonStateRetriever Holding
    {
        get { return _inputHelper.Hold; }
    }

    static public IButtonStateRetriever Is
    {
        get { return _inputHelper.Hold; }
    }
}