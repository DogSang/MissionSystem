using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MissionSystem
{
    public abstract class MissionBase : IMission
    {
        public bool IsRunning => EM_Stage.Running == em_Stage;

        private float _progress;
        public float Progress
        {
            get => _progress; set => OnSetProgress(Math.Clamp(_progress = value, 0f, 1f));
        }

        protected EM_Stage em_Stage;

        protected virtual string CustomData { get; set; }

        public ST_MissionState State => new()
        {
            em_Stage = em_Stage,
            progress = Progress,
            customData = CustomData,
        };

        public MissionBase()
        {
            _progress = 0;
            em_Stage = EM_Stage.Sleep;
            CustomData = string.Empty;
        }

        public async ValueTask<EM_MissionExcuteResult> ExecuteAsync(CancellationToken token, bool reset = true)
        {
            if (EM_Stage.Running == em_Stage)
                throw new System.Exception("不能反复开启");

            if (reset)
                Init(default);

            if (EM_Stage.Complete == em_Stage)
                return default;//已经执行完毕了

            em_Stage = EM_Stage.Running;
            BeforeExecute();
            Progress = 0;
            EM_MissionExcuteResult result = EM_MissionExcuteResult.Fail;
            try
            {
                result = await OnExecute(token, reset);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            if (result == EM_MissionExcuteResult.Success)
            {
                Progress = 1;
            }
            AfterExecute();

            switch (result)
            {
                case EM_MissionExcuteResult.Success:
                    em_Stage = EM_Stage.Complete;
                    break;
                default:
                    em_Stage = EM_Stage.Stop;
                    break;
            }

            return result;
        }

        protected virtual void BeforeExecute() { }
        protected abstract ValueTask<EM_MissionExcuteResult> OnExecute(CancellationToken token, bool reset = true);
        protected virtual void AfterExecute() { }

        protected virtual void OnSetProgress(float progress) { }

        public void Init(ST_MissionState state)
        {
            if (EM_Stage.Running == em_Stage)
                throw new System.Exception("运行中不能直接修改状态");

            if (EM_Stage.Running == state.em_Stage)
                em_Stage = EM_Stage.Sleep;
            else
                em_Stage = state.em_Stage;

            Progress = state.progress;
            CustomData = state.customData;
        }
    }
}
