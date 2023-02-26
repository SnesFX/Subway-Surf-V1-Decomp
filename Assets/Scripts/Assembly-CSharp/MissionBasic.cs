public struct MissionBasic
{
	public int[] goalArray;

	public Missions.MissionType type;

	public MissionBasic(Missions.MissionType type, int[] goalArray)
	{
		this.type = type;
		this.goalArray = goalArray;
	}
}
