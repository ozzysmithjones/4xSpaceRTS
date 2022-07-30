using UnityEngine;

public class AI : Empire
{
    private Analysis analysis = new Analysis();
    private AIModule module;

    public AI(Color flagColor, string factionName, AIModule module) : base(false, flagColor, factionName)
    {
        this.module = module;
        this.module.Init(this);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);


        analysis.Clear();

        //Could perform some preliminary analysis here. 

        module.Behave(analysis);
    }
}
