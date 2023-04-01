using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionSystem
{
    public enum EM_Stage
    {
        Sleep,
        Running,
        Complete,
        Stop,
    }
    [System.Serializable]
    public struct ST_MissionState
    {
        /// <summary>
        /// 状态
        /// </summary>
        public EM_Stage em_Stage;
        /// <summary>
        /// 进度
        /// </summary>
        public float progress;
        /// <summary>
        /// 自定义信息
        /// </summary>
        public string customData;
    }
}
