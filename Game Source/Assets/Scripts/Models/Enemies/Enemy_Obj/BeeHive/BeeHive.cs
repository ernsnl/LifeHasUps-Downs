using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Assets.Main;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Models.Enemies.Enemy_Obj.BeeHive
{
    public class BeeHive : Enemy
    {
        [Serializable]
        public class Goal
        {
            public string GoalName;
            public int GoalValue;

            public int GetDiscontentment(int newValue)
            {
                return newValue * newValue;
            }

            public Goal(string goalName, int goalValue)
            {
                GoalName = goalName;
                GoalValue = goalValue;
            }
        }

        public class Action
        {
            public string ActionName;
            public Func<Goal, int> ActionFunc;

            public int GetGoalChangeValue(Goal goal)
            {
                return ActionFunc(goal);
            }

            public Action(string name, Func<Goal, int> goalChangeFunc)
            {
                ActionFunc = goalChangeFunc;
                ActionName = name;
            }
        }

        [HideInInspector]
        public List<Goal> BeeHiveGoals;
        [HideInInspector]
        public List<Action> BeeHiveActions;
        public int InitialQueenCount = 1;
        public int requiredResources = 5000;
        private int gatheredResources;
        
        private float _timer;

        private GameObject WorkerBee;
        private GameObject DefenderBee;
        private Transform SpawnPoint;
        private Transform DefenderBeeController;
        

        private int GatherResources(Goal goal)
        {

            if (goal.GoalName.Contains("Explore"))
            {
                return goal.GoalValue - 100;
            }
            if (goal.GoalName.Contains("Defend"))
            {
                return goal.GoalValue + 25;
            }
            return goal.GoalValue;
        }

        private int Produce_DefenderBees(Goal goal)
        {
            if (goal.GoalName.Contains("Defend"))
            {
                return goal.GoalValue - 200;
            }
            return goal.GoalValue + 25;
        }

        private Goal FindBestGoal()
        {
            Goal foundGoal = BeeHiveGoals[0];
            foreach (var goal in BeeHiveGoals)
            {
                if (goal.GoalValue > foundGoal.GoalValue)
                    foundGoal = goal;

            }
            return foundGoal;
        }

        public void Start()
        {
            Initialize();
            _timer = 0f;
            WorkerBee = Resources.Load<GameObject>("Prefabs/Enemy/Bee");
            DefenderBee = Resources.Load<GameObject>("Prefabs/Enemy/DefenderBee");
            DefenderBeeController = transform.FindChild("DefenderBeeController");
            SpawnPoint = transform.FindChild("SpawnPoint");
        }

        public override void FixedUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer > AttackTimer / InitialQueenCount)
            {
                _timer = 0f;
                var bestAction = ChooseAction();
                //Debug.Log("Action:" + bestAction.ActionName);

                if (bestAction.ActionName == "GatherResources")
                {
                    gatheredResources += 100;
                    if (gatheredResources > requiredResources * InitialQueenCount)
                    {
                        gatheredResources = 0;
                        InitialQueenCount++;
                    }
                }
                else
                {
                    if (DefenderBeeController.GetComponent<DefenderBeeFlocking>().BeeCount < 8)
                    {
                        Instantiate(DefenderBee, SpawnPoint.position, DefenderBeeController.rotation,
                            DefenderBeeController);
                    }

                }         
                foreach (var goal in BeeHiveGoals)
                {
                    goal.GoalValue = bestAction.ActionFunc(goal);
                }
            }
        }

        private void GetAttacked()
        {
            BeeHiveGoals.Find(op => op.GoalName == "Defend").GoalValue += 1000;
        }

        private void GetAggressive()
        {
            DefenderBeeController.GetComponent<DefenderBeeFlocking>().IsAggressive = true;
        }

        private Action ChooseAction()
        {
            var bestAction = BeeHiveActions[0];
            var bestValue = CalculateDiscontentmentValue(bestAction);

            foreach (var action in BeeHiveActions)
            {
                var thisValue = CalculateDiscontentmentValue(action);
                if (thisValue < bestValue)
                {
                    bestValue = thisValue;
                    bestAction = action;
                }
            }
            return bestAction;
        }

        private int CalculateDiscontentmentValue(Action action)
        {
            var discontentment = 0;
            foreach (var goal in BeeHiveGoals)
            {
                var newValue = goal.GoalValue + action.ActionFunc(goal);
                discontentment += goal.GetDiscontentment(newValue);
            }
            return discontentment;
        }

        public void Initialize()
        {
            BeeHiveGoals = new List<Goal>();
            BeeHiveGoals.Add(new Goal("Explore", 1000));
            BeeHiveGoals.Add(new Goal("Defend", 0));

            BeeHiveActions = new List<Action>();
            BeeHiveActions.Add(new Action("Produce_DefenderBees", Produce_DefenderBees));
            BeeHiveActions.Add(new Action("GatherResources", GatherResources));
        }

        public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag == "Player_Bullet")
            {
                GetAggressive();
                GetAttacked();
                BaseHealth -= SimpleFire.GetFireDamage();
                if (BaseHealth < 0f)
                {
                    GameHandler.Game.Spawn.CreatePickUp(this.transform);
                    transform.DetachChildren();
                    Destroy(this.gameObject);
                }
            }
        }

        public BeeHive()
        {
            Initialize();
        }
    }
}