using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MissionSystem
{
    public enum EM_MissionExcuteResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 被终止
        /// </summary>
        Stop,
        /// <summary>
        /// 失败
        /// </summary>
        Fail,
    }
    public interface IMission
    {
        bool IsRunning { get; }
        float Progress { get; }
        ST_MissionState State { get; }
        ValueTask<EM_MissionExcuteResult> ExecuteAsync(CancellationToken token, bool reset = true);
        void Init(ST_MissionState state);
    }
}
