using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

    public BaseActor main;
    public BaseActor second;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            main.GetActorAttribute().ACTOR_UNIQUE_ID = 1;
            second.GetActorAttribute().ACTOR_UNIQUE_ID = 2;
            ActorManager.Instance.RegistActor(main);
            ActorManager.Instance.RegistActor(second);
            main.AttackBySkillID(1, second);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            float oldTime = Time.time;
            Debug.Log("oldTime = " + oldTime);
            SkillDispData data = SkillManager.Instance.LoadSkillDispData(101001);
            float newTime = Time.time;
            Debug.Log("newTime = " + newTime);
        }
    }
}
