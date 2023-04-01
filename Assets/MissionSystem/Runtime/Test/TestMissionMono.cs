using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

namespace MissionSystem.Test
{
    public class TestMissionMono : MonoBehaviour
    {
        TestMission testMission;
        [SerializeField,TextArea]
        string stateData;
        CancellationTokenSource tokenSource;

        MissionLog log;
        IMission whenAllTest, queueTest;

        private void Awake()
        {
            log = new MissionLog("开始起床", "起床完毕", "起床中，进度：{0}", 2);

            whenAllTest = new MissionWhenAll(new IMission[]{
                    new MissionLog("开启微波炉热早饭","微波炉热早饭完毕","微波炉进度：{0}",3),
                    new MissionLog("开始刷牙","刷牙完毕","刷牙中，进度：{0}",2),
                });
            queueTest = new MissionQueue(new IMission[]{
                new MissionLog("上午课程开始","上午课程结束","上午课程，进度：{0}",2),
                new MissionLog("中饭开始","中饭结束","吃中饭，进度：{0}",2),
                new MissionLog("下午课程结束","下午课程结束","下午课程，进度：{0}",2),
            });

            testMission = new TestMission();
        }

        private void OnDestroy()
        {
            if (null != tokenSource)
                tokenSource.Cancel();
        }

        // Update is called once per frame
        async void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("开启任务");
                if (null != tokenSource)
                    tokenSource.Cancel();
                tokenSource = new CancellationTokenSource();
                var result = await testMission.ExecuteAsync(tokenSource.Token);
                Debug.Log(result);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("终止任务");
                tokenSource.Cancel();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("保存任务状态 ");
                stateData = testMission.StatesData;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("读取任务状态");
                testMission.StatesData = stateData;
                tokenSource = new CancellationTokenSource();
                var result = testMission.ExecuteAsync(tokenSource.Token,false);
                Debug.Log(result);
            }
        }
    }
}
