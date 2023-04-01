using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MissionSystem
{
    public class MissionWhenAll : MissionBase
    {
        private readonly IMission[] arrChildMission;
        private readonly EM_MissionExcuteResult?[] arrResult;
        private int waitCount;
        public MissionWhenAll(params IMission[] arrChildMission)
        {
            this.arrChildMission = arrChildMission;
            arrResult = new EM_MissionExcuteResult?[arrChildMission.Length];
        }

        protected override async ValueTask<EM_MissionExcuteResult> OnExecute(CancellationToken token, bool reset = true)
        {
            for (int i = 0; i < arrChildMission.Length; i++)
                arrResult[i] = null;
            waitCount = arrChildMission.Length;

            for (int i = 0; i < arrChildMission.Length; i++)
                StartChildMissionAsync(i, token, reset);

            while (waitCount > 0)
            {
                //0.02秒检测一次
                if (token.IsCancellationRequested)
                    break;

                try
                {
                    await Task.Delay(20, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

            if (token.IsCancellationRequested)
                return EM_MissionExcuteResult.Stop;

            for (int i = 0; i < arrResult.Length; i++)
            {
                if (!arrResult[i].HasValue)
                    return EM_MissionExcuteResult.Fail;
                if (EM_MissionExcuteResult.Success != arrResult[i])
                    return arrResult[i] ?? EM_MissionExcuteResult.Fail;
            }

            return EM_MissionExcuteResult.Success;
        }
        async void StartChildMissionAsync(int index, CancellationToken token, bool reset = true)
        {
            arrResult[index] = await arrChildMission[index].ExecuteAsync(token, reset);
            waitCount--;
        }
    }
}
