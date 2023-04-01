using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MissionSystem
{
    public class MissionQueue : MissionBase
    {
        private readonly IMission[] arrChildMission;

        public MissionQueue(params IMission[] arrChildMission)
        {
            this.arrChildMission = arrChildMission;
        }

        protected override async ValueTask<EM_MissionExcuteResult> OnExecute(CancellationToken token, bool reset = true)
        {
            for (int i = 0; i < arrChildMission.Length; i++)
            {
                if (token.IsCancellationRequested)
                    return EM_MissionExcuteResult.Stop;

                if (null != arrChildMission[i])
                {
                    var result = await arrChildMission[i].ExecuteAsync(token,reset);

                    //子任务没完成直接终止
                    if (EM_MissionExcuteResult.Success != result)
                        return result;
                }
            }

            return EM_MissionExcuteResult.Success;
        }
    }
}
