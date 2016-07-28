using UnityEngine;
using System.Collections;

using GameEnum;

public class LoadScene : BaseScene {

    private enum LOAD_STEP
    {
        START = 0,
        INIT_MANAGER,
        LOAD_STRING,
        LOAD_DATA,
        LOAD_ACCOUNT,
        END,
    }

    int _step = 0;

    public LoadScene()
    {
        this._type = SCENE_TYPE.LOAD;
    }

    public void Awake()
    {
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        float rate = 0f;
        while (rate < 100f)
        {
            rate += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Load()
    {
        while (System.Enum.IsDefined(typeof(LOAD_STEP), this._step) == true)
        {
            switch ((LOAD_STEP)this._step)
            {
                case LOAD_STEP.START:
                    {
                        break;
                    }
                case LOAD_STEP.INIT_MANAGER:
                    {
                        break;
                    }
                case LOAD_STEP.END:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            this._step++;
            yield return null;
        }
    }
}
