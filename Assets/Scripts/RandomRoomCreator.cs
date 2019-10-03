using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomRoomCreator : MonoBehaviour
{
	public bool isTest;
	public int criticalPathLength;
	public Vector2Int mapSizePerRoom;
	public Vector2Int roomSizePerUnit;
	public List<GameObject> secretDownRooms;
	public List<GameObject> endRooms;
	public List<GameObject> horizontalRooms;
	public List<GameObject> verticalRooms;
	public List<GameObject> fillRooms;
	public List<GameObject> secretUpRooms;
	public int secretRate;
	public enum RoomType {
		fill,
		horizontal,
		vertical,
		secretDown,
		secretUp
	}
	public enum RandomMonsterType {
		none,
		physical,
		far,
		special
	}	
	public class RoomInfo {
		public Vector2Int roomPos;
		public RoomType roomType;
		public RandomMonsterType monsterType;
		public bool isEnd = false;
		public bool isTreasure = false;
	}

	bool[,] stuffed;
	private List<RoomInfo> roomData;

	RoomInfo currentRoom;
	RoomInfo previousRoom;

	private void Awake()
	{
		stuffed = new bool[mapSizePerRoom.x, mapSizePerRoom.y];
		roomData = new List<RoomInfo>();
		roomData.Clear();
		//选取开始房间
		RoomInfo startroom = new RoomInfo
		{
			roomPos = new Vector2Int(0, Random0ToN(mapSizePerRoom.y)),
			roomType = RoomType.horizontal
		};
		roomData.Add(startroom);
		currentRoom = startroom;
		previousRoom = currentRoom;
		stuffed[startroom.roomPos.x, startroom.roomPos.y] = true;
		while (!previousRoom.isEnd) {
			previousRoom = currentRoom;
			currentRoom = new RoomInfo();
			if (previousRoom.roomPos.x < mapSizePerRoom.x - 1)
			{
				if (previousRoom.roomPos.y == 0||stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y-1]==true)
				{
					if (previousRoom.roomPos.y<(mapSizePerRoom.y - 1) &&(stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == false))
					{
						if (previousRoom.roomPos.y > 0)
						{
							switch (Random0ToN(2))
							{
								case 0:
									{
										currentRoom.roomPos.x = previousRoom.roomPos.x;
										currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
										currentRoom.roomType = RoomType.horizontal;
										break;
									}
								default:
									{
										currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
										currentRoom.roomPos.y = previousRoom.roomPos.y;
										currentRoom.roomType = RoomType.vertical;
										break;
									}
							}
						}
						else {
							currentRoom.roomPos.x = previousRoom.roomPos.x;
							currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
							currentRoom.roomType = RoomType.horizontal;
						}
					}
					else {
						currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
						currentRoom.roomPos.y = previousRoom.roomPos.y;
						currentRoom.roomType = RoomType.vertical;
					}
				}
				else if (previousRoom.roomPos.y == (mapSizePerRoom.y - 1)||stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y+1]==true)
				{
					if (previousRoom.roomPos.y > 0 && stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1] == false)
					{
						if (previousRoom.roomPos.y < (mapSizePerRoom.y - 1))
						{
							switch (Random0ToN(2))
							{
								case 0:
									{
										currentRoom.roomPos.x = previousRoom.roomPos.x;
										currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
										currentRoom.roomType = RoomType.horizontal;
										break;
									}
								default:
									{
										currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
										currentRoom.roomPos.y = previousRoom.roomPos.y;
										currentRoom.roomType = RoomType.vertical;
										break;
									}
							}
						}
						else {
							currentRoom.roomPos.x = previousRoom.roomPos.x;
							currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
							currentRoom.roomType = RoomType.horizontal;
						}
					}
					else
					{
						currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
						currentRoom.roomPos.y = previousRoom.roomPos.y;
						currentRoom.roomType = RoomType.vertical;
					}
				}
				else
				{
					switch (Random0ToN(2))
					{
						case 0:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
								currentRoom.roomType = RoomType.horizontal;
								break;
							}
						default:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
								currentRoom.roomType = RoomType.horizontal;
								break;
							}
					}
				}
			}
			else
			{
				if (previousRoom.roomPos.y == 0 || stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1] == true)
				{
					if (previousRoom.roomPos.y < mapSizePerRoom.y - 1&&stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y+1]==false)
					{
						if (previousRoom.roomPos.y > 0)
						{
							switch (Random0ToN(2))
							{
								case 0:
									{
										currentRoom.roomPos.x = previousRoom.roomPos.x;
										currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
										currentRoom.roomType = RoomType.horizontal;
										break;
									}
								default:
									{
										currentRoom = null;
										previousRoom.isEnd = true;
										break;
									}
							}
						}
						else {
							currentRoom.roomPos.x = previousRoom.roomPos.x;
							currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
							currentRoom.roomType = RoomType.horizontal;
						}
					}
					else {
						currentRoom = null;
						previousRoom.isEnd = true;
					}
				}
				else if (previousRoom.roomPos.y == (mapSizePerRoom.y - 1) || stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == true)
				{
					if (previousRoom.roomPos.y > 0&&stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y-1]==false)
					{
						if (previousRoom.roomPos.y < (mapSizePerRoom.y - 1))
						{
							switch (Random0ToN(2))
							{
								case 0:
									{
										currentRoom.roomPos.x = previousRoom.roomPos.x;
										currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
										currentRoom.roomType = RoomType.horizontal;
										break;
									}
								default:
									{
										currentRoom = null;
										previousRoom.isEnd = true;
										break;
									}
							}
						}
						else
						{
							currentRoom.roomPos.x = previousRoom.roomPos.x;
							currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
							currentRoom.roomType = RoomType.horizontal;
						}
					}
					else {
						currentRoom = null;
						previousRoom.isEnd = true;
					}
				}
				else
				{
					switch (Random0ToN(2))
					{
						case 0:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
								currentRoom.roomType = RoomType.horizontal;
								break;
							}
						default:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
								currentRoom.roomType = RoomType.horizontal;
								break;
							}
					}
				}
			}
			if (currentRoom != null)
			{
				roomData.Add(currentRoom);
				stuffed[currentRoom.roomPos.x, currentRoom.roomPos.y] = true;
			}
		}
		criticalPathLength = roomData.Count;
		List<int> leftStartPos = new List<int>();
		List<int> rightStartPos = new List<int>();
		for (int i = 0; i < criticalPathLength; i++)
		{
			if ((roomData[i].roomPos.y != 0 )&& (stuffed[roomData[i].roomPos.x, roomData[i].roomPos.y - 1]==false)) leftStartPos.Add(i);
			else if ((roomData[i].roomPos.y != (mapSizePerRoom.y - 1)) && (stuffed[roomData[i].roomPos.x, roomData[i].roomPos.y + 1]==false)) rightStartPos.Add(i);
		}
		{
			while(leftStartPos.Count != 0)
			{
				int pos = Random0ToN(leftStartPos.Count);
				previousRoom = roomData[leftStartPos[pos]];
				if (stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1]) { leftStartPos.RemoveAt(pos);continue;}
				stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1] = true;
				currentRoom = new RoomInfo{roomType = RoomType.horizontal};
				currentRoom.roomPos.x = previousRoom.roomPos.x;
				currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
				roomData.Add(currentRoom);
				while (!previousRoom.isTreasure)
				{
					previousRoom = currentRoom;
					currentRoom = new RoomInfo();
					if (previousRoom.roomPos.y > 0&&(!stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y-1]))
					{
						currentRoom.roomPos.x = previousRoom.roomPos.x;
						currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
						currentRoom.roomType = RoomType.horizontal;
					}
					else {
						if (previousRoom.roomPos.x == (mapSizePerRoom.x - 1) || stuffed[previousRoom.roomPos.x + 1, previousRoom.roomPos.y] == true)
						{
							currentRoom = null;
							previousRoom.isTreasure = true;
						}
						else {
							currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
							currentRoom.roomPos.y = previousRoom.roomPos.y;
							currentRoom.roomType = RoomType.secretUp;
						}
					}
					if (currentRoom != null)
					{
						roomData.Add(currentRoom);
						stuffed[currentRoom.roomPos.x, currentRoom.roomPos.y] = true;
					}
				}
				leftStartPos.RemoveAt(pos);
			}
			while (rightStartPos.Count != 0)
			{
				int pos = Random0ToN(rightStartPos.Count);
				previousRoom = roomData[rightStartPos[pos]];
				if (stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == true) {rightStartPos.RemoveAt(pos); continue; }
				stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] = true;
				currentRoom = new RoomInfo { roomType = RoomType.horizontal };
				currentRoom.roomPos.x = previousRoom.roomPos.x;
				currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
				roomData.Add(currentRoom);
				while (!previousRoom.isTreasure)
				{
					previousRoom = currentRoom;
					currentRoom = new RoomInfo();
					if (previousRoom.roomPos.y < (mapSizePerRoom.y-1) && (!stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1]))
					{
						currentRoom.roomPos.x = previousRoom.roomPos.x;
						currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
						currentRoom.roomType = RoomType.horizontal;
					}
					else
					{
						if (previousRoom.roomPos.x == (mapSizePerRoom.x - 1) || stuffed[previousRoom.roomPos.x + 1, previousRoom.roomPos.y] == true)
						{
							currentRoom = null;
							previousRoom.isTreasure = true;
						}
						else
						{
							currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
							currentRoom.roomPos.y = previousRoom.roomPos.y;
							currentRoom.roomType = RoomType.secretUp;
						}
					}
					if (currentRoom != null)
					{
						roomData.Add(currentRoom);
						stuffed[currentRoom.roomPos.x, currentRoom.roomPos.y] = true;
					}
				}
				rightStartPos.RemoveAt(pos);
			}
		}
		for (int i = 0; i < mapSizePerRoom.x; i++) {
			for (int j = 0; j < mapSizePerRoom.y; j++) {
				if (!stuffed[i, j]) {
					var k = new RoomInfo
					{
						roomPos = new Vector2Int(i, j),
						roomType = RoomType.fill
					};
					roomData.Add(k);
				}
			}
		}

		foreach (RoomInfo room in roomData) {
			//if (room.isEnd)
			//{
			//	Instantiate(endRooms[Random0ToN(endRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform);
			//}
			switch (room.roomType)
				{
					case RoomType.secretDown: Instantiate(secretDownRooms[Random0ToN(secretDownRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform); break;
					case RoomType.horizontal: Instantiate(horizontalRooms[Random0ToN(horizontalRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform); break;
					case RoomType.vertical: Instantiate(verticalRooms[Random0ToN(verticalRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform); break;
					case RoomType.secretUp: Instantiate(secretUpRooms[Random0ToN(secretUpRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform); break;
					default: Instantiate(fillRooms[Random0ToN(fillRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform); break;
				}
		}
		if (!isTest)
		{
			var manager = FindObjectOfType<GameManager>();
			manager.spawnPos = transform.position + RoomToWorldPos(roomData[0].roomPos);
		}
	}

	Vector3 RoomToWorldPos(Vector2Int roomPos) {
		return new Vector3(roomPos.y*roomSizePerUnit.x, roomPos.x*roomSizePerUnit.y);
	}

	public int Random0ToN(int n)
	{
		int temp = Mathf.FloorToInt(Random.value * n);
		while (temp == n) temp = Mathf.FloorToInt(Random.value * n);
		return temp;
	}
}
