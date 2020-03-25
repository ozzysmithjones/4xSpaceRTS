public class StateMachine
{

    protected State[] states = new State[1];
    protected Transition[] fromAnyState = new Transition[0];
    public State currentState;


    public void Start()
    {
        AddStates();
        currentState = states[0];
    }

    public void End()
    {
        states = new State[0];
        fromAnyState = new Transition[0];
    }


    // Start is called before the first frame update
    public void OnEnter()
    {
        currentState = states[0];
        currentState.OnEnter();
    }

    public virtual void AddStates()
    {

    }

    // Update is called once per frame
    public void Tick()
    {
        currentState.OnTick();


        if (!currentState.canChangeState)
        {
            return;
        }

        for (int i = 0; i < fromAnyState.Length; i++)
        {
            if (fromAnyState[i].ShouldChangeState())
            {
                SetState(fromAnyState[i].state);
                return;
            }
        }

        for (int i = 0; i < currentState.transitions.Length; i++)
        {
            if (currentState.transitions[i].ShouldChangeState())
            {
                SetState(currentState.transitions[i].state);
                return;
            }
        }


    }

    public void SetState(State state)
    {
        currentState.OnExit();
        currentState = state;
        currentState.OnEnter();
    }

    public void Refresh()
    {
        SetState(states[0]);
    }





}
