public class Transition
{

    public State state;
    public virtual bool ShouldChangeState()
    {

        return true;
    }

}
