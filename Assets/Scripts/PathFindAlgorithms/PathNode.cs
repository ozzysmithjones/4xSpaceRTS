/*
public class PathNode : GraphNode
{
    private int end = -1;
    private int distanceToBegining, distanceToEnd;
    public PathNode(int coordinate, int lastCoordinate, float value, int end, int distanceToBegining, int distanceToEnd) : base(coordinate, lastCoordinate, value)
    {
        this.end = end;
        this.distanceToBegining = distanceToBegining;
        this.distanceToEnd = distanceToEnd;
    }

    public override bool EndHere()
    {
        return coordinate == end;
    }

    public override float CalculateValue(GraphNode previousNode)
    {
        value = distanceToBegining + distanceToEnd;
        return value;
    }

    protected override GraphNode CreateNeighbour(int neighbourCoordinate)
    {

        return new PathNode(neighbourCoordinate, coordinate, 0.0f, end, distanceToBegining + (int)(Calculation.SquareDistance(coordinate, neighbourCoordinate) * 10), (int)(Calculation.SquareDistance(neighbourCoordinate, end) * 10));
    }

}
*/
