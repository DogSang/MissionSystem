using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MissionSystem
{
    public abstract class MissionRoot : MissionBase
    {
        [System.Serializable]
        private class MissionStates
        {
            public List<ST_MissionState> states;

            public MissionStates()
            {
                states = new List<ST_MissionState>();
            }

            public string GetJson(List<IMission> missions)
            {
                states.Clear();
                for (int i = 0; i < missions.Count; i++)
                    states.Add(missions[i].State);

                return JsonUtility.ToJson(this);
            }
            public void SetJson(string json, List<IMission> missions)
            {
                JsonUtility.FromJsonOverwrite(json, this);

                for (int i = 0; i < missions.Count; i++)
                {
                    Debug.Log(JsonUtility.ToJson(states[i]));
                    missions[i].Init(states[i]);
                }
            }
        }

        protected readonly List<IMission> missions;
        private MissionStates missionStates;
        protected IMission mainMission;

        public string StatesData
        {
            get
            {
                return missionStates.GetJson(missions);
            }
            set
            {
                if (IsRunning)
                    throw new System.Exception("运行中不能修改状态");

                missionStates.SetJson(value, missions);
            }
        }

        public MissionRoot()
        {
            missions = new List<IMission>();
            missionStates = new MissionStates();

            InitChildMission();
        }

        protected abstract void InitChildMission();

        protected IMission RegisterMission(IMission mission, bool isMain = false)
        {
            if (missions.Contains(mission))
                throw new System.Exception("不能重复添加");

            if (isMain)
                mainMission = mission;
            missions.Add(mission);
            return mission;
        }

        protected override async ValueTask<EM_MissionExcuteResult> OnExecute(CancellationToken token, bool reset = true)
        {
            if (null == mainMission)
                return EM_MissionExcuteResult.Success;//没有直接完成
            else
                return await mainMission.ExecuteAsync(token, reset);
        }

    }
}
