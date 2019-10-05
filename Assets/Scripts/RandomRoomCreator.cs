using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomRoomCreator : MonoBehaviour
{
	public bool isTest;
	public bool isBlue;
	public int criticalPathLength;
	public Vector2Int mapSizePerRoom;
	public Vector2Int roomSizePerUnit;
	public List<GameObject> secretDownRooms;
	public List<GameObject> secretDownTemps;
	//public List<GameObject> endRooms;
	public List<GameObject> horizontalRooms;
	public List<GameObject> horizontalTemps;
	public List<GameObject> verticalRooms;
	public List<GameObject> verticalTemps;
	public List<GameObject> fillRooms;
	public List<GameObject> secretUpRooms;
	public List<GameObject> secretUpTemps;
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
			roomPos = new Vector2Int(0, Random0ToN(mapSizePerRoom.y-2)+1),
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
				if (previousRoom.roomPos.y <= 1||stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y-1]==true)
				{
					if (previousRoom.roomPos.y<(mapSizePerRoom.y - 2d) &&(stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == false))
					{
						if (previousRoom.roomPos.y > 1)
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
										if (isBlue){
											currentRoom.roomType = RoomType.horizontal;
											previousRoom.roomType = RoomType.vertical;
										}
										else currentRoom.roomType = RoomType.vertical;
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
						if (isBlue)
						{
							currentRoom.roomType = RoomType.horizontal;
							previousRoom.roomType = RoomType.vertical;
						}
						else currentRoom.roomType = RoomType.vertical;
					}
				}
				else if (previousRoom.roomPos.y >= (mapSizePerRoom.y - 2)||stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y+1]==true)
				{
					if (previousRoom.roomPos.y > 1 && stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1] == false)
					{
						if (previousRoom.roomPos.y < (mapSizePerRoom.y - 2))
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
										if (isBlue)
										{
											currentRoom.roomType = RoomType.horizontal;
											previousRoom.roomType = RoomType.vertical;
										}
										else currentRoom.roomType = RoomType.vertical;
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
						if (isBlue)
						{
							currentRoom.roomType = RoomType.horizontal;
							previousRoom.roomType = RoomType.vertical;
						}
						else currentRoom.roomType = RoomType.vertical;
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
				if (previousRoom.roomPos.y <= 1 || stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1] == true)
				{
					if (previousRoom.roomPos.y < mapSizePerRoom.y - 2&&stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y+1]==false)
					{
						if (previousRoom.roomPos.y > 1)
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
				else if (previousRoom.roomPos.y >= (mapSizePerRoom.y - 2) || stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == true)
				{
					if (previousRoom.roomPos.y > 1&&stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y-1]==false)
					{
						if (previousRoom.roomPos.y < (mapSizePerRoom.y - 2))
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
			if(leftStartPos.Count > 2)
			{
				int pos = Random0ToN(leftStartPos.Count - 2) + 1;
				bool up = Random0ToN(2) == 0;
				previousRoom = roomData[leftStartPos[pos]];
				//if (stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y - 1]) { leftStartPos.RemoveAt(pos);continue;}
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
						if ((up^isBlue)?(previousRoom.roomPos.x == (mapSizePerRoom.x - 1) || stuffed[previousRoom.roomPos.x + 1, previousRoom.roomPos.y] == true):(previousRoom.roomPos.x == 0 || stuffed[previousRoom.roomPos.x - 1, previousRoom.roomPos.y] == true))
						{
							currentRoom = null;
							previousRoom.isTreasure = true;
						}
						else {
							
							currentRoom.roomPos.y = previousRoom.roomPos.y;
							if (up)
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x + (isBlue ? -1 : 1);
								currentRoom.roomType = RoomType.secretUp;
							}
							else
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x + (isBlue ? 1 : -1);
								currentRoom.roomType = RoomType.secretDown;
								if (previousRoom.roomType == RoomType.horizontal) previousRoom.roomType = RoomType.vertical;
								else previousRoom.roomType = RoomType.secretUp;
							}
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
			if(rightStartPos.Count > 2)
			{
				int pos = Random0ToN(rightStartPos.Count-2)+1;
				bool up = Random0ToN(2) == 0;
				previousRoom = roomData[rightStartPos[pos]];
				//if (stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == true) {rightStartPos.RemoveAt(pos); continue; }
				if (previousRoom.roomPos.x == 0) up = true;
				if (previousRoom.roomPos.x == mapSizePerRoom.x - 1) up = false;
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
						if ((up^isBlue) ? (previousRoom.roomPos.x == (mapSizePerRoom.x - 1) || stuffed[previousRoom.roomPos.x + 1, previousRoom.roomPos.y] == true) : (previousRoom.roomPos.x == 0 || stuffed[previousRoom.roomPos.x - 1, previousRoom.roomPos.y] == true))
						{
							currentRoom = null;
							previousRoom.isTreasure = true;
						}
						else
						{
							currentRoom.roomPos.y = previousRoom.roomPos.y;
							if (up)
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x +(isBlue?-1: 1);
								currentRoom.roomType = RoomType.secretUp;
							}
							else
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x +(isBlue?1:- 1);
								currentRoom.roomType = RoomType.secretDown;
								if (previousRoom.roomType == RoomType.horizontal) previousRoom.roomType = RoomType.vertical;
								else previousRoom.roomType = RoomType.secretUp;
							}
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
			int pos;
			GameObject obj;
			switch (room.roomType)
				{
					case RoomType.secretDown:
					{
						if (secretDownRooms.Count == 0)
						{
							foreach (GameObject GO in secretDownTemps) secretDownRooms.Add(GO);
							secretDownTemps.Clear();
							pos = Random0ToN(secretDownRooms.Count - 1);
						}
						else pos = Random0ToN(secretDownRooms.Count);
						secretDownTemps.Add(secretDownRooms[pos]);
						obj=Instantiate(secretDownRooms[pos], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform);
						secretDownRooms.RemoveAt(pos);
						break;
					}
					case RoomType.horizontal:
					{
						if (horizontalRooms.Count == 0)
						{
							foreach (GameObject GO in horizontalTemps) horizontalRooms.Add(GO);
							horizontalTemps.Clear();
							pos = Random0ToN(horizontalRooms.Count - 1);
						}
						else pos = Random0ToN(horizontalRooms.Count);
						horizontalTemps.Add(horizontalRooms[pos]);
						obj = Instantiate(horizontalRooms[pos], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform);
						horizontalRooms.RemoveAt(pos);
						break;
					}
				case RoomType.vertical:
					{
						if (verticalRooms.Count == 0)
						{
							foreach (GameObject GO in verticalTemps) verticalRooms.Add(GO);
							verticalTemps.Clear();
							pos = Random0ToN(verticalRooms.Count - 1);
						}
						else pos = Random0ToN(verticalRooms.Count);
						verticalTemps.Add(verticalRooms[pos]);
						obj = Instantiate(verticalRooms[pos], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform);
						verticalRooms.RemoveAt(pos);
						break;
					}
				case RoomType.secretUp:
					{
						if (secretUpRooms.Count == 0)
						{
							foreach (GameObject GO in secretUpTemps) secretUpRooms.Add(GO);
							secretUpTemps.Clear();
							pos = Random0ToN(secretUpRooms.Count - 1);
						}
						else pos = Random0ToN(secretUpRooms.Count);
						secretUpTemps.Add(secretUpRooms[pos]);
						obj = Instantiate(secretUpRooms[pos], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform);
						secretUpRooms.RemoveAt(pos);
						break;
					}
				default: Instantiate(fillRooms[Random0ToN(fillRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation, transform); break;
				}
		}
		if (!isTest)
		{
			var manager = FindObjectOfType<GameManager>();
			manager.spawnPos = transform.position + RoomToWorldPos(roomData[0].roomPos)+new Vector3(0,6);
		}
	}

	Vector3 RoomToWorldPos(Vector2Int roomPos) {
		if (isBlue) return new Vector3(roomPos.y * roomSizePerUnit.x, -1 * roomPos.x * roomSizePerUnit.y);
		return new Vector3(roomPos.y*roomSizePerUnit.x, roomPos.x*roomSizePerUnit.y);
	}

	public int Random0ToN(int n)
	{
		int temp = Mathf.FloorToInt(Random.value * n);
		while (temp == n) temp = Mathf.FloorToInt(Random.value * n);
		return temp;
	}
}