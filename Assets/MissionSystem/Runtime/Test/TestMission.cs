using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissionSystem.Test
{
    public class TestMission : MissionRoot
    {
        protected override void InitChildMission()
        {
            RegisterMission(new MissionQueue(new IMission[]{
                CreateMission_Stage_1(),
                CreateMission_Stage_2(),
                CreateMission_Stage_3(),
            }), true);
        }
        //起床上学
        private IMission CreateMission_Stage_1()
        {
            return RegisterMission(new MissionQueue(new IMission[]{
                RegisterMission(new MissionLog("开始起床","起床完毕","起床中，进度：{0}",1)),
                RegisterMission(new MissionWhenAll(new IMission[]{
                    RegisterMission(new MissionLog("开启微波炉热早饭","微波炉热早饭完毕","微波炉进度：{0}",1.5f)),
                    RegisterMission(new MissionLog("开始刷牙","刷牙完毕","刷牙中，进度：{0}",1)),
                })),
                RegisterMission(new MissionLog("开始去学校","到学校了","去学习路上，进度：{0}",2)),
            }));
        }
        //学校学习
        private IMission CreateMission_Stage_2()
        {
            return RegisterMission(new MissionQueue(new IMission[]{
                RegisterMission(new MissionLog("上午课程开始","上午课程结束","上午课程，进度：{0}",3)),
                RegisterMission(new MissionLog("中饭开始","中饭结束","吃中饭，进度：{0}",1)),
                RegisterMission(new MissionLog("下午课程结束","下午课程结束","下午课程，进度：{0}",3)),
            }));
        }
        //放学回家
        private IMission CreateMission_Stage_3()
        {
            return RegisterMission(new MissionQueue(new IMission[]{
                RegisterMission(new MissionLog("放学回家","回到家了","回家路上，进度：{0}",2)),
            }));
        }
    }
}