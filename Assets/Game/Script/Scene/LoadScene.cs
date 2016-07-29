using UnityEngine;
using System.Collections;

using GameEnum;

public class LoadScene : BaseScene {
	
    public LoadScene()
    {
        this._type = SCENE_TYPE.LOAD;
    }

	private enum LOAD_STEP
	{
		START = 0,
		INIT_MANAGER,
		LOAD_STRING,
		LOAD_DATA,
		LOAD_ACCOUNT,
		END,
	}

	private int _step = 0;
	private ProgressUI _progressUI = null;

    private void Awake()
    {
		GameResourceManager.Instance.Load ("load", "load_resources");
		GameUIManager.Instance.Load ("load", "load_ui");

		GameObject instant = GameResourceManager.Instance.GetSingleUI ("progress_ui");
		if (instant != null) {
			this._progressUI = instant.GetComponent<ProgressUI> ();
			this._progressUI.Initialize ();
			this._progressUI.SetRate (0f);
			this._progressUI.SetEnable (true);
		}

		StartCoroutine (Load ());
    }

	private void OnDestroy()
	{
		GameUIManager.Instance.RemoveGroup ("load");
		GameResourceManager.Instance.RemoveGroup ("load");
	}

    private IEnumerator Load()
    {
		float rate = 0f;
		SetProgressRate (rate);

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
						GameStringManager.Instance.Initialize ();
                        break;
                    }
				case LOAD_STEP.LOAD_STRING:
					{
						GameStringManager.Instance.Load ("normal");
						break;
					}
				case LOAD_STEP.LOAD_DATA:
					{
						GameDataManager.Instance.LoadBlockData ();
						break;
					}
				case LOAD_STEP.LOAD_ACCOUNT:
					{
						break;
					}
                case LOAD_STEP.END:
                    {
						GameSceneManager.Instance.ChangeScene (SCENE_TYPE.MAIN);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            this._step++;

			rate = (float)this._step / (float)LOAD_STEP.END;
			SetProgressRate (rate);

            yield return null;
        }
    }

	private void SetProgressRate(float rate) {
		if (this._progressUI != null) {
			this._progressUI.SetRate (rate);
		}
	}
}
