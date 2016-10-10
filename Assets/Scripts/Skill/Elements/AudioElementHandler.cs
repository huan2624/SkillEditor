using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioElementHandler : BaseSkillElementHandler
{
    AudioElement m_AudioElement = null;
    int m_iAudioSoundUid = 0;

    public bool Setup(AudioElement audio_element)
    {
        if (audio_element == null
            || audio_element.m_StartupEvent == null)
        {
            return false;
        }

		//if (string.IsNullOrEmpty(audio_element.m_AudioInfo.m_Name))
		//{
		//	return false;
		//}

        m_AudioElement = audio_element;

        RegisterEventHandler(m_AudioElement.m_StartupEvent, m_AudioElement.m_TerminateEvent);

        return true;
    }

    public override bool Startup(SkillDispEvent evt)
    {
		if (string.IsNullOrEmpty(m_AudioElement.m_AudioInfo.m_EventPath))
		{
			Debug.LogError("SkillDisp AudioElement event missing, id:" + m_CurSkillInfo.m_nSkillDispId);
			return false;
		}
        switch (m_AudioElement.m_AudioInfo.m_Mode)
        {
            case EffectMode.EFT_ON_WORLD_POS:
                if (evt != null && evt.m_EventType == SkillDispEventType.DISPEVT_PROGRAM_CUSTOM)
                {
                    CustomSkillDispEvent custom_evt = evt as CustomSkillDispEvent;
                    if (custom_evt != null)
                    {
						PlayWorldPosSoundEffect(ref m_AudioElement.m_AudioInfo, custom_evt.m_Position);
                    }
                }
                break;

            case EffectMode.EFT_ON_CASTER_POS:
                PlayCasterSoundEffect(ref m_AudioElement.m_AudioInfo, false);
				break;

            case EffectMode.EFT_FOLLOW_CASTER:
                PlayCasterSoundEffect(ref m_AudioElement.m_AudioInfo, true);
                break;

            case EffectMode.EFT_ON_TARGET_POS:
                PlayTargetSoundEffect(ref m_AudioElement.m_AudioInfo, ref m_CurSkillInfo.HitTargets, false);
				break;

            case EffectMode.EFT_FOLLOW_TARGET:
                PlayTargetSoundEffect(ref m_AudioElement.m_AudioInfo, ref m_CurSkillInfo.HitTargets, true);
                break;

            default:
                break;
        }

        return true; // // Make clear event register now
    }

    public override void Terminate(SkillDispEvent evt)
    {
        base.Terminate(evt);

        AudioManager.Instance.StopSound(m_iAudioSoundUid);
    }

    #region Handle Audio

    bool PlayCasterSoundEffect(ref AudioInfo info, bool follow)
    {
        BaseActor caster = m_CurSkillInfo.Caster;
        if (caster == null)
        {
			return false;
		}
        string eventPath = GetEffectPath(info.m_EventPath);
		if (follow)
		{
			m_iAudioSoundUid = AudioManager.Instance.PlayFollowingSound(caster, eventPath);
            return m_iAudioSoundUid != 0;
		}
        m_iAudioSoundUid = AudioManager.Instance.PlaySound(eventPath, caster.transform.position);
        return m_iAudioSoundUid != 0;
    }

    bool PlayTargetSoundEffect(ref AudioInfo info, ref List<uint> lstShootTargets, bool follow)
    {
        IEnumerator<uint> it = ( IEnumerator<uint> )lstShootTargets.GetEnumerator();
        while ( it.MoveNext() )
        {
            uint target_id = it.Current;
			BaseActor ActorTarget = ActorManager.Instance.GetActor(target_id);
			if (ActorTarget != null)
            {
                string eventPath = GetEffectPath(info.m_EventPath);
                if (follow)
                {
					m_iAudioSoundUid = AudioManager.Instance.PlayFollowingSound(ActorTarget, eventPath);
                    return m_iAudioSoundUid != 0;
                }
                m_iAudioSoundUid = AudioManager.Instance.PlaySound(eventPath, ActorTarget.transform.position);
                return m_iAudioSoundUid != 0;
            }
        }

        return false;
    }

	bool PlayWorldPosSoundEffect(ref AudioInfo info, Vector3 pos)
	{
        string eventPath = info.m_EventPath;
        m_iAudioSoundUid = AudioManager.Instance.PlaySound(GetEffectPath(eventPath), pos);
        return m_iAudioSoundUid != 0;
	}

    #endregion
}