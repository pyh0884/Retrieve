using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomRoomCreator : MonoBehaviour
{
	public Vector2Int mapSizePerRoom;
	public Vector2Int roomSizePerUnit;
	public List<GameObject> startRooms;//初始房间一定在最左侧，仅向右连通
	public List<GameObject> endRooms;//结束房间一定在最右侧，仅向左连通
	public List<GameObject> horizontalRooms;//水平房间左右连通
	public List<GameObject> verticalRooms;//垂直房间向下连通，左右任意
	public List<GameObject> fillRooms;//填充房间任意
	public List<GameObject> serectRooms;//填充房间有一定几率抽取的是秘密房间,双向连通
	public int serectRate;
	public enum RoomType {
		start,
		horizontal,
		vertical,
		fill,
		serect
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
	}

	bool[,] stuffed;
	private List<RoomInfo> roomData;

	RoomInfo currentRoom;
	RoomInfo previousRoom;

	private void Awake()
	{
		stuffed = new bool[mapSizePerRoom.x, mapSizePerRoom.y];
		roomData.Clear();
		//选取开始房间
		RoomInfo startroom = new RoomInfo();
		startroom.roomPos = new Vector2Int(0, Random0ToN(mapSizePerRoom.y));
		startroom.roomType = RoomType.start;
		roomData.Add(startroom);
		currentRoom = startroom;
		stuffed[startroom.roomPos.x, startroom.roomPos.y] = true;
		bool finished = false;
		while (!finished) {
			previousRoom = currentRoom;
			currentRoom = new RoomInfo();
			if (previousRoom.roomPos.x != mapSizePerRoom.x - 1)
			{
				if (previousRoom.roomPos.y == 0||stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y-1]==true)
				{
					switch (Random0ToN(2))
					{
						case 0:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
								currentRoom.roomType = RoomType.vertical;
								break;
							}
						default:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
								currentRoom.roomPos.y = previousRoom.roomPos.y;
								currentRoom.roomType = RoomType.horizontal;
								break;
							}
					}
				}
				else if (previousRoom.roomPos.y == (mapSizePerRoom.y - 1)||stuffed[previousRoom.roomPos.x,previousRoom.roomPos.y+1]==true)
				{
					switch (Random0ToN(2))
					{
						case 0:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
								currentRoom.roomType = RoomType.horizontal;
								previousRoom.roomType = RoomType.vertical;
								break;
							}
						default:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
								currentRoom.roomPos.y = previousRoom.roomPos.y;
								currentRoom.roomType = RoomType.horizontal;
								break;
							}
					}
				}
				else
				{
					switch (Random0ToN(3))
					{
						case 0:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
								currentRoom.roomType = RoomType.horizontal;
								previousRoom.roomType = RoomType.vertical;
								break;
							}
						case 1
:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
								currentRoom.roomType = RoomType.vertical;
								break;
							}
						default:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x + 1;
								currentRoom.roomPos.y = previousRoom.roomPos.y;
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
					if (previousRoom.roomPos.y < mapSizePerRoom.y - 1)
					{
						switch (Random0ToN(2))
						{
							case 0:
								{
									currentRoom.roomPos.x = previousRoom.roomPos.x;
									currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
									currentRoom.roomType = RoomType.vertical;
									break;
								}
							default:
								{
									currentRoom.roomPos.x = previousRoom.roomPos.x;
									currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
									previousRoom.isEnd = true;
									currentRoom.roomType = RoomType.fill;
									finished = true;
									break;
								}
						}
					}
					else {
						currentRoom = null;
						previousRoom.isEnd = true;
						finished = true;
					}
				}
				else if (previousRoom.roomPos.y == (mapSizePerRoom.y - 1) || stuffed[previousRoom.roomPos.x, previousRoom.roomPos.y + 1] == true)
				{
					if (previousRoom.roomPos.y > 0)
					{
						switch (Random0ToN(2))
						{
							case 0:
								{
									currentRoom.roomPos.x = previousRoom.roomPos.x;
									currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
									currentRoom.roomType = RoomType.horizontal;
									previousRoom.roomType = RoomType.vertical;
									break;
								}
							default:
								{
									currentRoom.roomPos.x = previousRoom.roomPos.x;
									currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
									currentRoom.roomType = RoomType.fill;
									previousRoom.isEnd = true;
									break;
								}
						}
					}
					else {
						currentRoom = null;
						previousRoom.isEnd = true;
						finished = true;
					}
				}
				else
				{
					switch (Random0ToN(3))
					{
						case 0:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y - 1;
								currentRoom.roomType = RoomType.horizontal;
								previousRoom.roomType = RoomType.vertical;
								break;
							}
						case 1:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y + 1;
								currentRoom.roomType = RoomType.vertical;
								break;
							}
						default:
							{
								currentRoom.roomPos.x = previousRoom.roomPos.x;
								currentRoom.roomPos.y = previousRoom.roomPos.y-1;
								currentRoom.roomType = RoomType.fill;
								previousRoom.isEnd = true;
								finished = true;
								break;
							}
					}
				}
			}
			if (currentRoom != null) roomData.Add(currentRoom);
			stuffed[currentRoom.roomPos.x, currentRoom.roomPos.y] = true;
		}
		for (int i = 0; i < mapSizePerRoom.x; i++) {
			for (int j = 0; j < mapSizePerRoom.y; j++) {
				if (!stuffed[i, j]) {
					var k = new RoomInfo();
					k.roomPos = new Vector2Int(i, j);
					switch (Random0ToN(serectRate)) {
						case 0:k.roomType = RoomType.serect;break;
						default:k.roomType = RoomType.fill;break;
					}
					roomData.Add(k);
				}
			}
		}

		foreach (RoomInfo room in roomData) {
			if (room.isEnd) { /*生成传送门，机制没想好*/}
			switch (room.roomType) {
				case RoomType.start:Instantiate(startRooms[Random0ToN(startRooms.Count)],RoomToWorldPos(room.roomPos)+transform.position,transform.rotation);break;
				case RoomType.horizontal:Instantiate(horizontalRooms[Random0ToN(horizontalRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation);break;
				case RoomType.vertical:Instantiate(verticalRooms[Random0ToN(verticalRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation);break;
				case RoomType.serect:Instantiate(serectRooms[Random0ToN(serectRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation);break;
				default:Instantiate(fillRooms[Random0ToN(fillRooms.Count)], RoomToWorldPos(room.roomPos) + transform.position, transform.rotation);break;
			}
		}
	}

	Vector3 RoomToWorldPos(Vector2Int roomPos) {
		return new Vector3(roomPos.x*roomSizePerUnit.x, roomPos.y*roomSizePerUnit.y);
	}

	public int Random0ToN(int n)
	{
		int temp = Mathf.FloorToInt(Random.value * n);
		while (temp == n) temp = Mathf.FloorToInt(Random.value * n);
		return temp;
	}
}
