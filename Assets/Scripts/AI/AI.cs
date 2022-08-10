using UnityEngine;

public class AI : Empire
{
    private Analysis analysis;
    private AIModule module;

    public AI(Color flagColor, string factionName, AIModule module) : base(false, flagColor, factionName)
    {
        this.analysis = new Analysis(Master.instance.enviroment.stars.Length);
        this.module = module;
        this.module.Init(this);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);


        analysis.ClearValues();
        analysis.ClearInfluenceMaps();

        //Could perform some preliminary analysis here. 

        module.UpdateAI(analysis);
    }
}
