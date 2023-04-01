using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MissionSystem.Test
{
    public class MissionLog : MissionBase
    {
        private readonly string logStart, logEnd, logProgress;
        private readonly float waitTime;
        private float timer;

        // protected override string CustomData { get => timer.ToString(); set => Debug.Log(logStart + " " + (timer = float.TryParse(value, out timer) ? timer : 0)); }
        protected override string CustomData { get => timer.ToString(); set => timer = float.TryParse(value, out timer) ? timer : 0; }

        public MissionLog(string logStart, string logEnd, string logProgress, float waitTime)
        {
            this.logStart = logStart;
            this.logProgress = logProgress;
            this.logEnd = logEnd;
            this.waitTime = waitTime;

            timer = 0;
        }

        protected override void BeforeExecute()
        {
            base.BeforeExecute();
            Debug.Log(logStart);
        }
        protected override async ValueTask<EM_MissionExcuteResult> OnExecute(CancellationToken token, bool reset = true)
        {
            while (timer < waitTime)
            {
                if (token.IsCancellationRequested)
                    return EM_MissionExcuteResult.Stop;

                await Task.Delay(20);
                timer += 0.02f;
                Progress = timer / waitTime;
            }

            return EM_MissionExcuteResult.Success;
        }
        protected override void AfterExecute()
        {
            base.AfterExecute();
            Debug.Log(logEnd);
        }

        protected override void OnSetProgress(float progress)
        {
            // Debug.LogFormat(logProgress, progress);
        }
    }
}