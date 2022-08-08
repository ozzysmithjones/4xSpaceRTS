using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskState
{
    Success,
    Failure,
    Running,
}

public abstract class TacticTask : PrimitiveTask
{
    public abstract TaskState Run(Analysis analysis, List<Fleet> fleets);
}
