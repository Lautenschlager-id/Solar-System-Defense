﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SolarSystemDefense
{
	static class EnemySpawner
	{
		static int CurrentQueuePosition;
		static int[] Queue;
		static float TransitionTime;

		class Spawn
		{
			public int ID { get; private set; }
			public float TotalEnemy4Queue;

			public int stdSpawnCooldown = 10;
			public float stdTotalEnemy4Queue;

			public int SpawnCooldown;
			public int StandardCooldown
			{
				get
				{
					return stdSpawnCooldown;
				}
			}
			public float StandardTotalEnemy4Queue
			{
				get
				{
					return stdTotalEnemy4Queue;
				}
			}

			public Spawn(int ID, float TotalEnemy4Queue)
			{
				SpawnCooldown = stdSpawnCooldown;
				this.ID = ID;
				this.TotalEnemy4Queue = stdTotalEnemy4Queue = TotalEnemy4Queue;
			}

			public void RefreshTotalEnemy4Queue()
			{
				stdTotalEnemy4Queue += Graphic.Enemies.Length - Queue[CurrentQueuePosition];
				TotalEnemy4Queue = stdTotalEnemy4Queue;
			}
		}

		static List<Spawn> SpawnData = new List<Spawn>();
		public static void Reset()
		{
			SpawnData.Clear();

			SpawnData.Add(new Spawn(ID: 0, TotalEnemy4Queue: 10));
			SpawnData.Add(new Spawn(ID: 1, TotalEnemy4Queue: 13));
			SpawnData.Add(new Spawn(ID: 2, TotalEnemy4Queue: 8));
			SpawnData.Add(new Spawn(ID: 3, TotalEnemy4Queue: 1));

			CurrentQueuePosition = 0;
			Queue = new int[] { 0, 0, 0, 1, 0, 1, 2, 0, 3, 1, 0, 1, 2, 1, 0, 3 };
			TransitionTime = 20;
		}

		static EnemySpawner()
		{
			Reset();
		}

		static Vector2 Position;
		public static void SetInitialPosition(Vector2 p)
		{
			Position = p;
		}

		public static void Update()
		{
			int index = Queue[CurrentQueuePosition];

			Spawn s = SpawnData[index];
			if (SpawnData[index].TotalEnemy4Queue >= 0)
			{
				if (--s.SpawnCooldown <= 0)
				{
					s.SpawnCooldown = s.StandardCooldown;
					if (--s.TotalEnemy4Queue >= 0)
						EntityManager.New(new Enemy(s.ID, Position));
				}
			}
			else if (EntityManager.Enemies.Count == 0 && (TransitionTime -= .1f) <= 0)
			{
				TransitionTime = 10;
				SpawnData[index].RefreshTotalEnemy4Queue();
				if (++CurrentQueuePosition >= Queue.Length)
				{
					CurrentQueuePosition = 0;
					Data.Level += .4f;
				}
			}
		}
	}
}
